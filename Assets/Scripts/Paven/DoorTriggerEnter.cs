using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    private void Start()
    {
<<<<<<< HEAD
        GameEventSystem.current.OnRoomEntered += OnRoomEnter;
=======
        GameEventSystem.current.RoomEnterEvent += OnRoomEnter;
>>>>>>> main
    }

    private void OnDestroy()
    {
<<<<<<< HEAD
        GameEventSystem.current.OnRoomEntered -= OnRoomEnter;
=======
        GameEventSystem.current.RoomEnterEvent -= OnRoomEnter;
>>>>>>> main
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        GameObject player = other.transform.root.gameObject;
        if (player.CompareTag("Player"))
        {
            OnDoorTriggerEnter();
            if(popUpPrefabRef == null)
            {
                popUpPrefabRef = Instantiate(popUpPrefab);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.CompareTag("Player"))
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

    private void OnDoorTriggerEnter()
    {
<<<<<<< HEAD
        GameEventSystem.current.doorTriggerEnter(currentRoomTransform);
=======
        GameEventSystem.current.OnDoorTriggerEnter(currentRoomTransform);
>>>>>>> main
    }
}
