using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    public int SelectedCharacterIndex { get; private set; } = 0; // Default to the first character

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Go to the next character in the array.
    /// </summary>
    public void NextCharacter(int totalCharacters)
    {
        SelectedCharacterIndex = (SelectedCharacterIndex + 1) % totalCharacters; // Wrap around
    }

    /// <summary>
    /// Go to the previous character in the array.
    /// </summary>
    public void PreviousCharacter(int totalCharacters)
    {
        SelectedCharacterIndex = (SelectedCharacterIndex - 1 + totalCharacters) % totalCharacters; // Wrap around
    }
}