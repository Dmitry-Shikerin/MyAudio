using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send Sprite references </summary>
    [Serializable]
    public class SpriteEvent : UnityEvent<Sprite> { }
}