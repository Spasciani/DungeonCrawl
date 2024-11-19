using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCustomizer : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the in-game SpriteRenderer
    public Animator animator; // Reference to the Animator component
    public CharacterConfig[] characterConfigs; // Array of character configurations assigned in the Inspector

    // Network variable to synchronize character selection across clients
    private NetworkVariable<int> characterIndex = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // Retrieve the selected character index from the CharacterSelectionManager
            int selectedIndex = CharacterSelectionManager.Instance.SelectedCharacterIndex;

            // Inform the server about the selected character
            SetCharacterServerRpc(selectedIndex);
        }

        // Ensure the correct character is applied when the index changes
        characterIndex.OnValueChanged += OnCharacterIndexChanged;

        // Apply the current character configuration
        ApplyCharacterConfig(characterConfigs[characterIndex.Value]);
    }

    private void OnCharacterIndexChanged(int oldValue, int newValue)
    {
        ApplyCharacterConfig(characterConfigs[newValue]);
    }

    [ServerRpc]
    private void SetCharacterServerRpc(int index)
    {
        if (index >= 0 && index < characterConfigs.Length)
        {
            // Update the character index on all clients
            characterIndex.Value = index;
        }
        else
        {
            Debug.LogWarning("Invalid character index received!");
        }
    }

    private void ApplyCharacterConfig(CharacterConfig config)
    {
        if (config == null) return;

        // Update the sprite and animator controller
        spriteRenderer.sprite = config.characterSprite;
        animator.runtimeAnimatorController = config.animatorController;
    }
}
