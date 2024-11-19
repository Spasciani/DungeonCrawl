using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Game/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    public string characterName;
    public RuntimeAnimatorController animatorController;
    public Sprite characterSprite;
}

