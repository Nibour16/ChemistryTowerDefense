using UnityEngine;

public class BaseFacade<TInterface, TManager> : MonoBehaviour
    where TInterface : class
    where TManager : MonoBehaviour
{
    protected TManager Manager { get; private set; }

    #region Unity Lifecycle
    protected virtual void Awake()
    {
        Manager = ResolveManager();

        if (Manager == null)
        {
            Debug.LogError(
                $"{typeof(TManager).Name} could not be resolved on {name}");
        }

        RegisterService();
    }

    protected virtual void OnDestroy()
    {
        UnregisterService();
    }

    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (!(this is TInterface))
        {
            Debug.LogError(
                $"{GetType().Name} must implement {typeof(TInterface).Name}");
        }
    }
    #endif

    #endregion

    #region Manager Resolution
    /// <summary>
    /// Override this if you want custom resolving logic.
    /// Default: Try Singleton, fallback to GetComponent.
    /// </summary>
    protected virtual TManager ResolveManager()
    {
        // Try singleton first (safe, no reflection)
        if (ManagerIsSingleton(out var singletonInstance))
            return singletonInstance;

        // Fallback: same GameObject
        return GetComponent<TManager>();
    }

    /// <summary>
    /// Try resolve as Singleton if TManager inherits Singleton&lt;TManager&gt;
    /// </summary>
    private bool ManagerIsSingleton(out TManager instance)
    {
        instance = null;

        if (typeof(Singleton<TManager>).IsAssignableFrom(typeof(TManager)))
        {
            instance = Singleton<TManager>.Instance;
            return instance != null;
        }

        return false;
    }
    #endregion

    #region Service Registration
    protected virtual void RegisterService()
    {
        if (this is TInterface service)
        {
            ServiceLocator.Register<TInterface>(service);
        }
        else
        {
            Debug.LogError(
                $"{GetType().Name} must implement {typeof(TInterface).Name}");
        }
    }

    protected virtual void UnregisterService()
    {
        ServiceLocator.Unregister<TInterface>();
    }
    #endregion
}
