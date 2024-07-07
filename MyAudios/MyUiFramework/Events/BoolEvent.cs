using System;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send bool values </summary>
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }
}