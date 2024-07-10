using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Engine.Soundy;
using UnityEditor;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Editors
{
    public class SoundGroupEditor : UnityEditor.Editor
    {
        private SerializedProperty DatabaseNames { get; set; }
        private SerializedProperty SoundDatabases { get; set; }
        
        private VisualElement Root { get; set; }
        
        private FluidComponentHeader Header { get; set; }
        
        private FluidListView DatabaseNamesField { get; set; }
        private FluidListView SoundDatabasesField { get; set; }

        public override VisualElement CreateInspectorGUI()
        {
            SoundGroupData soundGroupData = serializedObject.targetObject as SoundGroupData;
            FindProperties();
            InitializeEditor();
            Compose();
            
            return Root;
        }

        private void FindProperties()
        {
            
        }

        private void InitializeEditor()
        {
            SoundDatabasesField = DesignUtils.NewObjectListView(
                SoundDatabases, "Sound Databases", "", typeof(SoundDatabase));
            SoundDatabasesField.listView.RegisterCallback<MouseDownEvent>(
                evt => SoundDatabasesField.listView.ClearSelection());
        }

        private void Compose()
        {
            
        }
    }
}