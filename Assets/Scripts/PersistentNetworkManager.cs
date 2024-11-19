using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PersistentNetworkManager : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<NetworkManager>().Length > 1)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
}

