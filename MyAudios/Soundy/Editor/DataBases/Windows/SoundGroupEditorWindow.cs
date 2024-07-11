using System.Security.Cryptography;
using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Editors;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundGroupEditorWindow : FluidWindow<SoundGroupEditorWindow>
    {
        private VisualElement Root => rootVisualElement;
        private static SoundGroupData SoundGroupData { get; set; }
        
        public static void Open(SoundGroupData soundGroupData)
        {
            SoundGroupData = soundGroupData;
            SoundGroupEditorWindow window = GetWindow<SoundGroupEditorWindow>();
            window.titleContent = new GUIContent("Sound Group");
            window.Show();
        }
        
        protected override void CreateGUI()
        {
            Root.Clear();
            SoundGroupDataEditor editor = (SoundGroupDataEditor)UnityEditor.Editor.CreateEditor(SoundGroupData);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot.Bind(editor.serializedObject);
            
            Root
                .AddChild(editorRoot)
                .SetStylePadding(15, 15, 15, 15)
                ;
            Debug.Log($"CreateGUI: {SoundGroupData}");
        }
    }
}