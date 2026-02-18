using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 移动覆盖系统，处理单位的移动覆盖逻辑
/// </summary>
partial struct MoveOverrideSystem : ISystem
{
    /// <summary>
     /// 更新系统，在每一帧执行移动覆盖逻辑
     /// </summary>
     /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 遍历所有具有LocalTransform、MoveOverride、UnitMover组件的实体
        foreach ((RefRO<LocalTransform> localTransform, RefRO<MoveOverride> moveOverride,
                     EnabledRefRW<MoveOverride> moveOverrideEnabled, RefRW<UnitMover> unitMover) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRO<MoveOverride>, EnabledRefRW<MoveOverride>, RefRW<UnitMover>>())
        {
            // 检查当前单位位置与目标位置的距离是否大于阈值
            if (math.distancesq(localTransform.ValueRO.Position, moveOverride.ValueRO.targetPosition) >
                UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ)
            {
                // 移动到目标位置
                unitMover.ValueRW.targetPosition = moveOverride.ValueRO.targetPosition;
            }
            else
            {
                // 已到达移动覆盖位置，禁用移动覆盖组件
                moveOverrideEnabled.ValueRW = false;
            }
        }
    }
}
