using System;
using UniRx;

namespace Core.Presenters
{
    public abstract class DisposablePresenter: IDisposable
    {
        protected readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public abstract void InitializePresenter();
    }
}