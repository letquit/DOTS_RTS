using Unity.Burst;
using Unity.Entities;

/// <summary>
/// 重置事件系统，用于在每一帧结束时重置各种组件中的事件标志位
/// </summary>
/// <remarks>
/// 该系统运行在LateSimulationSystemGroup中，确保在所有其他系统处理完事件后执行
/// </remarks>
// [UpdateBefore(typeof(TestingSystem))]
[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
partial struct ResetEventsSystem : ISystem
{
    /// <summary>
    /// 更新方法，在每一帧调用以重置所有事件标志位
    /// </summary>
    /// <param name="state">系统状态引用，提供对ECS框架的访问</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 重置Selected组件中的选择/取消选择事件标志
        foreach (RefRW<Selected> selected in SystemAPI.Query<RefRW<Selected>>().WithPresent<Selected>())
        {
            selected.ValueRW.onSelected = false;
            selected.ValueRW.onDeselected = false;
        }
        
        // 重置Health组件中的生命值变化事件标志
        foreach (RefRW<Health> health in SystemAPI.Query<RefRW<Health>>())
        {
            health.ValueRW.onHealthChanged = false;
        }
        
        // 重置ShootAttack组件中的射击触发标志
        foreach (RefRW<ShootAttack> shootAttack in SystemAPI.Query<RefRW<ShootAttack>>())
        {
            shootAttack.ValueRW.onShoot.isTriggered = false;
        }
    }
}
