using System;
using Doozy.Engine.Soundy;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Controllers
{
    public class SoundGroupDataPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroupData;
        private readonly ISoundGroupDataView _view;
        private readonly AudioDataViewFactory _audioDataViewFactory;

        public SoundGroupDataPresenter(
            SoundGroupData soundGroupData,
            ISoundGroupDataView view,
            AudioDataViewFactory audioDataViewFactory)
        {
            _soundGroupData = soundGroupData ?? throw new ArgumentNullException(nameof(soundGroupData));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _audioDataViewFactory = audioDataViewFactory ?? throw new ArgumentNullException(nameof(audioDataViewFactory));
        }

        public void Initialize()
        {
            AddAudioDatas();
            _view.SetIsOnButtonTab(_soundGroupData.Mode);
        }

        public void Dispose()
        {
            
        }

        private void AddAudioDatas()
        {
            foreach (AudioData audioData in _soundGroupData.Sounds)
            {
                IAudioDataView view = _audioDataViewFactory.Create(audioData, _soundGroupData);
                _view.AddAudioData(view);
            }
        }
        
        private void RemoveAudioDataViews()
        {
            foreach (IAudioDataView audioData in _view.AudioDataViews)
            {
                audioData.Root.RemoveFromHierarchy();
            }
        }

        public void SetPlayMode(SoundGroupData.PlayMode playMode) =>
            _soundGroupData.Mode = playMode;

        public void CreateAudioData()
        {
            AudioData audioData = _soundGroupData.AddAudioData();
            Debug.Log($"Added Audio Data: {_soundGroupData.Sounds.Count}");
            // RemoveAudioDataViews();
            IAudioDataView view = _audioDataViewFactory.Create(audioData, _soundGroupData);
            _view.AddAudioData(view);
        }
    }
}