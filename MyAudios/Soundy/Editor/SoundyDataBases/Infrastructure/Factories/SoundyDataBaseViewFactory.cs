using MyAudios.Soundy.Editor.SoundDataBases.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundyDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundyDataBases.Views.Implementation;
using MyAudios.Soundy.Editor.SoundyDataBases.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Infrastructure.Factories
{
    public class SoundyDataBaseViewFactory
    {
        public ISoundyDataBaseView Create(SoundyDatabase soundyDatabase)
        {
            SoundDataBaseViewFactory soundDataBaseViewFactory = new SoundDataBaseViewFactory();
            
            SoundyDataBaseView view = new SoundyDataBaseView();
            SoundyDataBasePresenter presenter = new SoundyDataBasePresenter(
                soundyDatabase, view, soundDataBaseViewFactory);
            view.Construct(presenter);
            
            return view;
        }
    }
}