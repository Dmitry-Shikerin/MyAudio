using UnityEngine;

namespace MyAudios.MyUiFramework.Utils
{
    public static class DataUtils
    {
        /// <summary> Sets the value of the preference identified by key </summary>
        public static void PlayerPrefsSetInt(string key, int value) =>
            PlayerPrefs.SetInt(key, value);

        /// <summary> Sets the value of the preference identified by key </summary>
        public static void PlayerPrefsSetFloat(string key, float value) =>
            PlayerPrefs.SetFloat(key, value);

        /// <summary> Sets the value of the preference identified by key </summary>
        public static void PlayerPrefsSetString(string key, string value) =>
            PlayerPrefs.SetString(key, value);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static int PlayerPrefsGetInt(string key) =>
            PlayerPrefs.GetInt(key);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static int PlayerPrefsGetInt(string key, int defaultValue) =>
            PlayerPrefs.GetInt(key, defaultValue);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static float PlayerPrefsGetFloat(string key) =>
            PlayerPrefs.GetFloat(key);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static float PlayerPrefsGetFloat(string key, float defaultValue) =>
            PlayerPrefs.GetFloat(key, defaultValue);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static string PlayerPrefsGetString(string key) =>
            PlayerPrefs.GetString(key);

        /// <summary> Returns the value corresponding to key in the preference file if it exists </summary>
        public static string PlayerPrefsGetString(string key, string defaultValue) =>
            PlayerPrefs.GetString(key, defaultValue);

        /// <summary> Removes key and its corresponding value from the preferences </summary>
        public static void PlayerPrefsDeleteKey(string key) =>
            PlayerPrefs.DeleteKey(key);

        /// <summary> Removes all keys and values from the preferences. Use with caution </summary>
        public static void PlayerPrefsDeleteAll() =>
            PlayerPrefs.DeleteAll();

        /// <summary> Writes all modified preferences to disk </summary>
        public static void PlayerPrefsSave() =>
            PlayerPrefs.Save();

        /// <summary> Returns true if key exists in the preferences </summary>
        public static bool PlayerPrefsHasKey(string key) =>
            PlayerPrefs.HasKey(key);
    }
}