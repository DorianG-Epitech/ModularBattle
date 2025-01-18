using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Vector3 MovementDirection;
    public float MoveSpeed = 1f;
    [SerializeField] private bool _randomInitialVelocity = false;
    [SerializeField] private bool _randomPosition = false;
    private bool _hasHitPoint = false;

    private void Start()
    {
        if (_randomInitialVelocity)
            MovementDirection = new Vector3(Random.Range(0, 2) * 2 - 1, 0, Random.Range(0, 2) * 2 - 1).normalized;
        if (_randomPosition)
            transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 0, Random.Range(-3.7f, 3.7f));
    }

    private void Update()
    {
        transform.position += MoveSpeed * Time.deltaTime * MovementDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitPoint"))
        {
            _hasHitPoint = true;
            collision.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 0, Random.Range(-3.7f, 3.7f));
            return;
        }

        MovementDirection = Vector3.Reflect(MovementDirection, collision.contacts[0].normal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, MovementDirection);
    }
}
