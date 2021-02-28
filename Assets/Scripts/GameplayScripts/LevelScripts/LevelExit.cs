using GeneralScripts;
using Spells;
using UnityEngine;

namespace GameplayScripts.LevelScripts
{
    public sealed class LevelExit : MonoBehaviour
    {
        private LevelLoader levelLoader = null;

        public void ConstructExit(LevelLoader newLevelLoader)
        {
            levelLoader = newLevelLoader;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!GeneralFunctions.IsObjectPlayer(collision.gameObject)) return;
            if (GeneralFunctions.IsObjectDead(collision.gameObject)) return;
            GeneralFunctions.GetGameplayManager().onLevelExitOverlap.Invoke();

            var activeSpells = FindObjectsOfType<Spell>();

            foreach (var spell in activeSpells)
            {
                if (spell)
                {
                    spell.PauseCooldown();
                }
            }

            levelLoader.LoadNextLevel();
        }
    }
}