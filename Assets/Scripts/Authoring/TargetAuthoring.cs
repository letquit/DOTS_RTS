using Unity.Entities;
using UnityEngine;

/// <summary>
/// 目标对象授权组件，用于将游戏对象转换为ECS实体系统中的目标数据
/// </summary>
public class TargetAuthoring : MonoBehaviour
{
    /// <summary>
    /// 目标游戏对象引用
    /// </summary>
    public GameObject targetGameObject;
    
    /// <summary>
    /// 将TargetAuthoring组件烘焙为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<TargetAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为ECS实体和组件数据
        /// </summary>
        /// <param name="authoring">TargetAuthoring授权组件实例</param>
        public override void Bake(TargetAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为目标实体添加Target组件，并设置目标实体引用
            AddComponent(entity, new Target
            {
                targetEntity = GetEntity(authoring.targetGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

/// <summary>
/// 目标实体组件数据结构，存储目标实体的引用
/// </summary>
public struct Target : IComponentData
{
    /// <summary>
    /// 目标实体引用
    /// </summary>
    public Entity targetEntity;
}
