using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors.Windows;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyAudios.Soundy.Editor.SoundGroups.Controllers
{
    public class SoundGroupPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroupData;
        private readonly ISoundGroupView _view;
        private readonly AudioSource _audioSource;

        public SoundGroupPresenter(
            SoundGroupData soundGroup, 
            ISoundGroupView view)
        {
            _soundGroupData = soundGroup ?? throw new ArgumentNullException(nameof(soundGroup));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _audioSource = Object.FindObjectOfType<AudioSource>();
        }

        public void Initialize()
        {
            _soundGroupData.IsPlaying = false;
        }

        public void Dispose()
        {
            
        }

        public void ShowSoundGroupData() =>
            SoundGroupDataEditorWindow.Open(_soundGroupData);
        
        public void StopSound()
        {
            _soundGroupData.IsPlaying = false;
            EditorApplication.update -= SetSliderValue;
            _view.SetPlayIcon();
            _soundGroupData.StopSoundPreview(_audioSource);
            _view.SetSliderValue(0);
        }

        private void PlaySound()
        {
            _view.StopAllAudioGroup();
            _soundGroupData.IsPlaying = true;
            EditorApplication.update += SetSliderValue;
            _view.SetStopIcon();
            _soundGroupData.PlaySoundPreview(_audioSource, null);
            _view.SetSliderMaxValue(_audioSource.clip.length);
        }

        private void SetSliderValue()
        {
            _view.SetSliderValue(_audioSource.time);
            
            if (_audioSource.time + 0.1f >= _audioSource.clip.length)
                StopSound();
        }

        public void ChangeSoundGroupState()
        {
            if (_soundGroupData.IsPlaying == false)
                PlaySound();
            else
                StopSound();
        }
    }
}