using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class LootSlotSelectionDisplay : MonoBehaviour
{
    [SerializeField] private Image _selectionBackground;
    [SerializeField] private GameObject _selectionMarkIcon;
    
    public void Select(bool isSelected)
    {
        _selectionBackground.gameObject.SetActive(isSelected);
        _selectionMarkIcon.SetActive(isSelected);
    }
}