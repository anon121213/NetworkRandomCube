using UnityEngine;

namespace _Scripts.Infrastructure.DontDestroyOnLoad
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}