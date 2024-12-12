using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CreateCircle : MonoBehaviour
{

    public GameObject prefab;

    public int numberOfPieces = 4;
    public float circleRadius = 2.5f;
    public Color backCircleColor = Color.black;
    public Color unseletedCircleColor = Color.gray;
    public Color selectedCircleColor = Color.cyan;
    public Color correctCircleColor = Color.green;
    public TextMeshProUGUI score;
    public TextMeshProUGUI totalScore;
    public GameObject completePopup;

    public int correctCircleNumbers = 1;

    private List<int> correctCircle = new List<int>();

    List<CircleManager> circlePieces = new List<CircleManager>();

    AudioSource buttonClickSound;


    void Start()
    {
        //To set defaul level
        //PlayerPrefs.SetInt("Level",1);
        //PlayerPrefs.Save();

        int level = PlayerPrefs.GetInt("Level", 1);
        List<LevelData> levelData = (new LevelData()).Levels();
        if (level <= levelData.Count)
        {
            LevelData currentLevelData = levelData[level - 1];
            correctCircleNumbers = currentLevelData.correctCircleNumbers;
            numberOfPieces = currentLevelData.numberOfPieces;
        }
        else
        {
            Debug.LogWarning("Invalid level: " + level);
            correctCircleNumbers = 1;
            numberOfPieces = 4;
        }


        UpdateUiBoard();
        buttonClickSound = GetComponent<AudioSource>();

        System.Random random = new System.Random();

        for (int i = 0; i < correctCircleNumbers; i++)
        {
            int randomInt;
            // Generate unique random numbers
            do
            {
                randomInt = random.Next(0, numberOfPieces);
            } while (correctCircle.Contains(randomInt));

            correctCircle.Add(randomInt);
        }

        float angleWeNeed = (360 / numberOfPieces);
        for (int i = 0; i < numberOfPieces; i++)
        {

            float angle = i * Mathf.PI * 2 / numberOfPieces;
            float x = Mathf.Cos(angle) * .04f * circleRadius;
            float y = Mathf.Sin(angle) * .04f * circleRadius;
            GameObject instance = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            instance.transform.SetParent(transform);
            CircleManager circleManager = instance.GetComponent<CircleManager>();
            circleManager.startAngle = angleWeNeed * i;
            circleManager.endAngle = angleWeNeed * (i + 1);
            circleManager.radius = circleRadius;
            circleManager.fillColor = unseletedCircleColor;
            circlePieces.Insert(0, circleManager);

            instance.name = $"Piece_{i + 1}";
        }

        GameObject frontCircle = Instantiate(prefab, new Vector3(0, 0, -0.1f), Quaternion.identity);
        frontCircle.transform.SetParent(transform);
        CircleManager frontCircleManager = frontCircle.GetComponent<CircleManager>();
        frontCircleManager.startAngle = 0;
        frontCircleManager.endAngle = 360;
        frontCircleManager.fillColor = backCircleColor;
        frontCircleManager.radius = .13f*circleRadius;
        frontCircleManager.name = "Small";


        GameObject backCircle = Instantiate(prefab, new Vector3(0, 0, 0.1f), Quaternion.identity);
        backCircle.transform.SetParent(transform);
        CircleManager backCircleManager = backCircle.GetComponent<CircleManager>();
        backCircleManager.startAngle = 0;
        backCircleManager.endAngle = 360;
        backCircleManager.fillColor = backCircleColor;
        backCircleManager.radius = .4f + circleRadius;
        backCircleManager.name = "Back";

    }

    void CheckAllSelected()
    {
        bool allSelected = circlePieces.TrueForAll(circle => circle.isSelected);
        if (allSelected)
        {
            completePopup.SetActive(true);
        }
    }

    public void onAddClick()
    {
        int firstUnselectedIndex = circlePieces.FindIndex(circle => !circle.isSelected);
        if (firstUnselectedIndex != -1)
        {
            buttonClickSound.Play();
            CircleManager firstUnselected = circlePieces[firstUnselectedIndex];
            firstUnselected.isSelected = true;
            if (correctCircle.Contains(firstUnselectedIndex))
            {
                firstUnselected.fillColor = correctCircleColor;
                firstUnselected.isCorrect = true;
                UpdateUiBoard();
            }
            else {
                firstUnselected.fillColor = selectedCircleColor;
            }
        }

        CheckAllSelected();
    }

    public void onRemoveClick()
    {
        buttonClickSound.Play();
        CircleManager lastSelected = circlePieces.FindLast(circle => circle.isSelected);
        if (lastSelected != null)
        {
            lastSelected.isSelected = false;
            lastSelected.isCorrect = false;
            lastSelected.fillColor = unseletedCircleColor;
            UpdateUiBoard();
        }
    }

    private void UpdateUiBoard()
    {
        totalScore.text = correctCircleNumbers.ToString();
        List<CircleManager> correctCirclePieces = circlePieces.FindAll(circle => circle.isCorrect);
        score.text = correctCirclePieces.Count.ToString();
    }




}
