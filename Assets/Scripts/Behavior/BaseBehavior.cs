using UnityEngine.Tilemaps;
using UnityEngine;

public abstract class BaseBehavior : MonoBehaviour
{
    public Tilemap Map
    {
        protected get;
        set;
    }

    protected void Flip()
    {
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
