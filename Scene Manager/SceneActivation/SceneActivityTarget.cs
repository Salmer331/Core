using UnityEngine;

namespace Core.SceneManagement
{
    public abstract class SceneActivityTarget : MonoBehaviour
    {
        public abstract void Activate(bool activate);
    }
}