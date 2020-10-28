﻿using PlayerCharacter.Controller;
using UnityEngine;

namespace StatusEffects.Effects
{
    public class PlayerSlowing_SE : StatusEffect
    {
        private PlayerMovement playerMovement = null;
        private bool justFired = false;
        private float defaultSpeed = 0;
        private float defaultJump = 0;

        private void Awake()
        {
            playerMovement = GeneralFunctions.GetPlayerGameObject().GetComponent<PlayerMovement>();

            if (playerMovement)
            {
                defaultSpeed = playerMovement.runSpeed;
                defaultJump = playerMovement.jumpForce;
            }
            else
            {
                Debug.LogError(gameObject.name + " Failed to get player movement");
            }
        }

        protected override void ApplyStatusEffect()
        {
            if (UsingTwoValues)
            {
                if (!justFired)
                {
                    playerMovement.runSpeed = playerMovement.runSpeed / Value1;

                    playerMovement.jumpForce -= Value2;

                    justFired = true;
                }
            }
            else
            {
                Debug.LogError("Failed to apply status " + name + " requires Using Two Values to be true");
            }
        }

        protected override void OnStatusEffectEnd()
        {      
            if (playerMovement)
            {
                playerMovement.runSpeed = defaultSpeed;

                playerMovement.jumpForce = defaultJump;
            }

            base.OnStatusEffectEnd();
        }
    }
}