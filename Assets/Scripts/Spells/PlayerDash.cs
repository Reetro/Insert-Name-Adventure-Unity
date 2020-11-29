using UnityEngine;

namespace Spells
{
    public class PlayerDash : Spell
    {
        private GameObject playerGameObject = null;

        protected override void CastSpell()
        {
            print("Dash spell has been casted");

            if (HasCoolDown)
            {
                StartCoolDown();
            }
            else
            {
                OnSpellCastEnded();
            }
        }

        protected override void OnUpackSpellValuesDone()
        {
            playerGameObject = GeneralFunctions.GetPlayerGameObject();
        }
    }
}
