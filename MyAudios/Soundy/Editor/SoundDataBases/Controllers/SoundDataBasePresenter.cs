using System;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundDataBases.Controllers
{
    public class SoundDataBasePresenter : IPresenter
    {
        private readonly SoundDatabase _soundDatabase;
        private readonly ISoundDataBaseView _view;
        private readonly SoundGroupViewFactory _soundGroupViewFactory;

        public SoundDataBasePresenter(
            SoundDatabase soundDatabase, 
            ISoundDataBaseView view,
            SoundGroupViewFactory soundGroupViewFactory)
        {
            _soundDatabase = soundDatabase ?? throw new ArgumentNullException(nameof(soundDatabase));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _soundGroupViewFactory = soundGroupViewFactory;
        }

        public void Initialize()
        {
            CreateSoundGroups();
        }

        public void Dispose()
        {
            
        }
        
        private void CreateSoundGroups()
        {
            foreach (var soundGroup in _soundDatabase.Database)
            {
                ISoundGroupView view = _soundGroupViewFactory.Create(soundGroup);
                view.SetDataBase(_view);
                _view.AddSoundGroup(view);
            }
        }
    }
}