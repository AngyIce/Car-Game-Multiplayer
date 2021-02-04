using UnityEngine;
using Mirror;

    public class Projectile : NetworkBehaviour
    {
        public float destroyAfter;
        public Rigidbody rigidBody;
        public float force;
        public GameObject projectile;

        void Start()
        {
            rigidBody.AddForce(transform.forward * force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Blocks"))
            {
                Destroy(projectile);
            }
                
        }
    }

