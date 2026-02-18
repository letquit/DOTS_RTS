using Unity.Entities;
using UnityEngine;

/// <summary>
/// 负责将SetupUnitMoverDefaultPosition组件添加到实体中的作者化MonoBehaviour类
/// </summary>
public class SetupUnitMoverDefaultPositionAuthoring : MonoBehaviour
{
    /// <summary>
    /// 将SetupUnitMoverDefaultPositionAuthoring转换为ECS实体的烘焙器
    /// </summary>
    public class Baker : Baker<SetupUnitMoverDefaultPositionAuthoring>
    {
        /// <summary>
        /// 将作者化组件烘焙为ECS实体和组件数据
        /// </summary>
        /// <param name="authoring">要烘焙的SetupUnitMoverDefaultPositionAuthoring实例</param>
        public override void Bake(SetupUnitMoverDefaultPositionAuthoring authoring)
        {
            // 创建具有动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            // 为实体添加SetupUnitMoverDefaultPosition组件
            AddComponent(entity, new SetupUnitMoverDefaultPosition
            {
                
            });
        }
    }
}

/// <summary>
/// 标记组件，用于标识需要设置默认位置的单位移动器实体
/// </summary>
public struct SetupUnitMoverDefaultPosition : IComponentData
{
    
}
