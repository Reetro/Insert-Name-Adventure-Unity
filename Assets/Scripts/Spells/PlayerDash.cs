namespace Spells
{
    public class PlayerDash : Spell
    {
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
    }
}
