using System;
using System.Collections.Generic;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Editor.SoundDataBases.Controllers;
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
            _headerVisualElement = 
                new SoundDataBaseHeaderVisualElement();
            _newSoundContentVisualElement =
                new NewSoundContentVisualElement();
            _soundGroups = new List<ISoundGroupView>();
            
            Root = DesignUtils.column
                .AddChild(_headerVisualElement)
                .AddChild(_newSoundContentVisualElement);
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
            Root.AddChild(soundGroupView.Root);
            _soundGroups.Add(soundGroupView);
        }

        public void Initialize()
        {
            _newSoundContentVisualElement
                .SetOnClick(() =>
                {
                    // _presenter.Add(
                    //     _newSoundContentVisualElement.SoundGroupTextField.value, false, true);
                    //Сделать рефрешь елементов
                    // UpdateDataBase(CurrentSoundDatabase);
                });
            _presenter.Initialize();
        }


        public void Dispose()
        {
            
        }
    }
}