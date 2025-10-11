# Unity AR博物馆场景开发指南

## 项目概述
本指南将帮助Unity初学者创建一个带有AI对话和数据可视化功能的3D博物馆AR场景。

## 技术栈
- **Unity引擎** (TuanjieHub版本)
- **DEEPSEEK API** - AI对话功能
- **XChart** - 数据可视化
- **AR Foundation** - AR功能支持

---

## 第一步：环境准备

### 1.1 安装Unity Hub和TuanjieHub
1. 访问Unity官网下载Unity Hub: https://unity.com/download
2. 安装Unity Hub后，下载TuanjieHub版本的Unity编辑器
3. 推荐版本：Unity 2021 LTS或更新版本

### 1.2 创建新项目
1. 打开Unity Hub
2. 点击"新建项目"
3. 选择"3D"模板
4. 项目名称：`AR_Museum_Project`
5. 选择保存位置
6. 点击"创建项目"

---

## 第二步：导入博物馆场景资源

### 2.1 推荐的免费博物馆场景资源

#### Unity Asset Store资源：
1. **Museum Kit** - 搜索"museum"
   - 链接：https://assetstore.unity.com/
   - 包含博物馆展厅、展示柜、艺术品等

2. **Art Gallery Scene**
   - 搜索"art gallery"或"museum interior"

3. **PolyWorld: Museum Pack**
   - 低多边形风格，适合AR设备性能要求

#### 其他免费资源平台：
- **Sketchfab**: https://sketchfab.com/ (搜索"museum")
- **TurboSquid Free**: https://www.turbosquid.com/Search/3D-Models/free/museum
- **CGTrader**: https://www.cgtrader.com/free-3d-models/museum

### 2.2 导入场景到Unity

**方法1：从Asset Store导入**
1. 在Unity编辑器中，点击菜单 `Window` → `Asset Store`
2. 搜索并选择博物馆资源包
3. 点击"Download"下载
4. 下载完成后点击"Import"
5. 在弹出窗口中保持全选，点击"Import"

**方法2：从外部导入FBX/OBJ模型**
1. 下载.fbx或.obj格式的3D模型
2. 在Unity项目窗口中，右键点击`Assets`文件夹
3. 选择 `Import New Asset`
4. 选择下载的模型文件
5. 拖拽导入的模型到场景(Hierarchy)窗口

### 2.3 场景基础设置
```
1. 删除默认的Main Camera和Directional Light（稍后会添加AR相机）
2. 将导入的博物馆场景预制件拖入Hierarchy窗口
3. 调整场景中心位置：选中场景，在Inspector中设置Position为(0, 0, 0)
```

---

## 第三步：配置AR功能

### 3.1 安装AR Foundation包
1. 打开 `Window` → `Package Manager`
2. 点击左上角的"+"号
3. 选择"Add package by name"
4. 输入：`com.unity.xr.arfoundation`，点击"Add"
5. 同样方式添加：
   - `com.unity.xr.arcore`（Android设备）
   - `com.unity.xr.arkit`（iOS设备）

### 3.2 创建AR Session
1. 右键Hierarchy窗口 → `XR` → `AR Session`
2. 右键Hierarchy窗口 → `XR` → `AR Session Origin`
3. AR Session Origin下会自动创建AR Camera

### 3.3 配置AR设置
在`Edit` → `Project Settings` → `XR Plug-in Management`中：
- 勾选你的目标平台（Android或iOS）
- Android: 勾选ARCore
- iOS: 勾选ARKit

---

## 第四步：集成DEEPSEEK API实现AI对话

### 4.1 创建AI对话管理脚本

在Unity中创建C#脚本：
1. 在Assets文件夹右键 → `Create` → `C# Script`
2. 命名为：`DeepSeekAIManager`

```csharp
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DeepSeekMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class DeepSeekRequest
{
    public string model = "deepseek-chat";
    public DeepSeekMessage[] messages;
    public float temperature = 0.7f;
}

[System.Serializable]
public class DeepSeekResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public DeepSeekMessage message;
}

public class DeepSeekAIManager : MonoBehaviour
{
    [Header("API配置")]
    public string apiKey = "你的DEEPSEEK_API_KEY";
    private string apiUrl = "https://api.deepseek.com/v1/chat/completions";

    [Header("UI引用")]
    public UnityEngine.UI.InputField userInputField;
    public UnityEngine.UI.Text conversationText;
    public UnityEngine.UI.Button sendButton;

    void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(SendMessage);
        }
    }

    public void SendMessage()
    {
        string userMessage = userInputField.text;
        if (string.IsNullOrEmpty(userMessage)) return;

        // 显示用户消息
        conversationText.text += "\n用户: " + userMessage;
        userInputField.text = "";

        // 调用API
        StartCoroutine(CallDeepSeekAPI(userMessage));
    }

    IEnumerator CallDeepSeekAPI(string userMessage)
    {
        // 构建请求
        DeepSeekRequest request = new DeepSeekRequest
        {
            messages = new DeepSeekMessage[]
            {
                new DeepSeekMessage 
                { 
                    role = "system", 
                    content = "你是一个博物馆导览AI助手，专门介绍博物馆的展品和历史。" 
                },
                new DeepSeekMessage 
                { 
                    role = "user", 
                    content = userMessage 
                }
            }
        };

        string jsonRequest = JsonUtility.ToJson(request);

        // 创建Web请求
        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                DeepSeekResponse response = JsonUtility.FromJson<DeepSeekResponse>(jsonResponse);
                
                if (response.choices != null && response.choices.Length > 0)
                {
                    string aiResponse = response.choices[0].message.content;
                    conversationText.text += "\nAI: " + aiResponse;
                }
            }
            else
            {
                Debug.LogError("API调用失败: " + webRequest.error);
                conversationText.text += "\nAI: 抱歉，连接失败，请稍后再试。";
            }
        }
    }
}
```

### 4.2 创建对话UI界面

1. 在Hierarchy中右键 → `UI` → `Canvas`
2. 创建UI元素：
   - 右键Canvas → `UI` → `Panel`（对话框背景）
   - 右键Panel → `UI` → `Text`（命名为ConversationText，显示对话历史）
   - 右键Panel → `UI` → `InputField`（用户输入框）
   - 右键Panel → `UI` → `Button`（发送按钮）

3. 将DeepSeekAIManager脚本附加到Canvas对象
4. 在Inspector中连接UI引用

### 4.3 获取DEEPSEEK API密钥

1. 访问 https://platform.deepseek.com/
2. 注册账号并登录
3. 进入API Keys页面
4. 创建新的API密钥
5. 复制密钥并粘贴到Unity脚本的apiKey字段

---

## 第五步：集成XChart数据可视化

### 5.1 下载并导入XChart

1. 访问XChart GitHub: https://github.com/XCharts-Team/XCharts
2. 或直接在Asset Store搜索"XCharts"
3. 下载最新版本的.unitypackage文件
4. 在Unity中 `Assets` → `Import Package` → `Custom Package`
5. 选择XCharts包并导入

### 5.2 创建简单的数据可视化

**创建柱状图示例：**

1. 在Hierarchy中右键 → `UI` → `XCharts` → `BarChart`
2. 选中创建的BarChart对象

3. 创建数据管理脚本 `ChartDataManager.cs`:

```csharp
using UnityEngine;
using XCharts.Runtime;

public class ChartDataManager : MonoBehaviour
{
    public BaseChart chart;

    void Start()
    {
        if (chart == null)
            chart = GetComponent<BaseChart>();

        // 设置图表标题
        if (chart.EnsureChartComponent<Title>() != null)
        {
            var title = chart.GetChartComponent<Title>();
            title.text = "博物馆参观人数统计";
        }

        // 添加数据
        chart.RemoveData();
        chart.AddSerie<Bar>("参观人数");
        
        // 添加X轴类别
        chart.AddXAxisData("周一");
        chart.AddXAxisData("周二");
        chart.AddXAxisData("周三");
        chart.AddXAxisData("周四");
        chart.AddXAxisData("周五");
        chart.AddXAxisData("周六");
        chart.AddXAxisData("周日");

        // 添加数据值
        chart.AddData(0, 120);
        chart.AddData(0, 132);
        chart.AddData(0, 101);
        chart.AddData(0, 134);
        chart.AddData(0, 90);
        chart.AddData(0, 230);
        chart.AddData(0, 210);
    }

    // 更新数据的方法
    public void UpdateChartData(int index, float value)
    {
        chart.UpdateData(0, index, value);
    }
}
```

**创建饼图示例：**

```csharp
public class PieChartManager : MonoBehaviour
{
    public PieChart pieChart;

    void Start()
    {
        if (pieChart == null)
            pieChart = GetComponent<PieChart>();

        // 设置标题
        var title = pieChart.EnsureChartComponent<Title>();
        title.text = "展品类别分布";

        // 添加数据
        pieChart.RemoveData();
        pieChart.AddSerie<Pie>("展品分类");
        
        pieChart.AddData(0, 35, "古代文物");
        pieChart.AddData(0, 25, "现代艺术");
        pieChart.AddData(0, 20, "科技展品");
        pieChart.AddData(0, 15, "自然标本");
        pieChart.AddData(0, 5, "其他");
    }
}
```

### 5.3 在3D空间中显示图表

1. 将Canvas的Render Mode改为"World Space"
2. 调整Canvas的Scale为适合AR场景的大小（如0.001, 0.001, 0.001）
3. 定位Canvas到博物馆场景中合适的位置

---

## 第六步：场景交互设计

### 6.1 创建展品交互脚本

```csharp
using UnityEngine;

public class ExhibitInteraction : MonoBehaviour
{
    public string exhibitName = "展品名称";
    public string exhibitDescription = "展品描述";
    
    public DeepSeekAIManager aiManager;
    public GameObject infoPanel;

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        // 点击展品时触发
        ShowExhibitInfo();
    }

    void ShowExhibitInfo()
    {
        if (infoPanel != null)
            infoPanel.SetActive(true);

        // 自动向AI询问关于展品的信息
        if (aiManager != null)
        {
            string query = "请介绍一下" + exhibitName;
            StartCoroutine(aiManager.CallDeepSeekAPI(query));
        }
    }
}
```

### 6.2 为展品添加Collider

1. 选择博物馆场景中的展品对象
2. 在Inspector中点击"Add Component"
3. 搜索并添加"Box Collider"或"Mesh Collider"
4. 附加ExhibitInteraction脚本

---

## 第七步：AR设备测试与部署

### 7.1 构建设置

1. 打开 `File` → `Build Settings`
2. 选择平台：
   - Android: 切换到Android平台
   - iOS: 切换到iOS平台
3. 点击"Switch Platform"

### 7.2 Android构建配置

1. 在Build Settings中点击"Player Settings"
2. 在Other Settings中：
   - 设置Package Name（如：com.yourname.armuseum）
   - Minimum API Level: Android 7.0（API Level 24）或更高
   - 勾选"ARCore Supported"
3. 连接Android设备
4. 点击"Build And Run"

### 7.3 iOS构建配置

1. 在Player Settings → Other Settings中：
   - 设置Bundle Identifier
   - 勾选"ARKit Supported"
   - Target minimum iOS Version: 11.0或更高
2. 点击"Build"生成Xcode项目
3. 在Mac上用Xcode打开生成的项目
4. 连接iOS设备并运行

---

## 第八步：测试流程

### 8.1 Unity编辑器测试
1. 点击Play按钮进入游戏模式
2. 测试UI交互（对话框、图表显示）
3. 检查脚本控制台是否有错误

### 8.2 真机测试
1. 构建并安装到AR设备
2. 允许相机和网络权限
3. 测试AR追踪和场景显示
4. 验证AI对话功能
5. 检查数据可视化图表

---

## 第九步：常见问题排查

### 9.1 AR不显示场景
- 检查是否安装了AR Foundation和ARCore/ARKit
- 确认设备支持AR功能
- 检查相机权限是否开启

### 9.2 API调用失败
- 验证API密钥是否正确
- 检查网络连接
- 查看Unity Console错误信息
- 确认API配额未超限

### 9.3 图表不显示
- 确认XCharts正确导入
- 检查Canvas渲染模式
- 验证数据是否正确添加

### 9.4 性能问题
- 降低场景多边形数量
- 优化光照设置
- 减少同时显示的UI元素
- 使用对象池管理动态对象

---

## 第十步：进阶优化

### 10.1 性能优化建议
1. 使用LOD（Level of Detail）系统
2. 启用Occlusion Culling
3. 优化材质和纹理尺寸
4. 使用对象池复用UI元素

### 10.2 用户体验改进
1. 添加语音识别输入（使用Unity Speech Recognition）
2. 实现手势交互（AR Foundation Gesture）
3. 添加音效和背景音乐
4. 设计引导教程

### 10.3 AI对话增强
```csharp
// 添加对话历史记录
private List<DeepSeekMessage> conversationHistory = new List<DeepSeekMessage>();

// 在API调用中使用历史记录
request.messages = conversationHistory.ToArray();
```

### 10.4 数据可视化动态更新
```csharp
// 定时更新图表数据
IEnumerator UpdateChartPeriodically()
{
    while (true)
    {
        yield return new WaitForSeconds(5f);
        
        // 模拟实时数据
        int randomValue = Random.Range(50, 250);
        chartManager.UpdateChartData(0, randomValue);
    }
}
```

---

## 资源链接汇总

### Unity学习资源
- Unity官方教程: https://learn.unity.com/
- Unity中文课堂: https://learn.u3d.cn/
- Unity AR Foundation文档: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/

### 3D资源下载
- Unity Asset Store: https://assetstore.unity.com/
- Sketchfab: https://sketchfab.com/
- TurboSquid: https://www.turbosquid.com/
- CGTrader: https://www.cgtrader.com/

### API文档
- DEEPSEEK API文档: https://platform.deepseek.com/api-docs/
- XCharts文档: https://xcharts-team.github.io/docs/

### 社区支持
- Unity中文社区: https://unity.cn/community
- GitHub XCharts Issues: https://github.com/XCharts-Team/XCharts/issues

---

## 开发时间线建议（1天）

虽然你说不需要时间规划，但这里提供一个参考流程：

1. **上午（4小时）**
   - 安装Unity和配置环境（1小时）
   - 导入博物馆场景资源（1小时）
   - 配置AR功能（1小时）
   - 创建基础UI（1小时）

2. **下午（4小时）**
   - 集成DEEPSEEK API（1.5小时）
   - 集成XChart可视化（1.5小时）
   - 添加交互功能（1小时）

3. **晚上（2-3小时）**
   - 真机测试和调试（1.5小时）
   - 优化和完善（1小时）

---

## 快速启动检查清单

- [ ] Unity Hub已安装
- [ ] Unity编辑器（TuanjieHub版本）已安装
- [ ] 创建新的3D项目
- [ ] 从Asset Store下载博物馆场景
- [ ] 导入AR Foundation包
- [ ] 配置AR Session和AR Camera
- [ ] 创建DeepSeekAIManager脚本
- [ ] 获取DEEPSEEK API密钥
- [ ] 导入XCharts包
- [ ] 创建图表UI
- [ ] 构建到AR设备测试

---

## 注意事项

1. **API密钥安全**: 不要将API密钥提交到公开仓库
2. **网络权限**: 确保应用有网络访问权限
3. **相机权限**: AR功能需要相机权限
4. **设备兼容性**: 确认你的AR设备支持ARCore或ARKit
5. **性能测试**: 在真机上测试性能，编辑器性能不代表真机性能

---

## 联系与支持

如果遇到问题，可以：
1. 查看Unity官方文档
2. 在Unity中文社区提问
3. 查看XCharts GitHub Issues
4. 参考DEEPSEEK API文档示例

祝开发顺利！🎉
