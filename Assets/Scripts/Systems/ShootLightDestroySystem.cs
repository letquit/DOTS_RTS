using Unity.Burst;
using Unity.Entities;

/// <summary>
/// 射击光效销毁系统，负责管理ShootLight组件的生命周期，当计时器归零时自动销毁对应的实体
/// </summary>
partial struct ShootLightDestroySystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // 要求更新EndSimulationEntityCommandBufferSystem单例系统
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    /// <summary>
    /// 系统每帧更新时执行的方法
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取实体命令缓冲区，用于在模拟结束时执行实体销毁操作
        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        // 遍历所有带有ShootLight组件的实体
        foreach ((RefRW<ShootLight> shootLight, Entity entity) in SystemAPI.Query<RefRW<ShootLight>>().WithEntityAccess())
        {
            // 更新射击光效的计时器
            shootLight.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            
            // 检查计时器是否已过期
            if (shootLight.ValueRO.timer < 0f)
            {
                // 将过期的实体加入销毁队列
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
