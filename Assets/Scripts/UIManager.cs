using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform[] players;

    public delegate void RequestUpdate();
    public static event RequestUpdate OnRequest;

    void Awake()
    {

    }

    void OnEnable()
    {
        Entity.OnHealth += SetHealth;
        Entity.OnAbility += SetEnergy;
        BossController.OnDamage += SetHealth;
        GlobalEvents.OnRefresh += RefreshUI;
    }

    void OnDisable()
    {
        Entity.OnHealth -= SetHealth;
        Entity.OnAbility -= SetEnergy;
        BossController.OnDamage += SetHealth;
        GlobalEvents.OnRefresh -= RefreshUI;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(UpdateUI());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 0 indexed player identifier, damage amount taken
    void SetHealth(int player_id, float max_health, float current_health, float amount)
    {
        //Debug.Log("The " + ((player_id == 3) ? "boss:" : "player: " + player_id) + "'s health was updated to " + current_health);
        SetFillAmount(players[player_id].GetChild(0).GetComponent<Image>(), (current_health / max_health), (amount / max_health));
        if (players[player_id].GetChild(0).GetComponent<Image>().fillAmount + (amount / max_health) <= 0.0f)
            Debug.Log("Player: " + player_id + " has reached 0 health, you ded son.");

    }

    void SetEnergy(int player_id, float max_energy, float current_energy, float amount)
    {
        //Debug.Log("The " + ((player_id == 3) ? "boss:" : "player: " + player_id) + " just used " + amount + " energy.");
        SetFillAmount(players[player_id].GetChild(1).GetComponent<Image>(), (current_energy / max_energy), (amount / max_energy));
        if (players[player_id].GetChild(1).GetComponent<Image>().fillAmount + (amount / max_energy) <= 0.0f)
            Debug.Log("Player: " + player_id + " has reached 0 energy, you tired son.");
    }

    IEnumerator UpdateUI()
    {
        while (true)
        {
            RefreshUI();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void RefreshUI()
    {
        //Debug.Log("Refreshing ui...");
        if (OnRequest != null)
        {
            OnRequest();
        }
    }

    void SetFillAmount(Image image, float current, float amount)
    {
        if (amount != 0)
        {
            if (image.fillAmount + amount <= 0.0f)
            {
                image.fillAmount = 0.0f;
            }
            else
            {
                image.fillAmount += amount;
            }
        }
        else
        {
            image.fillAmount = current;
        }
    }
}
