using System;
using UnityEngine;

namespace _Scripts.Gameplay.CubeComponent
{
    [Serializable]
    public struct CubeFace
    {
        public CubeFaceDirection direction;
        public int value; 

        public Vector3 GetDirectionVector()
        {
            return direction switch
            {
                CubeFaceDirection.Up => Vector3.up,
                CubeFaceDirection.Down => Vector3.down,
                CubeFaceDirection.Left => Vector3.left,
                CubeFaceDirection.Right => Vector3.right,
                CubeFaceDirection.Forward => Vector3.forward,
                CubeFaceDirection.Backward => Vector3.back,
                _ => Vector3.zero
            };
        }
    }
}