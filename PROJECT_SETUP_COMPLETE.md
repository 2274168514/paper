# Unity AR博物馆项目 - 完整设置说明

## 项目结构

```
AR_Museum_Project/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity          # 主场景
│   ├── Scripts/
│   │   ├── DeepSeekAIManager.cs     # AI对话管理
│   │   ├── ChartDataManager.cs      # 图表数据管理
│   │   ├── ExhibitInteraction.cs    # 展品交互
│   │   └── ARMuseumManager.cs       # AR场景管理
│   ├── Prefabs/
│   │   ├── MuseumScene.prefab       # 博物馆场景预制件
│   │   ├── UICanvas.prefab          # UI界面预制件
│   │   └── PlacementIndicator.prefab # 放置指示器
│   ├── Materials/
│   ├── Models/                       # 从Asset Store导入的3D模型
│   └── XCharts/                      # XCharts插件文件夹
├── Packages/
│   ├── manifest.json
│   └── ...
└── ProjectSettings/
```

## 详细配置步骤

### 步骤1：创建Unity项目（10分钟）

1. **启动Unity Hub**
   ```
   - 打开Unity Hub
   - 点击"Projects"标签
   ```

2. **创建新项目**
   ```
   - 点击右上角"New Project"按钮
   - 选择"3D"模板（不要选择3D URP或HDRP）
   - 项目名称：AR_Museum_Project
   - 位置：选择你想要保存的文件夹
   - 点击"Create Project"
   ```

3. **等待项目创建**
   - Unity会创建基础项目结构
   - 首次启动可能需要几分钟

### 步骤2：安装AR Foundation包（15分钟）

1. **打开Package Manager**
   ```
   菜单栏: Window → Package Manager
   ```

2. **添加AR Foundation**
   ```
   - 点击左上角的"+"按钮
   - 选择"Add package by name..."
   - 输入：com.unity.xr.arfoundation
   - 点击"Add"
   - 等待安装完成
   ```

3. **添加平台支持包**
   
   **对于Android设备：**
   ```
   - 同样方式添加：com.unity.xr.arcore
   - 版本：选择兼容的最新版本
   ```
   
   **对于iOS设备：**
   ```
   - 同样方式添加：com.unity.xr.arkit
   - 版本：选择兼容的最新版本
   ```

4. **配置XR插件**
   ```
   菜单栏: Edit → Project Settings
   左侧选择: XR Plug-in Management
   
   对于Android：
   - 切换到Android标签页
   - 勾选"ARCore"
   
   对于iOS：
   - 切换到iOS标签页
   - 勾选"ARKit"
   ```

### 步骤3：导入博物馆场景资源（20分钟）

1. **打开Asset Store**
   ```
   方法1: 菜单栏 → Window → Asset Store
   方法2: 在浏览器打开 https://assetstore.unity.com/
   ```

2. **搜索并下载博物馆资源**
   
   **推荐资源包：**
   ```
   搜索关键词：
   - "museum"
   - "art gallery"
   - "exhibition hall"
   
   推荐选择：
   - 免费资源包
   - 低多边形（Low Poly）风格（性能好）
   - 包含预制件（Prefabs）的资源包
   ```

3. **导入到项目**
   ```
   - 在Asset Store中点击资源的"Download"按钮
   - 下载完成后点击"Import"
   - 在弹出的Import窗口中：
     * 可以取消勾选不需要的文件（如示例场景）
     * 保留Models、Materials、Prefabs文件夹
   - 点击"Import"按钮
   - 等待导入完成
   ```

### 步骤4：导入XCharts插件（10分钟）

1. **下载XCharts**
   ```
   访问：https://github.com/XCharts-Team/XCharts/releases
   下载最新版本的 .unitypackage 文件
   ```

2. **导入到Unity**
   ```
   菜单栏: Assets → Import Package → Custom Package
   选择下载的XCharts .unitypackage文件
   在Import窗口中点击"Import"
   等待导入完成
   ```

3. **验证导入**
   ```
   检查Project窗口中是否有XCharts文件夹
   菜单栏中应该出现"XCharts"选项
   ```

### 步骤5：创建并配置主场景（30分钟）

#### 5.1 删除默认对象
```
在Hierarchy窗口中：
- 右键点击"Main Camera" → Delete
- 右键点击"Directional Light" → Delete（可选，保留也可以）
```

#### 5.2 创建AR组件

1. **创建AR Session**
   ```
   Hierarchy窗口右键 → XR → AR Session
   ```

2. **创建AR Session Origin**
   ```
   Hierarchy窗口右键 → XR → AR Session Origin
   这会自动创建：
   - AR Session Origin（父对象）
   - AR Camera（子对象）
   ```

3. **添加AR Raycast Manager**
   ```
   选中"AR Session Origin"对象
   在Inspector窗口点击"Add Component"
   搜索"AR Raycast Manager"
   点击添加
   ```

4. **添加AR Plane Manager（可选）**
   ```
   选中"AR Session Origin"对象
   在Inspector窗口点击"Add Component"
   搜索"AR Plane Manager"
   点击添加
   
   配置：
   - Plane Prefab: 使用默认或自定义平面可视化
   ```

#### 5.3 创建博物馆场景预制件

1. **创建空对象**
   ```
   Hierarchy窗口右键 → Create Empty
   命名为："MuseumScene"
   ```

2. **添加博物馆模型**
   ```
   从Project窗口中拖拽导入的博物馆3D模型到MuseumScene对象下
   调整位置使其中心在(0, 0, 0)
   ```

3. **创建预制件**
   ```
   将MuseumScene对象从Hierarchy拖拽到Project窗口的Prefabs文件夹
   （如果没有Prefabs文件夹，先创建一个）
   ```

#### 5.4 创建放置指示器

1. **创建平面指示器**
   ```
   Hierarchy右键 → 3D Object → Plane
   命名为："PlacementIndicator"
   
   设置：
   - Scale: (0.3, 0.3, 0.3)
   - 添加材质（可选）
   ```

2. **创建为预制件**
   ```
   拖拽到Prefabs文件夹
   从Hierarchy中删除
   ```

### 步骤6：创建UI界面（40分钟）

#### 6.1 创建主Canvas

```
Hierarchy右键 → UI → Canvas
Canvas会自动创建EventSystem对象
```

#### 6.2 创建AI对话界面

1. **创建对话面板**
   ```
   右键Canvas → UI → Panel
   命名："DialogPanel"
   
   设置RectTransform：
   - Anchor: 下半部分
   - Width: 700
   - Height: 400
   - 位置：底部偏上
   ```

2. **创建对话显示区域**
   ```
   右键DialogPanel → UI → Scroll View
   命名："ConversationScrollView"
   
   展开Scroll View：
   - Viewport → Content → 添加Text组件
   - Text命名为："ConversationText"
   - 设置：
     * Font Size: 24
     * Color: 黑色
     * Alignment: 左上对齐
     * 启用Rich Text
   ```

3. **创建输入框**
   ```
   右键DialogPanel → UI → InputField
   命名："UserInputField"
   
   设置：
   - Placeholder: "请输入问题..."
   - Font Size: 20
   - 位置：面板底部
   ```

4. **创建发送按钮**
   ```
   右键DialogPanel → UI → Button
   命名："SendButton"
   
   设置：
   - Text: "发送"
   - Font Size: 20
   - 位置：输入框右侧
   ```

5. **创建思考提示（可选）**
   ```
   右键DialogPanel → UI → Text
   命名："ThinkingText"
   
   设置：
   - Text: "AI思考中..."
   - Color: 灰色
   - 位置：面板中央
   - 初始状态：禁用
   ```

#### 6.3 创建图表显示区域

1. **创建图表面板**
   ```
   右键Canvas → UI → Panel
   命名："ChartPanel"
   
   设置RectTransform：
   - Width: 600
   - Height: 500
   - 位置：右上角或左上角
   ```

2. **添加XChart组件**
   ```
   右键ChartPanel → XCharts → BarChart
   （或LineChart、PieChart，根据需要选择）
   
   会自动创建一个Chart对象
   ```

### 步骤7：附加脚本和配置（30分钟）

#### 7.1 复制脚本文件

1. **从提供的脚本文件夹复制以下文件到Assets/Scripts/：**
   ```
   - DeepSeekAIManager.cs
   - ChartDataManager.cs
   - ExhibitInteraction.cs
   - ARMuseumManager.cs
   ```

2. **在Unity中验证**
   ```
   Unity会自动编译脚本
   检查Console窗口是否有错误
   ```

#### 7.2 配置DeepSeek AI Manager

1. **附加脚本**
   ```
   选中Canvas对象
   点击"Add Component"
   搜索"DeepSeekAIManager"
   添加组件
   ```

2. **连接引用**
   ```
   在Inspector中：
   - API Key: 输入你的DEEPSEEK API密钥
   - User Input Field: 拖拽UserInputField对象
   - Conversation Text: 拖拽ConversationText对象
   - Send Button: 拖拽SendButton对象
   - Thinking Text: 拖拽ThinkingText对象（可选）
   ```

3. **配置系统提示词（可选）**
   ```
   System Prompt字段：
   "你是一个专业的博物馆AI导览助手，负责介绍博物馆的展品、历史和相关知识。请用简洁、友好的语言回答问题。"
   ```

#### 7.3 配置Chart Manager

1. **附加脚本**
   ```
   选中Chart对象（XChart创建的）
   点击"Add Component"
   搜索"ChartDataManager"
   添加组件
   ```

2. **配置参数**
   ```
   - Chart: 自动引用（或手动拖拽Chart组件）
   - Chart Title: "博物馆参观统计"
   - Use Example Data: 勾选（用于测试）
   - Auto Update: 根据需要勾选
   - Update Interval: 5（秒）
   ```

#### 7.4 配置AR Museum Manager

1. **附加脚本**
   ```
   选中AR Session Origin对象
   点击"Add Component"
   搜索"ARMuseumManager"
   添加组件
   ```

2. **连接引用**
   ```
   - Raycast Manager: 自动引用
   - Plane Manager: 自动引用
   - Museum Scene Prefab: 拖拽MuseumScene预制件
   - Placement Indicator: 拖拽PlacementIndicator预制件
   ```

3. **配置参数**
   ```
   - Allow Reposition: 勾选（允许重新放置）
   - Scene Scale: 1
   - Enable Pinch Zoom: 勾选
   - Enable Two Finger Rotation: 勾选
   - Min Scale: 0.5
   - Max Scale: 5
   ```

#### 7.5 为展品添加交互

1. **选择展品对象**
   ```
   在MuseumScene预制件中选择一个展品（如雕塑、画作）
   ```

2. **添加脚本**
   ```
   点击"Add Component"
   搜索"ExhibitInteraction"
   添加组件
   ```

3. **配置展品信息**
   ```
   - Exhibit Name: "蒙娜丽莎"
   - Exhibit Description: "文艺复兴时期达芬奇的杰作..."
   - Exhibit Category: "油画"
   - Exhibit Period: "文艺复兴"
   - AI Manager: 拖拽Canvas上的DeepSeekAIManager组件
   ```

4. **确保有Collider**
   ```
   如果没有Collider组件，脚本会自动添加BoxCollider
   ```

### 步骤8：构建设置（20分钟）

#### 8.1 配置Build Settings

1. **打开Build Settings**
   ```
   菜单栏: File → Build Settings
   ```

2. **添加场景**
   ```
   - 点击"Add Open Scenes"按钮
   - 确保你的MainScene在Scenes列表中
   ```

3. **切换平台**
   
   **对于Android：**
   ```
   - 在Platform列表中选择"Android"
   - 点击"Switch Platform"
   - 等待切换完成（可能需要几分钟）
   ```
   
   **对于iOS：**
   ```
   - 在Platform列表中选择"iOS"
   - 点击"Switch Platform"
   - 注意：需要在Mac上构建
   ```

#### 8.2 配置Player Settings

1. **打开Player Settings**
   ```
   在Build Settings窗口点击"Player Settings"
   ```

2. **Android配置**
   ```
   公司名称（Company Name）: 你的名字或公司
   产品名称（Product Name）: AR Museum
   
   Other Settings:
   - Package Name: com.yourname.armuseum
   - Minimum API Level: Android 7.0 (API Level 24)
   - Target API Level: Automatic (Highest installed)
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64（勾选）
   
   XR Settings:
   - 确认ARCore Supported已勾选
   ```

3. **iOS配置**
   ```
   公司名称（Company Name）: 你的名字或公司
   产品名称（Product Name）: AR Museum
   
   Other Settings:
   - Bundle Identifier: com.yourname.armuseum
   - Target minimum iOS Version: 11.0
   - Architecture: ARM64
   
   XR Settings:
   - 确认ARKit Supported已勾选
   
   Camera Usage Description:
   - 添加描述："需要相机权限以实现AR功能"
   ```

### 步骤9：获取DEEPSEEK API密钥（10分钟）

1. **访问DEEPSEEK平台**
   ```
   浏览器打开: https://platform.deepseek.com/
   ```

2. **注册账号**
   ```
   - 点击注册
   - 填写邮箱和密码
   - 验证邮箱
   - 登录
   ```

3. **创建API密钥**
   ```
   - 进入控制台/Dashboard
   - 找到"API Keys"或"密钥管理"
   - 点击"创建新密钥"
   - 复制生成的密钥（保存好，只显示一次）
   ```

4. **配置到Unity**
   ```
   - 回到Unity
   - 选中Canvas对象
   - 在DeepSeekAIManager组件中
   - 粘贴API密钥到"Api Key"字段
   ```

### 步骤10：测试（30分钟）

#### 10.1 Unity编辑器测试

1. **测试UI**
   ```
   - 点击Play按钮
   - 测试输入框输入
   - 点击发送按钮
   - （注意：AR功能在编辑器中无法完全测试）
   ```

2. **测试图表**
   ```
   - 观察ChartPanel中的图表是否正确显示
   - 如果启用了Auto Update，观察数据是否更新
   ```

#### 10.2 真机测试

1. **连接设备**
   ```
   Android: 用USB连接手机，开启USB调试
   iOS: 用数据线连接iPhone/iPad
   ```

2. **构建并运行**
   ```
   在Build Settings窗口：
   - 点击"Build And Run"
   - 选择保存APK的位置（Android）
   - 等待构建完成
   - 应用会自动安装并运行
   ```

3. **测试AR功能**
   ```
   - 允许相机权限
   - 移动设备扫描平面
   - 出现放置指示器时点击屏幕
   - 观察博物馆场景是否正确放置
   ```

4. **测试交互**
   ```
   - 点击场景中的展品
   - 观察信息面板是否显示
   - 检查AI是否自动回复
   - 手动输入问题测试AI对话
   - 查看数据可视化图表
   ```

### 常见问题解决

#### 问题1：AR场景不显示
```
解决方法：
1. 确认设备支持ARCore/ARKit
2. 检查相机权限是否授予
3. 确认AR Foundation包正确安装
4. 查看Console是否有错误信息
5. 尝试在不同光照环境下测试
```

#### 问题2：API调用失败
```
解决方法：
1. 验证API密钥是否正确复制
2. 检查设备网络连接
3. 确认API账户有剩余配额
4. 查看Unity Console错误详情
5. 尝试在浏览器中测试API
```

#### 问题3：图表不显示
```
解决方法：
1. 确认XCharts正确导入
2. 检查Canvas Render Mode
3. 验证ChartDataManager脚本正确附加
4. 查看Console是否有XCharts相关错误
5. 尝试重新导入XCharts包
```

#### 问题4：性能问题
```
解决方法：
1. 降低博物馆模型多边形数量
2. 优化材质和纹理大小
3. 减少同时显示的UI元素
4. 禁用不必要的实时光照
5. 使用LOD系统
```

#### 问题5：构建失败
```
解决方法：
1. 检查所有脚本是否有编译错误
2. 验证Package Name格式正确
3. 确认必要的包都已安装
4. 清理构建缓存（Build Settings → Clean）
5. 重启Unity编辑器
```

## 完成后的检查清单

- [ ] Unity项目创建成功
- [ ] AR Foundation包已安装
- [ ] ARCore/ARKit配置完成
- [ ] 博物馆场景已导入
- [ ] XCharts插件已导入
- [ ] 所有脚本已复制并附加
- [ ] UI界面创建完成
- [ ] DeepSeek API密钥已配置
- [ ] 引用连接正确
- [ ] Build Settings配置完成
- [ ] Player Settings配置完成
- [ ] 编辑器测试通过
- [ ] 真机测试通过
- [ ] AR放置功能正常
- [ ] AI对话功能正常
- [ ] 数据可视化正常

## 下一步优化建议

1. **添加更多展品交互**
2. **优化UI设计和用户体验**
3. **实现语音输入功能**
4. **添加音效和背景音乐**
5. **实现数据持久化**
6. **添加用户引导教程**
7. **实现多语言支持**
8. **优化性能和加载速度**

祝你开发顺利！
