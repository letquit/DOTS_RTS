using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 用于在编辑器中配置射击受害者组件的作者化组件
/// </summary>
public class ShootVictimAuthoring : MonoBehaviour
{
    /// <summary>
    /// 用于定义击中位置的局部变换组件
    /// </summary>
    public Transform hitLocalPositionTransform;
    
    /// <summary>
    /// 将ShootVictimAuthoring组件烘焙为ECS实体组件的烘焙器
    /// </summary>
    public class Baker : Baker<ShootVictimAuthoring>
    {
        /// <summary>
        /// 将作者化组件数据烘焙到ECS实体中
        /// </summary>
        /// <param name="authoring">ShootVictimAuthoring类型的作者化组件实例</param>
        public override void Bake(ShootVictimAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootVictim
            {
                hitLocalPosition = authoring.hitLocalPositionTransform.localPosition
            });
        }
    }
}

/// <summary>
/// 表示射击受害者组件的数据结构，实现IComponentData接口以作为ECS组件使用
/// </summary>
public struct ShootVictim : IComponentData
{
    /// <summary>
    /// 击中点的局部坐标位置
    /// </summary>
    public float3 hitLocalPosition;
}
