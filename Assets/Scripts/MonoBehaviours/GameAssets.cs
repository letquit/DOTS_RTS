using System;
using UnityEngine;

/// <summary>
/// 游戏资源管理器类，用于管理游戏中的静态资源和配置
/// </summary>
public class GameAssets : MonoBehaviour
{
    /// <summary>
    /// 单位图层索引，用于标识游戏中单位对象所在的图层
    /// </summary>
    public const int UNITS_LAYER = 6;
    
    /// <summary>
    /// 获取GameAssets单例实例
    /// </summary>
    public static GameAssets Instance { get; private set; }

    /// <summary>
    /// 初始化组件时调用，设置单例实例
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }
}
