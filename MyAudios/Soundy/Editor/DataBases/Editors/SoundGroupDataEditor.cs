using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Events;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.DataBases.Domain.Data;
using MyAudios.Soundy.Editor.DataBases.Editors.UXMLs;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using UnityEditor;
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
            
            VisualElement toggleGroupRow = DesignUtils
                .row
                .AddChild(randomButtonTab)
                .AddChild(sequenceButtonTab);
            playModeToggleGroup
                .AddSpaceBlock()
                .AddChild(toggleGroupRow);

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
            
            Label volumeLabel = 
                DesignUtils
                    .NewLabel("Volume")
                    .SetStyleMinWidth(70);
            FluidMinMaxSlider volumeMinMaxSlider = new FluidMinMaxSlider();
            // volumeMinMaxSlider.SetStyleMinHeight(10);
            // volumeMinMaxSlider.contentContainer.SetStyleMinHeight(2);
            volumeMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            
            VisualElement volumeRow = DesignUtils
                .row
                .AddChild(volumeLabel)
                .AddChild(volumeMinMaxSlider);
            
            Label pitchLabel = 
                DesignUtils
                    .NewLabel("Pitch")
                    .SetStyleMinWidth(70);
            FluidMinMaxSlider pitchMinMaxSlider = new FluidMinMaxSlider();
            // pitchMinMaxSlider.SetStyleMinHeight(10);
            // pitchMinMaxSlider.contentContainer.SetStyleMinHeight(2);
            pitchMinMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            
            VisualElement pitchRow = DesignUtils
                .row
                .AddChild(pitchLabel)
                .AddChild(pitchMinMaxSlider);
            
            Label spatialBlendLabel = 
                DesignUtils
                    .NewLabel("SpatialBlend")
                    .SetStyleMinWidth(70);
            FluidRangeSlider spatialBlendSlider = new FluidRangeSlider();
            spatialBlendSlider.SetStyleFlexGrow(1);
            // spatialBlendSlider.SetStyleMinHeight(10);
            // spatialBlendSlider.contentContainer.SetStyleMinHeight(2);
            spatialBlendSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            
            VisualElement spatialBlendRow = DesignUtils
                .row
                .AddChild(spatialBlendLabel)
                .AddChild(spatialBlendSlider);
            
            Root
                .AddChild(Header)
                .AddChild(labelRow)
                .AddSpaceBlock(2)
                .AddChild(playModeRow)
                .AddSpaceBlock(2)
                .AddChild(loopRow)
                .AddSpaceBlock(2)
                .AddChild(volumeRow)
                // .AddSpaceBlock(2)
                .AddChild(pitchRow)
                // .AddSpaceBlock(2)
                .AddChild(spatialBlendRow)
                // .AddSpaceBlock(2)
                ;

            foreach (AudioData audioData in SoundGroupData.Sounds)
            {
                AudioDataVisualElement audioDataVisualElement =
                    new AudioDataVisualElement()
                        .SetAudioData(audioData)
                        .SetSoundGroupData(SoundGroupData)
                        .Initialize();
                
                Root
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