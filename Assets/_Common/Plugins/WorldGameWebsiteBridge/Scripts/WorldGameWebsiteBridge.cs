using UnityEngine;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public class WorldGameWebsiteBridge
{

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void GameReleaseFocus();

    [DllImport("__Internal")]
    private static extern void SendEventToBrowser(string message);

    [DllImport("__Internal")]
    private static extern int IsShareAvailable();

    [DllImport("__Internal")]
    private static extern void Share(string jsonPayload);
#endif

    public static void SendEventToBrowser(object json)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SendEventToBrowser(json.ToString());
#else
        Debug.Log("Event to browser: " + json.ToString());
#endif
    }

    public static void BrowserReleaseFocus()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("About to call GameReleaseFocus JavaScript function");
        GameReleaseFocus();
        Debug.Log("GameReleaseFocus JavaScript function called");
#else
        Debug.Log("BrowserReleaseFocus called");
#endif
    }

    public static bool IsShareSupported()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsShareAvailable() == 1;
#else
        return false;
#endif
    }

    [System.Serializable]
    private class SharePayload
    {
        public string title;
        public string text;
        public string url;
    }

    public static void TryShare(string title = null, string text = null, string url = null)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        var payload = new SharePayload { title = title, text = text, url = url };
        var json = JsonUtility.ToJson(payload);
        Share(json);
#else
        Debug.Log($"Share (simulated): title='{title}', text='{text}', url='{url}'");
#endif
    }
}
