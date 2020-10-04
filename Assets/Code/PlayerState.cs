using System;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class PlayerState
    {
        [Header("Orientation")]
        public float pitch = 0;
        public float yaw = 0;

        [Header("Movement")]
        public float horizontalMovement;
        public float verticalMovement;
    }
}