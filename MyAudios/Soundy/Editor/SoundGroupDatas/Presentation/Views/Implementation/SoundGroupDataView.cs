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
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation
{
    public class SoundGroupDataView : ISoundGroupDataView
    {
        private SoundGroupDataPresenter _presenter;
        private SoundGroupDataVisualElement _soundGroupDataVisualElement;
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
            _soundGroupDataVisualElement = new SoundGroupDataVisualElement();
            Root = _soundGroupDataVisualElement;
        }

        public void Initialize()
        {
            _soundGroupDataVisualElement.RandomButtonTab.SetOnClick(() => 
                _presenter.SetPlayMode(SoundGroupData.PlayMode.Random));
           _soundGroupDataVisualElement.SequenceButtonTab.SetOnClick(() => 
               _presenter.SetPlayMode(SoundGroupData.PlayMode.Sequence));
           _soundGroupDataVisualElement.NewSoundContentVisualElement.CreateButton.SetOnClick(
               () => _presenter.CreateAudioData());
           
           _presenter.Initialize();
        }

        public void Dispose()
        {
            
        }

        public void SetIsOnButtonTab(SoundGroupData.PlayMode playMode)
        {
            Action changePlayMode = playMode switch
            {
                SoundGroupData.PlayMode.Random => () => _soundGroupDataVisualElement.RandomButtonTab.isOn = true,
                SoundGroupData.PlayMode.Sequence => () => _soundGroupDataVisualElement.SequenceButtonTab.isOn = true,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            changePlayMode?.Invoke();
        }
        

        public void AddAudioData(IAudioDataView audioDataView)
        {
            _soundGroupDataVisualElement
                .AudioDataContent
                .AddChild(audioDataView.Root)
                .AddSpace(2);
            //
            // _audioDataViews.Add(audioDataView);
        }
    }
}