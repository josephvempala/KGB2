using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

class PlayerNetworkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    CharacterController Controller;

    [Header("network")]
    public uint current_tick;
    private static readonly uint buffer_size = 256;
    public Controls[] Inputs = new Controls[buffer_size];
    public ClientState[] ClientStates = new ClientState[buffer_size];
    private Queue<Tuple<Vector3,uint>> ReceivedPosition = new Queue<Tuple<Vector3,uint>>(10);

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        while (ReceivedPosition.Count > 0)
        {
            var latestReceivedPosition = ReceivedPosition.Dequeue();
            uint bufferSlot = latestReceivedPosition.Item2 % buffer_size;
            Vector3 positionError = latestReceivedPosition.Item1 - ClientStates[bufferSlot].position;

            if (positionError.sqrMagnitude > 0.0000001f)
            {
                Controller.enabled = false;
                Controller.transform.localPosition = latestReceivedPosition.Item1;
                Controller.enabled = true;
                uint rewindTick = latestReceivedPosition.Item2;
                while (rewindTick < current_tick)
                {
                    bufferSlot = rewindTick % buffer_size;
                    Inputs[bufferSlot] = movement.CurentInputs;
                    ClientStates[bufferSlot].position = transform.localPosition;
                    ClientStates[bufferSlot].rotation = transform.localRotation;

                    var movementdir = movement.CalculateGroundMovement(Inputs[bufferSlot]);
                    Controller.Move(movementdir * Time.deltaTime);
                    rewindTick++;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        SendControls();
        SendOrientation();
        current_tick++;
    }

    private void SendControls()
    {
        byte[] controlBuffer = ArrayPool<byte>.Shared.Rent(15);
        movement.CurentInputs.tick = current_tick;
        movement.CurentInputs.Serialize(ref controlBuffer);
        StoreStateInBuffers();
        ClientSend.SendControls(controlBuffer);
        movement.CurentInputs.Reset();
        ArrayPool<byte>.Shared.Return(controlBuffer);
    }

    private void StoreStateInBuffers()
    {
        uint buffer_slot = current_tick % buffer_size;
        Inputs[buffer_slot] = movement.CurentInputs;
        ClientStates[buffer_slot].position = transform.localPosition;
        ClientStates[buffer_slot].rotation = transform.localRotation;
    }

    private void SendOrientation()
    {
        byte[] orientationBuffer = ArrayPool<byte>.Shared.Rent(8);
        mouseLook.orientationToSend.tick = current_tick;
        mouseLook.orientationToSend.Serialize(ref orientationBuffer);
        ClientSend.SendOrientation(orientationBuffer);
        mouseLook.orientationToSend.Reset();
        ArrayPool<byte>.Shared.Return(orientationBuffer);
    }

    public void RecievePositionFromServer(ClientState newState, uint tick)
    {
        ReceivedPosition.Enqueue(new Tuple<Vector3, uint>(newState.position, tick));
    }
}
