using Doozy.Editor.EditorUI.Components.Internal;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Events;

namespace Doozy.Editor.EditorUI.Components
{
    public class PropertyFluidListViewItem : FluidListViewItem
    {
        public PropertyField propertyField { get; private set; }
        public UnityAction<SerializedProperty> OnRemoveButtonClick;

        public PropertyFluidListViewItem(FluidListView listView) : base(listView)
        {
            this.SetListView(listView);
            itemContentContainer.Clear();
            // itemContentContainer.Add(propertyField = new PropertyField().TryToHideLabel());
            itemContentContainer.Add(propertyField = new PropertyField());
        }

        public void Update(int index, SerializedProperty property)
        {
            //UPDATE INDEX
            showItemIndex = listView.showItemIndex;
            UpdateItemIndex(index);

            //UPDATE PROPERTY
            propertyField.BindProperty(property);

            //UPDATE REMOVE BUTTON
            itemRemoveButton.OnClick = () => OnRemoveButtonClick?.Invoke(property);
        }
    }
}
