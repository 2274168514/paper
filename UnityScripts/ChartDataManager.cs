using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

/// <summary>
/// XChart图表数据管理器
/// 用于博物馆场景中的数据可视化
/// </summary>
public class ChartDataManager : MonoBehaviour
{
    [Header("图表引用")]
    [Tooltip("要管理的图表组件")]
    public BaseChart chart;

    [Header("图表配置")]
    [Tooltip("图表标题")]
    public string chartTitle = "博物馆数据统计";
    
    [Tooltip("是否自动更新数据")]
    public bool autoUpdate = false;
    
    [Tooltip("自动更新间隔（秒）")]
    public float updateInterval = 5f;

    [Header("示例数据")]
    [Tooltip("是否使用示例数据初始化")]
    public bool useExampleData = true;

    void Start()
    {
        // 获取图表组件
        if (chart == null)
        {
            chart = GetComponent<BaseChart>();
        }

        if (chart == null)
        {
            Debug.LogError("未找到图表组件！请确保此脚本附加到带有XCharts组件的对象上。");
            return;
        }

        // 初始化图表
        InitializeChart();

        // 如果启用自动更新，开始协程
        if (autoUpdate)
        {
            StartCoroutine(AutoUpdateChart());
        }
    }

    /// <summary>
    /// 初始化图表
    /// </summary>
    void InitializeChart()
    {
        // 设置标题
        var title = chart.EnsureChartComponent<Title>();
        if (title != null)
        {
            title.show = true;
            title.text = chartTitle;
        }

        // 设置图例
        var legend = chart.EnsureChartComponent<Legend>();
        if (legend != null)
        {
            legend.show = true;
        }

        // 设置工具提示
        var tooltip = chart.EnsureChartComponent<Tooltip>();
        if (tooltip != null)
        {
            tooltip.show = true;
        }

        // 如果使用示例数据，加载示例
        if (useExampleData)
        {
            LoadExampleData();
        }
    }

    /// <summary>
    /// 加载示例数据
    /// </summary>
    void LoadExampleData()
    {
        // 清除现有数据
        chart.RemoveData();

        // 根据图表类型加载不同的示例数据
        if (chart is BarChart)
        {
            LoadBarChartExample();
        }
        else if (chart is LineChart)
        {
            LoadLineChartExample();
        }
        else if (chart is PieChart)
        {
            LoadPieChartExample();
        }
        else
        {
            // 通用加载方式
            LoadGenericChartExample();
        }
    }

    /// <summary>
    /// 柱状图示例数据
    /// </summary>
    void LoadBarChartExample()
    {
        // 添加系列
        chart.AddSerie<Bar>("参观人数");

        // 添加X轴数据
        chart.AddXAxisData("周一");
        chart.AddXAxisData("周二");
        chart.AddXAxisData("周三");
        chart.AddXAxisData("周四");
        chart.AddXAxisData("周五");
        chart.AddXAxisData("周六");
        chart.AddXAxisData("周日");

        // 添加Y轴数据
        chart.AddData(0, 120);
        chart.AddData(0, 132);
        chart.AddData(0, 101);
        chart.AddData(0, 134);
        chart.AddData(0, 90);
        chart.AddData(0, 230);
        chart.AddData(0, 210);

        Debug.Log("已加载柱状图示例数据");
    }

    /// <summary>
    /// 折线图示例数据
    /// </summary>
    void LoadLineChartExample()
    {
        // 添加系列
        chart.AddSerie<Line>("参观趋势");

        // 添加X轴数据
        for (int i = 1; i <= 12; i++)
        {
            chart.AddXAxisData(i + "月");
        }

        // 添加Y轴数据（模拟一年的参观数据）
        int[] monthlyVisitors = { 1200, 1350, 1580, 1890, 2100, 2400, 2650, 2580, 2200, 1950, 1700, 1500 };
        for (int i = 0; i < monthlyVisitors.Length; i++)
        {
            chart.AddData(0, monthlyVisitors[i]);
        }

        Debug.Log("已加载折线图示例数据");
    }

    /// <summary>
    /// 饼图示例数据
    /// </summary>
    void LoadPieChartExample()
    {
        // 添加系列
        chart.AddSerie<Pie>("展品分类");

        // 添加数据（名称和值）
        chart.AddData(0, 350, "古代文物");
        chart.AddData(0, 250, "现代艺术");
        chart.AddData(0, 200, "科技展品");
        chart.AddData(0, 150, "自然标本");
        chart.AddData(0, 50, "其他");

        Debug.Log("已加载饼图示例数据");
    }

    /// <summary>
    /// 通用图表示例数据
    /// </summary>
    void LoadGenericChartExample()
    {
        // 添加一个基础系列
        var serie = chart.AddSerie<Serie>("数据系列");

        // 添加一些示例数据点
        for (int i = 0; i < 7; i++)
        {
            chart.AddData(0, Random.Range(50, 200));
        }

        Debug.Log("已加载通用图表示例数据");
    }

    /// <summary>
    /// 更新指定索引的数据
    /// </summary>
    public void UpdateData(int serieIndex, int dataIndex, float value)
    {
        if (chart != null)
        {
            chart.UpdateData(serieIndex, dataIndex, value);
        }
    }

    /// <summary>
    /// 添加新数据点
    /// </summary>
    public void AddNewData(int serieIndex, float value, string category = null)
    {
        if (chart != null)
        {
            if (!string.IsNullOrEmpty(category))
            {
                chart.AddXAxisData(category);
            }
            chart.AddData(serieIndex, value);
        }
    }

    /// <summary>
    /// 自动更新图表（模拟实时数据）
    /// </summary>
    IEnumerator AutoUpdateChart()
    {
        while (autoUpdate)
        {
            yield return new WaitForSeconds(updateInterval);

            // 随机更新一个数据点
            if (chart != null && chart.series.Count > 0)
            {
                var serie = chart.series[0];
                if (serie.dataCount > 0)
                {
                    int randomIndex = Random.Range(0, serie.dataCount);
                    float randomValue = Random.Range(50, 250);
                    chart.UpdateData(0, randomIndex, randomValue);
                    Debug.Log($"自动更新数据: 索引{randomIndex} = {randomValue}");
                }
            }
        }
    }

    /// <summary>
    /// 清除所有数据
    /// </summary>
    public void ClearAllData()
    {
        if (chart != null)
        {
            chart.RemoveData();
            Debug.Log("已清除所有图表数据");
        }
    }

    /// <summary>
    /// 重新加载示例数据
    /// </summary>
    public void ReloadExampleData()
    {
        LoadExampleData();
    }

    /// <summary>
    /// 设置图表标题
    /// </summary>
    public void SetChartTitle(string newTitle)
    {
        chartTitle = newTitle;
        var title = chart.EnsureChartComponent<Title>();
        if (title != null)
        {
            title.text = newTitle;
        }
    }

    /// <summary>
    /// 启用/禁用自动更新
    /// </summary>
    public void SetAutoUpdate(bool enabled)
    {
        bool wasEnabled = autoUpdate;
        autoUpdate = enabled;

        if (enabled && !wasEnabled)
        {
            StartCoroutine(AutoUpdateChart());
        }
    }
}
