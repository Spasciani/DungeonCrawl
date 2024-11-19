using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void TransitionToTestScene()
    {
        // Despawn all players before transitioning
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                if (client.PlayerObject != null)
                {
                    client.PlayerObject.Despawn(true); // Despawn the player
                }
            }
        }

        // Load the Test Scene
        SceneManager.LoadScene("Test");
    }
}
