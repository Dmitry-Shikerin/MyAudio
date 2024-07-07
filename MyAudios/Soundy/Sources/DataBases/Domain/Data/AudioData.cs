using System;
using UnityEngine;

namespace MyAudios.Soundy.DataBases.Domain.Data
{
    /// <summary> Audio info for any referenced AudioClip in the Soundy system </summary>
    [Serializable]
    public class AudioData
    {
        #region Constants

        public const float DefaultWeight = 1f;
        public const float MaxWeight = 1f;
        public const float MinWeight = 0f;

        #endregion

        #region Public Variables

        /// <summary> Direct reference to an AudioClip </summary>
        public AudioClip AudioClip;

        /// <summary> (Not Implemented) Weight of this AudioClip in the SoundGroupData </summary>
        [Range(MinWeight, MaxWeight)]
        public float Weight = DefaultWeight;

        #endregion

        #region Constructors

        /// <summary> Creates a new instance for this class </summary>
        public AudioData() =>
            Reset();

        /// <summary> Creates a new instance for this class and sets the given AudioClip reference </summary>
        /// <param name="audioClip"> AudioClip reference </param>
        public AudioData(AudioClip audioClip)
        {
            Reset();
            AudioClip = audioClip;
        }

        /// <summary> Creates a new instance for this class and sets the given AudioClip reference with the given weight </summary>
        /// <param name="audioClip"> AudioClip reference </param>
        /// <param name="weight"> (Not Implemented) AudioClip weight </param>
        public AudioData(AudioClip audioClip, float weight)
        {
            Reset();
            AudioClip = audioClip;
            Weight = weight;
        }

        #endregion

        #region Public Methods

        /// <summary> Resets this instance to the default values </summary>
        public void Reset()
        {
            AudioClip = null;
            Weight = 1;
        }

        #endregion
    }
}