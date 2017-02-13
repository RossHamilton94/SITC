using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public int id;

    public float base_health = 20;
    public float health = 20;
    public float base_ammo_count = 30;
    public float ammo = 0;

    public float base_energy = 20;
    public float energy = 20;
    public float energy_cost = 5;
    public float regen_rate = 0.2f;

    public float speed = 12.0f;
    public float accel = 6.0f;
    public float air_accel = 3.0f;
    public float jump = 8.0f;
    public float gravity_scale = -9.81f;

    public delegate void HealthAction(int pid, float health, float current_health, float amount);
    public static event HealthAction OnHealth;

    public delegate void AbilityAction(int pid, float energy, float current_energy, float cost);
    public static event AbilityAction OnAbility;

    // Use this for initialization
    void Start()
    {

    }

    protected virtual void OnEnable()
    { 
        UIManager.OnRequest += SendUpdate;
    }

    protected virtual void OnDisable()
    {
        UIManager.OnRequest -= SendUpdate;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Physics.gravity = Vector3.one * gravity_scale;

        if (Input.GetKeyDown(KeyCode.H))
        {
            int damage = Mathf.RoundToInt(Random.Range(-5.0f, 0.0f)); 
            UpdateHealth(damage);
        }
    }

    void SendUpdate()
    {
        TriggerOnAbility(id, base_energy, energy, 0.0f);
        TriggerOnHealth(id, base_health, health, 0.0f);
    }

    void LateUpdate()
    {
        energy += Time.deltaTime * regen_rate;
    }

    public void TriggerOnHealth(int id, float base_health, float health, float damage)
    {
        // Update the UI
        if (OnHealth != null)
        {
            OnHealth(id, base_health, health, damage);
        }
    }

    public void TriggerOnAbility(int id, float base_energy, float current_energy, float cost)
    {
        // Update the UI
        if (OnAbility != null)
        {
            OnAbility(id, base_energy, current_energy, cost);
        }
    }

    public void UpdateHealth(int amount)
    {
        if (health + amount <= 0)
        {
            // We ded
            Debug.Log("I am entity: " + id + " and i just died");
            health = 0; 
        }
        else
        {
            health += amount;
        }
    }

    public void UpdateEnergy(int amount)
    {
        if (energy + amount <= 0)
        {
            // We ded
            Debug.Log("I am entity: " + id + " and i just ran out of energy");
            energy = 0;
        }
        else
        {
            energy += amount;
        }
    }
}
