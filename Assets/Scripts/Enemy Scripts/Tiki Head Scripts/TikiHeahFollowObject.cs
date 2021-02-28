using UnityEngine;

namespace EnemyCharacter.AI
{
    public class TikiHeahFollowObject : MonoBehaviour
    {
        [SerializeField] private float followSpeed = 1f;

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(GeneralFunctions.GetPlayerGameObject().transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        }
    }
}