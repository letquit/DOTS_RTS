using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 选中视觉效果系统，用于处理实体选中和取消选中时的视觉变换效果
/// </summary>
/// <remarks>
/// 该系统在LateSimulationSystemGroup中更新，并在ResetEventsSystem之前执行
/// </remarks>
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ResetEventsSystem))]
partial struct SelectedVisualSystem : ISystem
{
    /// <summary>
    /// 更新系统逻辑，处理选中状态变化时的视觉效果
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 遍历所有具有Selected组件的实体
        foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {
            // 检查是否触发了取消选中事件
            if (selected.ValueRO.onDeselected)
            {
                // 获取视觉实体的局部变换组件并将其缩放设置为0（隐藏效果）
                RefRW<LocalTransform> visualLocalTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                visualLocalTransform.ValueRW.Scale = 0f;
            }
            
            // 检查是否触发了选中事件
            if (selected.ValueRO.onSelected)
            {
                // 获取视觉实体的局部变换组件并将其缩放设置为指定的显示比例
                RefRW<LocalTransform> visualLocalTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                visualLocalTransform.ValueRW.Scale = selected.ValueRO.showScale;
            }
        }
    }
}
