using UnityEngine;

namespace PlayerCharacter.Controller
{
    public class SpearGroundCollider : MonoBehaviour
    {
        private PlayerSpear playerSpear = null;

        /// <summary>
        /// Get player spear
        /// </summary>
        private void Awake()
        {
            playerSpear = GetComponentInParent<PlayerSpear>();
        }
        /// <summary>
        /// Check to see if spear is touching the ground
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(playerSpear.WhatIsGround, collision.gameObject))
            {
                playerSpear.TouchingGround = true;
            }
        }
        /// <summary>
        /// Check to see if spear is no longer touching the ground
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(playerSpear.WhatIsGround, collision.gameObject))
            {
                playerSpear.TouchingGround = false;
            }
        }
    }
}
