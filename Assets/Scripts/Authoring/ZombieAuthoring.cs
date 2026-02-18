using Unity.Entities;
using UnityEngine;

/// <summary>
/// 负责将游戏对象转换为实体的授权组件，用于定义僵尸实体的数据结构
/// </summary>
public class ZombieAuthoring : MonoBehaviour
{
    /// <summary>
    /// 将ZombieAuthoring组件烘焙为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<ZombieAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为实体，创建对应的Zombie组件数据
        /// </summary>
        /// <param name="authoring">ZombieAuthoring授权组件实例</param>
        public override void Bake(ZombieAuthoring authoring)
        {
            // 创建具有动态变换使用标志的新实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            // 为实体添加Zombie组件数据
            AddComponent(entity, new Zombie());
        }
    }
}

/// <summary>
/// 表示僵尸实体的组件数据结构，实现IComponentData接口以作为ECS组件使用
/// </summary>
public struct Zombie : IComponentData
{
    
}
