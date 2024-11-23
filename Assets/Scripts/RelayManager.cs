// using System;
// using UnityEngine;
// using Unity.Services.Relay;
// using Unity.Services.Relay.Models;
// using Unity.Services.Core;
// using Unity.Services.Authentication;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using System.Threading.Tasks;
// using System.Linq;
// using UnityEngine.UI;

// using TMPro;
// using Unity.Networking.Transport.Relay;

using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class RelayManager : MonoBehaviour
{

    //New Attempt
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TextMeshProUGUI codeText;

    async void Start(){
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);
        joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));
    }

    async void CreateRelay(){
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        codeText.text = "Code: " + joinCode;
        Debug.Log($"Relay Join Code: {joinCode}");

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        
        NetworkManager.Singleton.StartHost();
    }

    async void JoinRelay(string joinCode){
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
    }



    //OLD CODE

    // private static bool isAuthenticating = false;

    // private async Task Authenticate()
    // {
    //     if (UnityServices.State == ServicesInitializationState.Uninitialized)
    //     {
    //         // Initialize Unity Services
    //         await UnityServices.InitializeAsync();
    //     }

    //     if (!AuthenticationService.Instance.IsSignedIn && !isAuthenticating)
    //     {
    //         try
    //         {
    //             isAuthenticating = true;
    //             Debug.Log("Starting authentication...");
    //             await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //             Debug.Log($"Player successfully authenticated. Player ID: {AuthenticationService.Instance.PlayerId}");
    //         }
    //         catch (AuthenticationException e)
    //         {
    //             Debug.LogError($"Authentication failed: {e}");
    //         }
    //         catch (RequestFailedException e)
    //         {
    //             Debug.LogError($"Request failed during authentication: {e}");
    //         }
    //         finally
    //         {
    //             isAuthenticating = false;
    //         }
    //     }
    //     else if (AuthenticationService.Instance.IsSignedIn)
    //     {
    //         Debug.Log($"Player is already authenticated. Player ID: {AuthenticationService.Instance.PlayerId}");
    //     }
    // }


    // private bool isRelayHosting = false; // Flag to prevent duplicate Relay hosting attempts

    //SuperOld Code
    // // public async Task<string> CreateRelay(int maxPlayers)
    // // {
    // //     if (isRelayHosting)
    // //     {
    // //         Debug.LogWarning("Relay hosting is already in progress.");
    // //         return null;
    // //     }

    // //     isRelayHosting = true;

    // //     await Authenticate();

    // //     try
    // //     {
    // //         // Create a Relay allocation
    // //         Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

    // //         // Validate and trim connection data
    // //         byte[] trimmedConnectionData = allocation.ConnectionData.Length > 16
    // //             ? allocation.ConnectionData[..16]
    // //             : allocation.ConnectionData;

    // //         Debug.Log($"AllocationIdBytes Length: {allocation.AllocationIdBytes.Length}");
    // //         Debug.Log($"ConnectionData Length (Trimmed): {trimmedConnectionData.Length}");
    // //         Debug.Log($"Key Length: {allocation.Key.Length}");

    // //         if (allocation.AllocationIdBytes.Length != 16 ||
    // //             trimmedConnectionData.Length != 16 ||
    // //             allocation.Key.Length != 64)
    // //         {
    // //             Debug.LogError("Relay allocation byte arrays have incorrect lengths!");
    // //             isRelayHosting = false;
    // //             return null;
    // //         }

    // //         // Get the join code for clients
    // //         string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    // //         if (string.IsNullOrEmpty(joinCode))
    // //         {
    // //             Debug.LogError("Join code is blank or null!");
    // //             isRelayHosting = false;
    // //             return null;
    // //         }
    // //         Debug.Log($"Relay Join Code: {joinCode}");

    // //         // Set Relay server data on Unity Transport
    // //         var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    // //         transport.SetRelayServerData(
    // //             allocation.RelayServer.IpV4,
    // //             (ushort)allocation.RelayServer.Port,
    // //             allocation.AllocationIdBytes,
    // //             trimmedConnectionData,
    // //             trimmedConnectionData, // Host connection data
    // //             allocation.Key
    // //         );

    // //         // Start the host
    // //         NetworkManager.Singleton.StartHost();

    // //         isRelayHosting = false; // Reset the flag after successful hosting
    // //         return joinCode;
    // //     }
    // //     catch (RelayServiceException e)
    // //     {
    // //         Debug.LogError($"Relay Create Exception: {e}");
    // //         isRelayHosting = false; // Reset the flag on failure
    // //         return null;
    // //     }
    // // }

    // //TEST::

    // // public void SetRelayServerData(
    // //     string ipv4Address,
    // //     ushort port,
    // //     byte[] allocationIdBytes,
    // //     byte[] key,
    // //     byte[] connectionData,
    // //     byte[] hostConnectionData,
    // //     bool isSecure = false
    // // )

    //End of Super Old code, middlge ground recent code:
    // public async Task<string> CreateRelay(int maxPlayers)
    // {
    //     if (isRelayHosting)
    //     {
    //         Debug.LogWarning("Relay hosting is already in progress.");
    //         return null;
    //     }

    //     isRelayHosting = true;

    //     await Authenticate();

    //     try
    //     {
    //         // Create a Relay allocation
    //         Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            
    //         // Get the join code
    //         string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //         Debug.Log($"Relay Join Code: {joinCode}");

    //         // Trim ConnectionData to 16 bytes manually
    //         byte[] trimmedConnectionData = new byte[16];
    //         Array.Copy(allocation.ConnectionData, 0, trimmedConnectionData, 0, 16);

    //         Debug.Log($"AllocationIdBytes Length: {allocation.AllocationIdBytes.Length}");
    //         Debug.Log($"ConnectionData Length (Trimmed): {trimmedConnectionData.Length}");
    //         Debug.Log($"Key Length: {allocation.Key.Length}");

    //         if (allocation.AllocationIdBytes.Length != 16 ||
    //             trimmedConnectionData.Length != 16 ||
    //             allocation.Key.Length != 64)
    //         {
    //             Debug.LogError("Relay allocation byte arrays have incorrect lengths!");
    //             Debug.Log($"AllocationIdBytes: {BitConverter.ToString(allocation.AllocationIdBytes)}");
    //             Debug.Log($"ConnectionData (Trimmed): {BitConverter.ToString(trimmedConnectionData)}");
    //             Debug.Log($"Key: {BitConverter.ToString(allocation.Key)}");
    //             isRelayHosting = false;
    //             return null;
    //         }

    //         // Set Relay server data on Unity Transport with correct parameter order
    //         var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    //         transport.SetRelayServerData(
    //             allocation.RelayServer.IpV4,
    //             (ushort)allocation.RelayServer.Port,
    //             allocation.AllocationIdBytes,
    //             allocation.Key,               // Corrected: Key is now the fourth parameter
    //             trimmedConnectionData,
    //             trimmedConnectionData         // Host connection data
    //         );

    //         // Start the host
    //         NetworkManager.Singleton.StartHost();

    //         isRelayHosting = false;
    //         return joinCode;
    //     }
    //     catch (RelayServiceException e)
    //     {
    //         Debug.LogError($"Relay Create Exception: {e}");
    //         isRelayHosting = false;
    //         return null;
    //     }
    // }



    // public async Task JoinRelay(string joinCode)
    // {
    //     await Authenticate();

    //     try
    //     {
    //         // Join a Relay allocation using the join code
    //         JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

    //         // Validate byte array lengths
    //         if (allocation.AllocationIdBytes.Length != 16 ||
    //             allocation.ConnectionData.Length != 16 ||
    //             allocation.HostConnectionData.Length != 16 ||
    //             allocation.Key.Length != 64)
    //         {
    //             Debug.LogError("Relay join allocation byte arrays have incorrect lengths!");
    //             return;
    //         }

    //         // Set Relay server data on Unity Transport with correct parameter order
    //         var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    //         transport.SetRelayServerData(
    //             allocation.RelayServer.IpV4,
    //             (ushort)allocation.RelayServer.Port,
    //             allocation.AllocationIdBytes,
    //             allocation.Key,                 // Key is the fourth parameter
    //             allocation.ConnectionData,
    //             allocation.HostConnectionData
    //         );

    //         // Start the client
    //         NetworkManager.Singleton.StartClient();
    //     }
    //     catch (RelayServiceException e)
    //     {
    //         Debug.LogError($"Relay Join Exception: {e}");
    //     }
    // }

}
