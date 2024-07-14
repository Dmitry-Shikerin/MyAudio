using System;
using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.NewSoundContents.Presentation.Controlls;
using MyAudios.Soundy.Editor.SoundDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Implementation
{
    public class SoundDataBaseView : ISoundDataBaseView
    {
        private SoundDataBasePresenter _presenter;
        private SoundDataBaseHeaderVisualElement _headerVisualElement;
        private NewSoundContentVisualElement _newSoundContentVisualElement;
        private SoundDataBaseAudioMixerVisualElement _audioMixerVisualElement;
        private VisualElement _spaseElement;
        private ScrollView _scrollView;
        private VisualElement _soundGroupsContainer;
        private List<ISoundGroupView> _soundGroups;
        
        public VisualElement Root { get; private set; }

        public void Construct(SoundDataBasePresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _headerVisualElement = new SoundDataBaseHeaderVisualElement();
            _newSoundContentVisualElement = new NewSoundContentVisualElement();
            _audioMixerVisualElement = new SoundDataBaseAudioMixerVisualElement();
            VisualElement spaseLine = DesignUtils.row
                .SetStyleMaxHeight(4)
                .SetStyleMinHeight(4)
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStyleBorderRadius(4,4,4,4);
            _spaseElement = DesignUtils.column
                .SetStyleMinWidth(25)
                .SetStyleMaxHeight(25)
                .AddChild(DesignUtils.row)
                .AddChild(spaseLine)
                .AddChild(DesignUtils.row);
            _scrollView = new ScrollView();
            _soundGroupsContainer = DesignUtils
                .column
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .AddSpace(4)
                .AddChild(_scrollView);
            
            Root = DesignUtils.column
                .AddChild(_headerVisualElement)
                .AddSpace(4)
                .AddChild(_audioMixerVisualElement)
                .AddSpace(4)
                .AddChild(_spaseElement)
                .AddSpace(4)
                .AddChild(_newSoundContentVisualElement)
                .AddChild(_soundGroupsContainer);
        }
        
        public void RemoveSoundGroup(ISoundGroupView soundGroupView)
        {
            _soundGroups.Remove(soundGroupView);
            Root.Remove(soundGroupView.Root);
        }

        public void StopAllSoundGroup()
        {
            foreach (ISoundGroupView soundGroup in _soundGroups)
                soundGroup.StopPlaySound();
        }

        public void AddSoundGroup(ISoundGroupView soundGroupView)
        {
            _scrollView
                .AddChild(soundGroupView.Root)
                .AddSpace(4);
            _soundGroups.Add(soundGroupView);
        }

        public void Initialize()
        {
            // _newSoundContentVisualElement
            //     .SetOnClick(() =>
            //     {
            //         // _presenter.Add(
            //         //     _newSoundContentVisualElement.SoundGroupTextField.value, false, true);
            //         //Сделать рефрешь елементов
            //         // UpdateDataBase(CurrentSoundDatabase);
            //     });
            _soundGroups = new List<ISoundGroupView>();
            _presenter.Initialize();
        }


        public void Dispose()
        {
            
        }
    }
}