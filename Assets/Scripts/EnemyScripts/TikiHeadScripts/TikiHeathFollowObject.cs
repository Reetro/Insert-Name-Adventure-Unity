using GeneralScripts;
using UnityEngine;

namespace EnemyScripts.TikiHeadScripts
{
    public class TikiHeathFollowObject : MonoBehaviour
    {
        [SerializeField] private float followSpeed = 1f;

        // Update is called once per frame
        private void Update()
        {
            var position = transform.position;
            position = Vector2.MoveTowards(position, new Vector2(GeneralFunctions.GetPlayerGameObject().transform.position.x, position.y), followSpeed * Time.deltaTime);
            transform.position = position;
        }
    }
}