using Unity.Entities;
using UnityEngine;

/// <summary>
/// Authoring组件，用于在编辑器中配置射击灯光效果的数据，并将其转换为ECS实体组件
/// </summary>
public class ShootLightAuthoring : MonoBehaviour
{
    /// <summary>
    /// 射击灯光效果的计时器值
    /// </summary>
    public float timer;
    
    /// <summary>
    /// 将ShootLightAuthoring组件烘焙为ECS实体组件的Baker类
    /// </summary>
    public class Baker : Baker<ShootLightAuthoring>
    {
        /// <summary>
        /// 将Authoring组件数据转换并添加到ECS实体中
        /// </summary>
        /// <param name="authoring">ShootLightAuthoring组件实例，包含要转换的数据</param>
        public override void Bake(ShootLightAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加ShootLight组件并初始化其数据
            AddComponent<ShootLight>(entity, new ShootLight
            {
                timer = authoring.timer,
            });
        }
    }
}

/// <summary>
/// ECS组件数据结构，存储射击灯光效果的计时器信息
/// </summary>
public struct ShootLight : IComponentData
{
    /// <summary>
    /// 射击灯光效果的计时器值
    /// </summary>
    public float timer;
}
