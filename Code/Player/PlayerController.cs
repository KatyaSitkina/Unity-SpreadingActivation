using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController playerController;
    AudioSource playerSteps;

    float defaultStepsDelay = 0.4f;
    float defaultSpeed = 3.0f;
    float speed;
    float delay;

    Vector3 velocity;   
    float gravity = -19.62f;

    bool isGrounded = true;     // флаг, что игрок находится на поверхности
    [SerializeField] Transform groundCheck;   // расположение мнимой сферы для проверки, что игрок находится на поверхности
    float groundDistance = 0.4f; // радиус сферы
    [SerializeField] LayerMask groundMask;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerSteps = GetComponent<AudioSource>();

        speed = defaultSpeed;
        delay = defaultStepsDelay;
    }

    void Update()
    {
        if (!InteractionController.interactionIsActive)
        {
            Movement();
            GravityPhysics();
            Run();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        playerController.Move((move * speed + velocity) * Time.deltaTime);

        if(move.magnitude > 0.1 && !playerSteps.isPlaying)
            playerSteps.PlayDelayed(delay);
        if(move.magnitude <= 0.1)
            playerSteps.Stop();
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = defaultSpeed * 2;
            delay = defaultStepsDelay / 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = defaultSpeed;
            delay = defaultStepsDelay;
        }
    }

    void GravityPhysics()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2.0f;
        velocity.y += gravity * Time.deltaTime;
    }
}
