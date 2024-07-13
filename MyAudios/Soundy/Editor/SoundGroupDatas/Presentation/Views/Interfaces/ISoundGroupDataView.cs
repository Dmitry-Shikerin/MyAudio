using System.Collections.Generic;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces
{
    public interface ISoundGroupDataView : IView
    {
        IReadOnlyList<IAudioDataView> AudioDataViews { get; }
        
        void AddAudioData(IAudioDataView audioDataView);
        public void SetIsOnButtonTab(SoundGroupData.PlayMode playMode);
    }
}