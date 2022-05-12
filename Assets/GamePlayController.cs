using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public enum NumberTpye
    {
        odd = 0,
        even = 1,
        divisibleThree = 2,
        divisibleFive = 3
    }

    private int currentLevel;
    private int highLevel;
    [SerializeField] float timeInALevel;

    private float time;
    private UIController uIController;
    private int theRightIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        uIController = GetComponent<UIController>();

        Reset();
        StartNewTurn();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            GameOver();
        }

        UpdateSlider();
    }

    public void StartNewTurn()
    {
        currentLevel++;

        if(highLevel < currentLevel)
        {
            highLevel = currentLevel;
            PlayerPrefs.SetInt("highestlevel", highLevel);
        }

        HandleSpawNumber();
        UpdateUI();
    }

    public void HandleSpawNumber()
    {
        List<int> result = new List<int>();
        for (int i = 0; i<4; i++)
        {
            result.Add(i);
        }

        List<int> A1 = new List<int>();
        List<int> A2 = new List<int>();
        int type1 = Random.Range(0, 1);
        int type2 = 1 - type1;

        A1 = GetNumber(type1);
        A2 = GetNumber(type2);

        theRightIndex = Random.Range(0, 4);
        result[theRightIndex] = A1[Random.Range(0, A1.Count)];

        for(int i = 0; i < result.Count; i++)
        {
            if (i > 3) break;
            if (theRightIndex == i) continue;
            result[i] = A2[Random.Range(0, A2.Count)];
        }

        uIController.UpdateNumber(result);
    }

    public List<int> GetNumber(int type)
    {
        List<int> result = new List<int>();
        switch (type)
        {
            case (int)NumberTpye.odd: result = SpawOdd(); break;
            case (int)NumberTpye.even: result = SpawEven(); break;
            case (int)NumberTpye.divisibleThree: result = SpawDivineThree(); break;
            case (int)NumberTpye.divisibleFive: result = SpawDivineFive(); break;
        }

        return result;
    }

    public List<int> SpawOdd()
    {
        List<int> result = new List<int>();

        for(int i = 1; i < 100; i++)
        {
            if(i%2 != 0)
            {
                result.Add(i);
            }
        }

        return result;
    }

    public List<int> SpawEven()
    {
        List<int> result = new List<int>();

        for (int i = 1; i < 100; i++)
        {
            if (i % 2 == 0)
            {
                result.Add(i);
            }
        }

        return result;
    }

    public List<int> SpawDivineThree()
    {
        List<int> result = new List<int>();

        for (int i = 1; i < 50; i++)
        {
            if (i % 3 == 0)
            {
                result.Add(i);
            }
        }

        return result;
    }

    public List<int> SpawDivineFive()
    {
        List<int> result = new List<int>();

        for (int i = 1; i < 50; i++)
        {
            if (i % 5 == 0)
            {
                result.Add(i);
            }
        }

        return result;
    }

    public void ChoosenNumber(int index)
    {
        if(theRightIndex == index)
        {
            //Start next level
            StartNewTurn();
        }
        else
        {
            //Game over
            GameOver();
        }
    }

    public void Reset()
    {
        Time.timeScale = 1;
        currentLevel = 0;
        highLevel = PlayerPrefs.GetInt("highestlevel");
        time = timeInALevel;
        UpdateUI();
    }

    public void UpdateUI()
    {
        time = timeInALevel;
        uIController.SetSlider(timeInALevel);
        uIController.UpdateLevel(currentLevel);
    }

    public void UpdateSlider()
    {
        uIController.UpdateSlider(time);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uIController.ShowGameOver();
    }
}
