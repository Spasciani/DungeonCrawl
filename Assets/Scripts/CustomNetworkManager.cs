// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Netcode;

// public class CustomNetworkManager : NetworkManager
// {
//     public override void OnServerAddPlayer(NetworkConnectionToClient client)
//     {
//         // Prevent player spawning in Character Selection Scene
//         if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CharacterSelectionScene")
//         {
//             Debug.Log("Skipping player spawn in Character Selection Scene.");
//             return;
//         }

//         // Call the base method to spawn the player in other scenes
//         base.OnServerAddPlayer(client);
//     }
// }
