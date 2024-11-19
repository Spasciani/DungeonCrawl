using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    public void StartHost()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager exists in the scene.");
            return;
        }

        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
       // SceneManager.LoadScene("Test");
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager exists in the scene.");
            return;
        }

        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
        //SceneManager.LoadScene("Test");
    }
}
