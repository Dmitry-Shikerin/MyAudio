using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Implementation;

namespace MyAudios.Soundy.Editor.SoundGroups.Controllers
{
    public class SoundGroupPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroup;
        private readonly SoundGroupView _view;

        public SoundGroupPresenter(SoundGroupData soundGroup, SoundGroupView view)
        {
            _soundGroup = soundGroup ?? throw new ArgumentNullException(nameof(soundGroup));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}