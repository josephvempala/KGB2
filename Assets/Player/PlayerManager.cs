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

        transform.localRotation = newState.Rotation;
        transform.localPosition = newState.Position;
    }
}