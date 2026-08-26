using UnityEngine;

namespace BoardMatch.Utilities
{
    public class MatchLog : MonoBehaviour
    {
        public static void Log(object message, bool canLogInBuilds = false, bool isError = false)
        {
            if (isError)
            {
                Debug.LogError($"~{message}");
            }
        
            if (canLogInBuilds)
            {
                Debug.LogWarning($"~{message}");
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log($"~{message}");
            }
#endif
        
        }
        
        //MatchLog.Log allows to debug, activate and deactivate log messages easily in builds
    }
}