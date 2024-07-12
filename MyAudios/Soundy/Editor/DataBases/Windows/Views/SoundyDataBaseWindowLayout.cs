using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows.Views
{
    public class SoundyDataBaseWindowLayout : FluidWindowLayout
    {
        public int order => 0;

        public override string layoutName => "Sound Databases";
        public override List<Texture2D> animatedIconTextures => EditorSpriteSheets.UIManager.Icons.SignalToAudioSource;
        public override Color accentColor => EditorColors.Default.UIComponent;
        public override EditorSelectableColorInfo selectableAccentColor => 
            EditorSelectableColors.Default.UIComponent;
        
        private SoundyDatabase Database { get; set; }
        private AudioClip PlugAudio { get; set; }
        
        private FluidFoldout CurrentFoldout { get; set; }
        private VisualElement CurrentVisualElement { get; set; }
        public List<SoundGroupVisualElement> CurrentSoundGroupVisualElements { get; private set; }
        public NewSoundContentVisualElement NewSoundContentVisualElement { get; private set; }
        public SoundDataBaseHeaderVisualElement HeaderVisualElement { get; private set; }
        public SoundDatabase CurrentSoundDatabase { get; private set; }

        public SoundyDataBaseWindowLayout()
        {
            AddHeader("Soundy Database", "UIButton Ids", animatedIconTextures);
            sideMenu
                .SetMenuLevel(FluidSideMenu.MenuLevel.Level_2)
                .IsCollapsable(false);
            
            CurrentSoundGroupVisualElements = new List<SoundGroupVisualElement>();
            NewSoundContentVisualElement =
                new NewSoundContentVisualElement();
            NewSoundContentVisualElement
                .SetOnClick(() =>
                {
                    CurrentSoundDatabase.Add(
                        NewSoundContentVisualElement.SoundGroupTextField.value, false, true);
                    //Сделать рефрешь елементов
                    UpdateDataBase(CurrentSoundDatabase);
                });

            HeaderVisualElement = new SoundDataBaseHeaderVisualElement()
                .SetParent(this)
                .Initialize();
        }

        public SoundyDataBaseWindowLayout AfterInitialize()
        {
            foreach (SoundDatabase soundDatabase in Database.SoundDatabases)
            {
                FluidToggleButtonTab button = 
                    sideMenu
                    .AddButton(soundDatabase.DatabaseName, EditorSelectableColors.EditorUI.Orange);
                    button
                    .AddOnClick(() =>
                    {
                        CurrentSoundDatabase = soundDatabase;
                        UpdateDataBase(soundDatabase);
                    });
            }
            
            return this;
        }
        
        public SoundyDataBaseWindowLayout ShowDataBase()
        {
            CurrentSoundDatabase = Database.SoundDatabases[0];
            UpdateDataBase(CurrentSoundDatabase);
            
            return this;
        }

        private void UpdateDataBase(SoundDatabase soundDatabase)
        {
            content.Clear();

            content
                .AddChild(HeaderVisualElement)
                .AddSpaceBlock(4)
                .AddChild(NewSoundContentVisualElement)
                .AddSpaceBlock(2);
            HeaderVisualElement.SetLabelText(soundDatabase.DatabaseName);

            foreach (SoundGroupData soundGroup in soundDatabase.Database)
            {
                SoundGroupVisualElement soundGroupVisualElement =
                    new SoundGroupVisualElement()
                        .SetSoundGroup(soundGroup)
                        .SetParent(this)
                        .Initialize()
                        .SetLabelText(soundGroup.SoundName)
                        .SetDeleteOnClick(() =>
                        {
                            CurrentSoundDatabase.Remove(soundGroup, saveAssets: true);
                            UpdateDataBase(CurrentSoundDatabase);
                        });
                CurrentSoundGroupVisualElements.Add(soundGroupVisualElement);
                content
                    .AddChild(soundGroupVisualElement)
                    .AddSpaceBlock();
            }
        }

        public void StopAllSounds()
        {
            foreach (SoundGroupVisualElement soundGroupVisualElement in CurrentSoundGroupVisualElements)
                soundGroupVisualElement.StopSound();
        }
        
        public SoundyDataBaseWindowLayout SetDatabase(SoundyDatabase dataBase)
        {
            Database = dataBase;
            
            return this;
        }
        
        public SoundyDataBaseWindowLayout SetPlugAudio(AudioClip clip)
        {
            PlugAudio = clip;
            
            return this;
        }
    }
}