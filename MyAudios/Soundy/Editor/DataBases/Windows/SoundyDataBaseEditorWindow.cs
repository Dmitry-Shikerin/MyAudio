using System;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundyDataBaseEditorWindow : EditorWindow
    {
        public VisualTreeAsset VisualTreeAsset;
        public SoundyDatabase Database;

        [MenuItem("Window/SoundyDataBase")]
        public static void ShowWindow()
        {
            GetWindow<SoundyDataBaseEditorWindow>("Soundy Data Base");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset.CloneTree(root);

            // root.Q<ListView>("SoundDataBases").itemsSource = Database.SoundDatabases;
            ListView listView = root.Q<ListView>("SoundDataBases");
            listView.itemsSource = Database.DatabaseNames;
            listView.selectionChanged += OnItemSelekted;
        }

        protected virtual void OnGUI()
        {
            // VisualElement root = new VisualElement();
            // VisualTreeAsset.CloneTree(root);
        }

        private void OnItemSelekted(IEnumerable<object> obj)
        {
            foreach (object item in obj)
            {
                Debug.Log($"Item");
            }
        }
    }
}