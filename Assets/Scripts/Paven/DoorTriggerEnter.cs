using Unity.VisualScripting;
using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    [SerializeField] private bool isPopupPoint;
    [SerializeField] private bool destroyOnContact;
    public int roomID { get; private set; }

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

            if (isPopupPoint)
            {
                if (!popUpPrefabRef)
                {
                    popUpPrefabRef = Instantiate(popUpPrefab);
                    if (destroyOnContact)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                GameEventSystem.Current?.OnRoomEnter();
                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.attachedRigidbody.gameObject;

        if(player.tag=="Player")
        {
            if (isPopupPoint)
            {
                if (!popUpPrefabRef)
                {
                    Destroy(popUpPrefabRef);
                }
            }
            else
            {

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
