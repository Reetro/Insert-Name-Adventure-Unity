using PlayerCharacter.Controller;
using UnityEngine;

namespace StatusEffects.Effects
{
    public class PlayerSlowing_SE : StatusEffect
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

        protected override void ApplyStatusEffect()
        {
            if (!justFired)
            {
                playerMovement.runSpeed = playerMovement.runSpeed / EffectValue;

                justFired = true;
            }
        }

        protected override void OnStatusEffectEnd()
        {      
            if (playerMovement)
            {
                playerMovement.runSpeed = defaultSpeed;
            }

            base.OnStatusEffectEnd();
        }
    }
}