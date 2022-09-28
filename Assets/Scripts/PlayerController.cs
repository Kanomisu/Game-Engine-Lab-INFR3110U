using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Player Movement
    public PlayerAction inputAction;
    Vector2 move;
    Vector2 rotate;
    private float walkSpeed = 5f;
    public float jump = 5f;


    //Jump
    Rigidbody rb;
    private float distanceToGround;
    private bool isGrounded = true;

    public Camera playerCamera;
    Vector3 cameraRotation;

    //Player animation
    Animator playerAnimator;
    private bool isWalking = false;


    //projectile
    public GameObject bullet;
    public Transform projectilePos;

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        
        if (!instance)
        {
            instance = this;
        }

        inputAction = new PlayerAction();

        inputAction.Player.Movement.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Movement.canceled += cntxt => move = Vector2.zero;

        inputAction.Player.Jump.performed += cntxt => Jump();
        inputAction.Player.Shoot.performed += cntxt => Shoot();

        inputAction.Player.Look.performed += cntxt => rotate = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => rotate = Vector2.zero;

        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isGrounded = false;
        }

    }

    private void Shoot()
    {
        Rigidbody bulletRb = Instantiate(bullet, projectilePos.position, Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        bulletRb.AddForce(transform.up * 5f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //looking around
        cameraRotation = new Vector3(cameraRotation.x + rotate.y, cameraRotation.y + rotate.x, cameraRotation.z);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.eulerAngles = new Vector3(transform.rotation.x, cameraRotation.y, transform.rotation.z);

        transform.Translate(Vector3.forward * move.y * Time.deltaTime * walkSpeed, Space.Self);
        transform.Translate(Vector3.right * move.x * Time.deltaTime * walkSpeed, Space.Self);

        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);

        //fix to movement
        if (move == Vector2.zero)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        playerAnimator.SetBool("isWalking", isWalking);
    }
}