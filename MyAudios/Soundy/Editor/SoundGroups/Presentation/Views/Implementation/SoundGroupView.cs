using System;
using Doozy.Editor.EditorUI.Components;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Editor.SoundGroups.Controllers;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Interfaces;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroups.Presentation.Implementation
{
    public class SoundGroupView : ISoundGroupView
    {
        private SoundGroupPresenter _presenter;
        private SoundGroupVisualElement _soundGroupVisualElement;
        public VisualElement Root { get; private set; }

        public void Construct(SoundGroupPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _soundGroupVisualElement = new SoundGroupVisualElement();
            
            Root = _soundGroupVisualElement;
        }

        public void Initialize()
        {
            // _soundGroupVisualElement.PlayButton
            //     .SetOnClick(() => _presenter.PlaySoundGroup());
            // _soundGroupVisualElement.TopSlider
            // _soundGroupVisualElement.Label
        }

        public void Dispose()
        {
            
        }
    }
}