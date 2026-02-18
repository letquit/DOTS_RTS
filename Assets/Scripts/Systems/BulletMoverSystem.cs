using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 子弹移动系统，负责处理子弹向目标移动、碰撞检测和伤害计算的逻辑
/// </summary>
partial struct BulletMoverSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    /// <summary>
    /// 系统更新方法，处理所有子弹的移动、碰撞检测和伤害应用
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRW<LocalTransform> localTransform, RefRO<Bullet> bullet, RefRO<Target> target, Entity entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Target>>().WithEntityAccess())
        {
            // 检查目标实体是否有效，无效则销毁子弹
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                entityCommandBuffer.DestroyEntity(entity);
                continue;
            }
            
            // 获取目标位置信息
            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            ShootVictim targetShootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetPosition = targetLocalTransform.TransformPoint(targetShootVictim.hitLocalPosition);

            float distanceBeforeSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);
            
            // 计算移动方向并标准化
            float3 moveDirection = targetPosition - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);
            
            // 更新子弹位置
            localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.speed * SystemAPI.Time.DeltaTime;

            float distanceAfterSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);

            // 检查是否超过目标位置，如果超过则将子弹定位到目标位置
            if (distanceAfterSq > distanceBeforeSq)
            {
                // Overshot
                localTransform.ValueRW.Position = targetPosition;
            }
            
            // 检查子弹是否到达目标范围内，如果是则造成伤害并销毁子弹
            float destroyDistanceSq = .2f;
            if (math.distancesq(localTransform.ValueRO.Position, targetPosition) < destroyDistanceSq)
            {
                // Close enough to damage target
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;
                targetHealth.ValueRW.onHealthChanged = true;
                
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
