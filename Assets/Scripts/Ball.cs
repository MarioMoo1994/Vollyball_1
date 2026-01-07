using UnityEngine;
using System.Collections;

public class VolleyballBallRespawn : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform spawnPlayer1;
    public Transform spawnPlayer2;

    [Header("Serve Settings")]
    public float serveDelay = 2f;
    public float serveUpForce = 4f;

    private Rigidbody rb;
    private bool isServing;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isServing)
            return;

        if (collision.gameObject.CompareTag("Ground_Player_1"))
        {
            StartCoroutine(RespawnAndServe(spawnPlayer1));
        }

        if (collision.gameObject.CompareTag("Ground_Player_2"))
        {
            StartCoroutine(RespawnAndServe(spawnPlayer2));
        }
    }

    IEnumerator RespawnAndServe(Transform spawnPoint)
    {
        isServing = true;

        // Nullstill fysikk
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Flytt ballen
        transform.position = spawnPoint.position;

        // Midlertidig "freeze" ballen i lufta
        rb.isKinematic = true;

        // Vent før serve
        yield return new WaitForSeconds(serveDelay);

        rb.isKinematic = false;

        // Gi ballen et lite dytt oppover
        rb.linearVelocity = Vector3.up * serveUpForce;

        isServing = false;
    }
}
