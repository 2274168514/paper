using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

/// <summary>
/// AR博物馆场景管理器
/// 处理AR放置、缩放和交互
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class ARMuseumManager : MonoBehaviour
{
    [Header("AR组件引用")]
    [Tooltip("AR Raycast管理器")]
    public ARRaycastManager raycastManager;
    
    [Tooltip("AR Plane管理器")]
    public ARPlaneManager planeManager;

    [Header("场景设置")]
    [Tooltip("博物馆场景预制件")]
    public GameObject museumScenePrefab;
    
    [Tooltip("放置指示器")]
    public GameObject placementIndicator;

    [Header("交互设置")]
    [Tooltip("是否允许重新放置")]
    public bool allowReposition = true;
    
    [Tooltip("场景缩放比例")]
    [Range(0.1f, 10f)]
    public float sceneScale = 1f;
    
    [Tooltip("场景旋转速度")]
    public float rotationSpeed = 50f;

    [Header("手势控制")]
    [Tooltip("启用双指缩放")]
    public bool enablePinchZoom = true;
    
    [Tooltip("启用双指旋转")]
    public bool enableTwoFingerRotation = true;
    
    [Tooltip("最小缩放")]
    public float minScale = 0.5f;
    
    [Tooltip("最大缩放")]
    public float maxScale = 5f;

    // 放置的博物馆场景实例
    private GameObject spawnedMuseum;
    
    // Raycast碰撞结果列表
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    // 场景是否已放置
    private bool isPlaced = false;
    
    // 放置位置和旋转
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Start()
    {
        // 自动获取组件
        if (raycastManager == null)
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
        }

        // 隐藏放置指示器
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
    }

    void Update()
    {
        // 如果场景未放置或允许重新放置
        if (!isPlaced || allowReposition)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            // 检测屏幕点击
            if (placementPoseIsValid && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    PlaceMuseumScene();
                }
            }
        }

        // 处理手势控制
        if (isPlaced && spawnedMuseum != null)
        {
            HandleGestures();
        }
    }

    /// <summary>
    /// 更新放置位置
    /// </summary>
    void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        
        // 从屏幕中心发射射线
        if (raycastManager != null && raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            placementPoseIsValid = hits.Count > 0;
            
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;
                
                // 调整旋转，使场景面向相机
                Vector3 cameraForward = Camera.current.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
        else
        {
            placementPoseIsValid = false;
        }
    }

    /// <summary>
    /// 更新放置指示器
    /// </summary>
    void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            if (placementPoseIsValid && !isPlaced)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(
                    placementPose.position, 
                    placementPose.rotation
                );
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 放置博物馆场景
    /// </summary>
    void PlaceMuseumScene()
    {
        if (museumScenePrefab == null)
        {
            Debug.LogError("博物馆场景预制件未设置！");
            return;
        }

        // 如果已经放置且允许重新放置，删除旧的
        if (spawnedMuseum != null && allowReposition)
        {
            Destroy(spawnedMuseum);
        }

        // 如果还没有放置，或者允许重新放置
        if (spawnedMuseum == null)
        {
            // 实例化博物馆场景
            spawnedMuseum = Instantiate(
                museumScenePrefab, 
                placementPose.position, 
                placementPose.rotation
            );

            // 应用缩放
            spawnedMuseum.transform.localScale = Vector3.one * sceneScale;

            isPlaced = true;

            Debug.Log($"博物馆场景已放置在: {placementPose.position}");

            // 放置后可以选择隐藏平面检测
            // HidePlanes();
        }
    }

    /// <summary>
    /// 处理手势控制
    /// </summary>
    void HandleGestures()
    {
        // 检测双指手势
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // 计算当前和上一帧的双指距离
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);
            Vector2 prevPos0 = touch0.position - touch0.deltaPosition;
            Vector2 prevPos1 = touch1.position - touch1.deltaPosition;
            float previousDistance = Vector2.Distance(prevPos0, prevPos1);

            // 双指缩放
            if (enablePinchZoom)
            {
                float scaleFactor = currentDistance / previousDistance;
                Vector3 newScale = spawnedMuseum.transform.localScale * scaleFactor;
                
                // 限制缩放范围
                float scaleValue = newScale.x;
                scaleValue = Mathf.Clamp(scaleValue, minScale, maxScale);
                spawnedMuseum.transform.localScale = Vector3.one * scaleValue;
            }

            // 双指旋转
            if (enableTwoFingerRotation)
            {
                Vector2 currentVector = touch1.position - touch0.position;
                Vector2 previousVector = prevPos1 - prevPos0;
                
                float angle = Vector2.SignedAngle(previousVector, currentVector);
                spawnedMuseum.transform.Rotate(Vector3.up, angle, Space.World);
            }
        }
        // 单指旋转（左右滑动）
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationAmount = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                spawnedMuseum.transform.Rotate(Vector3.up, -rotationAmount, Space.World);
            }
        }
    }

    /// <summary>
    /// 隐藏AR平面可视化
    /// </summary>
    void HidePlanes()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 显示AR平面可视化
    /// </summary>
    void ShowPlanes()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 重置场景
    /// </summary>
    public void ResetScene()
    {
        if (spawnedMuseum != null)
        {
            Destroy(spawnedMuseum);
            spawnedMuseum = null;
            isPlaced = false;
            Debug.Log("场景已重置");
        }
    }

    /// <summary>
    /// 设置场景缩放
    /// </summary>
    public void SetSceneScale(float scale)
    {
        sceneScale = Mathf.Clamp(scale, minScale, maxScale);
        if (spawnedMuseum != null)
        {
            spawnedMuseum.transform.localScale = Vector3.one * sceneScale;
        }
    }

    /// <summary>
    /// 获取当前放置的场景
    /// </summary>
    public GameObject GetSpawnedMuseum()
    {
        return spawnedMuseum;
    }
}
