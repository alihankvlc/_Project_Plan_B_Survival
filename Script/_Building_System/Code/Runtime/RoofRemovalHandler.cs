using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public sealed class RoofRemovalHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _ignoreRoofObjects;

    private bool _isEnable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetActiveRoof(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetActiveRoof(true);
    }

    private void SetActiveRoof(bool param)
    {
        _ignoreRoofObjects.ForEach(r => r.gameObject.SetActive(param));
        _isEnable = param;
    }
}