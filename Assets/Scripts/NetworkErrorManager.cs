using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkErrorManager : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
        }
    }

    private void OnTransportFailure()
    {
        Debug.LogError("Transport failure detected. Restarting NetworkManager.");

        // Optionally, display a message to the user or attempt to recreate the allocation.

        // Shutdown the NetworkManager
        NetworkManager.Singleton.Shutdown();
    }
}

