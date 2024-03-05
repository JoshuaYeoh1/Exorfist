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

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
}
