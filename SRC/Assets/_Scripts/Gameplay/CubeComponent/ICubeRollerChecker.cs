using System;
using UnityEngine;

namespace _Scripts.Gameplay.CubeComponent
{
    public interface ICubeRollerChecker
    {
        void Initialize(GameObject cube);
        void StartRollingCheck();
        void StopRollingCheck();
        event Action<int> OnChangeDiceValue;
    }
}