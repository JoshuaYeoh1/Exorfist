using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;

    private void Start()
    {
        GameEventSystem.current.OnRoomEntered += OnRoomEnter;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.OnRoomEntered -= OnRoomEnter;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(popUpPrefabRef == null)
            {
                popUpPrefabRef = Instantiate(popUpPrefab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(popUpPrefabRef != null)
            {
                Destroy(popUpPrefabRef);
            }
        }
    }

    private void OnRoomEnter()
    {
        Destroy(popUpPrefabRef);
    }
}
