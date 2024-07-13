using MyAudios.Soundy.Editor.SoundGroups.Presentation.Interfaces;
using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Interfaces
{
    public interface ISoundDataBaseView : IView
    {
        void AddSoundGroup(ISoundGroupView soundGroupView);
    }
}