using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Core.SceneManagement
{
    public class AsyncActivateSceneExecutor: IExecutableAsyncWithToken<SceneActivity, CommonExecutionResult>
    {
        [Inject]
        public AsyncActivateSceneExecutor(){}
        
        public async UniTask<CommonExecutionResult> Run(SceneActivity scene, CancellationToken token)
        {
            try
            {
                if (scene.SceneState == SceneState.Active) return CommonExecutionResult.Success;
                
                await scene.Activator.Activate(true, token);
                scene.SceneState = SceneState.Active;
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