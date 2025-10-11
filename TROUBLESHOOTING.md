# Unity AR博物馆开发 - 故障排除指南

## 目录
1. [AR相关问题](#ar相关问题)
2. [API集成问题](#api集成问题)
3. [XCharts可视化问题](#xcharts可视化问题)
4. [构建和部署问题](#构建和部署问题)
5. [性能优化问题](#性能优化问题)
6. [常见Unity错误](#常见unity错误)

---

## AR相关问题

### 问题1：AR场景不显示或AR Camera黑屏

**症状：**
- 应用启动后只看到黑屏
- 相机没有显示真实世界画面

**可能原因与解决方案：**

1. **相机权限未授予**
   ```
   解决方法：
   - Android: 设置 → 应用 → AR Museum → 权限 → 相机 → 允许
   - iOS: 设置 → 隐私 → 相机 → AR Museum → 开启
   - 卸载重装应用，确保正确提示权限请求
   ```

2. **设备不支持AR**
   ```
   检查方法：
   - Android: 设备必须支持ARCore
     * 查看支持设备列表：https://developers.google.com/ar/devices
   - iOS: 设备必须是iPhone 6S及以上，iOS 11+
     * iPad (第5代及以上)
   
   解决方法：
   - 更换支持AR的设备
   - 或者创建非AR版本（使用普通3D场景）
   ```

3. **AR Foundation配置错误**
   ```
   检查步骤：
   1. Window → Package Manager
   2. 确认已安装：
      - AR Foundation (4.2+)
      - ARCore XR Plugin (Android)
      - ARKit XR Plugin (iOS)
   3. Edit → Project Settings → XR Plug-in Management
   4. 确认对应平台的ARCore/ARKit已勾选
   ```

### 问题2：无法检测到平面

**症状：**
- 移动设备但放置指示器不出现
- 无法放置博物馆场景

**解决方案：**

1. **改善环境光照**
   ```
   - 避免在太暗的环境中使用
   - 避免在太亮或有强反光的表面
   - 确保有足够的环境特征点（不要对着白墙或单色表面）
   ```

2. **正确的扫描方式**
   ```
   - 缓慢移动设备
   - 保持设备与地面30-45度角
   - 扫描有纹理的表面（地板、桌面）
   - 不要移动太快
   ```

3. **检查AR Plane Manager配置**
   ```csharp
   - 选中AR Session Origin
   - 确认有AR Plane Manager组件
   - Plane Prefab设置正确
   - Detection Mode设置为"Everything"或"Horizontal"
   ```

### 问题3：场景放置位置不准确

**解决方案：**

1. **调整放置代码**
   ```csharp
   // 在ARMuseumManager.cs中
   // 调整placementPose的位置偏移
   void PlaceMuseumScene()
   {
       Vector3 adjustedPosition = placementPose.position;
       adjustedPosition.y += 0.1f; // 向上偏移10cm
       
       spawnedMuseum = Instantiate(
           museumScenePrefab, 
           adjustedPosition,  // 使用调整后的位置
           placementPose.rotation
       );
   }
   ```

2. **校准设备**
   ```
   - 重启应用
   - 重新扫描环境
   - 确保设备传感器正常工作
   ```

### 问题4：AR追踪丢失

**症状：**
- 场景突然消失或移动
- 场景位置不稳定

**解决方案：**

1. **改善追踪环境**
   ```
   - 保持环境光照稳定
   - 避免快速移动设备
   - 不要遮挡相机
   - 保持在检测到的平面范围内
   ```

2. **添加追踪丢失处理**
   ```csharp
   // 在ARMuseumManager.cs中添加
   void Update()
   {
       // 检查追踪状态
       if (ARSession.state == ARSessionState.SessionTracking)
       {
           // 追踪正常
       }
       else
       {
           // 显示提示信息
           Debug.LogWarning("AR追踪丢失，请重新扫描环境");
       }
   }
   ```

---

## API集成问题

### 问题5：DEEPSEEK API调用失败

**症状：**
- 发送消息后没有响应
- Console显示API错误

**可能原因与解决方案：**

1. **API密钥错误**
   ```
   检查：
   - 密钥是否完整复制（没有多余空格）
   - 密钥是否已过期
   - 密钥格式是否正确（通常以sk-开头）
   
   解决：
   - 重新从平台复制密钥
   - 确保粘贴时没有换行符
   - 检查是否误删了部分字符
   ```

2. **网络连接问题**
   ```
   检查：
   - 设备是否连接到互联网
   - 是否使用了VPN或代理
   - 防火墙是否阻止了连接
   
   解决：
   - 切换到稳定的WiFi网络
   - 关闭VPN重试
   - 检查网络权限设置
   ```

3. **API配额用尽**
   ```
   检查：
   - 登录DEEPSEEK平台查看剩余配额
   - 查看账户余额
   
   解决：
   - 充值账户
   - 或使用免费配额账户
   ```

4. **请求格式错误**
   ```
   检查Console输出的JSON请求格式
   确保包含必要字段：
   {
       "model": "deepseek-chat",
       "messages": [...],
       "temperature": 0.7
   }
   
   调试方法：
   - 在DeepSeekAIManager.cs中添加详细日志
   - 打印完整请求和响应
   ```

### 问题6：API响应解析失败

**症状：**
- Console显示JSON解析错误
- AI回复显示为空

**解决方案：**

1. **检查响应格式**
   ```csharp
   // 在DeepSeekAIManager.cs的API调用处添加
   Debug.Log("原始响应: " + webRequest.downloadHandler.text);
   
   // 检查响应是否符合预期格式
   ```

2. **更新数据结构**
   ```csharp
   // 确保DeepSeekResponse类与实际API响应匹配
   // 如果API更新了格式，需要更新C#类定义
   ```

3. **处理错误响应**
   ```csharp
   // 添加错误响应处理
   if (webRequest.responseCode != 200)
   {
       Debug.LogError($"HTTP错误: {webRequest.responseCode}");
       Debug.LogError($"错误详情: {webRequest.downloadHandler.text}");
   }
   ```

### 问题7：API响应速度慢

**解决方案：**

1. **添加超时处理**
   ```csharp
   // 在API调用中
   webRequest.timeout = 30; // 30秒超时
   ```

2. **优化请求**
   ```csharp
   // 减少max_tokens
   request.max_tokens = 500; // 而不是2000
   
   // 简化对话历史
   if (conversationHistory.Count > 5)
   {
       // 只保留最近5条对话
   }
   ```

3. **添加加载提示**
   ```csharp
   // 显示"正在思考"动画
   // 让用户知道系统正在处理
   ```

---

## XCharts可视化问题

### 问题8：图表不显示

**可能原因与解决方案：**

1. **XCharts未正确导入**
   ```
   检查：
   - Project窗口是否有XCharts文件夹
   - 菜单栏是否有XCharts选项
   
   解决：
   - 重新导入XCharts包
   - 确保导入时没有取消勾选重要文件
   ```

2. **Canvas设置问题**
   ```
   检查：
   - Canvas Render Mode
   - 对于3D AR场景，使用"World Space"
   - 对于2D UI，使用"Screen Space - Overlay"
   
   调整：
   - 选中Canvas
   - Inspector → Canvas组件
   - Render Mode → 选择合适模式
   ```

3. **Chart组件配置**
   ```
   检查：
   - Chart对象是否active
   - ChartDataManager脚本是否正确附加
   - Chart引用是否正确连接
   
   解决：
   - 在Hierarchy中确认Chart对象启用
   - 重新连接脚本引用
   ```

### 问题9：图表数据不更新

**解决方案：**

1. **检查脚本执行**
   ```csharp
   // 在ChartDataManager.cs的Start()中添加
   Debug.Log("ChartDataManager已启动");
   
   // 在UpdateData()中添加
   Debug.Log($"更新数据: {serieIndex}, {dataIndex}, {value}");
   ```

2. **验证数据添加**
   ```csharp
   // 确保正确调用了AddData
   chart.AddData(serieIndex, value);
   
   // 手动刷新图表
   chart.RefreshChart();
   ```

3. **检查自动更新协程**
   ```csharp
   // 确认autoUpdate已启用
   // 确认协程正在运行
   if (autoUpdate)
   {
       Debug.Log("自动更新已启用");
   }
   ```

### 问题10：图表显示位置错误

**解决方案：**

1. **调整RectTransform**
   ```
   - 选中Chart对象
   - Inspector → RectTransform
   - 调整Anchors、Position、Size
   ```

2. **设置Canvas Scaler**
   ```
   - 选中Canvas
   - 添加Canvas Scaler组件
   - UI Scale Mode → Scale With Screen Size
   - Reference Resolution: 1920x1080
   ```

---

## 构建和部署问题

### 问题11：Android构建失败

**常见错误与解决方案：**

1. **Gradle错误**
   ```
   错误信息：Gradle build failed
   
   解决：
   - Edit → Preferences → External Tools
   - 确认Android SDK、NDK、JDK路径正确
   - 更新Gradle版本
   - 清理构建缓存
   ```

2. **包名冲突**
   ```
   错误信息：Package name already exists
   
   解决：
   - Player Settings → Package Name
   - 修改为唯一的包名
   - 格式：com.公司名.应用名
   ```

3. **API Level不兼容**
   ```
   错误信息：Minimum API level mismatch
   
   解决：
   - Player Settings → Minimum API Level
   - 设置为Android 7.0 (API 24)或更高
   ```

4. **签名问题**
   ```
   解决：
   - Player Settings → Publishing Settings
   - 创建新的Keystore
   - 或使用已有Keystore签名
   ```

### 问题12：iOS构建失败

**解决方案：**

1. **Bundle Identifier错误**
   ```
   - 确保格式正确：com.yourname.appname
   - 全部小写，无特殊字符
   - 在Apple Developer账户中注册
   ```

2. **Provisioning Profile问题**
   ```
   - 在Xcode中设置正确的Team
   - 确保Provisioning Profile有效
   - 重新生成证书和描述文件
   ```

3. **相机权限描述缺失**
   ```
   - Player Settings → iOS
   - Camera Usage Description
   - 添加描述文字
   ```

### 问题13：应用安装后闪退

**调试方法：**

1. **查看日志**
   ```
   Android:
   - 连接设备到电脑
   - 使用adb logcat
   - 筛选Unity日志
   
   iOS:
   - Xcode → Window → Devices and Simulators
   - 选择设备查看Console
   ```

2. **常见原因**
   ```
   - 缺少必要权限
   - 依赖库冲突
   - 内存不足
   - 设备不支持
   ```

3. **解决步骤**
   ```
   1. 检查Player Settings配置
   2. 验证所有必要包已安装
   3. 降低场景复杂度测试
   4. 在不同设备上测试
   ```

---

## 性能优化问题

### 问题14：应用运行卡顿

**优化方案：**

1. **降低模型复杂度**
   ```
   - 使用低多边形模型
   - 合并网格
   - 移除不可见的面
   - 使用LOD系统
   ```

2. **优化材质和纹理**
   ```
   - 压缩纹理大小
   - 使用适当的纹理格式
   - 减少材质数量
   - 使用图集合并纹理
   ```

3. **优化UI**
   ```
   - 使用Canvas Group控制批处理
   - 减少Raycast Target
   - 分离静态和动态UI
   - 使用对象池
   ```

4. **代码优化**
   ```csharp
   // 避免在Update中频繁查找对象
   // 缓存组件引用
   private Transform cachedTransform;
   
   void Start()
   {
       cachedTransform = transform;
   }
   
   // 使用协程代替Update中的重复操作
   ```

### 问题15：内存占用过高

**解决方案：**

1. **资源释放**
   ```csharp
   // 及时释放不用的资源
   Resources.UnloadUnusedAssets();
   System.GC.Collect();
   ```

2. **纹理压缩**
   ```
   - 选择纹理
   - Inspector → Format
   - Android: ASTC
   - iOS: PVRTC
   ```

3. **音频压缩**
   ```
   - 选择音频文件
   - Inspector → Compression Format
   - 使用Vorbis或MP3
   ```

---

## 常见Unity错误

### 问题16：NullReferenceException

**原因：**
- 引用未赋值
- 对象已被销毁

**解决方案：**

```csharp
// 添加空值检查
if (myObject != null)
{
    myObject.DoSomething();
}

// 或使用?.运算符
myObject?.DoSomething();
```

### 问题17：Missing Reference

**解决方案：**

1. **重新连接引用**
   ```
   - 选中对象
   - Inspector中找到Missing引用
   - 重新拖拽正确对象
   ```

2. **检查预制件**
   ```
   - 打开预制件编辑模式
   - 修复引用
   - 保存预制件
   ```

### 问题18：Shader编译错误

**解决方案：**

1. **更新Shader**
   ```
   - 右键Shader文件
   - Reimport
   ```

2. **Graphics设置**
   ```
   - Edit → Project Settings → Graphics
   - 确认Shader包含在Always Included Shaders中
   ```

---

## 快速诊断流程

遇到问题时，按以下步骤排查：

1. **检查Console**
   - 红色错误信息
   - 黄色警告信息
   - 复制完整错误信息搜索

2. **验证配置**
   - Build Settings
   - Player Settings
   - Package Manager

3. **测试环境**
   - 编辑器测试
   - 真机测试
   - 不同设备测试

4. **逐步排除**
   - 禁用部分功能
   - 简化场景
   - 隔离问题代码

5. **查阅文档**
   - Unity官方文档
   - AR Foundation文档
   - XCharts文档
   - DEEPSEEK API文档

## 获取帮助

如果以上方法都无法解决问题：

1. **Unity社区**
   - Unity中文论坛
   - Unity Answers
   - Stack Overflow

2. **GitHub Issues**
   - AR Foundation GitHub
   - XCharts GitHub

3. **官方支持**
   - Unity Support
   - DEEPSEEK技术支持

---

**记住：大多数问题都有解决方案，耐心和系统的排查是关键！**
