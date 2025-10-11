# Paper Research Repository / 论文研究仓库

## 项目说明 / Project Description

本仓库包含两个独立的项目：

### 1. 社交媒体分析研究（原项目）
复现论文 "KaramouzasMademlis_PublicOpinionMonitoringTwitter_SNAM"
- 数据集：OLID、TSI、政治社交媒体帖子
- 实现：Python机器学习模型
- 详见：`CLAUDE.md`

### 2. Unity AR博物馆场景开发（新增）
使用TuanjieHub开发带有AI对话和数据可视化的3D博物馆AR场景

---

## Unity AR博物馆项目 / Unity AR Museum Project

### 快速开始 / Quick Start

**🎯 从这里开始：[`START_HERE.md`](START_HERE.md)** ← 完整导航指南

**新手指南：**
1. 📖 阅读 [`Unity_AR_Museum_Guide.md`](Unity_AR_Museum_Guide.md) - 完整开发教程
2. ⚡ 查看 [`UNITY_AR_QUICKSTART.md`](UNITY_AR_QUICKSTART.md) - 快速入门指南
3. 🔧 参考 [`PROJECT_SETUP_COMPLETE.md`](PROJECT_SETUP_COMPLETE.md) - 详细配置步骤
4. 🚑 遇到问题查看 [`TROUBLESHOOTING.md`](TROUBLESHOOTING.md) - 故障排除指南
5. 📋 准备工作 [`RESOURCES_CHECKLIST.md`](RESOURCES_CHECKLIST.md) - 资源清单
6. 🗺️ 流程图 [`DEVELOPMENT_WORKFLOW.md`](DEVELOPMENT_WORKFLOW.md) - 开发流程

### 技术栈 / Tech Stack

- **Unity引擎** (TuanjieHub版本)
- **AR Foundation** - AR功能支持
- **DEEPSEEK API** - AI对话功能
- **XCharts** - 数据可视化

### 核心脚本 / Core Scripts

所有Unity C#脚本位于 `UnityScripts/` 文件夹：

1. **DeepSeekAIManager.cs** - AI对话管理器
   - 处理与DEEPSEEK API的通信
   - 管理对话历史
   - UI交互控制

2. **ChartDataManager.cs** - 图表数据管理器
   - XCharts数据管理
   - 支持柱状图、折线图、饼图
   - 自动更新功能

3. **ExhibitInteraction.cs** - 展品交互脚本
   - 点击展品显示信息
   - 自动触发AI介绍
   - 高亮显示效果

4. **ARMuseumManager.cs** - AR场景管理器
   - AR放置和追踪
   - 手势控制（缩放、旋转）
   - 场景管理

### 开发流程 / Development Workflow

```
1. 安装Unity和TuanjieHub
   ↓
2. 导入博物馆3D场景资源
   ↓
3. 配置AR Foundation
   ↓
4. 集成DEEPSEEK API
   ↓
5. 添加XChart数据可视化
   ↓
6. 构建到AR设备测试
```

### 资源链接 / Resources

- **Unity Asset Store**: https://assetstore.unity.com/ (博物馆场景)
- **XCharts GitHub**: https://github.com/XCharts-Team/XCharts
- **DEEPSEEK API**: https://platform.deepseek.com/
- **AR Foundation文档**: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/

### 适用人群 / Target Audience

- Unity初学者（提供详细步骤指导）
- AR开发新手
- 希望快速搭建AR博物馆场景的开发者
- 需要集成AI对话的AR项目

### 预计开发时间 / Estimated Time

- **最小可用版本**: 2-3小时
- **完整功能版本**: 1天
- **优化和完善**: 额外1-2天

---

## 社交媒体分析项目 / Social Media Analysis Project

（原项目内容保持不变）

### 数据集 / Datasets
- OLID: 攻击性语言识别
- TSI: Twitter讽刺/反讽检测
- PSMP: 政治社交媒体帖子

### 实现进度 / Progress
详见 [`CLAUDE.md`](CLAUDE.md)

---

## 项目结构 / Project Structure

```
paper/
├── README.md                          # 本文件
├── START_HERE.md                      # 👈 新手从这里开始！导航指南
├── CLAUDE.md                          # 社交媒体分析项目说明
├── Unity_AR_Museum_Guide.md          # Unity AR完整教程（中文）
├── UNITY_AR_QUICKSTART.md             # 快速开始指南（英文）
├── PROJECT_SETUP_COMPLETE.md          # 完整配置步骤
├── TROUBLESHOOTING.md                 # 故障排除指南
├── RESOURCES_CHECKLIST.md             # 资源和工具清单
├── DEVELOPMENT_WORKFLOW.md            # 开发流程图
├── UnityScripts/                      # Unity C#脚本
│   ├── DeepSeekAIManager.cs
│   ├── ChartDataManager.cs
│   ├── ExhibitInteraction.cs
│   └── ARMuseumManager.cs
├── data/                              # 社交媒体数据集
├── requirements.txt                   # Python依赖
└── KaramouzasMademlis_*.pdf          # 研究论文
```

## 许可证 / License

本项目仅用于学习和研究目的。

---

**注意**: 两个项目相互独立，可以分别使用。
