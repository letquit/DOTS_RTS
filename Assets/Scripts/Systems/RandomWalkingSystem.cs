using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 随机行走系统，处理具有随机行走行为的实体的移动逻辑
/// </summary>
partial struct RandomWalkingSystem : ISystem
{
    /// <summary>
    /// 更新系统，在每一帧执行随机行走逻辑
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 遍历所有具有随机行走、单位移动器和本地变换组件的实体
        foreach ((RefRW<RandomWalking> randomWalking, RefRW<UnitMover> unitMover, RefRO<LocalTransform> localTransform)
                 in SystemAPI.Query<RefRW<RandomWalking>, RefRW<UnitMover>, RefRO<LocalTransform>>())
        {
            // 检查当前实体位置是否已到达目标位置
            if (math.distancesq(localTransform.ValueRO.Position, randomWalking.ValueRO.targetPosition) <
                UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ)
            {
                // 已到达目标位置，生成新的随机目标位置
                Random random = randomWalking.ValueRO.random;
                
                float3 randomDirection = new float3(random.NextFloat(-1f, +1f), 0, random.NextFloat(-1f, +1f));
                randomDirection = math.normalize(randomDirection);

                randomWalking.ValueRW.targetPosition = randomWalking.ValueRO.originPosition + randomDirection *
                    random.NextFloat(randomWalking.ValueRO.distanceMin, randomWalking.ValueRO.distanceMax);
                
                randomWalking.ValueRW.random = random;
            }
            else
            {
                // 距离目标位置还很远，向目标位置移动
                unitMover.ValueRW.targetPosition = randomWalking.ValueRO.targetPosition;
            }
        }
    }
}
