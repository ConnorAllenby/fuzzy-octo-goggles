using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapons;

namespace weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShootyTarget : MonoBehaviour
    {

        public float health = 10;


        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}
