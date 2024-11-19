using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    public CharacterConfig[] characterConfigs; // Assign in Inspector
    public Image characterPreviewImage; // Reference to the UI image for preview
    public Text characterNameText; // Reference to the UI text for the name

    private void Start()
    {
        if (CharacterSelectionManager.Instance == null)
        {
            Debug.LogError("CharacterSelectionManager is not initialized!");
            return;
        }

        UpdateCharacterPreview();
    }

    public void OnNextCharacter()
    {
        if (characterConfigs == null || characterConfigs.Length == 0)
        {
            Debug.LogError("CharacterConfigs is null or empty!");
            return;
        }

        CharacterSelectionManager.Instance.NextCharacter(characterConfigs.Length);
        UpdateCharacterPreview();
    }

    public void OnPreviousCharacter()
    {
        if (characterConfigs == null || characterConfigs.Length == 0)
        {
            Debug.LogError("CharacterConfigs is null or empty!");
            return;
        }

        CharacterSelectionManager.Instance.PreviousCharacter(characterConfigs.Length);
        UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
        if (characterConfigs == null || characterConfigs.Length == 0)
        {
            Debug.LogError("CharacterConfigs is null or empty!");
            return;
        }

        if (characterPreviewImage == null)
        {
            Debug.LogError("CharacterPreviewImage is not assigned!");
            return;
        }

        if (characterNameText == null)
        {
            Debug.LogError("CharacterNameText is not assigned!");
            return;
        }

        int index = CharacterSelectionManager.Instance.SelectedCharacterIndex;
        CharacterConfig currentConfig = characterConfigs[index];
        characterPreviewImage.sprite = currentConfig.characterSprite;
        characterNameText.text = currentConfig.characterName;
    }
}
