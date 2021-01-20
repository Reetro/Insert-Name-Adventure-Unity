using UnityEngine;
using UnityEngine.Events;
using PlayerCharacter.GameSaving;
using PlayerUI;
using GameplayManagement;
using System;

namespace ComponentLibrary
{
    [Serializable]
    public class HealthComponent : MonoBehaviour
    {
        [System.Serializable]
        public class TakeAnyDamge : UnityEvent<float> { }

        [Header("Health Settings")]
        [Tooltip("Maximum amount health this object can have")]
        [SerializeField] float maxHealth = 10f;

        [HideInInspector]
        public UnityEvent OnDeath;

        [HideInInspector]
        public TakeAnyDamge OnTakeAnyDamage;

        private GameplayManager gameplayManager = null;
        private HealthBar healthBar = null;

        private static bool setMaxhealth = false;

        /// <summary>
        /// Setup all needed health component variables and update player state if on player
        /// </summary>
        public void ConstructHealthComponent()
        {
            gameplayManager = GeneralFunctions.GetGameplayManager();

            IsCurrentlyDead = false;

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                if (!setMaxhealth)
                {
                    CurrentHealth = maxHealth;

                    setMaxhealth = true;

                    UpdatePlayerState();
                }
                else
                {
                    maxHealth = MyPlayerState.MaxHealth;
                    CurrentHealth = MyPlayerState.CurrentHealth;

                    UpdatePlayerState();
                }
            }
            else
            {
                CurrentHealth = maxHealth;
            }
        }
        /// <summary>
        /// Setup all needed health component variables and update player state if on player then setups the given health bar
        /// </summary>
        public void ConstructHealthComponent(HealthBar healthBar)
        {
            this.healthBar = healthBar;

            gameplayManager = GeneralFunctions.GetGameplayManager();

            IsCurrentlyDead = false;
        }
        /// <summary>
        /// Setup the health component for the player character
        /// </summary>
        public void ConstructHealthComponentForPlayer()
        {
            healthBar = FindObjectOfType<PlayerUIManager>().transform.GetChild(1).GetComponent<HealthBar>();

            gameplayManager = GeneralFunctions.GetGameplayManager();

            IsCurrentlyDead = false;

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                if (!setMaxhealth)
                {
                    CurrentHealth = maxHealth;

                    if (healthBar)
                    {
                        healthBar.SetMaxHealth(maxHealth);
                    }

                    setMaxhealth = true;

                    UpdatePlayerState();
                }
                else
                {
                    if (!MyPlayerState)
                    {
                        MyPlayerState = GeneralFunctions.GetPlayerState();
                    }

                    maxHealth = MyPlayerState.MaxHealth;
                    CurrentHealth = MyPlayerState.CurrentHealth;

                    if (healthBar)
                    {
                        healthBar.SetPlayerHealth(MyPlayerState);
                    }
                }
            }
            else
            {
                CurrentHealth = maxHealth;

                if (healthBar)
                {
                    healthBar.SetMaxHealth(maxHealth);
                }
            }
        }
        /// <summary>
        /// Add the given value to this Gameobjects current health
        /// </summary>
        /// <param name="amountToAdd"></param>
        public void AddHealth(float amountToAdd)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amountToAdd, 0, maxHealth);

            if (healthBar)
            {
                healthBar.SetHealth(CurrentHealth);
            }

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                UpdatePlayerState();
            }
        }
        /// <summary>
        /// Apply damage to the Gameobject this component is on if current health is below 0 the Gameobject is killed
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="showDamageText"></param>
        /// <param name="damageLayer"></param>
        public void ProccessDamage(float damage, bool showDamageText, LayerMask damageLayer)
        {
            if (GeneralFunctions.IsObjectOnLayer(damageLayer, gameObject))
            {
                if (!IsCurrentlyDead)
                {
                    CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);

                    if (showDamageText)
                    {
                        DamageText.CreateDamageText(damage, transform.position, gameplayManager.combatTextSpeed, 
                            gameplayManager.combatTextUpTime, gameplayManager.disappearTime);
                    }

                    OnTakeAnyDamage.Invoke(damage);

                    if (healthBar)
                    {
                        healthBar.SetHealth(CurrentHealth);
                    }

                    if (CurrentHealth <= 0)
                    {
                        IsCurrentlyDead = true;
                        OnDeath.Invoke();
                    }
                }
            }
        }
        /// <summary>
        /// Set current health to the given value
        /// </summary>
        /// <param name="value"></param>
        public void SetHealth(float value)
        {
            CurrentHealth = value;
            maxHealth = value;

            if (healthBar)
            {
                healthBar.SetHealth(CurrentHealth);
            }

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                UpdatePlayerState();
            }
        }
        /// <summary>
        /// Set current health and max health to the given values
        /// </summary>
        /// <param name="currentHP"></param>
        /// <param name="maxHP"></param>
        public void SetHealth(float currentHP, float maxHP)
        {
            CurrentHealth = currentHP;
            maxHealth = maxHP;

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                UpdatePlayerState();
            }

            if (healthBar)
            {
                healthBar.SetHealth(CurrentHealth);
            }
        }
        /// <summary>
        /// Reference to player currently in the world
        /// </summary>
        public PlayerState MyPlayerState { get; set; } = null;
        /// <summary>
        /// Get this Gameobjects current health
        /// </summary>
        public float CurrentHealth { get; private set; } = 0f;
        /// <summary>
        /// Check to see if the current Gameobject is dead
        /// </summary>
        public bool IsCurrentlyDead { get; private set; } = false;
        /// <summary>
        /// Gets the objects max health
        /// </summary>
        public float MaxHealth { get { return maxHealth; } }
        /// <summary>
        /// Update player state health values
        /// </summary>
        private void UpdatePlayerState()
        {
            if (MyPlayerState)
            {
                MyPlayerState.UpdatePlayerStateHP(CurrentHealth, maxHealth);
            }
            else
            {
                MyPlayerState = FindObjectOfType<PlayerState>();

                MyPlayerState.UpdatePlayerStateHP(CurrentHealth, maxHealth);
            }
        }
    }
}