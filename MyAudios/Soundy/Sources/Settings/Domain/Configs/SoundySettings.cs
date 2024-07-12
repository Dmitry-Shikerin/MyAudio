﻿using System;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Soundy.Sources.AudioPoolers.Controllers;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Const;
using UnityEngine;

namespace MyAudios.Soundy.Sources.Settings.Domain.Configs
{
    [Serializable]
    public class SoundySettings : ScriptableObject
    {
        [SerializeField] private SoundyDatabase _database;

        public static SoundySettings Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;
                
                s_instance = AssetUtils.GetScriptableObject<SoundySettings>(
                    SoundySettingsConst.FileName,
                    SoundySettingsConst.ResourcesPath,
                    false, 
                    false);
                
                return s_instance;
            }
        }

        private static SoundySettings s_instance;

        public static SoundyDatabase Database
        {
            get
            {
                if (Instance._database != null)
                    return Instance._database;
                
                UpdateDatabase();
                
                return Instance._database;
            }
        }

        public static void UpdateDatabase()
        {
            Instance._database = AssetUtils.GetScriptableObject<SoundyDatabase>(
                "_" + DoozyPath.SOUNDY_DATABASE, "Assets/MyAudios/Soundy/Resources/Soundy/DataBases", 
                false, false);
#if UNITY_EDITOR
            if (Instance._database == null)
                return;
            
            Instance._database.Initialize();
            Instance._database.SearchForUnregisteredDatabases(false);
            Instance.SetDirty(true);
#endif
        }

        public const bool AUTO_KILL_IDLE_CONTROLLERS_DEFAULT_VALUE = true;
        public const float CONTROLLER_IDLE_KILL_DURATION_DEFAULT_VALUE = 20f;
        public const float CONTROLLER_IDLE_KILL_DURATION_MIN = 0f;
        public const float CONTROLLER_IDLE_KILL_DURATION_MAX = 300f;
        public const float IDLE_CHECK_INTERVAL_DEFAULT_VALUE = 5f;
        public const float IDLE_CHECK_INTERVAL_MIN = 0.1f;
        public const float IDLE_CHECK_INTERVAL_MAX = 60f;
        public const int MINIMUM_NUMBER_OF_CONTROLLERS_DEFAULT_VALUE = 3;
        public const int MINIMUM_NUMBER_OF_CONTROLLERS_MIN = 0;
        public const int MINIMUM_NUMBER_OF_CONTROLLERS_MAX = 20;

        public bool AutoKillIdleControllers = AUTO_KILL_IDLE_CONTROLLERS_DEFAULT_VALUE;
        public float ControllerIdleKillDuration = CONTROLLER_IDLE_KILL_DURATION_DEFAULT_VALUE;
        public float IdleCheckInterval = IDLE_CHECK_INTERVAL_DEFAULT_VALUE;
        public int MinimumNumberOfControllers = MINIMUM_NUMBER_OF_CONTROLLERS_DEFAULT_VALUE;

        private void Reset()
        {
            AutoKillIdleControllers = AUTO_KILL_IDLE_CONTROLLERS_DEFAULT_VALUE;
            ControllerIdleKillDuration = CONTROLLER_IDLE_KILL_DURATION_DEFAULT_VALUE;
            IdleCheckInterval = IDLE_CHECK_INTERVAL_DEFAULT_VALUE;
            MinimumNumberOfControllers = MINIMUM_NUMBER_OF_CONTROLLERS_DEFAULT_VALUE;
        }
        
        public void Reset(bool saveAssets)
        {
            Reset();
            SetDirty(saveAssets);
        }

        public void ResetComponent(SoundyPooler pooler)
        {
            
        }
        
        /// <summary> [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects) </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void SetDirty(bool saveAssets) =>
            DoozyUtils.SetDirty(this, saveAssets);

        /// <summary> Records any changes done on the object after this function </summary>
        /// <param name="undoMessage"> The title of the action to appear in the undo history (i.e. visible in the undo menu) </param>
        public void UndoRecord(string undoMessage) =>
            DoozyUtils.UndoRecordObject(this, undoMessage);
    }
}