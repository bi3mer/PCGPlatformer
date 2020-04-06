using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using LightJson;

namespace Survey
{
    public class MultipleChoiceQuestion
    {
        private List<GameObject> answerObjects = null;
        private GameObject questionObject = null;

        public MultipleChoiceQuestion(GameObject content, string question, JsonArray answers)
        {
            questionObject = InstantiateEmpty();
            TextMeshProUGUI questionText = questionObject.AddComponent<TextMeshProUGUI>();
            questionText.text = question;

            questionObject.transform.SetParent(content.transform);

            foreach (string answer in answers)
            {
                
            }
        }

        public void DestroySelf()
        {
            GameObject.Destroy(questionObject);
        }

        private GameObject InstantiateEmpty()
        {
            GameObject go = new GameObject();
            return GameObject.Instantiate(go);
        }

    }
}