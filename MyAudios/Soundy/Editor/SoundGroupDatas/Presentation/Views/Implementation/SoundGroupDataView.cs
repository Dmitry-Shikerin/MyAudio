using System;
using System.Collections.Generic;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.SoundGroupDatas.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Controlls;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation
{
    public class SoundGroupDataView : ISoundGroupDataView
    {
        private SoundGroupDataPresenter _presenter;
        private SoundGroupDataVisualElement _visualElement;
        private SerializedProperty _sequenceResetTime;
        private List<IAudioDataView> _audioDataViews = new List<IAudioDataView>();
        public VisualElement Root { get; private set; }

        public IReadOnlyList<IAudioDataView> AudioDataViews => _audioDataViews;

        public void Construct(SoundGroupDataPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _visualElement = new SoundGroupDataVisualElement();
            Root = _visualElement;
        }

        public void Initialize()
        {
            _visualElement.RandomButtonTab.SetOnClick(() => 
                _presenter.SetPlayMode(SoundGroupData.PlayMode.Random));
           _visualElement.SequenceButtonTab.SetOnClick(() => 
               _presenter.SetPlayMode(SoundGroupData.PlayMode.Sequence));
           _visualElement.NewSoundContentVisualElement.CreateButton.SetOnClick(
               () => _presenter.CreateAudioData());
           // _visualElement.RandomButtonTab.SetIsOn(_presenter.PlayMode == SoundGroupData.PlayMode.Random);
           // _visualElement.SequenceButtonTab.SetIsOn(_presenter.PlayMode == SoundGroupData.PlayMode.Sequence);
           _visualElement.VolumeSlider.slider.RegisterValueChangedCallback((value) => 
               _presenter.ChangeVolume(value.newValue));
           _visualElement.PitchSlider.slider.RegisterValueChangedCallback((value) => 
               _presenter.ChangePitch(value.newValue));
           _visualElement.SpatialBlendSlider.slider.RegisterValueChangedCallback((value)
               => _presenter.ChangeSpatialBlend(value.newValue));
           
           _presenter.Initialize();
        }

        public void Dispose()
        {
            
        }
        
        public void SetVolume(Vector2 volume, Vector2 minMaxVolume)
        {
            _visualElement.VolumeSlider.slider.value = volume;
            _visualElement.VolumeSlider.slider.lowLimit = minMaxVolume.x;
            _visualElement.VolumeSlider.slider.highLimit = minMaxVolume.y;
        }

        public void SetPitch(Vector2 volume, Vector2 minMaxVolume)
        {
            _visualElement.PitchSlider.slider.value = volume;
            _visualElement.PitchSlider.slider.lowLimit = minMaxVolume.x;
            _visualElement.PitchSlider.slider.highLimit = minMaxVolume.y;
        }

        public void SetSpatialBlend(float spatialBlend, Vector2 minMaxSpatialBlend)
        {
            _visualElement.SpatialBlendSlider.slider.value = spatialBlend;
            _visualElement.SpatialBlendSlider.slider.lowValue = minMaxSpatialBlend.x;
            _visualElement.SpatialBlendSlider.slider.highValue = minMaxSpatialBlend.y;
        }

        public void SetIsOnButtonTab(SoundGroupData.PlayMode playMode)
        {
            Action changePlayMode = playMode switch
            {
                SoundGroupData.PlayMode.Random => () => _visualElement.RandomButtonTab.isOn = true,
                SoundGroupData.PlayMode.Sequence => () => _visualElement.SequenceButtonTab.isOn = true,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            changePlayMode?.Invoke();
        }
        

        public void AddAudioData(IAudioDataView audioDataView)
        {
            _visualElement
                .AudioDataContent
                .AddChild(audioDataView.Root)
                .AddSpace(2);
        }
    }
}