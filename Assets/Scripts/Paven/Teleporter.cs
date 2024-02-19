using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform teleporterEnd;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            TeleportPlayer(player);
        }
    }

    private void TeleportPlayer(GameObject Player)
    {
        Player.transform.position = teleporterEnd.position;
    }
}
