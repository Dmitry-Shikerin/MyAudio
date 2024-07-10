using System.Collections.Generic;
using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.DataBases.Editors;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundyDataBaseEditorWindow : FluidWindow<SoundyDataBaseEditorWindow>
    {
        public SoundyDatabase Database;

        [MenuItem("Window/SoundyDataBase")]
        public static void ShowWindow() =>
            GetWindow<SoundyDataBaseEditorWindow>("Soundy Data Base");

        protected override void CreateGUI()
        {
            root.Clear();

            SoundyDataBaseEditor editor = 
                (SoundyDataBaseEditor)UnityEditor.Editor.CreateEditor(Database);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot.Bind(editor.serializedObject);
            
            root.Add(editorRoot);
        }
    }
}