using UnityEngine;

namespace Spells
{
    public class PlayerDash : Spell
    {
        private Rigidbody2D playerRigidBody2D = null;
        private float defaultDashTime = 0f;
        private bool runDashTimer = false;
        private float dashTimer = 0f;
        private bool wasIdle = false;

        /// <summary>
        /// Starts the dash timer when spell is cast
        /// </summary>
        protected override void CastSpell()
        {
            if (UsingTwoValues)
            {
                runDashTimer = true;
            }
        }
        /// <summary>
        /// Get the Player's Rigidbody2D component and set default values
        /// </summary>
        protected override void OnUpackSpellValuesDone()
        {
            if (UsingTwoValues)
            {
                playerRigidBody2D = GeneralFunctions.GetPlayerGameObject().GetComponent<Rigidbody2D>();

                defaultDashTime = Value1;
                dashTimer = Value1;
            }
            else if (UsingThreeValues)
            {
                Debug.LogError("Unable to cast player dash spell it can't have more than 2 values");
            }
            else
            {
                Debug.LogError("Unable to cast player dash spell it requires 2 Values");
            }
        }
        /// <summary>
        /// Apply dash velocity on spell cast
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (runDashTimer)
            {
                if (dashTimer <= 0)
                {
                    dashTimer = defaultDashTime;

                    playerRigidBody2D.velocity = Vector2.zero;

                    if (HasCoolDown && !wasIdle)
                    {
                        StartCoolDown();

                        runDashTimer = false;
                    }
                    else
                    {
                        OnSpellCastEnded();

                        runDashTimer = false;
                    }
                }
                else
                {
                    dashTimer -= Time.deltaTime;

                    ApplyPlayerDash();
                }
            }
        }
        /// <summary>
        /// Determines what direction the player is moving in then dashes the player
        /// </summary>
        private void ApplyPlayerDash()
        {
            bool isIdle = false;

            bool leftOrRight = GeneralFunctions.IsPlayerMovingLeftOrRight(out isIdle);

            if (!isIdle)
            {
                wasIdle = false;

                if (leftOrRight)
                {
                    playerRigidBody2D.velocity = Vector2.left * Value2;
                }
                else
                {
                    playerRigidBody2D.velocity = Vector2.right * Value2;
                }
            }
            else
            {
                wasIdle = true;
            }
        }
    }
}