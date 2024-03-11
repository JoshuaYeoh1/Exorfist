using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    void OnEnable()
    {
        GameEventSystem.Current.RoomEnterEvent += OnRoomEnter;
    }
    void OnDisable()
    {
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.attachedRigidbody.gameObject;

        if(player.tag=="Player")
        {
            OnDoorTriggerEnter();

            if(!popUpPrefabRef)
            {
                popUpPrefabRef = Instantiate(popUpPrefab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.attachedRigidbody.gameObject;

        if(player.tag=="Player")
        {
            if(popUpPrefabRef)
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
