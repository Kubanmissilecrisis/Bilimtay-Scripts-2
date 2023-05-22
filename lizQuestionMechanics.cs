using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum lizMathOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}
/*
public class Question
{
    public int FirstNumber;
    public int SecondNumber;
    public int? MissingNumber; // nullable int to represent a missing number
    public int TotalAnswer;
    public Operator Operator;
} */

public class lizQuestionMechanics : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    //  public Text[] answerTexts;
    public TextMeshProUGUI[] answerTexts;
    public int[] firstNumbers;
    public int[] secondNumbers;

    private int currentQuestionIndex;
    private int correctAnswer;
    public int correctChoice;

    [SerializeField] private Button Candy1;
    [SerializeField] private Button Candy2;
    [SerializeField] private Button Candy3;
    [SerializeField] private Button Candy4;

   // [SerializeField] GameObject desk1;
  //  [SerializeField] GameObject desk2;
   // [SerializeField] GameObject desk3;
   // [SerializeField] GameObject desk4;
  //  [SerializeField] Sprite orangeSprite;

   // [SerializeField] AudioSource correctSound;
   // [SerializeField] AudioSource wrongSound;
   // [SerializeField] AudioSource crowdSound;

   // [SerializeField] Animator crowdCheer;
  //  [SerializeField] Animator kozuAnimations;

    private int equalNumber;
    public lizMathOperation lizselectedMathOperation;


    public static bool[] answerStatuses = new bool[8];

   // public GameObject ballImageHolder;

    public List<Sprite> candySprites; // List of candy sprites

    public Image[] candyButtons; // Array of candy buttons image components.

    //Store the last clicked button index here
    private int lastClickedButtonIndex = -1;

    //This will cotain the clicked candy object that was passed to the ResetPosition method. we will use it to reset the daraged candy to its initial pos
    private GameObject draggedCandyObject;
    private  Vector3 draggedCandyPos;

    private void DisableButtons()
    {
        Candy1.interactable = false;
        Candy2.interactable = false;
        Candy3.interactable = false;
        Candy4.interactable = false;
    }
    private void EnableButtons()
    {
        Candy1.interactable = true;
        Candy2.interactable = true;
        Candy3.interactable = true;
        Candy4.interactable = true;
    }

    /*  private void NormalDeskSprite()
      {
          desk1.GetComponent<SpriteRenderer>().sprite = orangeSprite;
          desk2.GetComponent<SpriteRenderer>().sprite = orangeSprite;
          desk3.GetComponent<SpriteRenderer>().sprite = orangeSprite;
          desk4.GetComponent<SpriteRenderer>().sprite = orangeSprite;
      } */

    void Start()
    {
        // Randomly assign a candy sprite to each button on game start
        foreach (Image candyButton in candyButtons)
        {
            candyButton.sprite = candySprites[Random.Range(0, candySprites.Count)];
        }

        currentQuestionIndex = 0;
        GenerateQuestion();
    }
    
    public void ReceiveCandyPos(Vector3 candyInitialPosition, GameObject candyObject)
    {
        //draggedCandyPos == candyInitialPosition;
        draggedCandyPos = candyInitialPosition;

        draggedCandyObject = candyObject;// given Gameobject(clickedbutton) parameter
        //draggedCandyPos = candyObject.transform.position;
    }

    //to delay the Generation of a new question
    public void GenerateQuestion(float delayTime)
    {
        Invoke("GenerateQuestion", delayTime);
        draggedCandyObject.transform.position = draggedCandyPos;
    }

    public void SwapClickedCandySprite()
    {
        // Randomly assign a new candy sprite to the clicked button
        candyButtons[lastClickedButtonIndex].sprite = candySprites[Random.Range(0, candySprites.Count)];
    }
    /*
    public void ResetCandyPosition (GameObject candyObject)
    {
        draggedCandyObject = candyObject;// given Gameobject(clickedbutton) parameter
        draggedCandyPos = candyObject.transform.position;

    }*/

    void GenerateQuestion()
    {
       

        if (!(draggedCandyObject == null))
        {
            // Deactivate clicked candy
            draggedCandyObject.SetActive(true);
            SwapClickedCandySprite();
        }

        EnableButtons();

        switch (lizselectedMathOperation)
        {

            case lizMathOperation.Addition:
                firstNumbers = new int[] { 1, 2, 3, 4, 5 };
                secondNumbers = new int[] { 1, 2, 3, 4, 5 };
                break;
            case lizMathOperation.Subtraction:
                firstNumbers = new int[] { 6, 7, 8, 9, 10, 16, 14, 17 };
                secondNumbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 3 };
                break;
            case lizMathOperation.Multiplication:
                firstNumbers = new int[] { 2, 3, 4, 5, 6 };
                secondNumbers = new int[] { 2, 3, 4, 5, 6 };
                break;
            case lizMathOperation.Division:
                firstNumbers = new int[] { 12, 15, 16, 20, 24 };
                secondNumbers = new int[] { 2, 3, 4, 5, 6 };
                break;
        }



        int firstNumber = firstNumbers[currentQuestionIndex];
        int secondNumber = secondNumbers[currentQuestionIndex];
        int questionFormat = Random.Range(0, 3); // choose a question format randomly

        switch (lizselectedMathOperation)
        {
            case lizMathOperation.Addition:
                switch (questionFormat)
                {
                    case 0:
                        equalNumber = firstNumber + secondNumber;
                        questionText.text = firstNumber + " + " + secondNumber + " = ?";
                        correctAnswer = equalNumber;
                        break;
                    case 1:
                        correctAnswer = secondNumber;
                        questionText.text = firstNumber + " + ? = " + (firstNumber + secondNumber);
                        break;
                    case 2:
                        correctAnswer = firstNumber;
                        questionText.text = "? + " + secondNumber + " = " + (firstNumber + secondNumber);
                        break;
                }
                break;

            case lizMathOperation.Subtraction:
                switch (questionFormat)
                {
                    case 0:
                        equalNumber = firstNumber - secondNumber;
                        questionText.text = firstNumber + " - " + secondNumber + " = ?";
                        correctAnswer = equalNumber;
                        break;
                    case 1:
                        correctAnswer = secondNumber;
                        questionText.text = firstNumber + " - ? = " + (firstNumber - secondNumber);
                        break;
                    case 2:
                        correctAnswer = firstNumber;
                        questionText.text = "? - " + secondNumber + " = " + (firstNumber - secondNumber);
                        break;
                }
                break;

            case lizMathOperation.Multiplication:
                switch (questionFormat)
                {
                    case 0:
                        equalNumber = firstNumber * secondNumber;
                        questionText.text = firstNumber + " × " + secondNumber + " = ?";
                        correctAnswer = equalNumber;
                        break;
                    case 1:
                        correctAnswer = secondNumber;
                        questionText.text = firstNumber + "× ? = " + (firstNumber * secondNumber);
                        break;
                    case 2:
                        correctAnswer = firstNumber;
                        questionText.text = "? × " + secondNumber + " = " + (firstNumber * secondNumber);
                        break;
                }
                break;

            case lizMathOperation.Division:
                switch (questionFormat)
                {
                    case 0:
                        equalNumber = firstNumber / secondNumber;
                        questionText.text = firstNumber + " ÷ " + secondNumber + " = ?";
                        correctAnswer = equalNumber;
                        break;
                    case 1:
                        correctAnswer = secondNumber;
                        questionText.text = firstNumber + "÷ ? = " + (firstNumber / secondNumber);
                        break;
                    case 2:
                        correctAnswer = firstNumber;
                        questionText.text = "? ÷ " + secondNumber + " = " + (firstNumber / secondNumber);
                        break;
                }
                break;
        }


        correctChoice = correctAnswer;

        List<int> possibleAnswers = new List<int>();
        possibleAnswers.Add(correctAnswer);

        while (possibleAnswers.Count < answerTexts.Length)
        {
            int randomAnswer = Random.Range(0, 20);
            if (!possibleAnswers.Contains(randomAnswer))
            {
                possibleAnswers.Add(randomAnswer);
            }
        }

        ShuffleList(possibleAnswers);

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = possibleAnswers[i].ToString();
        }


        // ballImageHolder.SetActive(true);
      
        // NormalDeskSprite();
    }



    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }



    public void CheckAnswer(int answerIndex)
    {

        int firstNumber = firstNumbers[currentQuestionIndex];
        int secondNumber = secondNumbers[currentQuestionIndex];

        // Store the clicked button index
        lastClickedButtonIndex = answerIndex;



        if (int.Parse(answerTexts[answerIndex].text) == correctAnswer)
        {
            // Highlight the clicked button in green
            //candyButtons[answerIndex].color = Color.green;

            Debug.Log("Correct!");

          //  correctSound.Play();
          //  crowdSound.Play();
          //  crowdCheer.SetTrigger("isCrowdCheer");
          //  kozuAnimations.SetTrigger("IsButtonPressed");

            //will be calling method in BillboardScore scrip and passing a bool parameter
            answerStatuses[currentQuestionIndex] = true;

            // questionText.text = firstNumber + " + " + secondNumber + " = " + correctAnswer;
        }
        else
        {

            Debug.Log("Wrong!");
          //  wrongSound.Play();
          //  kozuAnimations.SetTrigger("IsKozuFailed");

            //will be calling method in BillboardScore scrip and passing a bool parameter
            answerStatuses[currentQuestionIndex] = false;
        }
        // Call OnAnsweredQuestion method in QuizUI script to trigger a proccess of Score Billboard.
       // FindObjectOfType<BillboardScore>().OnAnsweredQuestion(answerStatuses[currentQuestionIndex]);


        currentQuestionIndex++;

        if (currentQuestionIndex >= firstNumbers.Length)
        {
            Debug.Log("Quiz finished!");
            return;
        }
        DisableButtons();

    }

    //Called by the Ball when it hits the ring To generate a new question with a delay
    public void GenerateQuestionDelay()
    {
        GenerateQuestion(2f);
    }
}
