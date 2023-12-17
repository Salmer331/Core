using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    public static class SceneManagerExtensions
    {
        internal static SceneActivity GetSceneActivator(this Scene scene)
        {
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObj in rootObjects)
            {
                if (rootObj.TryGetComponent(out SceneActivity activator))
                {
                    return activator;
                }
            }
            return null;
        }
    }
}

