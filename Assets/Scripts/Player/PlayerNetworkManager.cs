using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerNetworkManager : MonoBehaviour
{
    private static readonly uint buffer_size = 256;

    [Header("References")] [SerializeField]
    private Movement movement;

    [SerializeField] private MouseLook mouseLook;

    [Header("network")] public uint currentTick;

    private readonly ClientState[] _clientStates = new ClientState[buffer_size];
    private CharacterController _controller;
    private readonly Controls[] _inputs = new Controls[buffer_size];
    private readonly Queue<Tuple<Vector3, uint>> _receivedPosition = new Queue<Tuple<Vector3, uint>>(10);

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        while (_receivedPosition.Count > 0)
        {
            var latestReceivedPosition = _receivedPosition.Dequeue();
            var bufferSlot = latestReceivedPosition.Item2 % buffer_size;
            var positionError = latestReceivedPosition.Item1 - _clientStates[bufferSlot].Position;

            if (!(positionError.sqrMagnitude > 0.0000001f)) continue;
            _controller.enabled = false;
            _controller.transform.localPosition = latestReceivedPosition.Item1;
            _controller.enabled = true;
            var rewindTick = latestReceivedPosition.Item2;
            while (rewindTick < currentTick)
            {
                bufferSlot = rewindTick % buffer_size;
                _inputs[bufferSlot] = movement.CurrentInputs;
                var transform1 = transform;
                _clientStates[bufferSlot].Position = transform1.localPosition;
                _clientStates[bufferSlot].Rotation = transform1.localRotation;

                var movementDirection = movement.CalculateGroundMovement(_inputs[bufferSlot]);
                _controller.Move(movementDirection * Time.deltaTime);
                rewindTick++;
            }
        }
    }

    public void FixedUpdate()
    {
        SendControls();
        SendOrientation();
        currentTick++;
    }

    private void SendControls()
    {
        var controlBuffer = ArrayPool<byte>.Shared.Rent(15);
        movement.CurrentInputs.Tick = currentTick;
        var bufferToSend = movement.CurrentInputs.Serialize(controlBuffer);
        StoreStateInBuffers();
        ClientSend.SendControls(bufferToSend);
        movement.CurrentInputs.Reset();
        ArrayPool<byte>.Shared.Return(controlBuffer);
    }

    private void StoreStateInBuffers()
    {
        var bufferSlot = currentTick % buffer_size;
        _inputs[bufferSlot] = movement.CurrentInputs;
        var transform1 = transform;
        _clientStates[bufferSlot].Position = transform1.localPosition;
        _clientStates[bufferSlot].Rotation = transform1.localRotation;
    }

    private void SendOrientation()
    {
        var orientationBuffer = ArrayPool<byte>.Shared.Rent(8);
        mouseLook.OrientationToSend.Tick = currentTick;
        var writtenToBuffer = mouseLook.OrientationToSend.Serialize(orientationBuffer);
        ClientSend.SendOrientation(writtenToBuffer);
        mouseLook.OrientationToSend.Reset();
        ArrayPool<byte>.Shared.Return(orientationBuffer);
    }

    public void ReceivePositionFromServer(ClientState newState, uint tick)
    {
        _receivedPosition.Enqueue(new Tuple<Vector3, uint>(newState.Position, tick));
    }
}