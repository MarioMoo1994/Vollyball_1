using UnityEngine;

public class VolleyballBall : MonoBehaviour
{
    [Header("Ball Movement")]
    public float hitForceX = 5f;      // Hvor langt ballen slås i X-retning
    public float hitForceY = 6f;      // Hvor høyt ballen går
    public float ballSpeed = 1f;      // Generell fart-multiplikator

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player 1 slår ballen mot høyre (+X)
        if (collision.gameObject.CompareTag("Player_1"))
        {
            HitBall(Vector3.right);
        }

        // Player 2 slår ballen mot venstre (-X)
        if (collision.gameObject.CompareTag("Player_2"))
        {
            HitBall(Vector3.left);
        }
    }

    void HitBall(Vector3 direction)
    {
        // Nullstill nåværende fart for konsistent slag
        rb.linearVelocity = Vector3.zero;

        Vector3 force = new Vector3(
            direction.x * hitForceX,
            hitForceY,
            0f
        );

        rb.linearVelocity = force * ballSpeed;
    }
}
