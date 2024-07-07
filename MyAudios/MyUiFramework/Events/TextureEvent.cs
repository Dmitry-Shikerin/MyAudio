using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send Texture references </summary>
    [Serializable]
    public class TextureEvent : UnityEvent<Texture> { }
}