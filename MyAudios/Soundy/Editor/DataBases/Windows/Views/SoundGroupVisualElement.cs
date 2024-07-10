using System.Linq;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class SoundGroupVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public VisualElement SlidersContainer { get; private set; }
        public FluidButton PlayButton { get; private set; }
        public FluidButton SoundsDataButton { get; private set; }
        public FluidButton DeleteButton { get; private set; }
        public string Label { get; set; }
        public FluidRangeSlider TopSlider { get; private set; }
        public FluidRangeSlider BotomSlider { get; private set; }
        public SoundGroupData SoundGroupData { get; set; }
        public bool IsPlaying { get; private set; }
        public SoundyDataBaseWindowLayout Parent { get; private set; }

        public SoundGroupVisualElement()
        {
        }

        public SoundGroupVisualElement Initialize()
        {
            Container =
                DesignUtils.row
                    .ResetLayout()
                    .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center);
            
            SoundsDataButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetStyleMinWidth(130)
                    .SetStyleMaxWidth(130)
                    .SetLabelText(Label);
            
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
            TopSlider.slider.highValue = SoundGroupData?.Sounds.FirstOrDefault() != null 
                ? SoundGroupData.Sounds.First().AudioClip.length 
                : default;
            
            TopSlider
                .slider
                .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                .SetStyleColor(EditorColors.EditorUI.Orange);
            
            BotomSlider = new FluidRangeSlider().SetStyleMaxHeight(18);
            BotomSlider.slider.highValue = SoundGroupData?.Sounds.FirstOrDefault() != null 
                ? SoundGroupData.Sounds.First().AudioClip.length 
                : default;
            BotomSlider
                .RegisterCallback<ChangeEvent<float>>(value =>
                {
                    Object.FindObjectOfType<AudioSource>().time = value.newValue;
                });

            SlidersContainer
                .AddChild(TopSlider)
                // .AddChild(BotomSlider)
                ;
            
            DeleteButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Minus)
                ;
            
            Container
                .AddChild(SoundsDataButton)
                .AddChild(PlayButton)
                .AddChild(SlidersContainer)
                .AddChild(DeleteButton)
                ;

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
            Debug.Log($"UpdateSliderValue: {TopSlider.slider.value}");
        }

        public void PlaySound()
        {
            // EditorApplication.update -= UpdateSliderValue;
            EditorApplication.update += UpdateSliderValue;
            Debug.Log($"Listen");
            Parent.StopAllSounds();
            IsPlaying = true;
            PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop);
            SoundGroupData.PlaySoundPreview(
                Object.FindObjectOfType<AudioSource>(),
                null, SoundGroupData.Sounds.First().AudioClip);
        }

        public void StopSound()
        {
            EditorApplication.update -= UpdateSliderValue;
            IsPlaying = false;
            PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);
            SoundGroupData.StopSoundPreview(
                Object.FindObjectOfType<AudioSource>());
        }
        
        public SoundGroupVisualElement SetLabelText(string labelText)
        {
            Label = labelText;
            SoundsDataButton.SetLabelText(labelText);
            
            return this;
        }
        
        public SoundGroupVisualElement SetParent(SoundyDataBaseWindowLayout parentWindow)
        {
            Parent = parentWindow;
            
            return this;
        }

        public SoundGroupVisualElement SetSoundGroup(SoundGroupData soundGroupData)
        {
            SoundGroupData = soundGroupData;
            
            return this;
        }
        
        public SoundGroupVisualElement SetPlayOnClick(UnityAction callback)
        {
            PlayButton.SetOnClick(callback);
            
            return this;
        }
    }
}