using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.SoundGroupDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using UnityEditor;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors
{
    [CustomEditor(typeof(SoundGroupData))]
    public class SoundGroupDataEditor : UnityEditor.Editor
    {
        private SoundGroupData _soundGroupData;
        private VisualElement _root;

        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            ISoundGroupDataView view = new SoundGroupDataViewFactory().Create(_soundGroupData);
            _root = new VisualElement()
                .AddChild(view.Root);

            return _root;
        }

        private void FindProperties()
        {
            _soundGroupData = (SoundGroupData)serializedObject.targetObject;
        }
    }
}