using Unity.Entities;
using UnityEngine;

/// <summary>
/// 负责将游戏对象预制件引用转换为实体引用的作者化组件
/// </summary>
public class EntitiesReferencesAuthoring : MonoBehaviour
{
    /// <summary>
    /// 子弹预制件游戏对象
    /// </summary>
    public GameObject bulletPrefabGameObject;
    
    /// <summary>
    /// 僵尸预制件游戏对象
    /// </summary>
    public GameObject zombiePrefabGameObject;
    
    /// <summary>
    /// 射击灯光预制件游戏对象
    /// </summary>
    public GameObject shootLightPrefabGameObject;
    
    /// <summary>
    /// 将EntitiesReferencesAuthoring组件烘焙为实体数据的烘焙器
    /// </summary>
    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        /// <summary>
        /// 将作者化组件数据烘焙为实体组件数据
        /// </summary>
        /// <param name="authoring">EntitiesReferencesAuthoring类型的作者化组件实例</param>
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            // 创建动态变换使用标志的实体
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // 为实体添加EntitiesReferences组件，并将游戏对象预制件转换为实体预制件
            AddComponent(entity, new EntitiesReferences
            {
                bulletPrefabEntity = GetEntity(authoring.bulletPrefabGameObject, TransformUsageFlags.Dynamic),
                zombiePrefabEntity = GetEntity(authoring.zombiePrefabGameObject, TransformUsageFlags.Dynamic),
                shootLightPrefabEntity = GetEntity(authoring.shootLightPrefabGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

/// <summary>
/// 包含游戏对象预制件实体引用的组件数据结构
/// </summary>
public struct EntitiesReferences : IComponentData
{
    /// <summary>
    /// 子弹预制件实体引用
    /// </summary>
    public Entity bulletPrefabEntity;
    
    /// <summary>
    /// 僵尸预制件实体引用
    /// </summary>
    public Entity zombiePrefabEntity;
    
    /// <summary>
    /// 射击灯光预制件实体引用
    /// </summary>
    public Entity shootLightPrefabEntity;
}
