using GeneralScripts;
using PlayerScripts.PlayerControls;
using UnityEngine;

namespace StatusEffects.Effects
{
    public class PlayerSlowingSe : StatusEffect
    {
        private PlayerMovement playerMovement;
        private bool justFired;
        private float defaultSpeed;
        private float defaultJump;

        protected override void OnUnpackDone()
        {
            playerMovement = GeneralFunctions.GetPlayerGameObject().GetComponent<PlayerMovement>();

            if (playerMovement)
            {
                defaultSpeed = playerMovement.runSpeed;

                if (UsingTwoValues)
                {
                    defaultJump = playerMovement.jumpForce;
                }
            }
            else
            {
                Debug.LogError(gameObject.name + " Failed to get player movement");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void ApplyStatusEffect()
        {
            if (UsingTwoValues)
            {
                if (!justFired)
                {
                    playerMovement.runSpeed = playerMovement.runSpeed / Value1;

                    if (UsingTwoValues)
                    {
                        playerMovement.jumpForce -= Value2;
                    }

                    justFired = true;
                }
            }
            else
            {
                Debug.LogError("Failed to apply status " + name + " requires Using Two Values to be true");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnStatusEffectEnd()
        {      
            if (playerMovement)
            {
                playerMovement.runSpeed = defaultSpeed;

                if (UsingTwoValues)
                {
                    playerMovement.jumpForce = defaultJump;
                }
            }

            base.OnStatusEffectEnd();
        }
    }
}