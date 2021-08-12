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

    //[SerializeField]
    //private Transform aimTarget;

    //[SerializeField]
    //private float aimDistance = 10f;

    //---------------------------------------------------------------------------------------------------------------------//

    //[SerializeField]
    //private GameObject bulletPrefab;

    //[SerializeField]
    //private Transform barrelTransform;

    //[SerializeField]
    //private Transform bulletParent;

    //[SerializeField]
    //private float bulletHitMissDistance = 23f;

    private CharacterController controller;
    private PlayerInput playerinput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction JumpAction;
    //private InputAction ShootAction;

    //Animation
    private Animator anim;
    int JumpAnimation;
    int StopAnimation;
    int SprintAnimation;
    int moveXAnimationID;
    int moveZAnimationID;

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

        anim = GetComponent<Animator>();
        JumpAnimation = Animator.StringToHash("Jump");
        SprintAnimation = Animator.StringToHash("Sprint");
        StopAnimation = Animator.StringToHash("Stop");

        moveXAnimationID = Animator.StringToHash("MoveX");
        moveZAnimationID = Animator.StringToHash("MoveZ");

    }

    private void LateUpdate()
    {
        //aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    //anim.CrossFade(SprintAnimation, animationPlayTransition);
        //  //  anim.Play(SprintAnimation);

        //    playerSpeed = 9f;
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //   // anim.Play(StopAnimation);
        //    playerSpeed = 2.0f;
        //}

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


        anim.SetFloat(moveXAnimationID, currentAnimationBlendVector.x);
        anim.SetFloat(moveZAnimationID, currentAnimationBlendVector.y);


        // Changes the height position of the player..
        if (JumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.CrossFade(JumpAnimation, animationPlayTransition); 
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //rotate function
        Quaternion targetrotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, rotationspeed * Time.deltaTime);
    }
}
