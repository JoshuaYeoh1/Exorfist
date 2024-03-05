using UnityEngine;

public class Monostate<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Current
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<T>(); // Try to find an existing instance in the scene

                if(!instance)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name); // Create a new GameObject to host the singleton instance

                    instance = singletonObject.AddComponent<T>();

                    //DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if(!instance)
        {
            instance=this as T;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // if(!instance)
        // {
        //     instance = this as T;
        // }
        // else
        // {
        //     if(instance!=this)
        //         Destroy(gameObject);
        // }
    }
}
