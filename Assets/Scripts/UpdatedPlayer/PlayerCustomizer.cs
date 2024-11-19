using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizer : MonoBehaviour
{
    // References to the player's SpriteRenderer and Animator
    public SpriteRenderer spriteRenderer; // This should be the SpriteRenderer component on your player
    public Animator animator;            // This should be the Animator component on your player

    /// <summary>
    /// Applies the given CharacterConfig to customize the player's appearance and animations.
    /// </summary>
    /// <param name="config">The CharacterConfig to apply.</param>
    public void ApplyCharacterConfig(CharacterConfig config)
    {
        if (config == null)
        {
            Debug.LogWarning("CharacterConfig is null! Make sure to assign a valid configuration.");
            return;
        }

        // Set the character sprite (optional, for preview or static visuals)
        if (spriteRenderer != null && config.characterSprite != null)
        {
            spriteRenderer.sprite = config.characterSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer or Character Sprite is missing. Check your setup.");
        }

        // Set the Animator Controller for the character
        if (animator != null && config.animatorController != null)
        {
            animator.runtimeAnimatorController = config.animatorController;
        }
        else
        {
            Debug.LogWarning("Animator or Animator Controller is missing. Check your setup.");
        }
    }
}


