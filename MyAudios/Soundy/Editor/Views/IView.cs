using System;

namespace MyAudios.Soundy.Editor.Views
{
    public interface IView : IDisposable
    {
        void Initialize();
        void CreateView();
    }
}