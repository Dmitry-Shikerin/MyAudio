using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.NewSoundContents.Presentation.Controlls
{
    public class NewSoundContentVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public FluidButton CreateButton { get; private set; }

        public NewSoundContentVisualElement()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout()
                    .SetStylePadding(4,4,4,4)
                    .SetStyleBackgroundColor(EditorColors.Default.BoxBackground);

            SoundGroupTextField = new TextField();
            SoundGroupTextField
                .SetStyleFlexGrow(1);
            CreateButton = new FluidButton();
            CreateButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Large)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                .SetStyleMinWidth(130)
                .SetLabelText("New Sound Group");

            Container
                .AddChild(SoundGroupTextField)
                .AddChild(CreateButton)
                ;

            Add(Container);
        }
    }
}