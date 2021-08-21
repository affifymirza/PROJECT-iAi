using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParasiteController : MonoBehaviour
{
    public Camera myCamera;  
    public float speed = 2f;
    public float Sprintspeed = 6f;
    public float RotationSpeed = 15f;
   // public float JumpSpeed = 7f;
    public float AnimationBlendSpeed = 2f;

    Animator MyAnimator;
    CharacterController myController;

    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;
    float mSpeedY = 0;
    float mGravity = -9.81f;

    bool mJumping = false;
    bool mSprintint = false;

    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<CharacterController>();
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //if (Input.GetButtonDown("Jump") && !mJumping)
        //{
        //    mJumping = true;
        //    MyAnimator.SetTrigger("Jump");

        //    mSpeedY += JumpSpeed;

        //}
        //if (!myController.isGrounded)
        //{
        //    mSpeedY += mGravity * Time.deltaTime;
        //}
        //else if(mSpeedY < 0)
        //{
        //    mSpeedY = 0;
        //}

       // MyAnimator.SetFloat("SpeedY", mSpeedY / JumpSpeed);

        if(mJumping && mSpeedY < 0)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Default")))
            {
                mJumping = false;
               // MyAnimator.SetTrigger("Land");
            }
        }
        mSprintint = Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = new Vector3(x, 0, z).normalized;

        Vector3 rotatedMovement = Quaternion.Euler(0, myCamera.transform.rotation.eulerAngles.y, 0) * movement;
        Vector3 verticalmovement = Vector3.up * mSpeedY;

        myController.Move((verticalmovement + (rotatedMovement * (mSprintint ? Sprintspeed : speed))) * Time.deltaTime);

        if(rotatedMovement.magnitude > 0)
        {
            mDesiredRotation = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
            mDesiredAnimationSpeed = mSprintint ? 1 : 0.5f;
        }
        else
        {
            mDesiredAnimationSpeed = 0;
        }

        MyAnimator.SetFloat("Speed", Mathf.Lerp(MyAnimator.GetFloat("Speed"), mDesiredAnimationSpeed, AnimationBlendSpeed * Time.deltaTime));
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
}
