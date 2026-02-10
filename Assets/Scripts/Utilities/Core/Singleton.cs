using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>(); //Find first object in the scene that has the specific script type

                if (_instance == null)  //if cannot find it
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);    //create a new game object
                    _instance = singletonObject.AddComponent<T>();  //add the specific script component to this new game object
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this as T)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
