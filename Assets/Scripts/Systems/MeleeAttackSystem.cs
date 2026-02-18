using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

/// <summary>
/// 近战攻击系统，处理单位的近战攻击逻辑，包括距离检测、射线检测和伤害计算
/// </summary>
partial struct MeleeAttackSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    /// <summary>
    /// 系统更新方法，执行近战攻击的主要逻辑
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<RaycastHit> raycastHitList = new NativeList<RaycastHit>(Allocator.Temp);
        
        foreach ((RefRO<LocalTransform> localTransform, RefRW<MeleeAttack> meleeAttack, RefRO<Target> target,
                     RefRW<UnitMover> unitMover) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRW<MeleeAttack>, RefRO<Target>, RefRW<UnitMover>>().WithDisabled<MoveOverride>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            float meleeAttackDistanceSq = 2f;
            bool isCloseEnoughToAttack =
                math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) < meleeAttackDistanceSq;
            
            // 检测是否通过射线碰撞到目标实体
            bool isTouchingTarget = false;
            if (!isCloseEnoughToAttack)
            {
                float3 dirToTarget = targetLocalTransform.Position - localTransform.ValueRO.Position;
                dirToTarget = math.normalize(dirToTarget);
                float distanceExtraToTestRaycast = .4f;
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = localTransform.ValueRO.Position,
                    End = localTransform.ValueRO.Position + dirToTarget * (meleeAttack.ValueRO.colliderSize + distanceExtraToTestRaycast),
                    Filter = CollisionFilter.Default,
                };
                raycastHitList.Clear();
                if (collisionWorld.CastRay(raycastInput, ref raycastHitList))
                {
                    foreach (RaycastHit raycastHit in raycastHitList)
                    {
                        if (raycastHit.Entity == target.ValueRO.targetEntity)
                        {
                            // 射线击中目标，足够接近可以攻击此实体
                            isTouchingTarget = true;
                            break;
                        }
                    }
                }
            }
            
            // 根据与目标的距离决定移动行为或攻击行为
            if (!isCloseEnoughToAttack && !isTouchingTarget)
            {
                // 目标太远，设置移动目标位置
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
            }
            else
            {
                // 目标足够接近可以攻击
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
                
                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if (meleeAttack.ValueRO.timer > 0f)
                {
                    continue;
                }
                meleeAttack.ValueRW.timer = meleeAttack.ValueRO.timerMax;
                
                // 对目标造成伤害
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= meleeAttack.ValueRO.damageAmount;
                targetHealth.ValueRW.onHealthChanged = true;
            }
        }
    }
}
