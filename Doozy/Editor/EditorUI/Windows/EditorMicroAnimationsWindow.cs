﻿using Doozy.Editor.EditorUI.ScriptableObjects.MicroAnimations;
using Doozy.Editor.EditorUI.Windows.Internal;
using UnityEditor;

namespace Doozy.Editor.EditorUI.Windows
{
    public class EditorMicroAnimationsWindow : EditorUIDatabaseWindow<EditorMicroAnimationsWindow>
    {
        public const string k_WindowTitle = "Micro-Animations";
        public const string k_MenuPath = EditorUIWindow.k_WindowMenuPath + "/" + EditorUIWindow.k_WindowTitle + "/" + k_WindowTitle;
        public const int k_MenuItemPriority = -498;
        
        public static void Open() => InternalOpenWindow(k_WindowTitle);

        [MenuItem(k_MenuPath, false, k_MenuItemPriority)]
        private static void RefreshDatabase()
        {
            if (EditorUtility.DisplayDialog
                (
                    $"Refresh the {k_WindowTitle} database?",
                    "This will regenerate the database with the latest registered micro-animations, from the source files." +
                    "\n\n" +
                    "Takes around 1 to 30 seconds, depending on the number of source files and your computer's performance." +
                    "\n\n" +
                    "This operation cannot be undone!",
                    "Yes",
                    "No"
                )
               )
                EditorDataMicroAnimationDatabase.instance.RefreshDatabase();
        }
    }
}