using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.SoundGroups.Controllers;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Implementation;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Interfaces;

namespace MyAudios.Soundy.Editor.SoundGroups.Infrastructure.Factories
{
    public class SoundGroupViewFactory
    {
        public ISoundGroupView Create(SoundGroupData soundGroup)
        {
            var view = new SoundGroupView();
            var presenter = new SoundGroupPresenter(soundGroup, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}