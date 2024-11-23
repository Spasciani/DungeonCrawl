using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ConnectionUI : MonoBehaviour
{
    public InputField joinCodeInputField; // Input for join code
    public Button createHostButton; // Button to create a Relay-hosted game
    public Button joinGameButton; // Button to join a Relay-hosted game
    private RelayManager relayManager;

//     private void Start()
//     {
//         relayManager = FindObjectOfType<RelayManager>();

//         createHostButton.onClick.AddListener(async () => await CreateRelayHost());
//         joinGameButton.onClick.AddListener(async () => await JoinRelayGame());
//     }

//     private async Task CreateRelayHost()
//     {
//         string joinCode = await relayManager.CreateRelay(10); // Adjust max players as needed
//         if (!string.IsNullOrEmpty(joinCode))
//         {
//             Debug.Log($"Game hosted successfully. Join Code: {joinCode}");
//             // Display the join code to the user (e.g., update UI)
//         }
//     }

//     private async Task JoinRelayGame()
//     {
//         string joinCode = joinCodeInputField.text;
//         if (!string.IsNullOrEmpty(joinCode))
//         {
//             await relayManager.JoinRelay(joinCode);
//         }
//         else
//         {
//             Debug.LogError("Join Code is empty!");
//         }
//     }
}


