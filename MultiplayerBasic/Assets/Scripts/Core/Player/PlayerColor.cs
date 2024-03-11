using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private KabigonPlayer player;
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private Material[] tankMaterial;
    [SerializeField] private int materialIndex;

    private void Start()
    {
        HandlePlayerColorChanged(0, player.PlayerColorIndex.Value);
    }

    private void HandlePlayerColorChanged(int oldIndex, int newIndex)
    {
        materialIndex = newIndex;
        foreach (var sprite in playerSprites)
        {
            sprite.material = tankMaterial[materialIndex];
        }
    }

    private void OnDestroy()
    {
        player.PlayerColorIndex.OnValueChanged -= HandlePlayerColorChanged;
    }
}
