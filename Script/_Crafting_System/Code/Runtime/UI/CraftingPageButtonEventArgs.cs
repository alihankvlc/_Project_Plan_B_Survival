using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Crafting_System_.Runtime.UI
{
    public class CraftingPageButtonEventArgs : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public Button PageButton => _button;

        public void SetInteraction(bool value) => _button.interactable = value;
    }
}
