using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    private PlayerNetworkManager networkManager;

    private void Awake()
    {
        if(TryGetComponent(out PlayerNetworkManager manager))
        {
            networkManager = manager;
        }
    }

    public void RecieveMovement(ClientState newState, uint tick)
    {
        if (networkManager)
        {
            networkManager.RecievePositionFromServer(newState, tick);
            return;
        }
        transform.localRotation = newState.rotation;
        transform.localPosition = newState.position;
    }
}
