using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        rb.linearVelocity = new Vector3(
            moveInput * moveSpeed,
            rb.linearVelocity.y,
            rb.linearVelocity.z
        );
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );
        }
    }

    void CheckGround()
    {
        Vector3 origin = transform.position;
        isGrounded = Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    // Debug-visning i Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * groundCheckDistance
        );
    }
}
