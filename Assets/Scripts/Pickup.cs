using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Pickup : MonoBehaviour
{

    [Serializable]
    public class Effect
    {
        public enum EffectType
        {
            NONE = 0,
            HEALTH = 1,
            SPEED = 2,
            FIRE_RATE = 3,
            SHIELDS = 4,
            LIVES = 5,
            DAMAGE = 6,
            ENERGY = 7
        }
        public EffectType type;
        public int value;
        public float duration;

        public Effect(EffectType _type, int _value, float _duration = 0.0f)
        {
            type = _type;
            value = _value;
            duration = _duration;
        }
    }

    // Buff class contains a list of effects, this way we can define any type of buff as a combination of effect types

    [Serializable]
    public class Buff
    {
        public List<Effect> effects;

        public Buff(List<Effect> _effects)
        {
            effects = _effects;
        }
    }

    [Serializable]
    public class Ammo
    {
        public enum AmmoType
        {
            DEFAULT = 0,
            ROCKET = 1,
            GUN = 2,
            LASER = 3
        }
        public AmmoType type;
        public int amount;

        public Ammo(AmmoType _type, int _amount)
        {
            type = _type;
            amount = _amount;
        }
    }

    public enum PickupType
    {
        DEFAULT = 0,
        BUFF = 1, 
        AMMO = 2
    }

    public PickupType type;
    public Buff buff;
    public Ammo ammo;

    // Use this for initialization
    void Start()
    {
        // If the buff is null, create a default effect that gives health and gives increased damage for 10s
        if (buff == null && type == PickupType.BUFF)
        {
            buff = new Buff(
                new List<Effect>
                {
                    new Effect(Effect.EffectType.HEALTH, 5),        // Health is added perm so no timer (optional var)
                    new Effect(Effect.EffectType.DAMAGE, 3, 10.0f)  // Damage boost is added as temp, so define time limit
                }
            );
        }

        // If we've selected ammo type but not defined any behaviour, set the default ammo pickup to rockets and 5 ammo
        if (ammo == null && type == PickupType.AMMO)
        {
            ammo = new Ammo(Ammo.AmmoType.ROCKET, 5);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use(Entity playerInput)
    {
        switch (type)
        {
            case PickupType.DEFAULT:
                Debug.Log("No behaviour defined. This is a default pickup action");
                break;
            case PickupType.BUFF:
                Debug.Log("Calculating buff effects.");
                CalcBuff(playerInput);
                break;
            case PickupType.AMMO:
                Debug.Log("Picked up ammo.");
                CalcAmmo(playerInput);
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }

    void CalcBuff(Entity playerInput)
    {
        foreach (Effect e in buff.effects)
        {
            switch (e.type)
            {
                case Effect.EffectType.NONE:
                    break;
                case Effect.EffectType.HEALTH:
                    playerInput.UpdateHealth(e.value);
                     break;
                case Effect.EffectType.SPEED:
                    break;
                case Effect.EffectType.FIRE_RATE:
                    break;
                case Effect.EffectType.SHIELDS:
                    break;
                case Effect.EffectType.LIVES:
                    break;
                case Effect.EffectType.DAMAGE:
                    break;
                case Effect.EffectType.ENERGY:
                    playerInput.UpdateEnergy(e.value);
                    break;
                default:
                    break;
            }
        }
    }

    void CalcAmmo(Entity playerInput)
    {
        switch (ammo.type)
        {
            case Ammo.AmmoType.DEFAULT:
                break;
            case Ammo.AmmoType.ROCKET:
                break;
            case Ammo.AmmoType.GUN:
                break;
            case Ammo.AmmoType.LASER:
                break;
            default:
                break;
        }
    }
}
