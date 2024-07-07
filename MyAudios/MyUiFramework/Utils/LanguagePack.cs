using System;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils
{
    [Serializable]
    public class LanguagePack : ScriptableObject
    {
        private const string CURRENT_LANGUAGE_PREFS_KEY = "Doozy.CurrentLanguage";
        public const Doozy.Engine.Language DEFAULT_LANGUAGE = Doozy.Engine.Language.English;

        private static Doozy.Engine.Language s_currentLanguage = Doozy.Engine.Language.Unknown;

        public static Doozy.Engine.Language CurrentLanguage
        {
            get
            {
                if (s_currentLanguage != Doozy.Engine.Language.Unknown)
                    return s_currentLanguage;
                
                CurrentLanguage = (Doozy.Engine.Language)
                    PlayerPrefs.GetInt(CURRENT_LANGUAGE_PREFS_KEY, (int) DEFAULT_LANGUAGE);
                
                return s_currentLanguage;
            }
            set
            {
                SaveLanguagePreference(value);
                s_currentLanguage = value;
            }
        }

        private static void SaveLanguagePreference(Doozy.Engine.Language language) =>
            SaveLanguagePreference(CURRENT_LANGUAGE_PREFS_KEY, language);

        private static void SaveLanguagePreference(string prefsKey, Doozy.Engine.Language language)
        {
            PlayerPrefs.SetInt(prefsKey, (int) language);
            PlayerPrefs.Save();
        }
    }
}