using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 射击光效生成系统，负责在射击发生时创建光效实体
/// </summary>
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ShootLightSpawnerSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
    }

    /// <summary>
    /// 系统更新方法，处理射击光效的生成逻辑
    /// </summary>
    /// <param name="ref">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取实体引用配置
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        
        // 遍历所有射击攻击组件
        foreach (RefRO<ShootAttack> shootAttack in SystemAPI.Query<RefRO<ShootAttack>>())
        {
            // 检查射击是否被触发
            if (shootAttack.ValueRO.onShoot.isTriggered)
            {
                // 实例化射击光效预制体
                Entity shootLightEntity = state.EntityManager.Instantiate(entitiesReferences.shootLightPrefabEntity);
                
                // 设置光效实体的位置变换
                SystemAPI.SetComponent(shootLightEntity,
                    LocalTransform.FromPosition(shootAttack.ValueRO.onShoot.shootFromPosition));
            }
        }
    }
}
