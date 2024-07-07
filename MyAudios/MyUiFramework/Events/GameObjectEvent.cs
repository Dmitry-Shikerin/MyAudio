using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send GameObject references </summary>
    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }
}