using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 射击攻击系统，处理单位的射击逻辑，包括移动到目标位置、瞄准和发射子弹
/// </summary>
partial struct ShootAttackSystem : ISystem
{
    /// <summary>
    /// 系统创建时的初始化方法
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
    }

    /// <summary>
    /// 系统更新方法，处理射击单位的移动、瞄准和攻击逻辑
    /// </summary>
    /// <param name="ref state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        // 遍历所有具有射击攻击组件且未被禁用移动覆盖的实体
        foreach ((RefRW<LocalTransform> localTransform, RefRW<ShootAttack> shootAttack, RefRO<Target> target,
                     RefRW<UnitMover> unitMover) in SystemAPI
                     .Query<RefRW<LocalTransform>, RefRW<ShootAttack>, RefRO<Target>, RefRW<UnitMover>>().WithDisabled<MoveOverride>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }
            
            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            // 检查与目标的距离，如果太远则移动靠近
            if (math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position) >
                shootAttack.ValueRO.attackDistance)
            {
                // 太远，移动更近
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
                continue;
            }
            else
            {
                // 距离足够近，停止移动并开始攻击
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
            }

            // 计算瞄准方向并更新旋转
            float3 aimDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
            aimDirection = math.normalize(aimDirection);

            quaternion targetRotation = quaternion.LookRotation(aimDirection, math.up());
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, targetRotation,
                SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);
            
            // 更新射击计时器并检查是否可以射击
            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (shootAttack.ValueRW.timer > 0f)
            {
                continue;
            }
            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;
            
            // 创建子弹实体并设置其属性
            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);
            float3 bulletSpawnWorldPosition = localTransform.ValueRO.TransformPoint(shootAttack.ValueRO.bulletSpawnLocalPosition);
            SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(bulletSpawnWorldPosition));

            RefRW<Bullet> bulletBullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bulletBullet.ValueRW.damageAmount = shootAttack.ValueRO.damageAmount;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;

            // 触发射击事件
            shootAttack.ValueRW.onShoot.isTriggered = true;
            shootAttack.ValueRW.onShoot.shootFromPosition = bulletSpawnWorldPosition;
            
            // 创建射击光效
            Entity shootLightEntity = state.EntityManager.Instantiate(entitiesReferences.shootLightPrefabEntity);
            SystemAPI.SetComponent(shootLightEntity, LocalTransform.FromPosition(bulletSpawnWorldPosition));
        }
    }
}
