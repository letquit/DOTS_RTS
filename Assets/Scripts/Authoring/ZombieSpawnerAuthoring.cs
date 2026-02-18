using Unity.Entities;
using UnityEngine;

/// <summary>
/// 负责定义僵尸生成器的编辑器配置数据，用于在Unity编辑器中设置僵尸生成的相关参数
/// </summary>
public class ZombieSpawnerAuthoring : MonoBehaviour
{
    /// <summary>
    /// 僵尸生成的时间间隔最大值
    /// </summary>
    public float timerMax;
    
    /// <summary>
    /// 随机行走距离的最小值
    /// </summary>
    public float randomWalkingDistanceMin;
    
    /// <summary>
    /// 随机行走距离的最大值
    /// </summary>
    public float randomWalkingDistanceMax;
    
    /// <summary>
    /// 将ZombieSpawnerAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<ZombieSpawnerAuthoring>
    {
        /// <summary>
        /// 将Authoring组件烘焙为ECS实体和组件数据
        /// </summary>
        /// <param name="authoring">ZombieSpawnerAuthoring组件实例，包含配置数据</param>
        public override void Bake(ZombieSpawnerAuthoring authoring)
        {
            // 创建动态实体，允许运行时变换操作
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加ZombieSpawner组件，并将Authoring中的配置数据复制到组件中
            AddComponent(entity, new ZombieSpawner
            {
                timerMax = authoring.timerMax,
                randomWalkingDistanceMin = authoring.randomWalkingDistanceMin,
                randomWalkingDistanceMax = authoring.randomWalkingDistanceMax,
            });
        }
    }
}

/// <summary>
/// 表示僵尸生成器的ECS组件数据结构，包含生成定时器和随机行走距离参数
/// </summary>
public struct ZombieSpawner : IComponentData
{
    /// <summary>
    /// 当前计时器值，用于控制僵尸生成的时间间隔
    /// </summary>
    public float timer;
    
    /// <summary>
    /// 僵尸生成的时间间隔最大值
    /// </summary>
    public float timerMax;
    
    /// <summary>
    /// 随机行走距离的最小值
    /// </summary>
    public float randomWalkingDistanceMin;
    
    /// <summary>
    /// 随机行走距离的最大值
    /// </summary>
    public float randomWalkingDistanceMax;
}
