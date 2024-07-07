using Doozy.Engine.Soundy;
using UnityEditor;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Editors
{
    [CustomEditor(typeof(SoundyDatabase))]
    public class SoundyDataBaseEditor : UnityEditor.Editor
    {
        public VisualTreeAsset VisualTreeAsset;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset.CloneTree(root);

            return root;
        }
    }
}