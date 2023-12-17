using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Core.SceneManagement
{
    public class AsyncUnloadSceneExecutor: IExecutableAsyncWithToken<SceneActivity, CommonExecutionResult>
    {
        private readonly ISceneControllerManageable _scenesController;
        [Inject]
        public AsyncUnloadSceneExecutor(ISceneControllerManageable controller)
        {
            _scenesController = controller;
        }
        
        public async UniTask<CommonExecutionResult> Run(SceneActivity param, CancellationToken token)
        {
            try
            {
                await SceneManager.UnloadSceneAsync(param.SceneName).WithCancellation(token);
                _scenesController.RemoveCachedScene(param);
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