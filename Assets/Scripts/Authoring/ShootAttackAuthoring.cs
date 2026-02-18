using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 射击攻击组件的授权MonoBehaviour类，用于在Unity编辑器中配置射击攻击参数
/// </summary>
public class ShootAttackAuthoring : MonoBehaviour
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
    /// 攻击的有效距离
    /// </summary>
    public float attackDistance;
    
    /// <summary>
    /// 子弹发射位置的变换组件
    /// </summary>
    public Transform bulletSpawnPositionTransform;
    
    /// <summary>
    /// 将ShootAttackAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<ShootAttackAuthoring>
    {
        /// <summary>
        /// 将授权组件烘焙为ECS实体组件
        /// </summary>
        /// <param name="authoring">ShootAttackAuthoring授权组件实例</param>
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootAttack
            {
                timerMax = authoring.timerMax,
                damageAmount = authoring.damageAmount,
                attackDistance = authoring.attackDistance,
                bulletSpawnLocalPosition = authoring.bulletSpawnPositionTransform.localPosition,
            });
        }
    }
}

/// <summary>
/// 射击攻击的数据组件结构体，实现IComponentData接口用于ECS系统
/// </summary>
public struct ShootAttack : IComponentData
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
    /// 攻击的有效距离
    /// </summary>
    public float attackDistance;
    
    /// <summary>
    /// 子弹发射点的本地坐标位置
    /// </summary>
    public float3 bulletSpawnLocalPosition;
    
    /// <summary>
    /// 射击事件数据
    /// </summary>
    public OnShootEvent onShoot;
    
    /// <summary>
    /// 射击事件的内部结构体
    /// </summary>
    public struct OnShootEvent
    {
        /// <summary>
        /// 射击事件是否已被触发
        /// </summary>
        public bool isTriggered;
        
        /// <summary>
        /// 射击发起的位置坐标
        /// </summary>
        public float3 shootFromPosition;
    }
}
