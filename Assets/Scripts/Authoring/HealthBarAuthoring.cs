using Unity.Entities;
using UnityEngine;

/// <summary>
/// 健康条授权组件，用于在编辑器中配置健康条的可视化对象和健康对象引用
/// </summary>
public class HealthBarAuthoring : MonoBehaviour
{
    /// <summary>
    /// 健康条可视化游戏对象
    /// </summary>
    public GameObject barVisualGameObject;
    
    /// <summary>
    /// 健康相关游戏对象
    /// </summary>
    public GameObject healthGameObject;
    
    /// <summary>
    /// 将HealthBarAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<HealthBarAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为ECS实体和组件数据
        /// </summary>
        /// <param name="authoring">HealthBarAuthoring授权组件实例</param>
        public override void Bake(HealthBarAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加HealthBar组件数据
            AddComponent(entity, new HealthBar
            {
                barVisualEntity = GetEntity(authoring.barVisualGameObject, TransformUsageFlags.NonUniformScale),
                healthEntity = GetEntity(authoring.healthGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

/// <summary>
/// 健康条组件数据结构，存储健康条可视化实体和健康实体的引用
/// </summary>
public struct HealthBar : IComponentData
{
    /// <summary>
    /// 健康条可视化实体
    /// </summary>
    public Entity barVisualEntity;
    
    /// <summary>
    /// 健康实体
    /// </summary>
    public Entity healthEntity;
}
