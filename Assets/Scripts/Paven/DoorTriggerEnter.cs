using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    private void Start()
    {
        GameEventSystem.Current.RoomEnterEvent += OnRoomEnter;
    }

    private void OnDestroy()
    {
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEnter;
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
        GameEventSystem.Current.OnDoorTriggerEnter(currentRoomTransform);
    }
}
