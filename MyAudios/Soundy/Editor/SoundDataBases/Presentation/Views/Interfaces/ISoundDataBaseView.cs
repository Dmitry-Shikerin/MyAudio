using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces
{
    public interface ISoundDataBaseView : IView
    {
        void AddSoundGroup(ISoundGroupView soundGroupView);
        public void RemoveSoundGroup(ISoundGroupView soundGroupView);
        void StopAllSoundGroup();
    }
}