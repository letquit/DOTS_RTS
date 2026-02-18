using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 查找目标系统，用于在指定范围内查找符合特定阵营条件的目标单位
/// </summary>
partial struct FindTargetSystem : ISystem
{
    /// <summary>
    /// 更新系统逻辑，在每一帧执行目标查找操作
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHisList = new NativeList<DistanceHit>(Allocator.Temp);
        
        foreach ((RefRO<LocalTransform> localTransform, RefRW<FindTarget> findTarget, RefRW<Target> target) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRW<FindTarget>, RefRW<Target>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (findTarget.ValueRO.timer > 0f)
            {
                // Timer not elapsed
                continue;
            }
            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;
            
            // 清空距离命中列表并设置碰撞过滤器
            distanceHisList.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };
            
            // 在指定范围内执行球形重叠检测，查找潜在目标
            if (collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range,
                    ref distanceHisList, collisionFilter))
            {
                foreach (DistanceHit distanceHit in distanceHisList)
                {
                    // 验证命中的实体是否存在且具有Unit组件
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                    {
                        continue;
                    }
                    
                    // 检查目标单位的阵营是否匹配预设的目标阵营
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        // Valid target
                        target.ValueRW.targetEntity = distanceHit.Entity;
                        break;
                    }
                }
            }
        }
    }
}
