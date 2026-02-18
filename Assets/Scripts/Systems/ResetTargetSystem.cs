using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// 重置目标系统，用于清理无效的目标实体引用
/// 该系统在SimulationSystemGroup中优先执行，确保目标引用的有效性
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
partial struct ResetTargetSystem : ISystem
{
    /// <summary>
    /// 系统更新方法，在每次系统更新时检查并清理无效的目标实体引用
    /// </summary>
    /// <param name="state">系统状态引用，提供对ECS系统的访问接口</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 遍历所有具有Target组件的实体
        foreach (RefRW<Target> target in SystemAPI.Query<RefRW<Target>>())
        {
            // 检查当前目标实体是否有效
            if (target.ValueRW.targetEntity != Entity.Null)
            {
                // 验证目标实体是否存在且具有LocalTransform组件
                // 如果目标实体不存在或缺少LocalTransform组件，则将目标实体设置为Null
                if (!SystemAPI.Exists(target.ValueRO.targetEntity) ||
                    !SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity))
                {
                    target.ValueRW.targetEntity = Entity.Null;
                }
            }
        }
    }
}
