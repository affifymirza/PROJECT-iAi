using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform cam;
    public Animator anim;
    private Rigidbody rb;
    public LayerMask layerMask;
    public bool grounded;

   // public float speed = 6f;
    public float turnsmoothtime = 0.1f;
    //private float horizontalMove;
    //private float verticalMove;
    float turnsmoothvelocity;

    private void Start()
    {
        // this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Grounded();
        //Jump();
    }

    private void Move()
    {

        //float verticalAxis = movejoystick.Vertical;
        //float horizontalAxis = movejoystick.Horizontal;

        float verticalMove = Input.GetAxis("Vertical");
        float horizontalMove = Input.GetAxis("Horizontal");

        

        Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity, turnsmoothtime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            

            this.transform.position += moveDirection * 0.03f;

            this.anim.SetFloat("vertical", verticalMove);
            this.anim.SetFloat("horizontal", horizontalMove);

        }
    }

    private void Grounded()
    {
        if (Physics.CheckSphere(this.transform.position + Vector3.down, 0.2f, layerMask))
        {
            this.grounded = true;
        }
        else
        {
            this.grounded = false;
        }

        //this.anim.SetBool("fall", this.grounded);
    }

    //private void Jump()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space) && this.grounded)
    //    {
    //        this.rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
    //    }
    //}
}
