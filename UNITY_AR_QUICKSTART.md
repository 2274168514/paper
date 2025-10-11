# Unity AR Museum - Quick Start Guide

## Prerequisites
- Unity Hub installed
- TuanjieHub Unity Editor
- AR-capable device (ARCore/ARKit supported)
- DEEPSEEK API key
- Internet connection

## Step-by-Step Quick Setup

### 1. Create Unity Project (10 min)
```
1. Open Unity Hub
2. New Project → 3D Template
3. Name: AR_Museum_Project
4. Create
```

### 2. Import Museum Scene (15 min)
```
Unity Asset Store → Search "museum"
Recommended:
- Museum Kit
- Art Gallery Scene
- PolyWorld: Museum Pack

Download → Import → Done
```

### 3. Setup AR Foundation (10 min)
```
Window → Package Manager → Add Package by Name:
- com.unity.xr.arfoundation
- com.unity.xr.arcore (Android)
- com.unity.xr.arkit (iOS)

Hierarchy → Right Click:
- XR → AR Session
- XR → AR Session Origin
```

### 4. Create AI Dialogue Script (20 min)
```
Assets → Create → C# Script → DeepSeekAIManager.cs
Copy code from Unity_AR_Museum_Guide.md
Replace API key with your DEEPSEEK key
```

### 5. Create UI for Dialogue (15 min)
```
Hierarchy → UI → Canvas
Add:
- Panel (background)
- Text (conversation display)
- InputField (user input)
- Button (send)

Attach DeepSeekAIManager to Canvas
Connect UI references in Inspector
```

### 6. Import XChart (10 min)
```
Download from: https://github.com/XCharts-Team/XCharts
Assets → Import Package → Custom Package
Select XCharts package → Import All
```

### 7. Create Data Visualization (15 min)
```
Hierarchy → UI → XCharts → BarChart
Create ChartDataManager.cs script
Attach to BarChart
Configure data in Start()
```

### 8. Build and Test (20 min)
```
File → Build Settings
Switch Platform (Android/iOS)
Player Settings:
- Package Name
- Minimum API Level
- ARCore/ARKit Supported

Build and Run on device
```

## Critical Files to Create

### DeepSeekAIManager.cs (Essential)
- Handles AI API calls
- Manages conversation
- Updates UI with responses

### ChartDataManager.cs (Essential)
- Manages XChart data
- Updates visualizations
- Displays statistics

### ExhibitInteraction.cs (Optional)
- Handles object clicks
- Shows exhibit info
- Triggers AI queries

## Common Issues & Quick Fixes

### AR Not Working
```
✓ Check AR Foundation installed
✓ Verify ARCore/ARKit plugin enabled
✓ Confirm device supports AR
✓ Grant camera permissions
```

### API Fails
```
✓ Verify API key is correct
✓ Check internet connection
✓ Review Console errors
✓ Confirm API quota available
```

### Chart Not Displaying
```
✓ XCharts properly imported
✓ Canvas Render Mode set correctly
✓ Data added to chart
✓ Check chart component enabled
```

## Resource Links

**Unity Assets:**
- https://assetstore.unity.com/

**XCharts:**
- https://github.com/XCharts-Team/XCharts

**DEEPSEEK API:**
- https://platform.deepseek.com/

**AR Foundation Docs:**
- https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/

## Getting DEEPSEEK API Key

1. Visit: https://platform.deepseek.com/
2. Register account
3. Navigate to API Keys
4. Create New Key
5. Copy and paste into Unity script

## Testing Checklist

- [ ] Unity project created
- [ ] Museum scene imported and visible
- [ ] AR Session configured
- [ ] AR Camera working in scene
- [ ] DeepSeek API key added
- [ ] UI elements created and connected
- [ ] XCharts imported
- [ ] Chart displaying sample data
- [ ] Build successful
- [ ] App runs on AR device
- [ ] AI dialogue working
- [ ] Charts visible in AR

## Performance Tips

1. **Reduce Polygon Count**: Use low-poly museum assets
2. **Optimize Textures**: Compress textures to appropriate sizes
3. **Limit UI Elements**: Show/hide panels as needed
4. **Use Object Pooling**: Reuse UI elements
5. **Test on Device**: Editor performance ≠ device performance

## Next Steps After Setup

1. Customize museum scene layout
2. Add more exhibits with info panels
3. Implement voice input (optional)
4. Add gesture controls
5. Create tutorial/onboarding
6. Add sound effects
7. Implement analytics tracking

---

**Estimated Total Time: 2-3 hours for basic setup**

For detailed instructions, see `Unity_AR_Museum_Guide.md`
