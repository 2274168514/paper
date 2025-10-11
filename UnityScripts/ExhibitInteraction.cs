using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 博物馆展品交互脚本
/// 处理展品的点击、信息显示和AI查询
/// </summary>
public class ExhibitInteraction : MonoBehaviour
{
    [Header("展品信息")]
    [Tooltip("展品名称")]
    public string exhibitName = "展品名称";
    
    [Tooltip("展品简短描述")]
    [TextArea(3, 5)]
    public string exhibitDescription = "这是一个展品的描述信息。";
    
    [Tooltip("展品类别")]
    public string exhibitCategory = "未分类";
    
    [Tooltip("展品年代")]
    public string exhibitPeriod = "未知";

    [Header("交互设置")]
    [Tooltip("是否启用点击交互")]
    public bool enableClickInteraction = true;
    
    [Tooltip("是否在点击时自动询问AI")]
    public bool autoQueryAI = true;
    
    [Tooltip("高亮颜色")]
    public Color highlightColor = Color.yellow;

    [Header("外部引用")]
    [Tooltip("AI管理器引用")]
    public DeepSeekAIManager aiManager;
    
    [Tooltip("信息显示面板")]
    public GameObject infoPanel;
    
    [Tooltip("展品名称显示文本")]
    public Text nameText;
    
    [Tooltip("展品描述显示文本")]
    public Text descriptionText;

    [Header("音效")]
    [Tooltip("点击音效（可选）")]
    public AudioClip clickSound;

    // 原始材质颜色
    private Color originalColor;
    private Renderer objectRenderer;
    private AudioSource audioSource;
    private bool isHighlighted = false;

    void Start()
    {
        // 获取渲染器组件
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && objectRenderer.material != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // 获取或添加AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 自动查找AI管理器（如果未手动设置）
        if (aiManager == null)
        {
            aiManager = FindObjectOfType<DeepSeekAIManager>();
        }

        // 隐藏信息面板
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        // 确保有Collider用于射线检测
        EnsureCollider();
    }

    /// <summary>
    /// 确保物体有Collider组件
    /// </summary>
    void EnsureCollider()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // 如果没有Collider，添加一个BoxCollider
            BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
            Debug.Log($"为展品 {exhibitName} 自动添加了BoxCollider");
        }
    }

    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    void OnMouseDown()
    {
        if (!enableClickInteraction) return;

        Debug.Log($"点击了展品: {exhibitName}");

        // 播放点击音效
        PlayClickSound();

        // 显示展品信息
        ShowExhibitInfo();

        // 如果启用了自动AI查询
        if (autoQueryAI && aiManager != null)
        {
            QueryAIAboutExhibit();
        }
    }

    /// <summary>
    /// 鼠标悬停进入
    /// </summary>
    void OnMouseEnter()
    {
        if (!enableClickInteraction) return;
        HighlightObject(true);
    }

    /// <summary>
    /// 鼠标悬停离开
    /// </summary>
    void OnMouseExit()
    {
        if (!enableClickInteraction) return;
        HighlightObject(false);
    }

    /// <summary>
    /// 高亮/取消高亮物体
    /// </summary>
    void HighlightObject(bool highlight)
    {
        if (objectRenderer != null && objectRenderer.material != null)
        {
            if (highlight && !isHighlighted)
            {
                objectRenderer.material.color = highlightColor;
                isHighlighted = true;
            }
            else if (!highlight && isHighlighted)
            {
                objectRenderer.material.color = originalColor;
                isHighlighted = false;
            }
        }
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// 显示展品信息
    /// </summary>
    void ShowExhibitInfo()
    {
        // 激活信息面板
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }

        // 更新文本内容
        if (nameText != null)
        {
            nameText.text = exhibitName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = exhibitDescription;
        }

        Debug.Log($"展示信息: {exhibitName} - {exhibitDescription}");
    }

    /// <summary>
    /// 隐藏展品信息
    /// </summary>
    public void HideExhibitInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 向AI查询关于展品的信息
    /// </summary>
    void QueryAIAboutExhibit()
    {
        if (aiManager == null)
        {
            Debug.LogWarning("未找到AI管理器，无法查询");
            return;
        }

        // 构建查询问题
        string query = $"请详细介绍一下{exhibitName}";
        
        // 如果有类别和年代信息，添加到查询中
        if (!string.IsNullOrEmpty(exhibitCategory) && exhibitCategory != "未分类")
        {
            query += $"，它属于{exhibitCategory}";
        }
        
        if (!string.IsNullOrEmpty(exhibitPeriod) && exhibitPeriod != "未知")
        {
            query += $"，来自{exhibitPeriod}时期";
        }

        Debug.Log($"向AI查询: {query}");

        // 发送查询到AI
        aiManager.SendMessage(query);
    }

    /// <summary>
    /// 手动触发AI查询（供按钮调用）
    /// </summary>
    public void ManualQueryAI()
    {
        QueryAIAboutExhibit();
    }

    /// <summary>
    /// 设置展品信息
    /// </summary>
    public void SetExhibitInfo(string name, string description, string category = "", string period = "")
    {
        exhibitName = name;
        exhibitDescription = description;
        
        if (!string.IsNullOrEmpty(category))
            exhibitCategory = category;
            
        if (!string.IsNullOrEmpty(period))
            exhibitPeriod = period;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 在编辑器中绘制Gizmos
    /// </summary>
    void OnDrawGizmos()
    {
        // 在场景视图中显示展品名称
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, exhibitName);
    }
#endif
}
