using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public enum CommonExecutionResult
    {
        Success,
        Failure,
        Canceled
    }
    
    public interface IExecutable<TResult>
    {
        TResult Run();
    }
    public interface IExecutable<TParam, TResult>
    {
        TResult Run(TParam param);
    }
    public interface IExecutable<TParam1, TParam2, TResult>
    {
        TResult Run(TParam1 param1, TParam2 param2);
    }
    
    public interface IExecutableAsync<TResult>
    {
        UniTask<TResult> Run(CancellationTokenSource ctx);
    }

    public interface IExecutableAsync<TParam, TResult>
    {
        UniTask<TResult> Run(TParam param, CancellationTokenSource ctx);
    }

    public interface IExecutableAsyncWithToken<TParam, TResult>
    {
        UniTask<TResult> Run(TParam param, CancellationToken token);
    }
    public interface IExecutableAsyncWithToken<TParam1, TParam2,  TResult>
    {
        UniTask<TResult> Run(TParam1 param1, TParam2 param2, CancellationToken token);
    }
    public interface IExecutableAsyncWithToken<TParam1, TParam2, TParam3, TResult>
    {
        UniTask<TResult> Run(TParam1 param1, TParam2 param2, TParam3 param3, CancellationToken token);
    }
    
    public interface IExecutableAsyncWithToken<TResult>
    {
        UniTask<TResult> Run(CancellationToken token);
    }
}
