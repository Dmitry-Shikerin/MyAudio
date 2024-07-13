using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.SoundGroupDatas.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Ifrastructure.Factories
{
    public class SoundGroupDataViewFactory
    {
        public ISoundGroupDataView Create(SoundGroupData soundGroupData)
        {
            SoundGroupDataView view = new SoundGroupDataView();
            SoundGroupDataPresenter presenter = new SoundGroupDataPresenter(soundGroupData, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}