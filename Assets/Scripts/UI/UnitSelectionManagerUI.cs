using System;
using UnityEngine;

/// <summary>
/// 单位选择管理器UI类，负责处理单位选择区域的可视化显示
/// </summary>
public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform selectionAreaRectTransform; // 选择区域的RectTransform组件
    [SerializeField] private Canvas canvas; // 所属的Canvas组件

    /// <summary>
    /// 初始化方法，在对象开始运行时设置事件监听器并隐藏选择区域
    /// </summary>
    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectionAreaStart += UnitSelectionManager_OnSelectionAreaStart;
        UnitSelectionManager.Instance.OnSelectionAreaEnd += UnitSelectionManager_OnSelectionAreaEnd;
        
        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新方法，持续更新选择区域的视觉表现
    /// </summary>
    private void Update()
    {
        if (selectionAreaRectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }

    /// <summary>
    /// 当选择区域开始时的事件处理方法，激活选择区域并更新视觉表现
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">事件参数</param>
    private void UnitSelectionManager_OnSelectionAreaStart(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(true);

        UpdateVisual();
    }

    /// <summary>
    /// 当选择区域结束时的事件处理方法，隐藏选择区域
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">事件参数</param>
    private void UnitSelectionManager_OnSelectionAreaEnd(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新选择区域的视觉表现，根据选择区域矩形调整RectTransform的位置和大小
    /// </summary>
    private void UpdateVisual()
    {
        // 获取当前选择区域的矩形信息
        Rect selectionAreaRect = UnitSelectionManager.Instance.GetSelectionAreaRect();
        
        // 获取画布缩放比例以进行坐标转换
        float canvasScale = canvas.transform.localScale.x;
        selectionAreaRectTransform.anchoredPosition = new Vector2(selectionAreaRect.x, selectionAreaRect.y) / canvasScale;
        selectionAreaRectTransform.sizeDelta = new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
    }
}
