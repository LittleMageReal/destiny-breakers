using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int Hp;
    public static event System.Action OnHealthLost; // Correctly declared as static

    void OnCollisionEnter(Collision collision)
    {
        var damageScript = collision.gameObject.GetComponent<Damage>();
        if (damageScript!= null)
        {
            LoseHealth(damageScript.damageAmount);
        }
    }

    public void LoseHealth(int damageAmount)
    {
        // Ensure the event is invoked after modifying Hp
        if (damageAmount > 0)
        {
            Hp -= damageAmount;
            OnHealthLost?.Invoke(); // Correctly invoke the event
        }
    }

    public void GainHealth(int healAmount)
    {
        // Correctly increase health
        if (healAmount > 0)
        {
            Hp += healAmount;
        }
    }
}

