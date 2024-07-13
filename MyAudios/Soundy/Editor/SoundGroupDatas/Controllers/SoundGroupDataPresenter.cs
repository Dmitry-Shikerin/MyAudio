using System;
using Doozy.Engine.Soundy;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Controllers
{
    public class SoundGroupDataPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroupData;
        private readonly ISoundGroupDataView _view;

        public SoundGroupDataPresenter(
            SoundGroupData soundGroupData,
            ISoundGroupDataView view)
        {
            _soundGroupData = soundGroupData ?? throw new ArgumentNullException(nameof(soundGroupData));
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