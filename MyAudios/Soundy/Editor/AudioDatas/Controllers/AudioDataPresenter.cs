using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.View.Interfaces;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyAudios.Soundy.Editor.AudioDatas.Controllers
{
    public class AudioDataPresenter : IPresenter
    {
        private readonly AudioData _audioData;
        private readonly SoundGroupData _soundGroupData;
        private readonly IAudioDataView _view;

        public AudioDataPresenter(
            AudioData audioData,
            SoundGroupData soundGroupData,
            IAudioDataView view)
        {
            _audioData = audioData ?? throw new ArgumentNullException(nameof(audioData));
            _soundGroupData = soundGroupData ?? throw new ArgumentNullException(nameof(soundGroupData));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            
            Initialize();
        }

        public void Initialize()
        {
            _view.SetLabelText(_audioData.AudioClip.name);
        }

        public void Dispose()
        {
        }

        public void PlaySound()
        {
            _audioData.IsPlaying = true;
            _soundGroupData.IsPlaying = true;
            EditorApplication.update += SetSliderValue;
            _view.SetStopIcon();
            _soundGroupData.PlaySoundPreview(
                Object.FindObjectOfType<AudioSource>(),
                null, _audioData.AudioClip);
        }
        
        private void SetSliderValue()
        {
            _view.SetSliderValue(_audioData.AudioClip.length);
        }

        public void StopSound()
        {
            _audioData.IsPlaying = false;
            _soundGroupData.IsPlaying = false;
            EditorApplication.update -= SetSliderValue;
            _view.SetPlayIcon();
            _soundGroupData.StopSoundPreview(
                Object.FindObjectOfType<AudioSource>());
            _view.SetSliderValue(0);
        }

        public void DeleteAudioData()
        {
            _soundGroupData.RemoveAudioData(_audioData);
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            _audioData.AudioClip = audioClip;
        }

        public void ChangeSoundGroupState()
        {
            if (_audioData.IsPlaying)
                PlaySound();
            else
                StopSound();
        }
    }
}