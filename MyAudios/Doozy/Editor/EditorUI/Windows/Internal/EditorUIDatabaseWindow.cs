using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.UIElements;
using Doozy.Runtime.Common.Extensions;
using Doozy.Runtime.Common.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEditor;
using UnityEngine.UIElements;

namespace Doozy.Editor.EditorUI.Windows.Internal
{
    public class EditorUIDatabaseWindow<T> : FluidWindow<T> where T : EditorWindow
    {
        protected override void CreateGUI()
        {
            windowLayout = GetFluidWindowLayout($"{GetType().Name}");
            
            if (windowLayout == null)
                return;
            
            windowLayout.SetStyleFlexGrow(1);
            root
                .RecycleAndClear() 
                .AddChild(windowLayout);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ((FluidWindowLayout) windowLayout).Dispose();
        }

        private static VisualElement GetFluidWindowLayout(string layoutName)
        {
            if (layoutName.IsNullOrEmpty())
                return null;
            
            IEnumerable<Type> results = ReflectionUtils.GetTypesThatImplementInterface<IEditorUIDatabaseWindowLayout>();
            List<Type> layoutTypes = results.Where(result => result.Name.Contains(layoutName)).ToList();
            
            if (layoutTypes.Any() == false)
                return null;
            
            return (VisualElement)Activator.CreateInstance(layoutTypes[0]);
        }
    }
}
