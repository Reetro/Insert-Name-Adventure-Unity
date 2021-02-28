using GeneralScripts;
using UnityEngine;

namespace GameplayScripts.LevelScripts
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GeneralFunctions.KillTarget(collision.gameObject);
            }
        }
    }
}
