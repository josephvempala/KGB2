using System;
using UnityEngine;

public static class Models
{
    [Serializable]
    public class PlayerSettings
    {
        [Header("Mouse Settings")]
        public float MouseSensitivityX;
        public float MouseSensitivityY;

        public bool InvertMouseX;
        public bool InvertMouseY;
    }
}
