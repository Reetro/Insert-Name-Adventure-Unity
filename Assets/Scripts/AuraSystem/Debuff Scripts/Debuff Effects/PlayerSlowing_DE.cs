using PlayerCharacter.Controller;
using UnityEngine;

namespace AuraSystem.Effects
{
    public class PlayerSlowing_DE : DebuffEffect
    {
        private PlayerMovement playerMovement = null;
        private bool justFired = false;
        private float defaultSpeed = 0;

        private void Awake()
        {
            playerMovement = GeneralFunctions.GetPlayerGameObject().GetComponent<PlayerMovement>();

            if (playerMovement)
            {
                defaultSpeed = playerMovement.runSpeed;
            }
            else
            {
                Debug.LogError(gameObject.name + " Failed to get player movement");
            }
        }

        protected override void ApplyDebuffEffect()
        {
            if (!justFired)
            {
                playerMovement.runSpeed -= DebuffValue;

                justFired = true;
            }
        }

        protected override void OnDebuffEnd()
        {
            if (playerMovement)
            {
                playerMovement.runSpeed = defaultSpeed;
            }

            base.OnDebuffEnd();
        }
    }
}