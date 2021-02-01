using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComponentLibrary
{
    public class TileDestroyer : MonoBehaviour
    {
        private BoxCollider2D boxCollider = null;
        private Tilemap tileMap = null;
        private GameObject tileMapGameobject = null;

        [Tooltip("If true the tile destroyer will not be able destroy tiles until told to by parent object")]
        [SerializeField] private bool waitForInput = false;

        /// <summary>
        /// If wait for input is true the tile destroyer will not be able to destroy a tile until this is true
        /// </summary>
        public bool CanDestroyTile { get; set; }

        /// <summary>
        /// Called after the entire scene has been created makes sure the trigger collision ignores player collision
        /// </summary>
        public void OnSceneCreated()
        {
            boxCollider = GetComponent<BoxCollider2D>();

            if (boxCollider)
            {
                var playerCollider = GeneralFunctions.GetPlayerGameObject().GetComponent<BoxCollider2D>();

                if (playerCollider)
                {
                    Physics2D.IgnoreCollision(playerCollider, boxCollider);

                    tileMapGameobject = GameObject.FindGameObjectWithTag("Destructible");

                    if (tileMapGameobject)
                    {
                        gameObject.SetActive(true);

                        tileMap = tileMapGameobject.GetComponent<Tilemap>();
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError(name + " failed to get player collider");
                }
            }
            else
            {
                Debug.LogError(name + " does not have a box collider");
            }
        }
        /// <summary>
        /// When the box collider hit's an object if it's destructible terrain then convert hit location to a tilemap cell and destroy the tile
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Destructible", collision.gameObject))
            {
                if (waitForInput)
                {
                    if (CanDestroyTile)
                    {
                        Vector3 hitPosition = Vector3.zero;

                        if (tileMap)
                        {
                            foreach (ContactPoint2D hit in collision.contacts)
                            {
                                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                                tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
                            }
                        }
                    }
                }
                else
                {
                    Vector3 hitPosition = Vector3.zero;

                    if (tileMap)
                    {
                        foreach (ContactPoint2D hit in collision.contacts)
                        {
                            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                            tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
                        }
                    }
                }
            }
        }
    }
}