using Unity.Entities;
using UnityEngine;

/// <summary>
/// 近战攻击组件的授权MonoBehaviour类，用于在编辑器中配置近战攻击参数
/// </summary>
public class MeleeAttackAuthoring : MonoBehaviour
{
    /// <summary>
    /// 攻击计时器的最大值
    /// </summary>
    public float timerMax;
    
    /// <summary>
    /// 攻击造成的伤害数值
    /// </summary>
    public int damageAmount;
    
    /// <summary>
    /// 碰撞体的大小
    /// </summary>
    public float colliderSize;
    
    /// <summary>
    /// 将MeleeAttackAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<MeleeAttackAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为ECS实体组件
        /// </summary>
        /// <param name="authoring">MeleeAttackAuthoring授权组件实例</param>
        public override void Bake(MeleeAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MeleeAttack
            {
                timerMax = authoring.timerMax,
                damageAmount = authoring.damageAmount,
                colliderSize = authoring.colliderSize,
            });
        }
    }
}

/// <summary>
/// 近战攻击组件数据结构，实现IComponentData接口用于ECS系统
/// </summary>
public struct MeleeAttack : IComponentData
{
    /// <summary>
    /// 当前攻击计时器值
    /// </summary>
    public float timer;
    
    /// <summary>
    /// 攻击计时器的最大值
    /// </summary>
    public float timerMax;
    
    /// <summary>
    /// 攻击造成的伤害数值
    /// </summary>
    public int damageAmount;
    
    /// <summary>
    /// 碰撞体的大小
    /// </summary>
    public float colliderSize;
}
