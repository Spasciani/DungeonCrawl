using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Reference to BasePlayer prefab
    public Transform[] spawnPoints; // Array of predefined spawn points in the scene

    private Queue<Transform> availableSpawnPoints = new Queue<Transform>(); // Queue for cycling through spawn points

    private void Start()
    {
        // Initialize spawn points queue
        foreach (var spawnPoint in spawnPoints)
        {
            availableSpawnPoints.Enqueue(spawnPoint);
        }

        // Subscribe to the event when a client connects
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            // Handle host spawning its own character
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Only the server should spawn player objects
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        Debug.Log($"Attempting to spawn player for client {clientId}");

        Vector3 spawnPosition = GetSpawnPosition();
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // Spawn the player object for the client
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId);

        Debug.Log($"Spawned player {clientId} at position {spawnPosition}");
    }

    private Vector3 GetSpawnPosition()
    {
        if (availableSpawnPoints.Count > 0)
        {
            // Use the next spawn point in the queue
            Transform spawnPoint = availableSpawnPoints.Dequeue();
            availableSpawnPoints.Enqueue(spawnPoint); // Re-add to the end of the queue
            return spawnPoint.position;
        }

        // Fallback: Random position if no spawn points are defined
        Debug.LogWarning("No spawn points available. Using random spawn position.");
        return new Vector3(Random.Range(-5, 5), 0, 0);
    }
}
