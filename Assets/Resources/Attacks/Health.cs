using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int Hp;

    void OnCollisionEnter(Collision collision)
    {
        var damageScript = collision.gameObject.GetComponent<Damage>();
        if (damageScript != null)
        {
            LoseHealth(damageScript.damageAmount);
        }
    }

    public void LoseHealth(int damageAmount)
    {
        Hp -= damageAmount;
    }

    public void GainHealth(int healAmount)
    {
        Hp += healAmount;
    }
}

