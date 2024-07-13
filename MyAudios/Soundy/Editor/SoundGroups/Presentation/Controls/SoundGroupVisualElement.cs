using System.Linq;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors.Windows;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroups.Presentation.Controls
{
    public class SoundGroupVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public VisualElement SlidersContainer { get; private set; }
        public FluidButton PlayButton { get; private set; }
        public FluidButton SoundGroupDataButton { get; private set; }
        public FluidButton DeleteButton { get; private set; }
        public string Label { get; set; }
        public FluidRangeSlider TopSlider { get; private set; }
        public SoundGroupData SoundGroupData { get; set; }
        public bool IsPlaying { get; private set; }
        public SoundyDataBaseWindowLayout Parent { get; private set; }

        public SoundGroupVisualElement()
        {
            Initialize();
        }

        private void Initialize()
        {
            Container =
                DesignUtils.row
                    .ResetLayout()
                    .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            SoundGroupDataButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetStyleMinWidth(130)
                    .SetStyleMaxWidth(130)
                    .SetLabelText(Label)
                    .SetOnClick(() => SoundGroupDataEditorWindow.Open(SoundGroupData));
            
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
            
            TopSlider = new FluidRangeSlider()
                .SetStyleMaxHeight(18);
            // AudioClip audioClip = AudioData.AudioClip ?? 
            //                       Resources.Load<AudioClip>("MyAudios/Soundy/Resources/Soundy/Plugs/Christmas Villain Loop");;
            //
            // float sliderValue = 0;
            //
            // if (SoundGroupData.Sounds.FirstOrDefault() != null)
            // {
            //     if (SoundGroupData.Sounds.FirstOrDefault().AudioClip != null)
            //     {
            //         sliderValue = SoundGroupData.Sounds.First().AudioClip.length;
            //     }
            // }
            //
            // TopSlider.slider.highValue = sliderValue;
            //
            // TopSlider.highValueLabel.text = $"{TopSlider.slider.highValue:F2}";
            //
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
                .AddChild(SoundGroupDataButton)
                .AddSpace(4)
                .AddChild(PlayButton)
                .AddSpace(4)
                .AddChild(SlidersContainer)
                .AddSpace(4)
                .AddChild(DeleteButton)
                ;

            Add(Container);
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
    }
}