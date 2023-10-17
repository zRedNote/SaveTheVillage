using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPannel;
    [SerializeField] private GameObject defeatMsg;
    [SerializeField] private GameObject victoryMsg;
    [SerializeField] private GameObject warriorImg;
    [SerializeField] private GameObject menuPannel;

    [SerializeField] private ImageTimer eatingTimer;
    [SerializeField] private ImageTimer timer;
    [SerializeField] private ImageTimer harvestTimer;
    [SerializeField] private ImageTimer raidTimer;
    [SerializeField] private ImageTimer warriorCooldownTimer;
    [SerializeField] private ImageTimer workerCooldownTimer;

    [SerializeField] private Image harvestTimerImg;
    [SerializeField] private Image raidTimerImg;
    [SerializeField] private Image workerTimerImg;
    [SerializeField] private Image warriorTimerImg;

    [SerializeField] private Button workerBtn;
    [SerializeField] private Button warriorBtn;

    [SerializeField] private Text workerNumber;
    [SerializeField] private Text foodNumber;
    [SerializeField] private Text warriorNumber;
    [SerializeField] private Text enemiesNumber;
    [SerializeField] private Text workerPrice;
    [SerializeField] private Text warriorPrice;
    [SerializeField] private Text totalFoodAmount;
    [SerializeField] private Text totalWarriorsAmount;
    [SerializeField] private Text totalRaidsAmount;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip raidSound;
    [SerializeField] private AudioClip harvestSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;

    [SerializeField] private int workerAmount;
    [SerializeField] private int warriorAmount;
    [SerializeField] private int foodAmount;
    [SerializeField] private int enemiesAmount;

    [SerializeField] private int foodPerWorker;
    [SerializeField] private int foodPerWarrior;

    [SerializeField] private int hireCost;

    [SerializeField] private int raidIncrease;
    [SerializeField] private int nextRaid;

    private int totalRaids = -1; // Тут я задаю значение -1 чтобы в конце игры не считался рейд, который нас погубил
    private int totalWarriors;
    private int totalFood;

    void Start()
    {
        raidTimer.Restart();
        timer.Restart();
        harvestTimer.Restart();
        UpdateText(); 
    }

    void Update()
    {
        WarriorImgActivation();

        FoodCheck();

        UpdateText();

        GameOver();
    }

    public void HireWorker()
    {       
        foodAmount -= hireCost;
        workerBtn.interactable = false;
        workerCooldownTimer.Restart();  
    }

    public void HireWarrior()
    {      
        foodAmount -= hireCost;
        warriorBtn.interactable = false;
        warriorCooldownTimer.Restart();
    }

    public void OpenMenu()
    {
        menuPannel.SetActive(true);
    }

    public void CloseMenu()
    {
        menuPannel.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        DeactivateBtns();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        ActivateBtns();
    }

    public void RaidTimer()
    {
        warriorAmount -= nextRaid;
        totalRaids++;
            
        if (totalRaids > 1)
        {
            nextRaid += raidIncrease;
            enemiesAmount = nextRaid;
            audioSource.clip = raidSound;
            audioSource.Play();
        }

        raidTimer.Restart();
    }

    public void HarvestTimerReset()
    {       
        foodAmount += workerAmount * foodPerWorker;
        totalFood += workerAmount * foodPerWorker;
        audioSource.clip = harvestSound;
        audioSource.Play();

        harvestTimer.Restart();
    }

    public void EatingTick()
    {
        foodAmount -= warriorAmount * foodPerWarrior;
        timer.Restart();
    }

    public void WorkerHireCooldown()
    {
        workerAmount += 1;
        workerBtn.interactable = true;
        audioSource.clip = spawnSound;
        audioSource.Play();
    }

    public void WarriorHireCooldown()
    {        
        warriorAmount += 1;
        totalWarriors++;
        warriorBtn.interactable = true;
        audioSource.clip = spawnSound;
        audioSource.Play();
    }

    private void WarriorImgActivation()
    {
        if (warriorAmount > 0)
        {
            warriorImg.SetActive(true);     
        }
        else
        {
            warriorImg.SetActive(false);
        }
    }

    private void FoodCheck()
    {
        if (foodAmount < hireCost)
        {
            DeactivateBtns();
        }
        else if (warriorCooldownTimer.IsTimerRunning())
        {
            warriorBtn.interactable = false;
        }
        else if (workerCooldownTimer.IsTimerRunning())
        {
            workerBtn.interactable = false;
        }
        else
        {
            ActivateBtns();
        }
    }

    private void UpdateText()
    {
        workerNumber.text = workerAmount.ToString();
        warriorNumber.text = warriorAmount.ToString();
        foodNumber.text = foodAmount.ToString();
        enemiesNumber.text = enemiesAmount.ToString();
        workerPrice.text = hireCost.ToString();
        warriorPrice.text = hireCost.ToString();
        totalFoodAmount.text = totalFood.ToString(); 
        totalWarriorsAmount.text = totalWarriors.ToString();
        totalRaidsAmount.text = totalRaids.ToString();
    }

    private void GameOver()
    {
        if (warriorAmount < 0)
        {        
            gameOverPannel.SetActive(true);
            victoryMsg.SetActive(false);
            warriorAmount = 1; // костыль чтобы условие не выполнялось бесконечное кол-во раз. break почему-то не получилось использовать
            audioSource.clip = defeatSound;
            audioSource.Play();
            Time.timeScale = 0;
        }
        else if (foodAmount >= 100 && warriorAmount >= 20)
        {
            gameOverPannel.SetActive(true);
            defeatMsg.SetActive(false);
            foodAmount = 0; // костыль чтобы условие не выполнялось бесконечное кол-во раз. break почему-то не получилось использовать           
            audioSource.clip = victorySound;
            audioSource.Play();
            Time.timeScale = 0;
        }
    }

    private void ActivateBtns()
    { 
        workerBtn.interactable = true;
        warriorBtn.interactable = true;
    }

    private void DeactivateBtns()
    {
        workerBtn.interactable = false;
        warriorBtn.interactable = false;
    }
}
