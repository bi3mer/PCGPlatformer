using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Survey
{ 
    public class Description : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title = null;

        [SerializeField]
        private TextMeshProUGUI description = null;

        [SerializeField]
        private Button prevButton = null;

        [SerializeField]
        private Button nextButton = null;

        public Action NextCallback = null;
        public Action PrevCallback = null;

        private void Awake()
        {
            Assert.IsNotNull(title);
            Assert.IsNotNull(description);
            Assert.IsNotNull(prevButton);
            Assert.IsNotNull(nextButton);
        }

        private void Start()
        {
            nextButton.onClick.AddListener(NextClicked);
            prevButton.onClick.AddListener(PrevClicked);
        }

        private void NextClicked()
        {
            NextCallback?.Invoke();
        }

        private void PrevClicked()
        {
            PrevCallback?.Invoke();
        }

        public void Show(bool hasPrevious)
        {
            gameObject.SetActive(true);

            if (hasPrevious == false)
            {
                prevButton.gameObject.SetActive(false);
            }
        }

    }
}
