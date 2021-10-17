using System.Net;
using UnityEngine;
using UnityEngine.UI;

internal class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject startMenu;
    public InputField usernameField;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.gameObject.SetActive(false);
        usernameField.interactable = false;
        Client.Instance.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7787));
    }
}