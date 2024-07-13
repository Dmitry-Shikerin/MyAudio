using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.AudioDatas.Controllers;
using MyAudios.Soundy.Editor.AudioDatas.View.Interfaces;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace MyAudios.Soundy.Editor.AudioDatas.View.Implementation
{
    public class AudioDataView : IAudioDataView
    {
        private AudioDataPresenter _presenter;
        private VisualElement _slidersContainer;
        private FluidButton _playButton;
        private FluidButton _deleteButton;
        private FluidRangeSlider _topSlider;
        private AudioClip _audioClip;
        private bool _isPlaying;
        private ObjectField _objectField;
        private Label _label;
        
        public VisualElement Root { get; private set; }

        public void Construct(AudioDataPresenter audioDataPresenter)
        {
            _presenter = audioDataPresenter ?? throw new ArgumentNullException(nameof(audioDataPresenter));
            
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            Root =
                DesignUtils.column
                    .ResetLayout()
                    .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            VisualElement topLine = DesignUtils.row;
            VisualElement botLine = DesignUtils.row;
            
            _label = DesignUtils
                .NewLabel()
                .SetText("AudioClip");
            _label
                .SetStyleColor(EditorColors.Default.WindowHeaderTitle)
                .SetStyleMinWidth(70);
            
            _objectField = new ObjectField();
            _objectField
                .SetStyleFlexGrow(1);
            Object audioClip = _audioClip;
            _objectField.RegisterCallback<ChangeEvent<Object>>(
                (evt) =>
                    _presenter.SetAudioClip(evt.newValue as AudioClip));
            _objectField.SetObjectType(typeof(AudioClip));
            _objectField.SetValueWithoutNotify(audioClip);
            
            _playButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);                            

            _slidersContainer = DesignUtils.column;
            
            _topSlider = new FluidRangeSlider().SetStyleMaxHeight(18);
            _topSlider.slider.highValue = _audioClip != null 
                ? _audioClip.length 
                : default;
            
            _topSlider
                .slider
                .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                .SetStyleColor(EditorColors.EditorUI.Orange);
            
            _slidersContainer
                .AddChild(_topSlider);
            
            _deleteButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Minus);
            
            topLine
                .AddChild(_playButton)
                .AddChild(_slidersContainer)
                ;

            botLine
                .AddChild(_label)
                .AddChild(_objectField)
                .AddChild(new VisualElement().SetStyleMinWidth(7))
                .AddChild(_deleteButton)
                ;

            Root
                .AddChild(topLine)
                .AddSpaceBlock()
                .AddChild(botLine);
        }

        public void Initialize()
        {
            _playButton.SetOnClick(ChangeSoundGroupState);
            _deleteButton.SetOnClick(_presenter.DeleteAudioData);
            _presenter.Initialize();
        }

        private void ChangeSoundGroupState() =>
            _presenter.ChangeSoundGroupState();

        public void SetSliderValue(float value) =>
            _topSlider.slider.value = value;

        public void SetStopIcon() =>
            _playButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop);

        public void SetPlayIcon() =>
            _playButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);
        
        public void SetLabelText(string labelText) =>
            _label.text = labelText;

        public void Dispose()
        {
            _playButton?.Dispose();
            _deleteButton?.Dispose();
            _presenter?.Dispose();
        }
    }
}