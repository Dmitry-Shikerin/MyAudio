using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Events;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Editor.MinMaxSliders;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Controlls
{
    public class SoundGroupDataVisualElement : VisualElement
    {
        public SoundGroupDataVisualElement()
        {
            Root = new VisualElement();
            InitializeHeader();
            Compose();
        }

        public VisualElement Root { get; private set; }
        public ScrollView AudioDataContent { get; set; }
        private FluidComponentHeader Header { get; set; }
        public Label Label { get; private set; }
        public FluidToggleButtonTab RandomButtonTab { get; private set; }
        public FluidToggleButtonTab SequenceButtonTab { get; private set; }
        public FluidToggleSwitch ResetSequenceAfterInactiveTimeToggle { get; private set; }
        public FloatField SequenceResetTimeField { get; private set; }
        public FluidToggleSwitch LoopToggle { get; private set; }
        public FluidMinMaxSlider VolumeMinMaxSlider { get; private set; }
        public FluidButton ResetVolumeButton { get; private set; }
        public FluidMinMaxSlider PitchMinMaxSlider { get; private set; }
        public FluidButton ResetPitchButton { get; private set; }
        public NewSoundContentVisualElement NewSoundContentVisualElement { get; private set; }

        private void InitializeHeader()
        {
            Header =
                FluidComponentHeader
                    .Get()
                    .SetComponentNameText("Sound Group")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetAccentColor(EditorColors.EditorUI.Orange)
                ;
        }

        private void Compose()
        {
            Label = DesignUtils
                .NewLabel("");
            VisualElement labelRow = DesignUtils.row.AddChild(Label);
            
            //PlayMode
            FluidToggleGroup playModeToggleGroup = new FluidToggleGroup();
            playModeToggleGroup.iconContainer.image = EditorTextures.UIManager.Icons.UIToggleGroup;
            playModeToggleGroup
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetLabelText("Play Mode");
            RandomButtonTab = new FluidToggleButtonTab();
            RandomButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Random")
                // .SetOnClick(() => SoundGroupData.Mode = SoundGroupData.PlayMode.Random)
                ;
            RandomButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(RandomButtonTab);
            SequenceButtonTab = new FluidToggleButtonTab();
            SequenceButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Sequence")
                // .SetOnClick(() => SoundGroupData.Mode = SoundGroupData.PlayMode.Sequence)
                ;
            SequenceButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(SequenceButtonTab);
            
            // Action changePlayMode = SoundGroupData.Mode switch
            // {
            //     SoundGroupData.PlayMode.Random => () => RandomButtonTab.isOn = true,
            //     SoundGroupData.PlayMode.Sequence => () => SequenceButtonTab.isOn = true,
            //     _ => throw new ArgumentOutOfRangeException()
            // };
            //
            // changePlayMode?.Invoke();

            //ResetSequenceAfterInactiveTime
            Label resetSequenceAfterInactiveTimeLabel = 
                DesignUtils.NewLabel("Auto Reset Sequence After");
            ResetSequenceAfterInactiveTimeToggle = 
                new FluidToggleSwitch();
            // resetSequenceAfterInactiveTimeToggle.OnValueChanged -= ChangeResetSequenceAfterInactiveTimeRow;
            // resetSequenceAfterInactiveTimeToggle.OnValueChanged += ChangeResetSequenceAfterInactiveTimeRow;
            VisualElement resetSequenceAfterInactiveTimeRow = DesignUtils
                .row
                .AddChild(ResetSequenceAfterInactiveTimeToggle)
                .AddChild(resetSequenceAfterInactiveTimeLabel)
                .SetStyleFlexGrow(0)
                .SetStyleMarginRight(5);
            
            Label sequenceResetTimeLabel = 
                DesignUtils.NewLabel("seconds");
            // SequenceResetTimeField = DesignUtils
            //     .NewFloatField(SequenceResetTime)
            //     .ResetLayout()
            //     .SetStyleFlexGrow(0);
            // SequenceResetTimeField
            //     .Q<VisualElement>("unity-text-input")
            //     .SetStyleFlexGrow(0)
            //     .SetStyleMinWidth(50)
            //     .SetStyleMarginRight(5);
            VisualElement sequenceResetTimeRow = DesignUtils
                .row
                // .AddChild(SequenceResetTimeField)
                .AddChild(sequenceResetTimeLabel);
            VisualElement sequenceTimeRow = DesignUtils.row
                .ResetLayout()
                .AddChild(resetSequenceAfterInactiveTimeRow)
                .AddChild(sequenceResetTimeRow);
            VisualElement playModeRow = DesignUtils
                .column
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .AddChild(playModeToggleGroup)
                .AddSpaceBlock(2)
                .AddChild(sequenceTimeRow);
            
            VisualElement toggleGroupRow = DesignUtils
                .row
                .AddChild(RandomButtonTab)
                .AddChild(SequenceButtonTab);
            playModeToggleGroup
                .AddSpaceBlock()
                .AddChild(toggleGroupRow);
            
            //Loop
            Label loopLabel = DesignUtils.NewLabel("Loop");
            LoopToggle = 
                new FluidToggleSwitch();
            // loopToggle.OnValueChanged -= ChangeLoop;
            // loopToggle.OnValueChanged += ChangeLoop;
            VisualElement loopRow = DesignUtils
                .row
                .AddChild(loopLabel)
                .AddChild(LoopToggle)
                .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            //Sliders
            Label volumeLabel = 
                DesignUtils
                    .NewLabel("Volume")
                    .SetStyleMinWidth(70);
            VolumeMinMaxSlider = new FluidMinMaxSlider();
            // volumeMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            // volumeMinMaxSlider.slider.value = new Vector2(
            //     SoundGroupData.Volume.MinValue, SoundGroupData.Volume.MaxValue);
            // volumeMinMaxSlider.slider.lowLimit = SoundGroupDataConst.MinVolume;
            // volumeMinMaxSlider.slider.highLimit = SoundGroupDataConst.MaxVolume;
            // volumeMinMaxSlider.slider.RegisterValueChangedCallback((value) =>
            // {
            //     SoundGroupData.Volume.MinValue = value.newValue.x;
            //     SoundGroupData.Volume.MaxValue = value.newValue.y;
            // });

            ResetVolumeButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset);
                // .AddOnClick(() =>
                // {
                //     volumeMinMaxSlider.slider.value = new Vector2(
                //         SoundGroupDataConst.DefaultVolume, SoundGroupDataConst.DefaultVolume);
                // });
            
            VisualElement volumeRow = DesignUtils
                .row
                .AddChild(volumeLabel)
                .AddChild(VolumeMinMaxSlider)
                .AddChild(ResetVolumeButton);
            
            Label pitchLabel = 
                DesignUtils
                    .NewLabel("Pitch")
                    .SetStyleMinWidth(70);
            PitchMinMaxSlider = new FluidMinMaxSlider();
            // pitchMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            // pitchMinMaxSlider.slider.value = new Vector2(SoundGroupData.Pitch.MinValue, SoundGroupData.Pitch.MaxValue);
            // pitchMinMaxSlider.slider.lowLimit = SoundGroupDataConst.MinPitch;
            // pitchMinMaxSlider.slider.highLimit = SoundGroupDataConst.MaxPitch;
            // pitchMinMaxSlider.slider.RegisterValueChangedCallback((value) =>
            // {
            //     SoundGroupData.Pitch.MinValue = value.newValue.x;
            //     SoundGroupData.Pitch.MaxValue = value.newValue.y;
            // });

            ResetPitchButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset);
                // .AddOnClick(() =>
                // {
                //     pitchMinMaxSlider.slider.value = new Vector2(
                //         SoundGroupDataConst.DefaultPitch, SoundGroupDataConst.DefaultPitch);
                // });
            
            VisualElement pitchRow = DesignUtils
                .row
                .AddChild(pitchLabel)
                .AddChild(PitchMinMaxSlider)
                .AddChild(ResetPitchButton);
            
            Label spatialBlendLabel = 
                DesignUtils
                    .NewLabel("SpatialBlend")
                    .SetStyleMinWidth(70);
            FluidRangeSlider spatialBlendSlider = new FluidRangeSlider();
            spatialBlendSlider.SetStyleFlexGrow(1);
            spatialBlendSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            
            VisualElement spatialBlendRow = DesignUtils
                .row
                .AddChild(spatialBlendLabel)
                .AddChild(spatialBlendSlider);
            
            FluidAnimatedContainer slidersContainer = 
                new FluidAnimatedContainer()
                    .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            //SlidersToggles
            FluidToggleGroup slidersToggleGroup = new FluidToggleGroup();
            slidersToggleGroup.iconContainer.image = EditorTextures.UIManager.Icons.UIToggleGroup;
            slidersToggleGroup
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetLabelText("Sliders");

            FluidToggleButtonTab volumeButtonTab = new FluidToggleButtonTab();
            FluidToggleButtonTab pitchButtonTab = new FluidToggleButtonTab();
            FluidToggleButtonTab spatialBlendButtonTab = new FluidToggleButtonTab();

            volumeButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Volume")
                .SetOnClick(() =>
                {
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    volumeButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);
                    
                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(volumeRow)
                        .Show();
                });
            volumeButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(volumeButtonTab);

            pitchButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Pitch")
                .SetOnClick(() => 
                { 
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    pitchButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);
                    
                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(pitchRow)
                        .Show();
                });
            pitchButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(pitchButtonTab);

            spatialBlendButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Spatial Blend")
                .SetOnClick(() =>
                {
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    spatialBlendButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);
                    
                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(spatialBlendRow)
                        .Show();
                });
            spatialBlendButtonTab.AddOnValueChanged((isOn) =>
            {
                    spatialBlendButtonTab.ResetColors();
            });
            spatialBlendButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(spatialBlendButtonTab);


            VisualElement slidersToggleGroupRow = DesignUtils
                .row
                .AddChild(volumeButtonTab)
                .AddChild(pitchButtonTab)
                .AddChild(spatialBlendButtonTab);
            slidersToggleGroup
                .AddSpaceBlock()
                .AddChild(slidersToggleGroupRow)
                .AddChild(slidersContainer);

            NewSoundContentVisualElement =
                new NewSoundContentVisualElement();

            AudioDataContent = 
                new ScrollView()
                    .ResetLayout()
                    .SetStyleFlexGrow(1)
                    .SetStyleFlexShrink(1);

            VisualElement topContent = 
                DesignUtils
                    .column
                    .SetStyleMinHeight(300)
                    .AddChild(Header)
                    .AddChild(labelRow)
                    .AddSpaceBlock(2)
                    .AddChild(playModeRow)
                    .AddSpaceBlock(2)
                    .AddChild(loopRow)
                    .AddSpaceBlock(2)
                    .AddChild(slidersToggleGroup)
                    .AddSpaceBlock(2)
                    .AddChild(NewSoundContentVisualElement)
                    .AddSpaceBlock(2)
                ;

            Root
                .AddChild(topContent)
                .AddSpaceBlock(2)
                .AddChild(AudioDataContent)
                ;
            Debug.Log($"Root filled {Root}");
        }
    }
}