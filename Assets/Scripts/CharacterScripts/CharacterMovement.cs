using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        move = Camera.main.transform.TransformDirection(move);
        move.y = 0f;

        Vector3 newPos = rb.position + move.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    private void Rotate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (direction.sqrMagnitude > 0.01f)
        {
            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
