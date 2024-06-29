using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct MainMenuEventArgs
{
    public Button Button;
    public UnityEvent Action;
}

public sealed class GameMainMenuButtonEventArgs : MonoBehaviour
{
    [SerializeField] private List<MainMenuEventArgs> _menuEvents = new();

    private void Start()
    {
        _menuEvents.ForEach(r => r.Button.onClick.AddListener(() => OnButtonClicked(r.Action)));
    }

    private void OnButtonClicked(UnityEvent @event)
    {
        @event?.Invoke();
    }

    private void OnDestroy()
    {
        _menuEvents.ForEach(r => r.Button.onClick.RemoveListener(() => OnButtonClicked(r.Action)));
    }
}