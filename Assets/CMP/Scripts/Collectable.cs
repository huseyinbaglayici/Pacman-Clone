using UnityEngine;

namespace CMP.Scripts
{
    public enum CollectableType
    {
        Pellet,
        PowerPellet
    }

    public class Collectable : MonoBehaviour
    {
        public CollectableType Type;
    }
}