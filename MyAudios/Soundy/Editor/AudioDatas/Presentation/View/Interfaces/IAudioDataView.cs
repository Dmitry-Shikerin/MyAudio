using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces
{
    public interface IAudioDataView : IView
    {
        void SetSliderValue(float value);
        void SetStopIcon();
        void SetPlayIcon();
        void SetLabelText(string labelText);
    }
}