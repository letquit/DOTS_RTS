using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 测试系统，用于处理游戏中的测试逻辑
/// </summary>
partial struct TestingSystem : ISystem
{
    /// <summary>
    /// 系统更新时调用的方法
    /// </summary>
    /// <param name="state">系统状态引用</param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*
        // 遍历查询所有僵尸组件并统计数量
        int unitCount = 0;
        foreach (RefRW<Zombie> zombie in SystemAPI.Query<RefRW<Zombie>>())
        {
            unitCount++;
        }
        
        Debug.Log("unitCount: " + unitCount);
        */
    }
}
