using System;
using MyAudios.Soundy.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MyAudios.Tests
{
    public class AudioTestView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            OnClick();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
            OnClick();
        }

        private void OnClick()
        {
            SoundyManager.Play("Example Clicks", "Adding Machine", Vector3.zero);
        }
    }
}