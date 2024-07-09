using System.Collections;
using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.Reactor.Ticker;
using Doozy.Editor.UIElements;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.MyUiFramework.Scripts;
using MyAudios.Soundy.DataBases.Domain.Data;
using MyAudios.Soundy.Managers;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Editors
{
    [CustomEditor(typeof(SoundyDatabase))]
    public class SoundyDataBaseEditor : UnityEditor.Editor
    {
        public SoundyDatabase Target;
        public AudioClip PlugAudio;

        private SerializedProperty DatabaseNames { get; set; }
        private SerializedProperty SoundDatabases { get; set; }

        private VisualElement Root { get; set; }
        private FluidComponentHeader Header { get; set; }

        private FluidListView DatabaseNamesField { get; set; }
        private FluidListView SoundDatabasesField { get; set; }

        private FluidButton DatabasesButton { get; set; }

        private List<FluidFoldout> Foldouts { get; set; }

        private List<GameObject> _audioSources = new List<GameObject>();

        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            InitializeEditor();
            Compose();

            return Root;
        }

        private void Refresh()
        {
            InitializeNames();
            InitializeDataBases();
        }

        private void FindProperties()
        {
            DatabaseNames = serializedObject.FindProperty(nameof(SoundyDatabase.DatabaseNames));
            SoundDatabases = serializedObject.FindProperty(nameof(SoundyDatabase.SoundDatabases));
        }

        private void InitializeEditor()
        {
            Root = new VisualElement();

            InitializeHeader();
            InitializeNames();
            InitializeDataBases();
        }

        private void Compose()
        {
            // FluidFoldout dataBaseFoldout =
            //     new FluidFoldout()
            //         .AddContent(SoundDatabasesField);
            //
            FluidFoldout namesFoldout =
                new FluidFoldout()
                    .AddContent(DatabaseNamesField);

            FluidButton databaseButton =
                FluidButton
                    .Get()
                    .SetName("Data Base")
                    .SetLabelText("Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            FluidButton settingsButton =
                FluidButton
                    .Get()
                    .SetName("Settings")
                    .SetLabelText("Settings")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Settings)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement buttonsRow = DesignUtils.row;
            buttonsRow
                .AddSpaceBlock(2)
                .AddChild(databaseButton)
                .AddSpaceBlock(5)
                .AddChild(settingsButton);

            TextField addDataBaseTextField = new TextField();
            addDataBaseTextField.SetStyleMinWidth(250);

            FluidButton addDataBaseButton =
                FluidButton
                    .Get()
                    .SetName("New Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.CategoryPlus)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained)
                    .AddOnClick(() =>
                    {
                        if (string.IsNullOrEmpty(addDataBaseTextField.value))
                            return;

                        Target.CreateSoundDatabase(
                            "Assets/MyAudios/Resources/Soundy/DataBases",
                            addDataBaseTextField.value);
                    });

            FluidButton undoDataBaseButton =
                FluidButton
                    .Get()
                    .SetName("Undo Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);


            VisualElement addNewSoundDataRow = DesignUtils.row;
            addNewSoundDataRow
                .AddChild(addDataBaseTextField)
                .AddChild(addDataBaseButton)
                .AddChild(undoDataBaseButton);

            FluidFoldout newSoundDataBaseFoldout =
                new FluidFoldout()
                    .AddContent(addNewSoundDataRow);

            FluidButton newSoundDatabaseButton =
                FluidButton
                    .Get()
                    .SetName("New Sound Data Base")
                    .SetLabelText("New Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.CategoryPlus)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained)
                    .AddOnClick(() =>
                    {
                        if (newSoundDataBaseFoldout.isOn == false)
                            newSoundDataBaseFoldout.Open();
                        else
                            newSoundDataBaseFoldout.Close();
                    });

            FluidButton refreshButton =
                FluidButton
                    .Get()
                    .SetName("Refresh")
                    .SetLabelText("Refresh")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Refresh)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement secondButtonsRow = DesignUtils.row;
            secondButtonsRow
                .AddChild(newSoundDatabaseButton)
                .AddSpaceBlock(5)
                .AddChild(refreshButton)
                .AddSpaceBlock(5);

            FluidButton saveButton =
                FluidButton
                    .Get()
                    .SetName("Save")
                    .SetLabelText("Save")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            FluidButton soundDatabasesButton =
                FluidButton
                    .Get()
                    .SetName("Sound Databases")
                    .SetLabelText("Sound Databases")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Settings)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement thirdButtonsRow = DesignUtils.row;
            thirdButtonsRow
                .AddSpaceBlock(5)
                .AddChild(saveButton)
                .AddSpaceBlock(5)
                .AddChild(soundDatabasesButton);


            Root
                .AddChild(Header)
                .AddSpaceBlock(2)
                .AddChild(namesFoldout)
                .AddSpaceBlock()
                // .AddChild(dataBaseFoldout)
                .AddSpaceBlock(2)
                .AddChild(buttonsRow)
                .AddChild(secondButtonsRow)
                .AddChild(thirdButtonsRow)
                .AddChild(newSoundDataBaseFoldout)
                ;

            Foldouts = new List<FluidFoldout>();

            AddSoundDataBases();
        }

        private void AddSoundDataBases()
        {
            foreach (var database in Target.SoundDatabases)
            {
                FluidFoldout foldout = new FluidFoldout();
                VisualElement row = DesignUtils.row;
                Label label = new Label();
                label
                    .SetStyleMinWidth(20)
                    .SetText("Renamed");
                TextField textField = new TextField();
                textField
                    .SetStyleMinWidth(170);

                FluidButton renameButton = new FluidButton();
                renameButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
                // renameButton.OnClick -= () => Target.DeleteDatabase(database);
                // renameButton.OnClick += () => Target.DeleteDatabase(database);

                FluidButton deleteButton = new FluidButton();
                deleteButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close);
                deleteButton.OnClick -= () => Target.DeleteDatabase(database);
                deleteButton.OnClick += () => Target.DeleteDatabase(database);
                row
                    .AddChild(label)
                    .AddChild(textField)
                    .AddChild(renameButton)
                    .AddChild(deleteButton);

                Label createSoundGroupLabel = new Label();
                createSoundGroupLabel.SetText("CreateSoundGroup");
                TextField createSoundGroupTextField = new TextField();
                createSoundGroupTextField.SetStyleMinWidth(170);
                FluidButton createSoundGroupButton = new FluidButton();
                createSoundGroupButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
                createSoundGroupButton.OnClick += () =>
                    database.Add(createSoundGroupTextField.value, false, true);

                VisualElement createSoundGroupVisualElement = DesignUtils.row;
                createSoundGroupVisualElement
                    .AddChild(createSoundGroupLabel)
                    .AddChild(createSoundGroupTextField)
                    .AddChild(createSoundGroupButton);

                foldout.SetLabelText(database.DatabaseName);
                foldout
                    .AddContent(row)
                    .AddContent(createSoundGroupVisualElement);

                //Crete sound group data
                AddSoundGroupData(
                    database,
                    createSoundGroupVisualElement,
                    createSoundGroupLabel,
                    createSoundGroupButton,
                    foldout);

                Foldouts.Add(foldout);
                Root.AddChild(foldout);
            }
        }

        private void AddSoundGroupData(
            SoundDatabase database,
            VisualElement createSoundGroupVisualElement,
            Label createSoundGroupLabel,
            FluidButton createSoundGroupButton,
            FluidFoldout foldout)
        {
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
                            FindObjectOfType<AudioSource>().time = value.newValue;
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
                                    FindObjectOfType<AudioSource>(),
                                    null, audioData.AudioClip);

                                EditorApplication.update -= () 
                                    => rangeSlider.slider.value = FindObjectOfType<AudioSource>().time;
                                EditorApplication.update += () 
                                    => rangeSlider.slider.value = FindObjectOfType<AudioSource>().time;
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
                                    FindObjectOfType<AudioSource>());
                            });


                    audioDataVisualElement
                        .AddChild(audioDataLabel)
                        .SetStyleMinHeight(20)
                        .AddChild(propertyField)
                        .AddChild(playSoundButton)
                        .AddChild(stopSoundButton);
                    audioDataFoldout
                        .AddContent(audioDataVisualElement)
                        .AddContent(rangeSlider)
                        .AddContent(rangeSlider2);
                }


                Label addSoundLabel = new Label();
                createSoundGroupLabel.SetText(soundGroup.SoundName);
                FluidButton addSoundButton = new FluidButton();
                createSoundGroupButton
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save);
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
                createSoundGroupButton.OnClick += () =>
                    database.Add(addSoundGroupTextField.value, false, true);

                addSoundVisualElement
                    .AddChild(addSoundGroupLabel)
                    .AddChild(addSoundGroupTextField)
                    .AddChild(addSoundGroupButton);
                createSoundGroupVisualElement
                    .AddChild(addSoundLabel)
                    .AddChild(addSoundButton);
                foldout
                    .AddSpaceBlock(5)
                    .AddContent(addSoundVisualElement)
                    .AddContent(audioDataFoldout)
                    ;
            }
        }
        
        private IEnumerator PlaySound(AudioClip audioClip, AudioSource audioSource, FluidRangeSlider rangeSlider)
        {
            while (audioSource.time + 0.1f < audioClip.length)
            {
                rangeSlider.slider.value = audioSource.time;
                
                Debug.Log(rangeSlider.slider.value);
                Debug.Log(audioSource.time);
                Debug.Log(audioClip.length);
                
                yield return null;
            }
            
            Debug.Log($"end");
        }   

        private AudioSource GetAudioSource()
        {
            foreach (GameObject audioSourceGameObject in _audioSources)
                DestroyImmediate(audioSourceGameObject);
            
            GameObject gameObject = new GameObject();
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(gameObject);
            
            return audioSource;
        }

        private void InitializeNames()
        {
            DatabaseNamesField =
                DesignUtils.NewStringListView(DatabaseNames, "Database Names", "");
        }

        private void InitializeDataBases()
        {
            // SoundDatabasesField = DesignUtils.NewObjectListView(
            //     SoundDatabases, "Sound Databases", "", typeof(SoundDatabase));
        }

        private void InitializeHeader()
        {
            Header =
                FluidComponentHeader
                    .Get()
                    .SetComponentNameText("Soundy Database")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetAccentColor(EditorColors.EditorUI.Orange)
                ;
        }
    }
}