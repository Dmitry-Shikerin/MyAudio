using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Events;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.DataBases.Domain.Data;
using MyAudios.Soundy.Editor.DataBases.Editors.UXMLs;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        private SerializedProperty PlayMode { get; set; }
        private SerializedProperty Loop { get; set; }
        private SerializedProperty Volume { get; set; }
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
            PlayMode = serializedObject.FindProperty(nameof(SoundGroupData.Mode));
            Loop = serializedObject.FindProperty(nameof(SoundGroupData.Loop));
            ResetSequenceAfterInactiveTime = 
                serializedObject.FindProperty(nameof(SoundGroupData.ResetSequenceAfterInactiveTime));
            SequenceResetTime = serializedObject.FindProperty(nameof(SoundGroupData.SequenceResetTime));
            Volume = serializedObject.FindProperty(nameof(SoundGroupData.Volume));
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

            Label playModeLabel = DesignUtils.NewLabel("Play Mode");
            EnumField playModeField = DesignUtils.NewEnumField(PlayMode);
            VisualElement playModeRow = DesignUtils
                .row
                .AddChild(playModeLabel)
                .AddChild(playModeField);
            
            Label loopLabel = DesignUtils.NewLabel("Loop");
            FluidToggleSwitch loopToggle = 
               new FluidToggleSwitch();
            loopToggle.OnValueChanged -= ChangeLoop;
            loopToggle.OnValueChanged += ChangeLoop;
            VisualElement loopRow = DesignUtils
                .row
                .AddChild(loopLabel)
                .AddChild(loopToggle);
            
            Label resetSequenceAfterInactiveTimeLabel = 
                DesignUtils.NewLabel("ResetSequenceAfterInactiveTimeRow");
            FluidToggleSwitch resetSequenceAfterInactiveTimeToggle = 
               new FluidToggleSwitch();
            resetSequenceAfterInactiveTimeToggle.OnValueChanged -= ChangeResetSequenceAfterInactiveTimeRow;
            resetSequenceAfterInactiveTimeToggle.OnValueChanged += ChangeResetSequenceAfterInactiveTimeRow;
            VisualElement resetSequenceAfterInactiveTimeRow = DesignUtils
                .row
                .AddChild(resetSequenceAfterInactiveTimeLabel)
                .AddChild(resetSequenceAfterInactiveTimeToggle);
            
            Label sequenceResetTimeLabel = 
                DesignUtils.NewLabel("SequenceResetTime");
            FloatField sequenceResetTimeField = DesignUtils.NewFloatField(SequenceResetTime);
            VisualElement sequenceResetTimeRow = DesignUtils
                .row
                .AddChild(sequenceResetTimeLabel)
                .AddChild(sequenceResetTimeField);            
            Label volumeLabel = 
                DesignUtils
                    .NewLabel("Volume")
                    .SetStyleMinWidth(70);
            FluidMinMaxSlider minMaxSlider = new FluidMinMaxSlider();
            // VisualElement dragHandle = new VisualElement(volumeSlider.sliderDragger);
            // dragContainer.AddChild();
            // minMaxSlider.lowLimit = SoundGroupData.Volume.MinValue;
            // minMaxSlider.highLimit = SoundGroupData.Volume.MinValue;
            // minMaxSlider.value = new Vector2(-30, -20);
            minMaxSlider.SetStyleMinHeight(10);
            minMaxSlider.contentContainer.SetStyleMinHeight(2);
            minMaxSlider.RegisterCallback<DragUpdatedEvent>((even) => {});
            
            VisualElement volumeRow = DesignUtils
                .row
                .AddChild(volumeLabel)
                .AddChild(minMaxSlider)
                .SetStyleMinHeight(40);
            
            Root
                .AddChild(Header)
                .AddChild(labelRow)
                .AddSpaceBlock()
                .AddChild(playModeRow)
                .AddChild(loopRow)
                .AddChild(resetSequenceAfterInactiveTimeRow)
                .AddChild(sequenceResetTimeRow)
                .AddChild(volumeRow)
                .AddSpaceBlock(2)
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