using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send Color values </summary>
    [Serializable]
    public class ColorEvent : UnityEvent<Color> { }
}