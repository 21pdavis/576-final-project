using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AnsweringStudyQuestionButtons : MonoBehaviour
{
    Question question = null;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text[] answers = new TMP_Text[4];
    [SerializeField] Button[] answersButtons = new Button[4];
    [SerializeField] Slider slider;
    StudyGame game = null;

    // Start is called before the first frame update
    void Start()
    {
        game = FindAnyObjectByType<StudyGame>();
        foreach(Transform transform in GetComponentsInChildren<Transform>()) {
            Debug.Log(transform.transform.name);
            if (transform.name.Contains("1")) {
                answers[0] = transform.GetComponentInChildren<TMP_Text>();
                answersButtons[0] = transform.GetComponent<Button>();
                answersButtons[0].onClick.AddListener(() => answer(0));
            } else if (transform.name.Contains("2")) {
                answers[1] = transform.GetComponentInChildren<TMP_Text>();
                answersButtons[1] = transform.GetComponent<Button>();
                answersButtons[1].onClick.AddListener(() => answer(1));
            } else if (transform.name.Contains("3")) {
                answers[2] = transform.GetComponentInChildren<TMP_Text>();
                answersButtons[2] = transform.GetComponent<Button>();
                answersButtons[2].onClick.AddListener(() => answer(2));
            } else if (transform.name.Contains("4")) {
                answers[3] = transform.GetComponentInChildren<TMP_Text>();
                answersButtons[3] = transform.GetComponent<Button>();
                answersButtons[3].onClick.AddListener(() => answer(3));
            } else if (transform.name.Contains("Question")) {
                questionText = transform.GetComponent<TMP_Text>();
            } else if (transform.name.Contains("ProgressBar")) {
                slider = transform.GetComponent<Slider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(game != null) {
            slider.value = game.progress;
            question = game.currQuestion;
        }
        //displays questions and options
        if(question != null) {
            questionText.text = "What is \n" + question.questionText;
            answers[0].text = question.index1Ans;
            answers[1].text = question.index2Ans;
            answers[2].text = question.index3Ans;
            answers[3].text = question.index4Ans;
        } else {
            Debug.Log("what");
            answers[0].text = "--";
            answers[1].text = "--";
            answers[2].text = "--";
            answers[3].text = "--";
        }
    }
    void answer(int index) {
        if (game != null && question != null) {
            bool result = game.answer(index + 1);
            if (result) {
                questionText.text = "Correct!!!";
            } else {
                questionText.text = "Incorrect!!!";
            }
        }
    }
}
