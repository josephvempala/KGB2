using UnityEngine;
using UnityEngine.UI;

class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public InputField usernameField;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.gameObject.SetActive(false);
        usernameField.interactable = false;
        Client.instance.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 7787));
    }
}
