using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct SelectionButton
{
    public Image colorButton;
    public GameObject selectionBox;
    public Material material;
}
public class ColorSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private SelectionButton[] selectionButtons;
    [SerializeField] private int materialIndex = 0;

    public const string PlayerMaterialKey = "PlayerMaterialIndex";

    private void Start()
    {
        materialIndex = PlayerPrefs.GetInt(PlayerMaterialKey, 0);
        HandleColorChanged();
    }

    private void HandleColorChanged()
    {
        foreach (var selection in selectionButtons)
        {
            selection.selectionBox.SetActive(false);
        }

        foreach (var sprite in playerSprites)
        {
            sprite.material = selectionButtons[materialIndex].material;
        }
        selectionButtons[materialIndex].selectionBox.SetActive(true);
    }

    public void SelectColor(int materialIndex)
    {
        SoundManager.instancne.PlaySoundEffect(7);
        this.materialIndex = materialIndex;
        HandleColorChanged();
    }

    public void SaveColor()
    {
        PlayerPrefs.SetInt(PlayerMaterialKey,materialIndex);
    }

    private void OnValidate()
    {
        foreach (var selection in selectionButtons)
        {
            selection.colorButton.material = selection.material;
        }
    }
}
