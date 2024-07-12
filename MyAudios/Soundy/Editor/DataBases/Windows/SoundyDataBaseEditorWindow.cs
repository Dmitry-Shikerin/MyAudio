using System.Linq;
using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.DataBases.Editors;
using MyAudios.Soundy.Editor.DataBases.Windows.Views;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundyDataBaseEditorWindow : FluidWindow<SoundyDataBaseEditorWindow>
    {
        private SoundyDatabase _database;
        private AudioClip _plugAudio;

        [MenuItem("Window/SoundyDataBase")]
        public static void ShowWindow() =>
            GetWindow<SoundyDataBaseEditorWindow>("Soundy Data Base");

        protected override void CreateGUI()
        {
            root.Clear();

            _database = SoundySettings.Database;
            _plugAudio = Resources.Load<AudioClip>("MyAudios/Soundy/Resources/Soundy/Plugs/Christmas Villain Loop");
            SoundyDataBaseEditor editor = 
                (SoundyDataBaseEditor)UnityEditor.Editor.CreateEditor(_database);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot
                .Bind(editor.serializedObject);
            
            windowLayout = 
                new SoundyDataBaseWindowLayout()
                    .SetDatabase(_database)
                    .SetPlugAudio(_plugAudio)
                    .AfterInitialize()
                    .ShowDataBase();
            
            root
                .Add(windowLayout);
        }
    }
}