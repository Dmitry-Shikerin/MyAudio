using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls
{
    public class SoundDataBaseHeaderVisualElement : VisualElement
    {
        public SoundDataBaseHeaderVisualElement()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout()
                    .SetStylePadding(4,4,4,4)
                    .SetStyleBackgroundColor(EditorColors.Default.BoxBackground);

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
                .SetLabelText("Rename")
                .SetElementSize(ElementSize.Large);
            
            PingAssetButton = new FluidButton();
            PingAssetButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.PingPong)
                .SetElementSize(ElementSize.Large);
            
            RemoveButton = new FluidButton();
            RemoveButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close)
                .SetLabelText("Remove")
                .SetElementSize(ElementSize.Large);

            Container
                .AddChild(SoundGroupTextField)
                .AddChild(RenameButton)
                .AddChild(PingAssetButton)
                .AddChild(RemoveButton);
            
            Add(Container);
        }

        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public FluidButton RenameButton { get; private set; }
        public FluidButton RemoveButton { get; private set; }
        public FluidButton PingAssetButton { get; private set; }
    }
}