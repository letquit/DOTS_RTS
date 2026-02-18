using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

/// <summary>
/// 随机行走组件的授权类，用于在Unity编辑器中配置随机行走相关参数并转换为ECS实体组件
/// </summary>
public class RandomWalkingAuthoring : MonoBehaviour
{
    /// <summary>
    /// 目标位置，用于随机行走的目标点
    /// </summary>
    public float3 targetPosition;
    
    /// <summary>
    /// 起始位置，用于随机行走的原点
    /// </summary>
    public float3 originPosition;
    
    /// <summary>
    /// 最小距离，限制随机行走的最小范围
    /// </summary>
    public float distanceMin;
    
    /// <summary>
    /// 最大距离，限制随机行走的最大范围
    /// </summary>
    public float distanceMax;
    
    /// <summary>
    /// 随机种子，用于生成可重现的随机数序列
    /// </summary>
    public uint randomSeed;
    
    /// <summary>
    /// 将授权类数据烘焙为ECS实体组件的烘焙器
    /// </summary>
    public class Baker : Baker<RandomWalkingAuthoring>
    {
        /// <summary>
        /// 将RandomWalkingAuthoring组件的数据烘焙为ECS实体和RandomWalking组件
        /// </summary>
        /// <param name="authoring">RandomWalkingAuthoring授权组件实例</param>
        public override void Bake(RandomWalkingAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加RandomWalking组件并初始化其属性
            AddComponent(entity, new RandomWalking
            {
                targetPosition = authoring.targetPosition,
                originPosition = authoring.originPosition,
                distanceMin = authoring.distanceMin,
                distanceMax = authoring.distanceMax,
                random = new Random(authoring.randomSeed),
            });
        }
    }
}

/// <summary>
/// 随机行走组件数据结构，实现IComponentData接口用于ECS系统
/// </summary>
public struct RandomWalking : IComponentData
{
    /// <summary>
    /// 目标位置，用于随机行走的目标点
    /// </summary>
    public float3 targetPosition;
    
    /// <summary>
    /// 起始位置，用于随机行走的原点
    /// </summary>
    public float3 originPosition;
    
    /// <summary>
    /// 最小距离，限制随机行走的最小范围
    /// </summary>
    public float distanceMin;
    
    /// <summary>
    /// 最大距离，限制随机行走的最大范围
    /// </summary>
    public float distanceMax;
    
    /// <summary>
    /// 随机数生成器，用于产生随机行走相关的随机数值
    /// </summary>
    public Random random;
}
