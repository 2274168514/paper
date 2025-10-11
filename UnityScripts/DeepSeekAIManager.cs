using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// DEEPSEEK API数据结构
/// </summary>
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
    public int max_tokens = 2000;
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

/// <summary>
/// DEEPSEEK AI对话管理器
/// 用于博物馆场景中的AI导览功能
/// </summary>
public class DeepSeekAIManager : MonoBehaviour
{
    [Header("API配置")]
    [Tooltip("从 https://platform.deepseek.com/ 获取API密钥")]
    public string apiKey = "YOUR_DEEPSEEK_API_KEY_HERE";
    private string apiUrl = "https://api.deepseek.com/v1/chat/completions";

    [Header("UI引用")]
    [Tooltip("用户输入框")]
    public UnityEngine.UI.InputField userInputField;
    
    [Tooltip("对话历史显示文本")]
    public UnityEngine.UI.Text conversationText;
    
    [Tooltip("发送按钮")]
    public UnityEngine.UI.Button sendButton;
    
    [Tooltip("思考中的提示文本（可选）")]
    public UnityEngine.UI.Text thinkingText;

    [Header("对话设置")]
    [Tooltip("AI角色设定")]
    [TextArea(3, 5)]
    public string systemPrompt = "你是一个专业的博物馆AI导览助手，负责介绍博物馆的展品、历史和相关知识。请用简洁、友好的语言回答问题。";
    
    [Tooltip("是否保留对话历史")]
    public bool keepConversationHistory = true;
    
    [Tooltip("最大历史消息数")]
    public int maxHistoryMessages = 10;

    // 对话历史记录
    private List<DeepSeekMessage> conversationHistory = new List<DeepSeekMessage>();
    
    // 是否正在等待AI响应
    private bool isWaitingForResponse = false;

    void Start()
    {
        // 初始化系统提示词
        if (keepConversationHistory)
        {
            conversationHistory.Add(new DeepSeekMessage
            {
                role = "system",
                content = systemPrompt
            });
        }

        // 绑定发送按钮事件
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }

        // 隐藏思考提示
        if (thinkingText != null)
        {
            thinkingText.gameObject.SetActive(false);
        }

        // 设置输入框回车键发送
        if (userInputField != null)
        {
            userInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }
    }

    /// <summary>
    /// 输入框结束编辑时（按回车）
    /// </summary>
    void OnInputFieldEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSendButtonClicked();
        }
    }

    /// <summary>
    /// 点击发送按钮
    /// </summary>
    public void OnSendButtonClicked()
    {
        if (isWaitingForResponse)
        {
            Debug.LogWarning("请等待AI响应完成");
            return;
        }

        string userMessage = userInputField?.text;
        if (string.IsNullOrEmpty(userMessage))
        {
            Debug.LogWarning("请输入问题");
            return;
        }

        SendMessage(userMessage);
    }

    /// <summary>
    /// 发送消息给AI
    /// </summary>
    public void SendMessage(string userMessage)
    {
        // 显示用户消息
        if (conversationText != null)
        {
            conversationText.text += "\n\n<b>用户:</b> " + userMessage;
        }

        // 清空输入框
        if (userInputField != null)
        {
            userInputField.text = "";
        }

        // 添加到历史记录
        if (keepConversationHistory)
        {
            conversationHistory.Add(new DeepSeekMessage
            {
                role = "user",
                content = userMessage
            });

            // 限制历史记录长度
            while (conversationHistory.Count > maxHistoryMessages)
            {
                // 保留system消息，删除最早的对话
                if (conversationHistory.Count > 1 && conversationHistory[1].role != "system")
                {
                    conversationHistory.RemoveAt(1);
                }
            }
        }

        // 调用API
        StartCoroutine(CallDeepSeekAPI(userMessage));
    }

    /// <summary>
    /// 调用DEEPSEEK API
    /// </summary>
    public IEnumerator CallDeepSeekAPI(string userMessage)
    {
        // 检查API密钥
        if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_DEEPSEEK_API_KEY_HERE")
        {
            ShowError("请先设置DEEPSEEK API密钥！\n访问 https://platform.deepseek.com/ 获取");
            yield break;
        }

        isWaitingForResponse = true;

        // 显示思考提示
        if (thinkingText != null)
        {
            thinkingText.gameObject.SetActive(true);
            thinkingText.text = "AI思考中...";
        }

        // 构建请求
        DeepSeekRequest request = new DeepSeekRequest();
        
        if (keepConversationHistory)
        {
            request.messages = conversationHistory.ToArray();
        }
        else
        {
            request.messages = new DeepSeekMessage[]
            {
                new DeepSeekMessage { role = "system", content = systemPrompt },
                new DeepSeekMessage { role = "user", content = userMessage }
            };
        }

        string jsonRequest = JsonUtility.ToJson(request);
        Debug.Log("发送API请求: " + jsonRequest);

        // 创建Web请求
        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // 设置超时时间（30秒）
            webRequest.timeout = 30;

            yield return webRequest.SendWebRequest();

            // 隐藏思考提示
            if (thinkingText != null)
            {
                thinkingText.gameObject.SetActive(false);
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("收到API响应: " + jsonResponse);

                try
                {
                    DeepSeekResponse response = JsonUtility.FromJson<DeepSeekResponse>(jsonResponse);
                    
                    if (response.choices != null && response.choices.Length > 0)
                    {
                        string aiResponse = response.choices[0].message.content;
                        
                        // 显示AI响应
                        if (conversationText != null)
                        {
                            conversationText.text += "\n\n<b>AI:</b> " + aiResponse;
                        }

                        // 添加到历史记录
                        if (keepConversationHistory)
                        {
                            conversationHistory.Add(new DeepSeekMessage
                            {
                                role = "assistant",
                                content = aiResponse
                            });
                        }
                    }
                    else
                    {
                        ShowError("AI响应格式错误");
                    }
                }
                catch (Exception e)
                {
                    ShowError("解析响应失败: " + e.Message);
                    Debug.LogError("JSON解析错误: " + e.ToString());
                }
            }
            else
            {
                ShowError("API调用失败: " + webRequest.error + "\n请检查网络连接和API密钥");
                Debug.LogError("API Error: " + webRequest.error);
                Debug.LogError("Response: " + webRequest.downloadHandler.text);
            }
        }

        isWaitingForResponse = false;
    }

    /// <summary>
    /// 显示错误消息
    /// </summary>
    void ShowError(string errorMessage)
    {
        if (conversationText != null)
        {
            conversationText.text += "\n\n<color=red><b>错误:</b> " + errorMessage + "</color>";
        }
        Debug.LogError(errorMessage);
    }

    /// <summary>
    /// 清除对话历史
    /// </summary>
    public void ClearConversation()
    {
        conversationHistory.Clear();
        conversationHistory.Add(new DeepSeekMessage
        {
            role = "system",
            content = systemPrompt
        });

        if (conversationText != null)
        {
            conversationText.text = "对话已清空";
        }
    }

    /// <summary>
    /// 预设问题快速查询
    /// </summary>
    public void AskPresetQuestion(string question)
    {
        if (userInputField != null)
        {
            userInputField.text = question;
        }
        OnSendButtonClicked();
    }
}
