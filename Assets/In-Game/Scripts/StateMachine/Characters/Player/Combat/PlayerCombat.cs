using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> comboList;
    float lastClickedTime;
    float lastComboEnd;
    int comboCount;
    Animator anim;

    [SerializeField] Weapon weapon;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !anim.GetBool("isDashing"))
        {
            Attack();
        }
        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > 0.5f && comboCount < comboList.Count)
        {
            InputSystem.DisableDevice(Keyboard.current);
            CancelInvoke("EndCombo");

            if(Time.time - lastClickedTime >= 0.4f)
            {
                anim.runtimeAnimatorController = comboList[comboCount].animatorOV;
                anim.Play("Attack", 0, 0);
                weapon.damage = comboList[comboCount].damage;
                comboCount++;
                lastClickedTime = Time.time;    
                if(comboCount >= comboList.Count)
                {
                    comboCount = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            InputSystem.EnableDevice(Keyboard.current);
            Invoke("EndCombo", 0.9f);
        }
    }

    void EndCombo()
    {
        comboCount = 0;
        lastComboEnd = Time.time;
    }

}
