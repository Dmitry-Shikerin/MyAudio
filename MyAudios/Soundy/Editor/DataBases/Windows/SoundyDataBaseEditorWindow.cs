using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundyDataBaseEditorWindow : EditorWindow
    {
        public VisualTreeAsset RootUxml;
        public VisualTreeAsset DataBaseFoldoutUxml;
        public VisualTreeAsset DataBaseUxml;
        public SoundyDatabase Database;

        [MenuItem("Window/SoundyDataBase")]
        public static void ShowWindow()
        {
            GetWindow<SoundyDataBaseEditorWindow>("Soundy Data Base");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            RootUxml.CloneTree(root);

            // root.Q<ListView>("SoundDataBases").itemsSource = Database.SoundDatabases;
            // ListView listView = root.Q<ListView>("SoundDataBases");
            // listView.itemsSource = Database.SoundDatabases;
            // listView.selectionChanged += OnItemSelected;
            ScrollView dataBase = root.Q<ScrollView>("ElementContainer");

            foreach (SoundDatabase soundDatabase in Database.SoundDatabases)
            {
                VisualElement foldoutVisualElement = new VisualElement();
                DataBaseFoldoutUxml.CloneTree(foldoutVisualElement);
                dataBase.Add(foldoutVisualElement);
                Label label = foldoutVisualElement.Q<Label>("Label");
                VisualElement dataContainer = foldoutVisualElement.Q<VisualElement>("ElementContainer");
                label.text = soundDatabase.DatabaseName;
                
                foreach (SoundGroupData soundGroup in soundDatabase.Database)
                {
                    VisualElement dataBaseVisualElement = new VisualElement();
                    DataBaseUxml.CloneTree(dataBaseVisualElement);
                    dataContainer.Add(dataBaseVisualElement);
                    dataBaseVisualElement.transform.position = new Vector3(0, 0, 0);
                    dataBaseVisualElement.Q<Label>("Label").text = soundGroup.SoundName;
                }
            }
        }

        protected virtual void OnGUI()
        {
            // VisualElement root = new VisualElement();
            // VisualTreeAsset.CloneTree(root);
        }

        private void OnItemSelected(IEnumerable<object> obj)
        {
            foreach (object item in obj)
            {
                // string itemName = (string)item;
                // Debug.Log($"{itemName}");
            }
        }
    }
}