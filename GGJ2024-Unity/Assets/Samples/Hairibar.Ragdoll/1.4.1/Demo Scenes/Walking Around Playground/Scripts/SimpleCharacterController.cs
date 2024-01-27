using UnityEngine;

namespace Hairibar.Ragdoll.Demo
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class SimpleCharacterController : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 60;
        public float blendTreeDamping = 0.1f;

        [System.NonSerialized] public float verticalInput;
        [System.NonSerialized] public float horizontalInput;


        Rigidbody rb;
        Animator animator;


        private void FixedUpdate()
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 velocity = verticalInput * new Vector3(0, 0, speed);
            velocity.y = currentVelocity.y;
            rb.velocity = rb.rotation * velocity;

            Quaternion deltaRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
            rb.MoveRotation(deltaRotation * rb.rotation);

            animator?.SetFloat("Running", verticalInput, blendTreeDamping, Time.deltaTime);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator?.SetTrigger("attack_1");
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator?.SetTrigger("attack_2");
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                animator?.SetTrigger("ded");
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                animator?.SetTrigger("not_ded");
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                animator?.SetTrigger("t_pose");
            }
        }




        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            ConfigureRigidbody();
        }

        private void ConfigureRigidbody()
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
