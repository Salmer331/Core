using UnityEngine;
using VContainer;

namespace Core
{
    public class CameraInjector: MonoBehaviour
    {
        [SerializeField] private Canvas canvas = default;

        [Inject]
        public void InjectCamera(Camera rootCamera)
        {
            canvas.worldCamera = rootCamera;
        }
    }
}