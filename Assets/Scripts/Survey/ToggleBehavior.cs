using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Survey
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleBehavior : MonoBehaviour
    {
        private Toggle toggle = null;
        private TextMeshProUGUI label = null;

        public Action OnToggleSelected = null;
        public Action OnToggleDeselected = null;

        public string Label
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = value;
            }
        }

        private void Awake()
        {
            label = GetComponentInChildren<TextMeshProUGUI>();

            Assert.IsNotNull(toggle);
            Assert.IsNotNull(label);
        }

        private void Start()
        {
            toggle.onValueChanged.AddListener(HandleToggleValueChange);
        }

        private void HandleToggleValueChange(bool selected)
        {
            if (selected)
            {
                OnToggleSelected?.Invoke();
            }
            else
            {
                OnToggleDeselected?.Invoke();
            }
        }
    }
}