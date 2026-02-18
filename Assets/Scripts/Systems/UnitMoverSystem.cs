using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 单位移动系统，负责处理单位的移动逻辑
/// </summary>
partial struct UnitMoverSystem : ISystem
{
    /// <summary>
    /// 到达目标位置的距离阈值（平方值）
    /// </summary>
    public const float REACHED_TARGET_POSITION_DISTANCE_SQ = 2f;
    
    /// <summary>
    /// 更新系统，在每一帧调用
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJob unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        // unitMoverJob.Run();
        
        // ScheduleParallel() 将根据匹配的实体数量自动将工作拆分为多个并行 Job 分片（slices），
        // 每个分片在独立线程上执行，且仅处理其分配到的实体子集。
        //
        // 注意：并行度受限于实体总数。
        //   - 若实体数量较少（例如少于 Job Worker 线程数 × 最小分片大小），则可能仅生成 1 个分片，
        //     导致整个 Job 在单线程中顺序执行——即使逻辑复杂，也无法利用多核。
        //   - 因此，对于「实体少但计算密集」的场景，建议手动将逻辑拆分为多个独立 Job（如分阶段处理），
        //     以主动提升并发性，而非依赖自动分片。
        unitMoverJob.ScheduleParallel();
        
        /*
        foreach ((RefRW<LocalTransform> localTransform, RefRO<UnitMover> unitMover,
                     RefRW<PhysicsVelocity> physicsVelocity) in SystemAPI
                     .Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {
            // float3 targetPosition = MouseWorldPosition.Instance.GetPosition();
            // float3 moveDirection =  targetPosition - localTransform.ValueRO.Position;
            float3 moveDirection =  unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);
            
            // localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,
                quaternion.LookRotation(moveDirection, math.up()),
                SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);
            
            physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.moveSpeed;
            physicsVelocity.ValueRW.Angular = float3.zero;
            // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }
        */
        
    }
}

/// <summary>
/// 单位移动作业，用于并行处理单位移动逻辑
/// </summary>
[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    /// <summary>
    /// 时间增量，用于平滑动画和物理计算
    /// </summary>
    public float deltaTime;
    
    /// <summary>
    /// 执行单位移动逻辑
    /// </summary>
    /// <param name="localTransform">本地变换组件引用</param>
    /// <param name="unitMover">单位移动组件（只读）</param>
    /// <param name="physicsVelocity">物理速度组件引用</param>
    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection =  unitMover.targetPosition - localTransform.Position;

        float reachedTargetDistanceSq = UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ;
        if (math.lengthsq(moveDirection) <= reachedTargetDistanceSq)
        {
            // Reached the target position
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }
        
        moveDirection = math.normalize(moveDirection);
            
        localTransform.Rotation = math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirection, math.up()),
            deltaTime * unitMover.rotationSpeed);
            
        physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
        physicsVelocity.Angular = float3.zero;
    }
}
