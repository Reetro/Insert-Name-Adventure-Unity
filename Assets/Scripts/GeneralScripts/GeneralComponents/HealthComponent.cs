using System;
using GameplayScripts;
using GeneralScripts.UI;
using PlayerScripts.PlayerControls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GeneralScripts.GeneralComponents
{
    [Serializable]
    public class HealthComponent : MonoBehaviour
    {
        [Serializable]
        public class TakeAnyDamage : UnityEvent<float> { }

        [Header("Health Settings")]
        [Tooltip("Maximum amount health this object can have")]
        [SerializeField]
        private float maxHealth = 10f;

        [FormerlySerializedAs("OnDeath")] [HideInInspector]
        public UnityEvent onDeath;

        [FormerlySerializedAs("OnTakeAnyDamage")] [HideInInspector]
        public TakeAnyDamage onTakeAnyDamage;

        private GameplayManager gameplayManager;
        private HealthBar healthBar;

        private static bool setMaxHealth;

        /// <summary>
        /// Setup all needed health component variables and update player state if on player
        /// </summary>
        public void ConstructHealthComponent()
        {
            gameplayManager = GeneralFunctions.GetGameplayManager();

            IsCurrentlyDead = false;

            if (GeneralFunctions.IsObjectOnPlayer(gameObject))
            {
                if (!setMaxHealth)
                {
                    CurrentHealth = maxHealth;

                    setMaxHealth = true;

                    UpdatePlayerState();
                }
                else
                {
                    maxHealth = PlayerState.MaxHealth;
                    CurrentHealth = PlayerState.CurrentHealth;

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
        public void ConstructHealthComponent(HealthBar newHealthBar)
        {
            this.healthBar = newHealthBar;

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
                if (!setMaxHealth)
                {
                    CurrentHealth = maxHealth;

                    if (healthBar)
                    {
                        healthBar.SetMaxHealth(maxHealth);
                    }

                    setMaxHealth = true;

                    UpdatePlayerState();
                }
                else
                {
                    if (!MyPlayerState)
                    {
                        MyPlayerState = GeneralFunctions.GetPlayerState();
                    }

                    maxHealth = PlayerState.MaxHealth;
                    CurrentHealth = PlayerState.CurrentHealth;

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
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Apply damage to the Gameobject this component is on if current health is below 0 the Gameobject is killed
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="showDamageText"></param>
        /// <param name="damageLayer"></param>
        public void ProcessesDamage(float damage, bool showDamageText, LayerMask damageLayer)
        {
            if (GeneralFunctions.IsObjectOnLayer(damageLayer, gameObject))
            {
                if (!IsCurrentlyDead)
                {
                    CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);

                    if (showDamageText)
                    {
                        DamageText.CreateDamageText(damage, transform.position, gameplayManager.combatTextSpeed, gameplayManager.combatTextUpTime, gameplayManager.disappearTime, gameplayManager.textDistance);
                    }

                    onTakeAnyDamage.Invoke(damage);

                    if (healthBar)
                    {
                        healthBar.SetHealth(CurrentHealth);
                    }

                    if (CurrentHealth <= 0)
                    {
                        IsCurrentlyDead = true;
                        onDeath.Invoke();
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
        /// <param name="currentHp"></param>
        /// <param name="maxHp"></param>
        public void SetHealth(float currentHp, float maxHp)
        {
            CurrentHealth = currentHp;
            maxHealth = maxHp;

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
        public PlayerState MyPlayerState { get; set; }
        /// <summary>
        /// Get this Gameobjects current health
        /// </summary>
        public float CurrentHealth { get; private set; }
        /// <summary>
        /// Check to see if the current Gameobject is dead
        /// </summary>
        public bool IsCurrentlyDead { get; private set; }
        /// <summary>
        /// Gets the objects max health
        /// </summary>
        public float MaxHealth => maxHealth;

        /// <summary>
        /// Update player state health values
        /// </summary>
        private void UpdatePlayerState()
        {
            if (MyPlayerState)
            {
                PlayerState.UpdatePlayerStateHp(CurrentHealth, maxHealth);
            }
            else
            {
                MyPlayerState = FindObjectOfType<PlayerState>();

                PlayerState.UpdatePlayerStateHp(CurrentHealth, maxHealth);
            }
        }
    }
}