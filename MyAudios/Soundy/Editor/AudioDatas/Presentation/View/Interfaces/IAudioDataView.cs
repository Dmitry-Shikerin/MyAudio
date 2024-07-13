using MyAudios.Soundy.Editor.Views;
using UnityEngine;

namespace MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces
{
    public interface IAudioDataView : IView
    {
        void StopPlaySound();
        void SetSliderValue(float value);
        void SetStopIcon();
        void SetPlayIcon();
        void SetAudioClip(AudioClip audioClip);
    }
}