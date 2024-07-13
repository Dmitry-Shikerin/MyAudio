using MyAudios.Soundy.Editor.AudioDatas.View.Interfaces;
using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces
{
    public interface ISoundGroupDataView : IView
    {
        void AddAudioData(IAudioDataView audioDataView);
    }
}