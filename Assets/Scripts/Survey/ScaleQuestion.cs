using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Survey
{
    public class ScaleQuestion : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title = null;

        [SerializeField]
        private TextMeshProUGUI description = null;

        [SerializeField]
        private Toggle notAtAllToggle = null;

        [SerializeField]
        private Toggle slightlyToggle = null;

        [SerializeField]
        private Toggle moderatelyToggle = null;

        [SerializeField]
        private Toggle fairlyToggle = null;

        [SerializeField]
        private Toggle extremelyToggle = null;

        [SerializeField]
        private Button next = null;

        [SerializeField]
        private Button previous = null;

        private Scale activeScale = Scale.None;

        public Action NextCallback = null;
        public Action PrevCallback = null;

        private void Awake()
        {
            Assert.IsNotNull(title);
            Assert.IsNotNull(description);
            Assert.IsNotNull(notAtAllToggle);
            Assert.IsNotNull(slightlyToggle);
            Assert.IsNotNull(moderatelyToggle);
            Assert.IsNotNull(fairlyToggle);
            Assert.IsNotNull(extremelyToggle);
            Assert.IsNotNull(next);
            Assert.IsNotNull(previous);
        }

        private void Start()
        {
            notAtAllToggle.onValueChanged.AddListener((bool change) =>
            {
                ValueChange(Scale.NotAtAll, change);
            });

            slightlyToggle.onValueChanged.AddListener((bool change) =>
            {
                ValueChange(Scale.Slightly, change);
            });

            moderatelyToggle.onValueChanged.AddListener((bool change) =>
            {
                ValueChange(Scale.Moderately, change);
            });

            fairlyToggle.onValueChanged.AddListener((bool change) =>
            {
                ValueChange(Scale.Fairly, change);
            });

            extremelyToggle.onValueChanged.AddListener((bool change) =>
            {
                ValueChange(Scale.Extremely, change);
            });

            next.onClick.AddListener(NextClicked);
            previous.onClick.AddListener(PrevClicked);
        }

        private void ValueChange(Scale scale, bool change)
        {
            if (activeScale == Scale.None)
            {
                activeScale = scale;
            }
            else if (change == true && activeScale != scale)
            {
                UpdateToggle(activeScale, false);
                activeScale = scale;
            }
            else if (change == false && activeScale == scale)
            {
                activeScale = Scale.None;
            }

            ShowButtons(activeScale != Scale.None);
        }

        private void UpdateToggle(Scale scale, bool value)
        {
            switch (scale)
            {
                case Scale.NotAtAll:
                    notAtAllToggle.SetIsOnWithoutNotify(value);
                    break;
                case Scale.Slightly:
                    slightlyToggle.SetIsOnWithoutNotify(value);
                    break;
                case Scale.Moderately:
                    moderatelyToggle.SetIsOnWithoutNotify(value);
                    break;
                case Scale.Fairly:
                    fairlyToggle.SetIsOnWithoutNotify(value);
                    break;
                case Scale.Extremely:
                    extremelyToggle.SetIsOnWithoutNotify(value);
                    break;
                case Scale.None:
                default:
                    break;
            }
        }

        private void ShowButtons(bool show)
        {
            previous.gameObject.SetActive(show);
            next.gameObject.SetActive(show);
        }

        public void Show(bool hasPrevious)
        {
            gameObject.SetActive(true);

            if (hasPrevious == false)
            {
                previous.gameObject.SetActive(false);
            }
        }

        public int GetUserResponse()
        {
            return activeScale.ToInt();
        }

        private void NextClicked()
        {
            NextCallback?.Invoke();
        }

        private void PrevClicked()
        {
            PrevCallback?.Invoke();
        }
    }
}