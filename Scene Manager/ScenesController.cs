using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.SceneManagement
{
    public interface ISceneControllerManageable
    {
        void AddCachedScene(SceneActivity scene);
        void RemoveCachedScene(SceneActivity scene);
        bool IsSceneLoaded(string sceneName);
    }

    public interface ISceneControllerService
    {
        /// <summary>
        /// Load scene without activating it.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        UniTask<CommonExecutionResult> LoadScene(string sceneName, CancellationToken token);
        /// <summary>
        /// Use it to activate scene if it's loaded too.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        UniTask<CommonExecutionResult> LoadAndActivateScene(string sceneName, CancellationToken token);
        /// <summary>
        /// Deactivate scene without unloading it.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        UniTask<CommonExecutionResult> DeactivateScene(string sceneName, CancellationToken token);
        UniTask<CommonExecutionResult> UnloadScene(string sceneName, CancellationToken token);
        UniTask<CommonExecutionResult> DeactivateSceneWithActivityRules(string sceneName, CancellationToken token);
    }
    
    public class ScenesController: ISceneControllerManageable, ISceneControllerService
    {
        private List<SceneActivity> _cachedScenes = new List<SceneActivity>();

        private readonly AsyncLoadSceneExecutor _loadScene;
        private readonly AsyncUnloadSceneExecutor _unloadScene;
        private readonly AsyncActivateSceneExecutor _activateScene;
        private readonly AsyncDeactivateSceneExecutor _deactivateScene;
        
        [Inject]
        internal ScenesController()
        {
            _loadScene = new AsyncLoadSceneExecutor(this);
            _unloadScene = new AsyncUnloadSceneExecutor(this);
            _activateScene = new AsyncActivateSceneExecutor();
            _deactivateScene = new AsyncDeactivateSceneExecutor();
        }
        
        void ISceneControllerManageable.AddCachedScene(SceneActivity scene)
        {
            if (_cachedScenes.Contains(scene)) return;
            _cachedScenes.Add(scene);
        }
        void ISceneControllerManageable.RemoveCachedScene(SceneActivity scene)
        {
            if (_cachedScenes.Contains(scene))
            {
                _cachedScenes.Remove(scene);
            }
        }
        bool ISceneControllerManageable.IsSceneLoaded(string sceneName)
        {
            foreach (var sceneActivity in _cachedScenes)
            {
                if (sceneActivity.SceneName == sceneName) return true;
            }

            return false;
        }
        
        async UniTask<CommonExecutionResult> ISceneControllerService.LoadScene(string sceneName, CancellationToken token)
        {
            var result = await _loadScene.Run(sceneName, token);
            return result;
        }
        async UniTask<CommonExecutionResult> ISceneControllerService.LoadAndActivateScene(string sceneName, CancellationToken token)
        {
            var result = await _loadScene.Run(sceneName, token);
            if (result != CommonExecutionResult.Success) return result;

            if (!GetActivityBySceneName(sceneName, out var activity))
            {
                return CommonExecutionResult.Failure;
            }
            
            result = await _activateScene.Run(activity, token);
            return result;
        }
        async UniTask<CommonExecutionResult> ISceneControllerService.DeactivateScene(string sceneName, CancellationToken token)
        {
            if (!GetActivityBySceneName(sceneName, out var activity))
            {
                return CommonExecutionResult.Failure;
            }
            var result = await _deactivateScene.Run(activity, token);
            return result;
        }
        async UniTask<CommonExecutionResult> ISceneControllerService.UnloadScene(string sceneName, CancellationToken token)
        {
            if (!GetActivityBySceneName(sceneName, out var activity))
            {
                return CommonExecutionResult.Failure;
            }
            
            var result = await _unloadScene.Run(activity, token);
            return result;
        }
        async UniTask<CommonExecutionResult> ISceneControllerService.DeactivateSceneWithActivityRules(string sceneName, CancellationToken token)
        {
            if (!GetActivityBySceneName(sceneName, out var activity))
            {
                return CommonExecutionResult.Failure;
            }

            if (activity.LifeCycleType == SceneLifeCycleType.PreserveAfterUnloading)
            {
                var result = await _deactivateScene.Run(activity, token);
                return result;
            }

            if (activity.LifeCycleType == SceneLifeCycleType.RemoveAfterUnloading)
            {
                var result = await _deactivateScene.Run(activity, token);
                if (result == CommonExecutionResult.Failure) return result;
                result = await _unloadScene.Run(activity, token);
                return result;
            }

            return CommonExecutionResult.Failure;
        }

        bool GetActivityBySceneName(string sceneName, out SceneActivity sceneActivity)
        {
            sceneActivity = _cachedScenes.Find(x => x.SceneName == sceneName);
            if (sceneActivity == null) return false;
            return true;
        }
    }
}