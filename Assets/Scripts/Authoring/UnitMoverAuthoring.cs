using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 负责将Unity GameObject中的移动组件数据转换为ECS实体组件的授权类
/// </summary>
public class UnitMoverAuthoring : MonoBehaviour
{
    /// <summary>
    /// 单位移动速度
    /// </summary>
    public float moveSpeed;
    
    /// <summary>
    /// 单位旋转速度
    /// </summary>
    public float rotationSpeed;

    /// <summary>
    /// 将授权组件数据烘焙为ECS实体组件的烘焙器
    /// </summary>
    public class Baker : Baker<UnitMoverAuthoring>
    {
        /// <summary>
        /// 将授权组件的数据烘焙到ECS实体中
        /// </summary>
        /// <param name="authoring">UnitMoverAuthoring类型的授权组件实例</param>
        public override void Bake(UnitMoverAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加UnitMover组件并赋值
            AddComponent(entity, new UnitMover
            {
                moveSpeed = authoring.moveSpeed,
                rotationSpeed = authoring.rotationSpeed
            });
        }
    }
}

/// <summary>
/// 表示单位移动行为的ECS组件数据结构
/// 包含移动速度、旋转速度和目标位置信息
/// </summary>
public struct UnitMover : IComponentData
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
    
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotationSpeed;
    
    /// <summary>
    /// 目标位置
    /// </summary>
    public float3 targetPosition;
}

