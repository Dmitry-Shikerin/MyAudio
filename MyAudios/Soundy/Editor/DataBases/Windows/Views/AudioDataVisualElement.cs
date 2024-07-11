using System.Linq;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.DataBases.Domain.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class AudioDataVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public VisualElement SlidersContainer { get; private set; }
        public FluidButton PlayButton { get; private set; }
        public FluidButton DeleteButton { get; private set; }
        public string Label { get; set; }
        public FluidRangeSlider TopSlider { get; private set; }
        public AudioData AudioData { get; set; }
        public bool IsPlaying { get; private set; }
        public SoundGroupData ParentDatabase { get; private set; }
        public VisualElement TopLine { get; private set; }
        public VisualElement BotLine { get; set; }
        public ObjectField ObjectField { get; private set; }
        public Label LabelField { get; private set; }

        public AudioDataVisualElement()
        {
        }

        public AudioDataVisualElement Initialize()
        {
            Container =
                DesignUtils.column
                    .ResetLayout()
                    .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);

            TopLine = DesignUtils.row;
            BotLine = DesignUtils.row;
            
            LabelField = DesignUtils
                .NewLabel()
                .SetText("AudioClip");
            LabelField
                .SetStyleColor(EditorColors.Default.WindowHeaderTitle)
                .SetStyleMinWidth(70);
            
            ObjectField = new ObjectField();
            ObjectField.SetStyleFlexGrow(1);
            SerializedObject audioDatSerializedObject =
                new SerializedObject(AudioData.AudioClip);
            ObjectField.RegisterCallback<ChangeEvent<Object>>(
                (evt) =>
                    AudioData.AudioClip = evt.newValue != null
                        ? evt.newValue as AudioClip
                        : null);
            ObjectField
                .SetObjectType(typeof(AudioClip))
                .BindProperty(audioDatSerializedObject);
            ObjectField.SetValueWithoutNotify(AudioData.AudioClip);
            
            PlayButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Play)
                    .SetOnClick(() =>
                    {
                        ChangeSoundGroupState();
                    });                            

            SlidersContainer = DesignUtils.column;
            
            TopSlider = new FluidRangeSlider().SetStyleMaxHeight(18);
            TopSlider.slider.highValue = AudioData.AudioClip != null 
                ? AudioData.AudioClip.length 
                : default;
            
            TopSlider
                .slider
                .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                .SetStyleColor(EditorColors.EditorUI.Orange);
            
            SlidersContainer
                .AddChild(TopSlider);
            
            DeleteButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Minus)
                ;
            
            TopLine
                .AddChild(PlayButton)
                .AddChild(SlidersContainer)
                ;

            BotLine
                .AddChild(LabelField)
                .AddChild(ObjectField)
                .AddChild(new VisualElement().SetStyleMinWidth(7))
                .AddChild(DeleteButton)
                ;

            Container
                .AddChild(TopLine)
                .AddSpaceBlock()
                .AddChild(BotLine);

            Add(Container);

            return this;
        }

        private void ChangeSoundGroupState()
        {
            if (IsPlaying == false)
                PlaySound();
            else
                StopSound();
        }

        private void UpdateSliderValue()
        {
            TopSlider.slider.value = Object.FindObjectOfType<AudioSource>().time;
        }

        public void PlaySound()
        {
            // Parent.StopAllSounds();
            EditorApplication.update += UpdateSliderValue;
            IsPlaying = true;
            PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop);
            ParentDatabase.PlaySoundPreview(
                Object.FindObjectOfType<AudioSource>(),
                null, AudioData.AudioClip);
        }

        public void StopSound()
        {
            EditorApplication.update -= UpdateSliderValue;
            IsPlaying = false;
            PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);
            ParentDatabase.StopSoundPreview(
                Object.FindObjectOfType<AudioSource>());
            TopSlider.slider.value = 0;
        }
        
        public AudioDataVisualElement SetLabelText(string labelText)
        {
            Label = labelText;
            
            return this;
        }

        public AudioDataVisualElement SetSoundGroupData(SoundGroupData parentData)
        {
            ParentDatabase = parentData;
            
            return this;
        }

        public AudioDataVisualElement SetAudioData(AudioData audioData)
        {
            AudioData = audioData;
            
            return this;
        }
        
        public AudioDataVisualElement SetPlayOnClick(UnityAction callback)
        {
            PlayButton.SetOnClick(callback);
            
            return this;
        }
    }
}