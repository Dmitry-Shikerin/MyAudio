﻿using System;
using JetBrains.Annotations;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundySetting.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundySetting.Controllers
{
    public class SoundySettingsPresenter : IPresenter
    {
        private readonly SoundySettings _soundySettings;
        private readonly ISoundySettingsView _view;

        public SoundySettingsPresenter(
            SoundySettings soundySettings,
            ISoundySettingsView view)
        {
            _soundySettings = soundySettings ?? throw new ArgumentNullException(nameof(soundySettings));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            _view.SetAutoKillIdleControllers(_soundySettings.AutoKillIdleControllers);
            _view.SetControllerAutoKillDuration(
                new Vector2Int(
                    (int)SoundySettings.CONTROLLER_IDLE_KILL_DURATION_MIN, 
                    (int)SoundySettings.CONTROLLER_IDLE_KILL_DURATION_MAX),
                (int)_soundySettings.ControllerIdleKillDuration);
            _view.SetIdleCheckInterval(new Vector2Int(
                (int)SoundySettings.IDLE_CHECK_INTERVAL_MIN, 
                (int)SoundySettings.IDLE_CHECK_INTERVAL_MAX),
                (int)_soundySettings.IdleCheckInterval);
            _view.SetMinimumNumberOfControllers(new Vector2Int(
                (int)SoundySettings.MINIMUM_NUMBER_OF_CONTROLLERS_MIN, 
                (int)SoundySettings.MINIMUM_NUMBER_OF_CONTROLLERS_MAX),
                _soundySettings.MinimumNumberOfControllers);
        }

        public void Dispose()
        {
            
        }

        public void ChangeIdleCheckInterval(int value)
        {
            _soundySettings.IdleCheckInterval = value;
        }
    }
}