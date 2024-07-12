using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Events;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIManager.UIMenu;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Editors.UXMLs;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Editors
{
    [CustomEditor(typeof(SoundGroupData))]
    public class SoundGroupDataEditor : UnityEditor.Editor
    {
        private SoundGroupData SoundGroupData { get; set; }
        
        private SerializedProperty Name { get; set; }
        private SerializedProperty Sounds { get; set; }   
        private SerializedProperty PlayModeProperty { get; set; }
        private SerializedProperty Loop { get; set; }
        private SerializedProperty Volume { get; set; }
        private SerializedProperty Pitch { get; set; }
        private SerializedProperty SpatialBlend { get; set; }
        public SerializedProperty ResetSequenceAfterInactiveTime { get; set; }
        public SerializedProperty SequenceResetTime { get; set; }

        public VisualElement Root { get; private set; }
        public ScrollView AudioDataContent { get; set; }
        private FluidComponentHeader Header { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            InitializeEditor();
            Compose();

            return Root;
        }

        private void FindProperties()
        {
            SoundGroupData = (SoundGroupData)serializedObject.targetObject;
            Name = serializedObject.FindProperty(nameof(SoundGroupData.DatabaseName));
            PlayModeProperty = serializedObject.FindProperty(nameof(SoundGroupData.Mode));
            Loop = serializedObject.FindProperty(nameof(SoundGroupData.Loop));
            ResetSequenceAfterInactiveTime = 
                serializedObject.FindProperty(nameof(SoundGroupData.ResetSequenceAfterInactiveTime));
            SequenceResetTime = serializedObject.FindProperty(nameof(SoundGroupData.SequenceResetTime));
            Volume = serializedObject.FindProperty(nameof(SoundGroupData.Volume));
            Pitch = serializedObject.FindProperty(nameof(SoundGroupData.Pitch));
            SpatialBlend = serializedObject.FindProperty(nameof(SoundGroupData.SpatialBlend));
            Sounds = serializedObject.FindProperty(nameof(SoundGroupData.Sounds));
        }

        private void InitializeEditor()
        {
            Root = new VisualElement();

            InitializeHeader();
        }

        private void Compose()
        {
            Label label = DesignUtils
                .NewLabel(Name.stringValue);
            VisualElement labelRow = DesignUtils.row.AddChild(label);
            
            //PlayMode
            FluidToggleGroup playModeToggleGroup = new FluidToggleGroup();
            playModeToggleGroup.iconContainer.image = EditorTextures.UIManager.Icons.UIToggleGroup;
            playModeToggleGroup
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetLabelText("Play Mode");
            FluidToggleButtonTab randomButtonTab = new FluidToggleButtonTab();
            randomButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Random")
                .SetOnClick(() => SoundGroupData.Mode = SoundGroupData.PlayMode.Random);
            randomButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(randomButtonTab);
            FluidToggleButtonTab sequenceButtonTab = new FluidToggleButtonTab();
            sequenceButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Sequence")
                .SetOnClick(() => SoundGroupData.Mode = SoundGroupData.PlayMode.Sequence);
            sequenceButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(sequenceButtonTab);
            
            Action changePlayMode = SoundGroupData.Mode switch
            {
                SoundGroupData.PlayMode.Random => () => randomButtonTab.isOn = true,
                SoundGroupData.PlayMode.Sequence => () => sequenceButtonTab.isOn = true,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            changePlayMode?.Invoke();

            //ResetSequenceAfterInactiveTime
            Label resetSequenceAfterInactiveTimeLabel = 
                DesignUtils.NewLabel("Auto Reset Sequence After");
            FluidToggleSwitch resetSequenceAfterInactiveTimeToggle = 
                new FluidToggleSwitch();
            resetSequenceAfterInactiveTimeToggle.OnValueChanged -= ChangeResetSequenceAfterInactiveTimeRow;
            resetSequenceAfterInactiveTimeToggle.OnValueChanged += ChangeResetSequenceAfterInactiveTimeRow;
            VisualElement resetSequenceAfterInactiveTimeRow = DesignUtils
                .row
                .AddChild(resetSequenceAfterInactiveTimeToggle)
                .AddChild(resetSequenceAfterInactiveTimeLabel)
                .SetStyleFlexGrow(0)
                .SetStyleMarginRight(5);
            
            Label sequenceResetTimeLabel = 
                DesignUtils.NewLabel("seconds");
            FloatField sequenceResetTimeField = DesignUtils
                .NewFloatField(SequenceResetTime)
                .ResetLayout()
                .SetStyleFlexGrow(0);
            sequenceResetTimeField
                .Q<VisualElement>("unity-text-input")
                .SetStyleFlexGrow(0)
                .SetStyleMinWidth(50)
                .SetStyleMarginRight(5);
            VisualElement sequenceResetTimeRow = DesignUtils
                .row
                .AddChild(sequenceResetTimeField)
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
                .AddChild(randomButtonTab)
                .AddChild(sequenceButtonTab);
            playModeToggleGroup
                .AddSpaceBlock()
                .AddChild(toggleGroupRow);
            
            //Loop
            Label loopLabel = DesignUtils.NewLabel("Loop");
            FluidToggleSwitch loopToggle = 
                new FluidToggleSwitch();
            loopToggle.OnValueChanged -= ChangeLoop;
            loopToggle.OnValueChanged += ChangeLoop;
            VisualElement loopRow = DesignUtils
                .row
                .AddChild(loopLabel)
                .AddChild(loopToggle)
                .SetStyleBackgroundColor(EditorColors.Default.Background);
            
            //Sliders
            Label volumeLabel = 
                DesignUtils
                    .NewLabel("Volume")
                    .SetStyleMinWidth(70);
            FluidMinMaxSlider volumeMinMaxSlider = new FluidMinMaxSlider();
            volumeMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            volumeMinMaxSlider.slider.value = new Vector2(
                SoundGroupData.Volume.MinValue, SoundGroupData.Volume.MaxValue);
            volumeMinMaxSlider.slider.lowLimit = SoundGroupDataConst.MinVolume;
            volumeMinMaxSlider.slider.highLimit = SoundGroupDataConst.MaxVolume;
            volumeMinMaxSlider.slider.RegisterValueChangedCallback((value) =>
            {
                SoundGroupData.Volume.MinValue = value.newValue.x;
                SoundGroupData.Volume.MaxValue = value.newValue.y;
            });
            
            FluidButton resetVolumeButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset)
                .AddOnClick(() =>
                {
                    volumeMinMaxSlider.slider.value = new Vector2(
                        SoundGroupDataConst.DefaultVolume, SoundGroupDataConst.DefaultVolume);
                });
            
            VisualElement volumeRow = DesignUtils
                .row
                .AddChild(volumeLabel)
                .AddChild(volumeMinMaxSlider)
                .AddChild(resetVolumeButton);
            
            Label pitchLabel = 
                DesignUtils
                    .NewLabel("Pitch")
                    .SetStyleMinWidth(70);
            FluidMinMaxSlider pitchMinMaxSlider = new FluidMinMaxSlider();
            pitchMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            pitchMinMaxSlider.slider.value = new Vector2(SoundGroupData.Pitch.MinValue, SoundGroupData.Pitch.MaxValue);
            pitchMinMaxSlider.slider.lowLimit = SoundGroupDataConst.MinPitch;
            pitchMinMaxSlider.slider.highLimit = SoundGroupDataConst.MaxPitch;
            pitchMinMaxSlider.slider.RegisterValueChangedCallback((value) =>
            {
                SoundGroupData.Pitch.MinValue = value.newValue.x;
                SoundGroupData.Pitch.MaxValue = value.newValue.y;
            });
            
            FluidButton resetPitchButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset)
                .AddOnClick(() =>
                {
                    pitchMinMaxSlider.slider.value = new Vector2(
                        SoundGroupDataConst.DefaultPitch, SoundGroupDataConst.DefaultPitch);
                });
            
            VisualElement pitchRow = DesignUtils
                .row
                .AddChild(pitchLabel)
                .AddChild(pitchMinMaxSlider)
                .AddChild(resetPitchButton);
            
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

            NewSoundContentVisualElement newSoundContentVisualElement = 
                new NewSoundContentVisualElement()
                    .SetOnClick(() =>
                    {
                        SoundGroupData.AddAudioData();
                        RefreshAudioData();
                    });
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
                    .AddChild(newSoundContentVisualElement)
                    .AddSpaceBlock(2)
                ;

            Root
                .AddChild(topContent)
                .AddSpaceBlock(2)
                .AddChild(AudioDataContent)
                ;

                RefreshAudioData();
        }

        public void RefreshAudioData()
        {
            AudioDataContent.Clear();
            
            foreach (AudioData audioData in SoundGroupData.Sounds)
            {
                AudioDataVisualElement audioDataVisualElement =
                    new AudioDataVisualElement()
                        .SetAudioData(audioData)
                        .SetSoundGroupData(SoundGroupData)
                        .Initialize()
                        .SetDeleteOnClick(() =>
                        {
                            SoundGroupData.RemoveAudioData(audioData);
                            RefreshAudioData();
                        });
                
                AudioDataContent
                    .AddChild(audioDataVisualElement)
                    .AddSpaceBlock();
            }
        }

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
        
        private void ChangeLoop(FluidBoolEvent newValue) =>
            SoundGroupData.Loop = newValue.newValue;
        
        private void ChangeResetSequenceAfterInactiveTimeRow(FluidBoolEvent newValue) =>
            SoundGroupData.ResetSequenceAfterInactiveTime = newValue.newValue;
    }
}