using System;
using UnityEngine;

/// <summary>
/// 鼠标世界位置获取器 - 用于将鼠标屏幕坐标转换为世界坐标
/// </summary>
public class MouseWorldPosition : MonoBehaviour
{
    /// <summary>
    /// 获取单例实例
    /// </summary>
    public static MouseWorldPosition Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 获取鼠标在世界空间中的位置
    /// </summary>
    /// <returns>鼠标对应的世界坐标位置，如果无法计算则返回Vector3.zero</returns>
    public Vector3 GetPosition()
    {
        // 从主摄像机创建射线，起点为鼠标屏幕位置
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // 创建一个以Y轴向上为法线、原点为参考点的平面
        Plane palne = new Plane(Vector3.up, Vector3.zero);

        // 检测射线与平面的交点
        if (palne.Raycast(mouseCameraRay, out float distance))
        {
            // 返回射线上距离指定点的位置
            return mouseCameraRay.GetPoint(distance);
        } 
        else
        {
            // 无法计算交点时返回零向量
            return Vector3.zero;
        }
    }
}
