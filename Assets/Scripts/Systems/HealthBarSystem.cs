using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 健康条系统，负责更新健康条的位置、旋转和缩放状态
/// </summary>
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{
    // [BurstCompile]
    
    /// <summary>
    /// 更新系统逻辑
    /// </summary>
    /// <param name="state">系统状态引用</param>
    public void OnUpdate(ref SystemState state)
    {
        Vector3 cameraForward = Vector3.zero;
        if (Camera.main != null)
        {
            cameraForward = Camera.main.transform.forward;
        }

        // 遍历所有具有LocalTransform和HealthBar组件的实体
        foreach ((RefRW<LocalTransform> localTransform, RefRO<HealthBar> healthBar) in SystemAPI
                     .Query<RefRW<LocalTransform>, RefRO<HealthBar>>())
        {
            LocalTransform parentLocalTransform =
                SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);
            if (localTransform.ValueRO.Scale == 1f)
            {
                // 当健康条可见时，将其朝向相机方向
                localTransform.ValueRW.Rotation =
                    parentLocalTransform.InverseTransformRotation(quaternion.LookRotation(cameraForward, math.up()));
            }
            
            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);

            if (!health.onHealthChanged)
            {
                continue;
            }
            
            float healthNormalized = (float)health.healthAmount / health.healthAmountMax;

            if (healthNormalized == 1f)
            {
                // 当生命值满时隐藏健康条
                localTransform.ValueRW.Scale = 0f;
            }
            else
            {
                // 当生命值未满时显示健康条
                localTransform.ValueRW.Scale = 1f;
            }
            
            // 更新健康条视觉元素的变换矩阵以反映当前生命值比例
            RefRW<PostTransformMatrix> barVisualPostTransformMatrix =
                SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.barVisualEntity);
            barVisualPostTransformMatrix.ValueRW.Value = float4x4.Scale(healthNormalized, 1, 1);
        }
    }
}
