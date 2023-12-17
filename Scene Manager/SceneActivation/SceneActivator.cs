using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.SceneManagement
{
    public abstract class SceneActivator : MonoBehaviour
    {
        [SerializeField] private SceneActivityTarget[] activityTargets = default;

        public async UniTask Activate(bool activate, CancellationToken token)
        {
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
            foreach (var target in activityTargets)
            {
                target.Activate(activate);
            }
        }
    }
}