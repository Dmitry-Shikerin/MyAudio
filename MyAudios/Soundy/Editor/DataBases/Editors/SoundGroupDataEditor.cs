using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.DataBases.Domain.Data;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
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

        private VisualElement Root { get; set; }
        private FluidComponentHeader Header { get; set; }

        private FluidListView SoundsField { get; set; }
        
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
            Sounds = serializedObject.FindProperty(nameof(SoundGroupData.Sounds));
        }

        private void InitializeEditor()
        {
            Root = new VisualElement();

            InitializeHeader();
            InitializeDataBases();
        }

        private void Compose()
        {
            Label label = DesignUtils
                .NewLabel((string)Name.stringValue);
            VisualElement labelRow = DesignUtils.row.AddChild(label);
            
            Root
                .AddChild(Header)
                .AddChild(labelRow)
                .AddSpaceBlock(2)
                .AddSpaceBlock()
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

        private void InitializeDataBases()
        {
            SoundsField = DesignUtils.NewObjectListView(
                Sounds, "Sounds", "", typeof(AudioData));
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
    }
}