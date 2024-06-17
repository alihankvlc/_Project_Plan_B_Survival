using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToolBeltSlotNavigation : MonoBehaviour
{
    [SerializeField] private GameObject _slotSelectionPrefab;
    public void Initialize(GameObject selectionPrefab) => _slotSelectionPrefab = selectionPrefab;

    public void Select()
    {
        ApplySelectionEffect(true);
    }

    public void Deselect()
    {
        ApplySelectionEffect(false);
    }

    private void ApplySelectionEffect(bool isSelected)
    {
        _slotSelectionPrefab.SetActive(isSelected);
        transform.parent.DOScale(isSelected ? Vector3.one * 1.05f : Vector3.one, 0.25f);
    }
}
