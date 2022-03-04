using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public int artCollected;

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float walkSpeed = 6f;
    public float sprintSpeed = 9f;

    public float turnSmoothTime = 0.1f; // base speed
    float turnSmoothVelocity;

    Vector3 gravity = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
            speed = walkSpeed;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        if(!controller.isGrounded)
        {
            gravity += Physics.gravity * Time.deltaTime;
            controller.Move(gravity*Time.deltaTime);
        }
        else
        {

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Art"))
        {
            Destroy(collision.gameObject);
            artCollected++;
            Debug.Log(artCollected);
        }
    }
}
