using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// 设置单位移动器默认位置系统，用于初始化UnitMover组件的目标位置并移除设置标记组件
/// </summary>
partial struct SetupUnitMoverDefaultPositionSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref SystemState state">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    /// <summary>
    /// 系统更新方法，遍历具有LocalTransform、UnitMover和SetupUnitMoverDefaultPosition组件的实体，
    /// 将LocalTransform的位置设置为UnitMover的目标位置，并移除SetupUnitMoverDefaultPosition组件
    /// </summary>
    /// <param name="ref SystemState state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取实体命令缓冲区用于在更新循环中执行命令
        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        // 遍历所有具有LocalTransform、UnitMover和SetupUnitMoverDefaultPosition组件的实体
        foreach ((RefRO<LocalTransform> localTransform, RefRW<UnitMover> unitMover,
                     RefRO<SetupUnitMoverDefaultPosition> setupUnitMoverDefaultPosition, Entity entity) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRW<UnitMover>, RefRO<SetupUnitMoverDefaultPosition>>().WithEntityAccess())
        {
            // 将当前变换位置设置为移动器的目标位置
            unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
            // 移除设置完成的标记组件
            entityCommandBuffer.RemoveComponent<SetupUnitMoverDefaultPosition>(entity);
            
            UnityEngine.Debug.Log("Setup");
        }
    }
}
