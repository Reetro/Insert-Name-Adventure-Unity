using UnityEngine;

namespace Spells
{
    public class PlayerDash : Spell
    {
        private Rigidbody2D playerRigidBody2D = null;
        private float defaultDashTime = 0f;
        private bool runDashTimer = false;
        private float dashTimer = 0f;

        protected override void CastSpell()
        {
            if (UsingTwoValues)
            {
                runDashTimer = true;
            }
        }

        protected override void OnUpackSpellValuesDone()
        {
            if (UsingTwoValues)
            {
                playerRigidBody2D = GeneralFunctions.GetPlayerGameObject().GetComponent<Rigidbody2D>();

                defaultDashTime = Value1;
                dashTimer = Value1;
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

                    if (HasCoolDown)
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

                    var playerAngle = GeneralFunctions.GetPlayerGameObject().transform.localEulerAngles.y;

                    if (playerAngle <= 0)
                    {
                        playerRigidBody2D.velocity = Vector2.right * Value2;
                    }
                    else
                    {
                        playerRigidBody2D.velocity = Vector2.left * Value2;
                    }
                }
            }
        }
    }
}