using UnityEngine;

namespace Building {
    [RequireComponent(typeof(Collider))]
    public class SelectableCollider : MonoBehaviour {
        public Building SelectableObject;
    }
}
