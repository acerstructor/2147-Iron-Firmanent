using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("[Singleton] Trying to instantiate multiple instances of a singleton class.");
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                        }
                    }
                }
            }
            return _instance;
        }
    }
}
