using System.Linq;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

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
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            SoundsDataButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetStyleMinWidth(130)
                    .SetStyleMaxWidth(130)
                    .SetLabelText(Label)
                    .SetOnClick(() => SoundGroupEditorWindow.Open(SoundGroupData));
            
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
            // AudioClip audioClip = AudioData.AudioClip ?? 
            //                       Resources.Load<AudioClip>("MyAudios/Soundy/Resources/Soundy/Plugs/Christmas Villain Loop");;

            float sliderValue = 0;

            if (SoundGroupData.Sounds.FirstOrDefault() != null)
            {
                if (SoundGroupData.Sounds.FirstOrDefault().AudioClip != null)
                {
                    sliderValue = SoundGroupData.Sounds.First().AudioClip.length;
                }
            }

            TopSlider.slider.highValue = sliderValue;
            
            TopSlider.highValueLabel.text = $"{TopSlider.slider.highValue:F2}";
            
            TopSlider
                .slider
                .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                .SetStyleColor(EditorColors.EditorUI.Orange);
            TopSlider.slider.RegisterCallback<ChangeEvent<float>>(value =>
            {
            });
            
            SlidersContainer
                .AddChild(TopSlider)
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
                .AddSpace(4)
                .AddChild(PlayButton)
                .AddSpace(4)
                .AddChild(SlidersContainer)
                .AddSpace(4)
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
        }

        public void PlaySound()
        {
            Parent.StopAllSounds();
            EditorApplication.update += UpdateSliderValue;
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
            TopSlider.slider.value = 0;
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
        
        public SoundGroupVisualElement SetDeleteOnClick(UnityAction callback)
        {
            DeleteButton.SetOnClick(callback);
            
            return this;
        }
    }
}