using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Controls
{
    public class SoundyDataBaseWindowLayout : FluidWindowLayout
    {
        public override string layoutName => "Sound Databases";
        public override List<Texture2D> animatedIconTextures => EditorSpriteSheets.UIManager.Icons.SignalToAudioSource;
        public override Color accentColor => EditorColors.Default.UIComponent;
        public override EditorSelectableColorInfo selectableAccentColor => 
            EditorSelectableColors.Default.UIComponent;

        public SoundyDataBaseWindowLayout()
        {
            AddHeader("Soundy Database", "Sound Groups", animatedIconTextures);
        }
    }
}