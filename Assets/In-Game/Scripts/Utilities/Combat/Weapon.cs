using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;

    BoxCollider boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void EnableTriggerBox()
    {
        boxCollider.enabled = true;
    }
    public void DisableTriggerBox()
    {
        boxCollider.enabled = false;
    }
}
