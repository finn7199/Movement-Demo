using UnityEngine;

[CreateAssetMenu(menuName =  "Custom/Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
