using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Question")]
    QuestionSO CurrentQuestion;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    [Header("Button Colors")]
    [SerializeField]Sprite defaultAnswerSprite;
    [SerializeField]Sprite CorrectAnswerSprite;
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;
    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    Scorekeeper scorekeeper;
    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    public bool isComplete;
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scorekeeper = FindObjectOfType<Scorekeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Start()
    {
        progressBar.value = 0;
        scoreText.text = "Score: " + 0 + "%";
    }

    void Update() 
    {
        timerImage.fillAmount = timer.fillFraction;    
        if(timer.loadNextQuestion)
        {
            
            if(progressBar.value==progressBar.maxValue)
            {
                isComplete=true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly&&!timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonstate(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonstate(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scorekeeper.CalculateScore() + "%";

    }

    void DisplayAnswer(int index)
    {
            Image buttonImage;

        if(index == CurrentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = CorrectAnswerSprite;
            scorekeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = CurrentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = CurrentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Wrong! The correct answer is \n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = CorrectAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if(questions.Count > 0)
        {
        SetButtonstate(true);
        SetDefaultButtonSprite();
        GetRandomQuestion();
        DisplayQuestion();
        progressBar.value++;
        scorekeeper.IncrementQuestionSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        CurrentQuestion = questions[index];
        if(questions.Contains(CurrentQuestion))
        {
        questions.Remove(CurrentQuestion);
        }
    }

    void SetDefaultButtonSprite()
    {
        for(int i=0; i<answerButtons.Length;i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void DisplayQuestion(){
        questionText.text = CurrentQuestion.GetQuestion();

        for(int i=0;i<answerButtons.Length;i++)
        {
        TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = CurrentQuestion.GetAnswer(i);
        }
    }

    void SetButtonstate(bool state)
    {
        for(int i=0;i<answerButtons.Length;i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
}
