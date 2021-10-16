using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    private PlayerNetworkManager _networkManager;

    private void Awake()
    {
        if (TryGetComponent(out PlayerNetworkManager manager)) _networkManager = manager;
    }

    public void ReceiveMovement(ClientState newState, uint tick)
    {
        if (_networkManager)
        {
            _networkManager.ReceivePositionFromServer(newState, tick);
            return;
        }

        var transform1 = transform;
        transform1.localRotation = newState.Rotation;
        transform1.localPosition = newState.Position;
    }
}