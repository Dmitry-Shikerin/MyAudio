using System;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundyDataBases.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Controllers
{
    public class SoundyDataBasePresenter : IPresenter
    {
        private readonly SoundyDatabase _soundyDatabase;
        private readonly ISoundyDataBaseView _view;

        public SoundyDataBasePresenter(
            SoundyDatabase soundyDatabase,
            ISoundyDataBaseView view)
        {
            _soundyDatabase = soundyDatabase ?? throw new ArgumentNullException(nameof(soundyDatabase));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            
        }
    }
}