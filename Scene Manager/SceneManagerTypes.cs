namespace Core.SceneManagement
{
    public enum SceneLifeCycleType
    {
        PreserveAfterUnloading,
        RemoveAfterUnloading
    }

    public enum SceneState
    {
        Active,
        Unloaded
    }
}