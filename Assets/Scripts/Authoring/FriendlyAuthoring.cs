using Unity.Entities;
using UnityEngine;

/// <summary>
/// 友方实体的授权组件，用于在转换为ECS实体时定义友方属性
/// </summary>
public class FriendlyAuthoring : MonoBehaviour
{
    /// <summary>
    /// 将FriendlyAuthoring组件烘焙为ECS实体的转换器
    /// </summary>
    public class Baker : Baker<FriendlyAuthoring>
    {
        /// <summary>
        /// 将授权组件转换为ECS实体和组件数据
        /// </summary>
        /// <param name="authoring">FriendlyAuthoring授权组件实例</param>
        public override void Bake(FriendlyAuthoring authoring)
        {
            // 创建具有动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            // 为实体添加Friendly组件数据
            AddComponent(entity, new Friendly());
        }
    }
}

/// <summary>
/// 标记实体为友方的组件数据结构
/// </summary>
public struct Friendly : IComponentData
{
    
}
