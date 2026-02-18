using Unity.Entities;
using UnityEngine;

/// <summary>
/// Authoring组件，用于定义目标查找系统的行为参数，并通过Baker转换为ECS组件
/// </summary>
public class FindTargetAuthoring : MonoBehaviour
{
    /// <summary>
    /// 查找目标的有效范围
    /// </summary>
    public float range;
    
    /// <summary>
    /// 目标阵营类型，用于确定要寻找的目标所属的阵营
    /// </summary>
    public Faction targetFaction;
    
    /// <summary>
    /// 计时器最大值，控制目标查找的时间间隔或冷却时间
    /// </summary>
    public float timerMax;
    
    /// <summary>
    /// Baker类，负责将Authoring组件转换为ECS实体组件数据
    /// </summary>
    public class Baker : Baker<FindTargetAuthoring>
    {
        /// <summary>
        /// 将Authoring组件的数据烘焙到ECS实体中
        /// </summary>
        /// <param name="authoring">FindTargetAuthoring组件实例</param>
        public override void Bake(FindTargetAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FindTarget
            {
                range = authoring.range,
                targetFaction = authoring.targetFaction,
                timerMax = authoring.timerMax,
            });
        }
    }
}

/// <summary>
/// ECS组件数据结构，存储目标查找系统所需的状态信息
/// </summary>
public struct FindTarget : IComponentData
{
    /// <summary>
    /// 查找目标的有效范围
    /// </summary>
    public float range;
    
    /// <summary>
    /// 目标阵营类型，用于确定要寻找的目标所属的阵营
    /// </summary>
    public Faction targetFaction;
    
    /// <summary>
    /// 当前计时器值，用于跟踪目标查找的时间进度
    /// </summary>
    public float timer;
    
    /// <summary>
    /// 计时器最大值，控制目标查找的时间间隔或冷却时间
    /// </summary>
    public float timerMax;
}
