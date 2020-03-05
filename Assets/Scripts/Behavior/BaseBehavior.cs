using UnityEngine.Tilemaps;
using UnityEngine;

public abstract class BaseBehavior : MonoBehaviour
{
    public Tilemap Map
    {
        protected get;
        set;
    }
}
