﻿using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class SoundDataBaseHeaderVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public SoundyDataBaseWindowLayout Parent { get; private set; }
        public FluidButton RenameButton { get; private set; }
        public FluidButton RemoveButton { get; private set; }
        public FluidButton PingAssetButton { get; private set; }
        public string LabelText { get; private set; }

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
                .SetStyleFlexGrow(1)
                .SetEnabled(false);
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

        public SoundDataBaseHeaderVisualElement SetParent(SoundyDataBaseWindowLayout parentWindow)
        {
            Parent = parentWindow;

            return this;
        }
        
        public SoundDataBaseHeaderVisualElement SetLabelText(string labelText)
        {
            LabelText = labelText;
            SoundGroupTextField.SetValueWithoutNotify(labelText);

            return this;
        }
    }
}