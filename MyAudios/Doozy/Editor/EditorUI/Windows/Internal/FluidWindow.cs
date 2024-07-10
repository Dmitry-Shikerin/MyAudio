﻿using Doozy.Editor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Doozy.Editor.EditorUI.Windows.Internal
{
    public abstract class FluidWindow<T> : EditorWindow where T : EditorWindow
    {
        protected string EditorPrefsKey(string variableName) => $"{GetType().FullName} - {variableName}";

        protected PlayModeStateChange currentPlayModeState { get; private set; } = PlayModeStateChange.EnteredEditMode;

        protected VisualElement root => rootVisualElement;
        protected VisualElement windowLayout { get; set; }

        // public static bool isOpen { get; private set; }
        public static bool isOpen => HasOpenInstances<T>();

        public static bool isFocused => instance != null && instance.hasFocus;
        
        #region Instance

        private static T s_instance;

        public static T instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;
                
                s_instance = window;
                
                if (s_instance != null)
                    return s_instance;
                
                s_instance = GetWindow<T>(false);
                
                return s_instance;
            }
        }

        /*
       * An alternative way to get Window, because
       * GetWindow<T>() forces window to be active and present
       */
        private static T window
        {
            get
            {
                T[] windows = Resources.FindObjectsOfTypeAll<T>();
                
                return windows.Length > 0 ? windows[0] : null;
            }
        }

        #endregion

        protected static void InternalOpenWindow(string windowTitle)
        {
            instance.Show();
            instance.titleContent.text = windowTitle;
        }

        protected virtual void Awake() {}

        protected virtual void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        protected virtual void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        protected virtual void OnDestroy() {}

        protected virtual void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // Debugger.Log($"(FluidWindow).{nameof(OnPlayModeStateChanged)}({state})");

            currentPlayModeState = state;
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                {
                    OnDestroy();
                    root.RecycleAndClear();
                    CreateGUI();
                    break;
                }
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                {
                    //ignored
                    break;
                }
            }
        }

        protected abstract void CreateGUI();
    }
}
