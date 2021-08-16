using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class yBotMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 3.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationspeed = 5f;

    [SerializeField]
    private float animationSmoothTime = 0.1f;
    [SerializeField]
    private float animationPlayTransition = 0.15f;

    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float decelaration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    Animator animator;

    //[SerializeField]
    //private Transform aimTarget;

    //[SerializeField]
    //private float aimDistance = 10f;

    private CharacterController controller;
    private PlayerInput playerinput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction JumpAction;
    //private InputAction ShootAction;

    ////Animation
    //private Animator anim;
    
    //int StopAnimation;
    //int SprintAnimation;
    //int moveXAnimationID;
    //int moveZAnimationID;


    int JumpAnimation;

    //increase performance
    int VelocityZHash;
    int VelocityXHash;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); ;
        playerinput = GetComponent<PlayerInput>(); 
        cameraTransform = Camera.main.transform;
        moveAction = playerinput.actions["Move"];
        JumpAction = playerinput.actions["Jump"];


        Cursor.lockState = CursorLockMode.Locked;

        //anim = GetComponent<Animator>();
        JumpAnimation = Animator.StringToHash("Jump");
        //moveXAnimationID = Animator.StringToHash("MoveX");
        //moveZAnimationID = Animator.StringToHash("MoveZ");

        animator = GetComponent<Animator>();
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");

    }

    private void LateUpdate()
    {
       // aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
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

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; 
        }


        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(VelocityXHash, currentAnimationBlendVector.x);
        animator.SetFloat(VelocityZHash, currentAnimationBlendVector.y);

        //anim.SetFloat(moveXAnimationID, currentAnimationBlendVector.x);
        //anim.SetFloat(moveZAnimationID, currentAnimationBlendVector.y

        // Changes the height position of the player..
        if (JumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(JumpAnimation, animationPlayTransition); 
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //rotate function
        Quaternion targetrotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, rotationspeed * Time.deltaTime);
    }



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
            playerSpeed = 7f;
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
            playerSpeed = 3.0f;
        }
    }
}
