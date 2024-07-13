using System;
using Doozy.Engine.Soundy;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.AudioDatas.View.Interfaces;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

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
        }

        public void Dispose()
        {
            
        }

        public void AddAudioDatas()
        {
            foreach (AudioData audioData in _soundGroupData.Sounds)
            {
                IAudioDataView view = _audioDataViewFactory.Create(audioData, _soundGroupData);
                _view.AddAudioData(view);
            }
        }
    }
}