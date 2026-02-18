using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

/// <summary>
/// 系统用于检测健康值为零或负数的实体，并销毁这些死亡的实体
/// </summary>
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthDeadTestSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref SystemState">系统状态引用</param>
    /// <returns>void</returns>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }
    
    /// <summary>
    /// 系统更新时执行的主要逻辑
    /// 遍历所有具有Health组件的实体，检查其健康值，销毁健康值小于等于0的实体
    /// </summary>
    /// <param name="ref SystemState">系统状态引用</param>
    /// <returns>void</returns>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取实体命令缓冲区，用于在更新循环中执行实体操作
        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        // 遍历所有具有Health组件的实体
        foreach ((RefRO<Health> health, Entity entity) in SystemAPI.Query<RefRO<Health>>().WithEntityAccess())
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                // 这个实体已经死亡，将其从世界中销毁
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
