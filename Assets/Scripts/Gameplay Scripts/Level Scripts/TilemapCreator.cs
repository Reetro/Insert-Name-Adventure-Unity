using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelObjects
{
    [RequireComponent(typeof(Tilemap), typeof(TilemapRenderer), typeof(TilemapCollider2D))]
    [RequireComponent(typeof(Rigidbody2D), typeof(CompositeCollider2D))]
    public class TilemapCreator : MonoBehaviour
    {
        private new Rigidbody2D rigidbody2D = null;
        private CompositeCollider2D compositeCollider = null;
        private TilemapRenderer tilemapRenderer = null;
        private new TilemapCollider2D collider2D = null;

        private void OnValidate()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();

            if (rigidbody2D)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
            else
            {
                Debug.LogError("Tilemap creator " + gameObject.name + " failed to get rigidbody2D default value not set");
            }

            compositeCollider = GetComponent<CompositeCollider2D>();

            if (compositeCollider)
            {
                compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
            else
            {
                Debug.LogError("Tilemap creator " + gameObject.name + " failed to get compositeCollider default value not set");
            }

            tilemapRenderer = GetComponent<TilemapRenderer>();

            if (tilemapRenderer)
            {
                tilemapRenderer.sortingLayerID = SortingLayer.NameToID("Foreground");

                tilemapRenderer.sortingOrder = 10;
            }
            else
            {
                Debug.LogError("Tilemap creator " + gameObject.name + " failed to get tilemapRenderer default values not set");
            }

            collider2D = GetComponent<TilemapCollider2D>();

            if (collider2D)
            {
                collider2D.usedByComposite = true;
            }
            else
            {
                Debug.LogError("Tilemap creator " + gameObject.name + " failed to get collider2D default value not set");
            }
        }
    }
}