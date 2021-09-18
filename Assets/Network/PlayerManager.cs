using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;

    public void RecieveMovement(Quaternion rotation, Vector3 movement)
    {
        transform.localRotation = rotation;
        transform.localPosition = movement;
    }
}
