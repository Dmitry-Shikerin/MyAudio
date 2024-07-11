using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.DataBases.Domain.Data;
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
        public NewSoundGroupVisualElement NewSoundGroupVisualElement { get; private set; }
        public SoundDataBaseHeaderVisualElement HeaderVisualElement { get; private set; }

        public SoundyDataBaseWindowLayout()
        {
            AddHeader("Soundy Database", "UIButton Ids", animatedIconTextures);
            sideMenu
                .SetMenuLevel(FluidSideMenu.MenuLevel.Level_2)
                .IsCollapsable(false);
            
            CurrentSoundGroupVisualElements = new List<SoundGroupVisualElement>();
            NewSoundGroupVisualElement = 
                new NewSoundGroupVisualElement()
                .Initialize();

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
                    .AddOnClick(() => UpdateDataBase(soundDatabase));
            }
            
            return this;
        }

        private void UpdateDataBase(SoundDatabase soundDatabase)
        {
            content.Clear();

            content
                .AddChild(HeaderVisualElement)
                .AddSpaceBlock(4)
                .AddChild(NewSoundGroupVisualElement);
            HeaderVisualElement.SetLabelText(soundDatabase.DatabaseName);

            foreach (SoundGroupData soundGroup in soundDatabase.Database)
            {
                SoundGroupVisualElement soundGroupVisualElement =
                    new SoundGroupVisualElement()
                        .SetSoundGroup(soundGroup)
                        .SetParent(this)
                        .Initialize()
                        .SetLabelText(soundGroup.DatabaseName);
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

        private void OnClick(SoundDatabase database, FluidToggleButtonTab button)
        {
            if (CurrentFoldout != null && CurrentVisualElement != null)
            {
                CurrentFoldout.RemoveFromHierarchy();
                CurrentVisualElement.RemoveFromHierarchy();
                CurrentFoldout = null;
                CurrentVisualElement = null;
                content.Clear();
            }
            
            foreach (SoundGroupData soundGroup in database.Database)
            {
                Label soundGroupLabel = new Label();
                soundGroupLabel
                    .SetText(soundGroup.SoundName)
                    .SetStyleMinWidth(230);
                FluidButton renameSoundGroupButton = new FluidButton();
                renameSoundGroupButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
                // renameSoundGroupButton.OnClick += () => soundGroup.

                FluidButton deleteSoundGroupButton = new FluidButton();
                deleteSoundGroupButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close);
                deleteSoundGroupButton.OnClick += () =>
                {
                    database.Remove(soundGroup);
                    database.RefreshDatabase(false, true);
                };

                VisualElement visualElement = DesignUtils.row;
                visualElement
                    .AddChild(soundGroupLabel)
                    .AddChild(renameSoundGroupButton)
                    .AddChild(deleteSoundGroupButton);
                FluidFoldout audioDataFoldout = new FluidFoldout();
                audioDataFoldout
                    .SetLabelText(soundGroup.SoundName)
                    .AddContent(visualElement);
                
                foreach (AudioData audioData in soundGroup.Sounds)
                {
                    if (audioData.AudioClip == null)
                        audioData.AudioClip = PlugAudio;
                    
                    VisualElement audioDataVisualElement = DesignUtils.row;
                    Label audioDataLabel = new Label();
                    audioDataLabel.SetStyleMinWidth(150);
                    ObjectField propertyField = new ObjectField();
                    
                    SerializedObject audioDatSerializedObject =
                        new SerializedObject(PlugAudio);
                    
                    propertyField.RegisterCallback<ChangeEvent<Object>>(
                        (evt) =>
                            audioData.AudioClip = evt.newValue != null
                                ? evt.newValue as AudioClip
                                : null);

                    propertyField
                        .SetObjectType(typeof(AudioClip))
                        .BindProperty(audioDatSerializedObject);
                    propertyField.SetValueWithoutNotify(audioData.AudioClip);

                    if (audioData.AudioClip)
                        audioDataLabel.SetText(audioData.AudioClip.name);
                    else
                        audioDataLabel.SetText("_");

                    FluidRangeSlider rangeSlider = new FluidRangeSlider();
                    rangeSlider.slider.highValue = audioData.AudioClip != null 
                        ? audioData.AudioClip.length 
                        : PlugAudio.length;
                    rangeSlider
                        .RegisterCallback<ChangeEvent<float>>(value =>
                        {
                            // Debug.Log("Value changed");
                            // FindObjectOfType<AudioSource>().time = rangeSlider.slider.value;
                        });
                    rangeSlider
                        .slider
                        .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                        .SetStyleColor(EditorColors.EditorUI.Orange);
                    FluidRangeSlider rangeSlider2 = new FluidRangeSlider();
                    rangeSlider2.slider.highValue = audioData.AudioClip.length;
                    rangeSlider2
                        .RegisterCallback<ChangeEvent<float>>(value =>
                        {
                            Object.FindObjectOfType<AudioSource>().time = value.newValue;
                        });
                    
                    FluidButton playSoundButton =
                        FluidButton
                            .Get()
                            .SetButtonStyle(ButtonStyle.Contained)
                            .SetElementSize(ElementSize.Normal)
                            .SetIcon(EditorSpriteSheets.EditorUI.Icons.Play)
                            .SetOnClick(() =>
                            {
                                soundGroup.PlaySoundPreview(
                                    Object.FindObjectOfType<AudioSource>(),
                                    null, audioData.AudioClip);

                                EditorApplication.update -= () 
                                    => rangeSlider.slider.value = Object.FindObjectOfType<AudioSource>().time;
                                EditorApplication.update += () 
                                    => rangeSlider.slider.value = Object.FindObjectOfType<AudioSource>().time;
                            });


                    FluidButton stopSoundButton =
                        FluidButton
                            .Get()
                            .SetButtonStyle(ButtonStyle.Contained)
                            .SetElementSize(ElementSize.Normal)
                            .SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop)
                            .SetOnClick(() =>
                            {
                                soundGroup.StopSoundPreview(
                                    Object.FindObjectOfType<AudioSource>());
                            });
                    
                    audioDataVisualElement
                        .AddChild(audioDataLabel)
                        .SetStyleMinHeight(20)
                        .AddChild(propertyField)
                        .AddChild(playSoundButton)
                        .AddChild(stopSoundButton);
                    CurrentFoldout = audioDataFoldout
                        .AddContent(audioDataVisualElement)
                        .AddContent(rangeSlider)
                        .AddContent(rangeSlider2);
                }


                Label addSoundLabel = new Label();
                // createSoundGroupLabel.SetText(soundGroup.SoundName);
                FluidButton addSoundButton = new FluidButton();
                // createSoundGroupButton
                //     .SetButtonStyle(ButtonStyle.Contained)
                //     .SetElementSize(ElementSize.Normal)
                //     .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
                // renameSoundGroupButton.OnClick += () => soundGroup.

                VisualElement addSoundVisualElement = DesignUtils.row;
                Label addSoundGroupLabel = new Label();
                addSoundGroupLabel.SetText("CreateSoundGroup");
                TextField addSoundGroupTextField = new TextField();
                addSoundGroupTextField.SetStyleMinWidth(170);
                FluidButton addSoundGroupButton = new FluidButton();
                addSoundGroupButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
                // createSoundGroupButton.OnClick += () =>
                //     database.Add(addSoundGroupTextField.value, false, true);

                CurrentVisualElement = addSoundVisualElement
                    .AddChild(addSoundGroupLabel)
                    .AddChild(addSoundGroupTextField)
                    .AddChild(addSoundGroupButton);
                
                // createSoundGroupVisualElement
                //     .AddChild(addSoundLabel)
                //     .AddChild(addSoundButton);
                content
                    .AddSpaceBlock(5)
                    .AddChild(addSoundVisualElement)
                    .AddChild(audioDataFoldout)
                    ;
            }
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