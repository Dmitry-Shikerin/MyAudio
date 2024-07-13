using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
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
        
        public SoundyDatabase Database { get; set; }
        private AudioClip PlugAudio { get; set; }
        
        private FluidFoldout CurrentFoldout { get; set; }
        private VisualElement CurrentVisualElement { get; set; }
        public List<SoundGroupVisualElement> CurrentSoundGroupVisualElements { get; private set; }
        public NewSoundContentVisualElement NewSoundContentVisualElement { get; private set; }
        public SoundDataBaseHeaderVisualElement HeaderVisualElement { get; private set; }
        public SoundDatabase CurrentSoundDatabase { get; private set; }
        private List<FluidToggleButtonTab> FluidToggleButtonTabs { get; set; }

        public SoundyDataBaseWindowLayout()
        {
            AddHeader("Soundy Database", "Sound Groups", animatedIconTextures);
            // sideMenu
            //     .SetMenuLevel(FluidSideMenu.MenuLevel.Level_2)
            //     .IsCollapsable(false);
            
            // FluidToggleButtonTabs = new List<FluidToggleButtonTab>();
            // CurrentSoundGroupVisualElements = new List<SoundGroupVisualElement>();
            // NewSoundContentVisualElement =
            //     new NewSoundContentVisualElement();
            // NewSoundContentVisualElement
            //     .SetOnClick(() =>
            //     {
            //         CurrentSoundDatabase.Add(
            //             NewSoundContentVisualElement.SoundGroupTextField.value, false, true);
            //         //Сделать рефрешь елементов
            //         UpdateDataBase(CurrentSoundDatabase);
            //     });
            //
            // HeaderVisualElement = new SoundDataBaseHeaderVisualElement()
            //     .SetParent(this)
            //     .Initialize();
        }
        
        // public void AddHeader()
        // {
        //     AddHeader("Soundy Database", "Sound Groups", animatedIconTextures);
        // }

        // public void RefreshDataBasesButtons()
        // {
        //     foreach (FluidToggleButtonTab button in FluidToggleButtonTabs)
        //         button.Recycle();
        //     
        //     FluidToggleButtonTabs.Clear();
        //     AfterInitialize();
        // }
        
        // public SoundyDataBaseWindowLayout AfterInitialize()
        // {
        //     foreach (SoundDatabase soundDatabase in Database.SoundDatabases)
        //     {
        //         FluidToggleButtonTab button = 
        //             sideMenu
        //             .AddButton(soundDatabase.DatabaseName, EditorSelectableColors.EditorUI.Orange);
        //             button
        //             .AddOnClick(() =>
        //             {
        //                 CurrentSoundDatabase = soundDatabase;
        //                 UpdateDataBase(soundDatabase);
        //             });
        //             
        //             FluidToggleButtonTabs.Add(button);
        //     }
        //     
        //     return this;
        // }
        
        // public SoundyDataBaseWindowLayout ShowDataBase()
        // {
        //     CurrentSoundDatabase = Database.SoundDatabases[0];
        //     UpdateDataBase(CurrentSoundDatabase);
        //     
        //     return this;
        // }

        // public void UpdateDataBase(SoundDatabase soundDatabase)
        // {
        //     content.Clear();
        //
        //     content
        //         .AddChild(HeaderVisualElement)
        //         .AddSpaceBlock(4)
        //         .AddChild(NewSoundContentVisualElement)
        //         .AddSpaceBlock(2);
        //     HeaderVisualElement
        //         .SetLabelText(soundDatabase.DatabaseName);
        //
        //     foreach (SoundGroupData soundGroup in soundDatabase.Database)
        //     {
        //         SoundGroupVisualElement soundGroupVisualElement =
        //             new SoundGroupVisualElement()
        //                 .SetSoundGroup(soundGroup)
        //                 .SetParent(this)
        //                 .Initialize()
        //                 .SetLabelText(soundGroup.SoundName)
        //                 .SetDeleteOnClick(() =>
        //                 {
        //                     CurrentSoundDatabase.Remove(soundGroup, saveAssets: true);
        //                     UpdateDataBase(CurrentSoundDatabase);
        //                 });
        //         CurrentSoundGroupVisualElements.Add(soundGroupVisualElement);
        //         content
        //             .AddChild(soundGroupVisualElement)
        //             .AddSpaceBlock();
        //     }
        // }

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