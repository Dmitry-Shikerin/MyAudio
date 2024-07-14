using System.Collections.Generic;
using Doozy.Editor.EditorUI.Components;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine.Events;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Views.Interfaces
{
    public interface ISoundyDataBaseView : IView
    {
        public IReadOnlyList<FluidToggleButtonTab> DatabaseButtons { get; }
        
        void AddDataBaseButton(string name, UnityAction callback);
        void SetSoundDataBase(ISoundDataBaseView dataBaseView);
        void RefreshDataBasesButtons();
        void RenameButtons();
        void UpdateDataBase();
        public void ClearButtons();
    }
}