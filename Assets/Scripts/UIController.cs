using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System; 

public class UIController : MonoBehaviour
{
    public GameObject[] canvasHolder;
    int currentActiveCanvas = 0;

    //Game UI
    public Image bossHealthImage;
    public Image bossHealthDetail;
    public float healthAnimationLength = 1.0f;
    public Text clonesRemainingText;
    public GameObject[] playerUIHolder;
    public Image[] playerChargeLevel;

    //Winner UI
    public Text winnerText;

    void Start()
    {
        SetupInitialUI();
    }

    void Update()
    {
        
    }
     
    void SetupInitialUI()
    {
        bossHealthImage.fillAmount = 1;
        bossHealthDetail.fillAmount = 1;
        clonesRemainingText.text = PlayerPrefs.GetInt("InitialClones").ToString();
    }

    public void SwitchCanvas(int indexToEnable)
    {
        canvasHolder[currentActiveCanvas].SetActive(false);
        canvasHolder[indexToEnable].SetActive(true);
        currentActiveCanvas = indexToEnable;
    }

    #region Player UI Methods

    public void ActivatePlayerUI(int cloneNumber)
    {
        playerUIHolder[cloneNumber].SetActive(true);
    }

    public void UpdatePlayerCharge(int cloneNumber, float fillAmount)
    {
        playerChargeLevel[cloneNumber].fillAmount = fillAmount;
    }

    #endregion

    #region Boss UI Methods
    public void UpdateBossHealth(float currentHealth, float maxHealth)
    {
        float currentFillAmount = bossHealthImage.fillAmount * 100;
        if(currentHealth <= 0)
        {
            GameManager.instance.SetState(GameManager.GameState.GAMEOVER);
        }
        StartCoroutine(AnimateHealthLoss(currentFillAmount, currentHealth, 100));
    }

    internal void ToggleUI(int index, bool state)
    {
        canvasHolder[index].SetActive(state);
    } 

    IEnumerator AnimateHealthLoss(float oldHealth, float newHealth, float maxHealth)
    {
        bool finishedAnimating = false;
        float localTimer = healthAnimationLength;
        while(!finishedAnimating)
        {
            localTimer -= Time.deltaTime;
            if (localTimer < 0.0f)
                localTimer = 0.0f;

            bossHealthImage.fillAmount = (newHealth + ((oldHealth - newHealth) * localTimer)) / maxHealth;
            bossHealthDetail.fillAmount = (newHealth + ((oldHealth - newHealth) * localTimer)) / maxHealth;

            if (localTimer <= 0.0f)
                finishedAnimating = true;

            yield return finishedAnimating;
        }
        
        yield return new WaitForSeconds(healthAnimationLength);
    }
    #endregion

    #region Player UI Methods

    public void UpdateCloneNumber(int amountOfClones)
    {
        clonesRemainingText.text = amountOfClones.ToString();
    }

    #endregion
}
