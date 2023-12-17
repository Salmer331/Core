using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Core.SceneManagement
{
    public class AsyncLoadSceneExecutor: IExecutableAsyncWithToken<string, CommonExecutionResult>
    {
        private readonly ISceneControllerManageable _scenesController;
        [Inject]
        public AsyncLoadSceneExecutor(ISceneControllerManageable controller)
        {
            _scenesController = controller;
        }
        
        public async UniTask<CommonExecutionResult> Run(string param, CancellationToken token)
        {
            try
            {
                if (!_scenesController.IsSceneLoaded(param))
                {
                    await SceneManager.LoadSceneAsync(param, LoadSceneMode.Additive).WithCancellation(token);
                
                    var activity = SceneManager.GetSceneByName(param).GetSceneActivator();
                    if (activity == null) return CommonExecutionResult.Failure;
                    _scenesController.AddCachedScene(activity);
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                return CommonExecutionResult.Failure;
            }
            
            return CommonExecutionResult.Success;
        }
    }
}