using System.Text;
using BoardMatch.Core;
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
        
        public static void FormatGrid(int[,] grid)
        {
            #if UNITY_EDITOR
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            var sb = new StringBuilder();

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    int gemType = grid[x, y];
                    sb.Append(gemType == BoardModel.EmptyCell ? " . " : $" {gemType} ");
                }
                sb.AppendLine();
            }

            Log(sb.ToString()); 
            #endif
        }
        
        //MatchLog.Log allows to debug, activate and deactivate log messages easily in builds
    }
}