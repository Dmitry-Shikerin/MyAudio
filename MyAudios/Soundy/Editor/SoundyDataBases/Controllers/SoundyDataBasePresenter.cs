﻿using System;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundyDataBases.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Controllers
{
    public class SoundyDataBasePresenter : IPresenter
    {
        private readonly SoundyDatabase _soundyDatabase;
        private readonly ISoundyDataBaseView _view;
        private readonly SoundDataBaseViewFactory _soundDataBaseViewFactory;

        public SoundyDataBasePresenter(
            SoundyDatabase soundyDatabase,
            ISoundyDataBaseView view,
            SoundDataBaseViewFactory soundDataBaseViewFactory)
        {
            _soundyDatabase = soundyDatabase ?? throw new ArgumentNullException(nameof(soundyDatabase));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _soundDataBaseViewFactory = soundDataBaseViewFactory ?? 
                                        throw new ArgumentNullException(nameof(soundDataBaseViewFactory));
        }

        public void Initialize()
        {
            AddDataBasesButtons();
        }

        public void Dispose()
        {
            
        }

        private void CreateView(SoundDatabase soundDatabase)
        {
            ISoundDataBaseView view = _soundDataBaseViewFactory.Create(soundDatabase, _soundyDatabase);
            view.SetSoundyDataBaseView(_view);
            _view.SetSoundDataBase(view);
        }

        private void AddDataBasesButtons()
        {
            foreach (SoundDatabase database in _soundyDatabase.SoundDatabases)
                _view.AddDataBaseButton(database.DatabaseName,() => CreateView(database));
        }

        public void CreateNewDataBase()
        {
            _soundyDatabase.CreateSoundDatabase("Default", true, true);
            _view.RefreshDataBasesButtons();
            AddDataBasesButtons();
        }

        public void RefreshDataBases() =>
            _soundyDatabase.RefreshDatabase();

        public void RenameButtons()
        {
            for (int i = 0; i < _view.DatabaseButtons.Count; i++)
                _view.DatabaseButtons[i].SetLabelText(_soundyDatabase.SoundDatabases[i].DatabaseName);
        }

        public void UpdateDataBase()
        {
            _view.ClearButtons();
            AddDataBasesButtons();
        }
    }
}