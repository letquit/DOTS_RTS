using Unity.Entities;
using UnityEngine;

/// <summary>
/// 子弹组件的授权类，用于在Unity编辑器中配置子弹属性并将其转换为ECS实体组件
/// </summary>
public class BulletAuthoring : MonoBehaviour
{
    /// <summary>
    /// 子弹移动速度
    /// </summary>
    public float speed;
    
    /// <summary>
    /// 子弹伤害数值
    /// </summary>
    public int damageAmount;
    
    /// <summary>
    /// 子弹授权烘焙器，负责将BulletAuthoring组件数据转换为ECS实体组件
    /// </summary>
    public class Baker : Baker<BulletAuthoring>
    {
        /// <summary>
        /// 将授权组件的数据烘焙到ECS实体中
        /// </summary>
        /// <param name="authoring">子弹授权组件实例</param>
        public override void Bake(BulletAuthoring authoring)
        {
            // 创建动态实体，用于运行时变换操作
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加子弹组件，并复制授权组件中的数据
            AddComponent(entity, new Bullet
            {
                speed = authoring.speed,
                damageAmount = authoring.damageAmount,
            });
        }
    }
}

/// <summary>
/// 子弹组件数据结构，实现IComponentData接口以作为ECS组件使用
/// </summary>
public struct Bullet : IComponentData
{
    /// <summary>
    /// 子弹移动速度
    /// </summary>
    public float speed;
    
    /// <summary>
    /// 子弹伤害数值
    /// </summary>
    public int damageAmount;
}
