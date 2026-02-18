using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Authoring组件，用于在编辑器中配置移动覆盖设置，并通过Baker转换为运行时实体组件
/// </summary>
public class MoveOverrideAuthoring : MonoBehaviour
{
    /// <summary>
    /// 将MoveOverrideAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<MoveOverrideAuthoring>
    {
        /// <summary>
        /// 将Authoring组件烘焙为ECS实体和组件
        /// </summary>
        /// <param name="authoring">MoveOverrideAuthoring组件实例</param>
        public override void Bake(MoveOverrideAuthoring authoring)
        {
            // 创建动态实体并添加移动覆盖组件，初始状态为禁用
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveOverride());
            SetComponentEnabled<MoveOverride>(entity, false);
        }
    }
}

/// <summary>
/// 移动覆盖组件数据，实现可启用/禁用的组件接口
/// </summary>
public struct MoveOverride : IComponentData, IEnableableComponent
{
    /// <summary>
    /// 目标位置，用于指定实体需要移动到的位置
    /// </summary>
    public float3 targetPosition;
}
