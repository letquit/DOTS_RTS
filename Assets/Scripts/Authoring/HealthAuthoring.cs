using Unity.Entities;
using UnityEngine;

/// <summary>
/// 健康值组件的授权类，用于在Unity编辑器中定义健康值相关的属性
/// </summary>
public class HealthAuthoring : MonoBehaviour
{
    /// <summary>
    /// 当前健康值
    /// </summary>
    public int healthAmount;
    
    /// <summary>
    /// 最大健康值
    /// </summary>
    public int healthAmountMax;
    
    /// <summary>
    /// 将HealthAuthoring转换为ECS实体的烘焙器类
    /// </summary>
    public class Baker : Baker<HealthAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为ECS实体组件
        /// </summary>
        /// <param name="authoring">HealthAuthoring授权组件实例</param>
        public override void Bake(HealthAuthoring authoring)
        {
            // 创建动态实体并添加Health组件
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Health
            {
                healthAmount = authoring.healthAmount,
                healthAmountMax = authoring.healthAmountMax,
                onHealthChanged = true,
            });
        }
    }  
}

/// <summary>
/// 健康值组件数据结构，实现IComponentData接口用于ECS系统
/// </summary>
public struct Health : IComponentData
{
    /// <summary>
    /// 当前健康值
    /// </summary>
    public int healthAmount;
    
    /// <summary>
    /// 最大健康值
    /// </summary>
    public int healthAmountMax;
    
    /// <summary>
    /// 健康值是否发生变化的标志
    /// </summary>
    public bool onHealthChanged;
}
