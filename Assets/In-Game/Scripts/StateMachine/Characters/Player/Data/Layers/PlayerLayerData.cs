using System;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerLayerData 
    {
        [field:SerializeField] public LayerMask GroundLayer { get; private set; }
        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0; //bitwise shift op
        }
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(GroundLayer, layer);
        }
    }
}
