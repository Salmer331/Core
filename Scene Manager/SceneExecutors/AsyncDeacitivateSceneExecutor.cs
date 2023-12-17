using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.SceneManagement
{
    public class AsyncDeactivateSceneExecutor: IExecutableAsyncWithToken<SceneActivity, CommonExecutionResult>
    {
        [Inject]
        public AsyncDeactivateSceneExecutor()
        {
        }

        public async UniTask<CommonExecutionResult> Run(SceneActivity scene, CancellationToken token)
        {
            try
            {
                if (scene.SceneState == SceneState.Unloaded) return CommonExecutionResult.Success;
                
                await scene.Activator.Activate(false, token);
                scene.SceneState = SceneState.Unloaded;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return CommonExecutionResult.Failure;
            }

            return CommonExecutionResult.Success;
        }
    }
}