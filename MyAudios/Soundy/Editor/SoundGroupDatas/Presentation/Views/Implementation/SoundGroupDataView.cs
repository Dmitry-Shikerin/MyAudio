using System;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.AudioDatas.View.Interfaces;
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
        private SoundGroupDataVisualElement _soundGroupDataVisualElement;
        private SerializedProperty _sequenceResetTime;
        public VisualElement Root { get; private set; }

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
        }

        public void Dispose()
        {
            
        }

        public void AddAudioData(IAudioDataView audioDataView)
        {
            _soundGroupDataVisualElement.AudioDataContent.AddChild(audioDataView.Root);
        }
    }
}