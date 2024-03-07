using UnityEngine;

public class Monostate<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    static bool isInitialized = false;

    public static T Current
    {
        get
        {
            if(!isInitialized)
            {
                InitializeSingleton();
            }
            return instance;
        }
    }

    void Awake()
    {
        if(!isInitialized)
        {
            InitializeSingleton();
        }
        else if(instance!=this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    static void InitializeSingleton()
    {
        instance = FindObjectOfType<T>();

        if(!instance)
        {
            GameObject singletonObject = new GameObject(typeof(T).Name);
            instance = singletonObject.AddComponent<T>();
            DontDestroyOnLoad(singletonObject);
        }

        isInitialized = true;
    }
}
