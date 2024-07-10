using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class NewSoundGroupVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public SoundyDataBaseWindowLayout Parent { get; private set; }
        public FluidButton CreateButton { get; private set; }

        public NewSoundGroupVisualElement Initialize()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout();
            Add(Container);

            SoundGroupTextField = new TextField();
            //Потом включить
            SoundGroupTextField
                .SetStyleFlexGrow(1)
                .SetEnabled(false);
            CreateButton = new FluidButton();
            CreateButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                .SetStyleMinWidth(130)
                .SetLabelText("New Sound Group");

            Container
                .AddChild(SoundGroupTextField)
                .AddChild(CreateButton)
                ;

            return this;
        }

        public NewSoundGroupVisualElement SetParent(SoundyDataBaseWindowLayout parentWindow)
        {
            Parent = parentWindow;

            return this;
        }
    }
}