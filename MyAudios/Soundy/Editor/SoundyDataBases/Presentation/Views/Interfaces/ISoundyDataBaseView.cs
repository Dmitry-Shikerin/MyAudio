using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine.Events;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Views.Interfaces
{
    public interface ISoundyDataBaseView : IView
    {
        void AddDataBaseButton(string name, UnityAction callback);
        void SetSoundDataBase(ISoundDataBaseView dataBaseView);
    }
}