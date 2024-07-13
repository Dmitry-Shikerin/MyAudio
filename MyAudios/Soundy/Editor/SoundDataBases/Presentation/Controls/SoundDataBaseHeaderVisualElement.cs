using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class SoundDataBaseHeaderVisualElement : VisualElement
    {
        public SoundDataBaseHeaderVisualElement()
        {
            Initialize();
        }

        public VisualElement Container { get; private set; }

        public TextField SoundGroupTextField { get; private set; }

        public FluidButton RenameButton { get; private set; }

        public FluidButton RemoveButton { get; private set; }

        public FluidButton PingAssetButton { get; private set; }

        public SoundDataBaseHeaderVisualElement Initialize()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout();
            Add(Container);

            SoundGroupTextField = new TextField();
            //Потом включить
            SoundGroupTextField
                .SetStyleFlexGrow(1);
            SoundGroupTextField
                .SetValueWithoutNotify("New Sound Group");
            RenameButton = new FluidButton();
            RenameButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                .SetStyleMinWidth(130)
                .SetLabelText("Rename");
            
            PingAssetButton = new FluidButton();
            PingAssetButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.PingPong);
            
            RemoveButton = new FluidButton();
            RemoveButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close)
                .SetLabelText("Remove");

            Container
                .AddChild(SoundGroupTextField)
                .AddChild(RenameButton)
                .AddChild(PingAssetButton)
                .AddChild(RemoveButton);

            return this;
        }
        
        public SoundDataBaseHeaderVisualElement SetTextFieldText(string labelText)
        {
            SoundGroupTextField.SetValueWithoutNotify(labelText);

            return this;
        }

        public SoundDataBaseHeaderVisualElement SetRenameOnClick(UnityAction callback)
        {
            RenameButton.SetOnClick(callback);
            
            return this;
        }       
        
        public SoundDataBaseHeaderVisualElement SetRemoveOnClick(UnityAction callback)
        {
            RemoveButton.SetOnClick(callback);
            
            return this;
        }
    }
}