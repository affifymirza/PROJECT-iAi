using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBlendTree : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float decelaration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    //increase performance
    int VelocityZHash;
    int VelocityXHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        //increase performance
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
    }

    void Update()
    {
        bool fowardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        //set current Velocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        //Handle changes in velocity
        changeVelocity(fowardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        lockOrResetVelocity(fowardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }

    //kita nak handle acceleration dengan deceleration
    void changeVelocity(bool fowardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //kalau player tekan forward, increase velocity in Z direction
        if (fowardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //increase velocity in left direction
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //decrease velocity in right direction
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //decrease velocity Z
        if (!fowardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * decelaration;
        }

        //increase velocity X kalau left tak tekan dan velocity X < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * decelaration;
        }

        //decrease velocity X kalau right tak tekan dan velocity X > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * decelaration;
        }
    }

    //Kita nak handle reset dan lock Velocity
    void lockOrResetVelocity(bool fowardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //reset velocity
        if (!fowardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }


        //lock
        if (fowardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (fowardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * decelaration;
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (fowardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }
    } 
}
