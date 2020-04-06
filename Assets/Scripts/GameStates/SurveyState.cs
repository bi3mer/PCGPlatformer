using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using LightJson;
using Survey;

public class SurveyState : BaseState
{
    protected override string DefaultName => "Survey State";
    private List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();
    private List<GameObject> texts = new List<GameObject>();

    public SurveyState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        Debug.LogWarning("not using blackboard for building questions");
        Debug.LogWarning("need to add checker to make sure all questions are answered.");
        Debug.LogWarning("need to add button to move to the next state that calls checker first.");
        JsonObject surveyInfo = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;
        string surveyFile = surveyInfo[FlowKeys.SurveyName].AsString;

        TextAsset surveyJsonText = Resources.Load<TextAsset>($"Survey/{surveyFile}");
        JsonArray survey = JsonValue.Parse(surveyJsonText.text).AsJsonArray;

        foreach (JsonObject question in survey)
        {
            string questionType = question[SurveyKeys.Type].AsString;
            if (questionType.Equals(SurveyTypes.Text))
            {
                GameObject go = new GameObject();
                go = GameObject.Instantiate(go);
                TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
                text.text = question[SurveyKeys.Title].AsString;
                go.transform.parent = blackBoard.SurveyContentSection.transform;
            }
            else if (questionType.Equals(SurveyTypes.MultipleChoice))
            {
                MultipleChoiceQuestion q = new MultipleChoiceQuestion(
                    blackBoard.SurveyContentSection,
                    question[SurveyKeys.Question].AsString,
                    question[SurveyKeys.Answers].AsJsonArray);
            }
            else
            {
                Debug.LogError($"Unhandled survey type: {questionType}");
            }
        }

        blackBoard.Survey.SetActive(true);
    }

    protected override void OnStateExit()
    {
        foreach (MultipleChoiceQuestion question in questions)
        {
            question.DestroySelf();
        }

        foreach (GameObject go in texts)
        {
            GameObject.Destroy(go);
        }

        questions.Clear();
        texts.Clear();
    }
}
