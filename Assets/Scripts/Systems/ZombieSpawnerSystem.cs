using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 僵尸生成系统，负责根据僵尸生成器组件定时生成僵尸实体
/// </summary>
partial struct ZombieSpawnerSystem : ISystem
{
    private Random random;
    
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<EntitiesReferences>();
        random = new Random((uint)System.DateTime.Now.Ticks);
    }

    /// <summary>
    /// 系统更新方法，处理僵尸生成逻辑
    /// </summary>
    /// <param name="ref">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        // 遍历所有具有LocalTransform和ZombieSpawner组件的实体
        foreach ((RefRO<LocalTransform> localTransform, RefRW<ZombieSpawner> zombieSpawner) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRW<ZombieSpawner>>())
        {
            zombieSpawner.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (zombieSpawner.ValueRO.timer > 0f)
            {
                continue;
            }
            zombieSpawner.ValueRW.timer = zombieSpawner.ValueRO.timerMax;

            // 实例化僵尸预制体并设置位置
            Entity zombieEntity = state.EntityManager.Instantiate(entitiesReferences.zombiePrefabEntity);
            SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
            
            uint zombieSeed = random.NextUInt();
            
            // 为新生成的僵尸添加随机行走组件
            entityCommandBuffer.AddComponent(zombieEntity, new RandomWalking
            {
                originPosition = localTransform.ValueRO.Position,
                targetPosition = localTransform.ValueRO.Position,
                distanceMin = zombieSpawner.ValueRO.randomWalkingDistanceMin,
                distanceMax = zombieSpawner.ValueRO.randomWalkingDistanceMax,
                // random = new Random((uint)zombieEntity.Index),
                random = new Random(zombieSeed),
            });
        }
    }
}
