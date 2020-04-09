using UnityEngine.Assertions;
using UnityEngine;
using LightJson;

namespace Survey
{
    public class Survey : MonoBehaviour
    {
        private ScaleQuestion scaleQuestion = null;
        private Description description = null;
        private int index;

        private void Awake()
        {
            scaleQuestion = GetComponentInChildren<ScaleQuestion>();
            description = GetComponentInChildren<Description>();

            Assert.IsNotNull(scaleQuestion);
            Assert.IsNotNull(description);
        }

        public void RunSurvey(string surveyName)
        {
            TextAsset surveyJsonText = Resources.Load<TextAsset>($"Survey/{surveyName}");
            JsonArray survey = JsonValue.Parse(surveyJsonText.text).AsJsonArray;
            index = 0;
        }
    }
}