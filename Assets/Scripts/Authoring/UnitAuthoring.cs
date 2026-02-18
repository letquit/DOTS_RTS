using Unity.Entities;
using UnityEngine;

/// <summary>
/// 单位组件的授权类，用于在Unity编辑器中配置单位数据并将其转换为ECS实体组件
/// </summary>
public class UnitAuthoring : MonoBehaviour
{
    /// <summary>
    /// 单位所属的阵营
    /// </summary>
    public Faction faction;
    
    /// <summary>
    /// 将UnitAuthoring组件烘焙为ECS实体的转换器类
    /// </summary>
    public class Baker : Baker<UnitAuthoring>
    {
        /// <summary>
        /// 将授权组件的数据烘焙到ECS实体系统中
        /// </summary>
        /// <param name="authoring">UnitAuthoring类型的授权组件实例</param>
        public override void Bake(UnitAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Unit
            {
                faction = authoring.faction,
            });
        }
    }
}

/// <summary>
/// 表示单位实体的ECS组件数据结构
/// </summary>
public struct Unit : IComponentData
{
    /// <summary>
    /// 单位所属的阵营
    /// </summary>
    public Faction faction;
}
