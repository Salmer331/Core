using UnityEngine;

namespace Core.SceneManagement
{
    public class SceneActivity : MonoBehaviour
    {
        [SerializeField] private SceneActivator activator = default;
        [SerializeField] private SceneLifeCycleType lifecycleType = default;
        [SerializeField] private SceneState state = default;


        public string SceneName => this.gameObject.scene.name;
        public SceneActivator Activator => activator;
        public SceneLifeCycleType LifeCycleType => lifecycleType;

        public SceneState SceneState
        {
            get => state;
            set => state = value;
        }
    }
}