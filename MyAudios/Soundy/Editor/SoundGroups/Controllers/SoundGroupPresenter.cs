using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors.Windows;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Implementation;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Interfaces;

namespace MyAudios.Soundy.Editor.SoundGroups.Controllers
{
    public class SoundGroupPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroup;
        private readonly ISoundGroupView _view;

        public SoundGroupPresenter(SoundGroupData soundGroup, ISoundGroupView view)
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

        public void ShowSoundGroupData()
        {
            SoundGroupDataEditorWindow.Open(_soundGroup);
        }
    }
}