using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class NewSoundContentVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public FluidButton CreateButton { get; private set; }

        public NewSoundContentVisualElement()
        {
            Initialize();
        }
        
        public NewSoundContentVisualElement Initialize()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout();
            Add(Container);

            SoundGroupTextField = new TextField();
            SoundGroupTextField
                .SetStyleFlexGrow(1);
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
        
        public NewSoundContentVisualElement SetOnClick(UnityAction callback)
        {
            CreateButton.SetOnClick(callback);
            
            return this;
        }
    }
}