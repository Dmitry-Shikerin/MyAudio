using MyAudios.Soundy.Editor.Views;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.AudioDatas.View.Interfaces
{
    public interface IAudioDataView : IView
    {
        void SetSliderValue(float value);
        void SetStopIcon();
        void SetPlayIcon();
        void SetLabelText(string labelText);
    }
}