using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    // public void StartHost()
    // {
    //     if (NetworkManager.Singleton == null)
    //     {
    //         Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager exists in the scene.");
    //         return;
    //     }

    //     Debug.Log("Starting Host...");
    //     NetworkManager.Singleton.StartHost();
    //    // SceneManager.LoadScene("Test");
    // }

    // public void StartClient()
    // {
    //     if (NetworkManager.Singleton == null)
    //     {
    //         Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager exists in the scene.");
    //         return;
    //     }

    //     Debug.Log("Starting Client...");
    //     NetworkManager.Singleton.StartClient();
    //     //SceneManager.LoadScene("Test");
    // }

    //Test Part - Believe this is right
//     private RelayManager relayManager;

//     private void Start()
//     {
//         relayManager = FindObjectOfType<RelayManager>();
//     }

//     public async void HostGame()
//     {
//         string joinCode = await relayManager.CreateRelay(10); // Adjust max players as needed
//         Debug.Log($"Game hosted successfully. Join Code: {joinCode}");
//     }

//     public async void JoinGame(string joinCode)
//     {
//         await relayManager.JoinRelay(joinCode);
//         Debug.Log("Joined game successfully.");
//     }
// }


    // public RelayManager relayManager;

    // public async void HostGame()
    // {
    //     Debug.Log("Host button pressed.");
    //     await relayManager.CreateRelay(10); // Calls RelayManager for hosting
    // }

    // public async void JoinGame(string joinCode)
    // {
    //     Debug.Log($"Join button pressed with code: {joinCode}");
    //     await relayManager.JoinRelay(joinCode); // Calls RelayManager for joining
    // }


}
