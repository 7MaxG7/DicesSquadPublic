using CustomTypes.Enums.Infrastructure;
using UnityEngine;

namespace Infrastructure
{
    public sealed class DebugService
    {
        public void Log(DebugType debugType, string message)
        {
#if UNITY_EDITOR
            switch (debugType)
            {
                case DebugType.Log:
                    Debug.Log(message);
                    break;
                case DebugType.Warning:
                    Debug.LogWarning(message);
                    break;
                case DebugType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    return;
            }
#endif
        }
    }
}