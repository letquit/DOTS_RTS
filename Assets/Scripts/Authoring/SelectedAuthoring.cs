using Unity.Entities;
using UnityEngine;

/// <summary>
/// Authoring组件，用于定义选中状态的相关配置数据
/// </summary>
public class SelectedAuthoring : MonoBehaviour
{
    /// <summary>
    /// 可视化游戏对象，用于显示选中效果
    /// </summary>
    public GameObject visualGameObject;
    
    /// <summary>
    /// 显示缩放比例
    /// </summary>
    public float showScale;
    
    /// <summary>
    /// Baker类，负责将Authoring组件转换为ECS实体组件
    /// </summary>
    public class SelectedAuthoringBaker : Baker<SelectedAuthoring>
    {
        /// <summary>
        /// 将Authoring数据烘焙为ECS组件
        /// </summary>
        /// <param name="authoring">源Authoring组件实例</param>
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Selected
            {
                visualEntity = GetEntity(authoring.visualGameObject, TransformUsageFlags.Dynamic),
                showScale = authoring.showScale,
            });
            SetComponentEnabled<Selected>(entity, false);
        }
    }
}

/// <summary>
/// 选中状态组件，实现IComponentData和IEnableableComponent接口
/// </summary>
public struct Selected : IComponentData, IEnableableComponent
{
    /// <summary>
    /// 可视化实体引用
    /// </summary>
    public Entity visualEntity;
    
    /// <summary>
    /// 显示缩放比例
    /// </summary>
    public float showScale;

    /// <summary>
    /// 选中事件标志
    /// </summary>
    public bool onSelected;
    
    /// <summary>
    /// 取消选中事件标志
    /// </summary>
    public bool onDeselected;
}

