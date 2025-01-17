﻿using System;
using System.Collections.Generic;
using MyAudios.MyUiFramework.Attributes;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Scripts;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.AudioPoolers.Controllers;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Managers.Controllers;
using MyAudios.Soundy.Sources.Managers.Domain.Constants;
using MyAudios.Soundy.Utils;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Doozy.Engine.Soundy
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains all the relevant info needed by Soundy to play the referenced sounds in a managed way
    /// </summary>
    [Serializable]
    public class SoundGroupData : ScriptableObject
    {
        #region Enums

        /// <summary> The order in which clips will be played when you repeatedly fire the Play (sound) method </summary>
        public enum PlayMode
        {
            /// <summary> Sounds are played randomly from a sounds list and refilled after all have been played. This uses true no-repeat, so even when all the sounds in the list have been played, it will not play the previous sound again on the next pass </summary>
            Random = 0,

            /// <summary> Sounds are played in the order they have been added to the sounds list. This option has additional settings </summary>
            Sequence = 1
        }

        #endregion

        #region Properties

        /// <summary> Returns TRUE if this SoundGroupData is either empty or has at least one null (or missing) AudioClip reference. If set to 'No Sound', returns FALSE </summary>
        public bool HasMissingAudioClips
        {
            get
            {
                if (SoundName.Equals(SoundyManagerConstant.NoSound))
                    return false;
                
                if (Sounds == null || Sounds.Count == 0)
                    return true;

                foreach (AudioData audioData in Sounds)
                {
                    if (audioData == null || audioData.AudioClip == null)
                        return true;
                }

                return false;
            }
        }

        /// <summary> Returns TRUE if this SoundGroupData has at least one AudioClip referenced. If set to 'No Sound', returns FALSE </summary>
        public bool HasSound
        {
            get
            {
                if (SoundName.Equals(SoundyManagerConstant.NoSound))
                    return false; //if SoundName is set to No Sound -> has no sound
                
                if (Sounds == null || Sounds.Count == 0)
                    return false;      //if the Sounds list is null or empty -> has no sound
                
                return !HasMissingAudioClips;
            }
        }

        /// <summary> Returns a random pitch value between 0f and 4f </summary>
        public float RandomPitch =>
            SoundyUtils.SemitonesToPitch(Random.Range(Pitch.MinValue, Pitch.MaxValue));

        /// <summary> Returns a random volume value between 0f and 1f </summary>
        public float RandomVolume =>
            SoundyUtils.DecibelToLinear(Random.Range(Volume.MinValue, Volume.MaxValue));

        #endregion

        #region Public Variables

        /// <summary> The SoundDatabase name this SoundGroupData belongs to </summary>
        public string DatabaseName;

        /// <summary> Sound name as defined in the database. This is set by default as the first AudioClip name </summary>
        public string SoundName;

        /// <summary> Allows this SoundGroupData to play even though AudioListener.pause is set to true. This is useful for the menu element sounds or background music in pause menus </summary>
        public bool IgnoreListenerPause;

        /// <summary>
        ///     The custom volume interval of the AudioSource in decibels (-80dB to 0dB)
        ///     <para />
        ///     Every time a clip is played, a random value from the 0f - 1f interval will get set to the AudioSource.
        /// </summary>
        [MinMaxRange(SoundGroupDataConst.MinVolume, SoundGroupDataConst.MaxVolume)]
        public RangedFloat Volume;

        /// <summary>
        ///     The pitch interval for the AudioSource in semitones (-24 to 24)
        ///     <para />
        ///     Every time a clip is played, a random value from the 0f - 4f interval will get set to the AudioSource.
        /// </summary>
        [MinMaxRange(SoundGroupDataConst.MinPitch, SoundGroupDataConst.MaxPitch)]
        public RangedFloat Pitch;
        
        /// <summary> Sets how much this AudioSource is affected by 3D space calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D </summary>
        [Range(SoundGroupDataConst.MinSpatialBlend, SoundGroupDataConst.MaxSpatialBlend)]
        public float SpatialBlend;

        /// <summary> Play in a loop? </summary>
        public bool Loop;

        /// <summary> Sets the rules for playing the referenced AudioClips </summary>
        public PlayMode Mode;

        /// <summary> If Mode is set to PlayMode.Sequence and this flag is set to TRUE, then the play sequence will get automatically reset after an inactive time has passed since the last played sound </summary>
        public bool ResetSequenceAfterInactiveTime;

        /// <summary>
        ///     If Mode is set to PlayMode.Sequence and the ResetSequenceAfterInactiveTime flag is set to TRUE, this is the time period that needs to pass in order for the sequence to reset itself and start playing again from the first entry
        ///     <para> Note that the time is measured from the time a sound starts to play, not when it ends </para>
        /// </summary>
        public float SequenceResetTime;

        /// <summary> List of AudioData that are available to play </summary>
        public List<AudioData> Sounds = new List<AudioData>();

        public bool IsPlaying;

        #endregion

        #region Private Variables

        /// <summary> Internal variable that keeps track of the last played sounds index </summary>
        private int m_lastPlayedSoundsIndex = -1;

        /// <summary> Internal variable that keeps track of the last played sound time (used by the PlayMode.Sequence with the resetSequenceAfterInactiveTime flag set to TRUE</summary>
        private float m_lastPlayedSoundTime;

        /// <summary> Internal data list that keeps track of the played sounds </summary>
        private readonly List<AudioData> m_playedSounds = new List<AudioData>();

        /// <summary> Internal variable that holds a reference to the previously played AudioData </summary>
        private AudioData m_lastPlayedAudioData;

        #endregion

        #region Unity Methods

        private void Reset()
        {
            SoundName = SoundGroupDataConst.DefaultSoundName;
            IgnoreListenerPause = SoundGroupDataConst.DefaultIgnoreListenerPause;
            Loop = SoundGroupDataConst.DefaultLoop;
            Volume = new RangedFloat
            {
                MinValue = SoundGroupDataConst.DefaultVolume, MaxValue = SoundGroupDataConst.DefaultVolume
            };
            Pitch = new RangedFloat
            {
                MinValue = SoundGroupDataConst.DefaultPitch, MaxValue = SoundGroupDataConst.DefaultPitch
            };
            
            SpatialBlend = SoundGroupDataConst.DefaultSpatialBlend;
            Mode = SoundGroupDataConst.DefaultPlayMode;
            ResetSequenceAfterInactiveTime = SoundGroupDataConst.DefaultResetSequenceAfterInactiveTime;
            SequenceResetTime = SoundGroupDataConst.DefaultSequenceResetTime;
        }

        #endregion

        #region Public Methods

        /// <summary> Returns TRUE if the passed audio clip is referenced inside this Sound Group </summary>
        /// <param name="audioClip"> AudioClip reference to search for </param>
        public bool Contains(AudioClip audioClip)
        {
            if (audioClip == null)
                return false;

            foreach (AudioData audioData in Sounds)
            {
                if (audioData.AudioClip == audioClip)
                    return true;
            }
            
            return false;
        }
        
        public AudioData AddAudioData(AudioClip audioClip = null)
        {
            AudioData audioData = new AudioData(audioClip);
            Sounds.Add(audioData);

            return audioData;
        }

        public void RemoveAudioData(AudioData audioData)
        {
            Sounds.Remove(audioData);
        }

        /// <summary> Plays one of the sounds from the AudioClip list, with the set randomized values, and also tells the controller that it has a target transform it needs to follow while playing </summary>
        /// <param name="followTarget"> The target transform that the sound will follow while playing </param>
        /// <param name="outputAudioMixerGroup"> The output AudioMixerGroup that the sound will get routed through </param>
        public SoundyController Play(Transform followTarget, AudioMixerGroup outputAudioMixerGroup = null)
        {
            SoundyController controller = Play(followTarget.position, outputAudioMixerGroup);
            controller.SetFollowTarget(followTarget);
            
            return controller;
        }

        /// <summary> Plays one of the sounds from the AudioClip list, with the set randomized values, at the specified position </summary>
        /// <param name="position"> The world position where this sound will be played at </param>
        /// <param name="outputAudioMixerGroup"> The output AudioMixerGroup that the sound will get routed through </param>
        public SoundyController Play(Vector3 position, AudioMixerGroup outputAudioMixerGroup = null)
        {
            SoundyController controller = SoundyPooler.GetControllerFromPool();
            m_lastPlayedAudioData = GetAudioData(Mode);
            controller.SetSourceProperties(
                m_lastPlayedAudioData.AudioClip, RandomVolume, RandomPitch, Loop, SpatialBlend);
            controller.SetOutputAudioMixerGroup(outputAudioMixerGroup);
            controller.SetPosition(position);
            
            if (m_lastPlayedAudioData == null)
                return controller;
            
            controller.gameObject.name = "[" + SoundName + "]-(" + m_lastPlayedAudioData.AudioClip.name + ")";
            controller.Play();
            return controller;
        }

        /// <summary> [Editor Only] Plays a sound preview in the Editor </summary>
        /// <param name="audioSource"> AudioSource that will play the sound </param>
        /// <param name="outputAudioMixerGroup"> The output AudioMixerGroup that the sound will get routed through </param>
        /// <param name="audioClip"> AudioClip to play </param>
        public void PlaySoundPreview(AudioSource audioSource, AudioMixerGroup outputAudioMixerGroup, AudioClip audioClip)
        {
            if (audioSource == null)
                return;
            
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
            }
            else
            {
                m_lastPlayedAudioData = GetAudioData(Mode);
                
                if (m_lastPlayedAudioData == null)
                    return;
                
                audioSource.clip = m_lastPlayedAudioData.AudioClip;
            }

            audioSource.ignoreListenerPause = IgnoreListenerPause;
            audioSource.outputAudioMixerGroup = outputAudioMixerGroup;
            audioSource.volume = RandomVolume;
            audioSource.pitch = RandomPitch;
            audioSource.loop = Loop;
            audioSource.spatialBlend = SpatialBlend;
            Camera main = Camera.main;
            audioSource.transform.position = main == null ? Vector3.zero : main.transform.position;
            audioSource.Play();
        }

        /// <summary> [Editor Only] Plays a sound preview in the Editor </summary>
        /// <param name="audioSource"> AudioSource that will play the sound </param>
        /// <param name="outputAudioMixerGroup"> The output AudioMixerGroup that the sound will get routed through </param>
        public void PlaySoundPreview(AudioSource audioSource, AudioMixerGroup outputAudioMixerGroup) =>
            PlaySoundPreview(audioSource, outputAudioMixerGroup, null);

        /// <summary> [Editor Only] Stops the sound preview on the target AudioSource </summary>
        /// <param name="audioSource"> Target AudioSource </param>
        public void StopSoundPreview(AudioSource audioSource)
        {
            if (audioSource == null)
                return;
            
            audioSource.Stop();
        }

        /// <summary> [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects) </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void SetDirty(bool saveAssets) =>
            MyUtils.SetDirty(this, saveAssets);

        #endregion

        #region Private Methods

        /// <summary> Returns the proper AudioData that needs to get played according to the set settings </summary>
        /// <param name="playMode">The play mode.</param>
        private AudioData GetAudioData(PlayMode playMode)
        {
            switch (playMode)
            {
                case PlayMode.Random:

                    if (m_playedSounds.Count == Sounds.Count)
                        m_playedSounds.Clear();

                    AudioData foundClip = null; //look for a sound that has not been played
                    
                    while (foundClip == null)   //until such a sound is found continue the search
                    {
                        int randomIndex = Random.Range(0, Sounds.Count);
                        foundClip = Sounds[randomIndex];        //get a random sound
                        
                        if (m_playedSounds.Contains(foundClip)) //check that it has not been played
                        {
                            foundClip = null; //it has been played -> discard it
                        }
                        else
                        {
                            m_playedSounds.Add(foundClip); //it has not been played -> add it to the _playedSounds list and continue
                            m_lastPlayedSoundsIndex = randomIndex;
                        }
                    }

                    return foundClip; //return the sound that will get played

                case PlayMode.Sequence:
                    if (m_playedSounds.Count == Sounds.Count) //if all the sounds in the sounds list were played
                        m_lastPlayedSoundsIndex = -1;         //-> reset the sequence index

                    if (ResetSequenceAfterInactiveTime &&                                      //if resetSequenceAfterInactiveTime 
                        Time.realtimeSinceStartup - m_lastPlayedSoundTime > SequenceResetTime) //and enough time has passed since the last sound in the sequence has been played //Time.unscaledTime
                        m_lastPlayedSoundsIndex = -1;                                          //-> reset the sequence index

                    if (m_lastPlayedSoundsIndex == -1) //if the last played index is in the reset state (-1)
                        m_playedSounds.Clear();        //-> reset the played sounds list

                    m_lastPlayedSoundsIndex = m_lastPlayedSoundsIndex == -1 || m_lastPlayedSoundsIndex >= Sounds.Count - 1
                                                  ? //if the index has been reset (-1)
                                                  0
                                                  :                            //-> set the last played index as the first entry in the sounds list
                                                  m_lastPlayedSoundsIndex + 1; //-> otherwise set the last played index as the next entry in the sequence

                    m_playedSounds.Add(Sounds[m_lastPlayedSoundsIndex]); //add the played sound to the playedSounds list
                    m_lastPlayedSoundTime = Time.realtimeSinceStartup;   //save the last played sound time
                    return Sounds[m_lastPlayedSoundsIndex];              //return the sound that will get played
            }

            return null; //this should never happen
        }

        #endregion
    }
}