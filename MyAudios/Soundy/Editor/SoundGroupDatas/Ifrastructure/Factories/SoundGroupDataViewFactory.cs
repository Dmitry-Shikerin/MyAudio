using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroupDatas.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using UnityEditor;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Ifrastructure.Factories
{
    public class SoundGroupDataViewFactory
    {
        public ISoundGroupDataView Create(SoundGroupData soundGroupData)
        {
            AudioDataViewFactory audioDataViewFactory = new AudioDataViewFactory();
            
            SoundGroupDataView view = new SoundGroupDataView();
            SoundGroupDataPresenter presenter = new SoundGroupDataPresenter(
                soundGroupData, view, audioDataViewFactory);
            view.Construct(presenter);
            
            return view;
        }
    }
}