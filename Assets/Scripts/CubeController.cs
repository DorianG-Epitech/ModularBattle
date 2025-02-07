using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Vector3 MovementDirection;
    public float MoveSpeed = 1f;
    public bool HasHitPoint {
        get { return _hasHitPoint; }
        set {
            _hasHitPoint = value;
            _playerAura.SetActive(_hasHitPoint);
        }
    }
    [SerializeField] private float _deathSize = 0.2f;
    [SerializeField] private GameObject _playerAura;
    [SerializeField] private bool _randomInitialVelocity = false;
    [SerializeField] private bool _randomPosition = false;
    private bool _hasHitPoint = false;
    private List<int> _currentCollisions = new List<int>();

    private void Start()
    {
        if (_randomInitialVelocity)
        {
            MovementDirection = new Vector3(Random.Range(0, 2) * 2 - 1, 0, Random.Range(0, 2) * 2 - 1).normalized;
        }
        if (_randomPosition)
            transform.position = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-3.5f, 3.5f));
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;
        transform.position += MoveSpeed * Time.deltaTime * MovementDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.IsGameActive)
            return;
        if (_currentCollisions.Contains(collision.gameObject.GetInstanceID()))
            return;
        _currentCollisions.Add(collision.gameObject.GetInstanceID());
        if (collision.gameObject.CompareTag("HitPoint"))
        {
            HasHitPoint = true;
            _currentCollisions.Remove(collision.gameObject.GetInstanceID());
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            return;
        }


        if (collision.gameObject.TryGetComponent(out CubeController otherCube) && HasHitPoint)
        {
            HasHitPoint = false;
            transform.localScale += Vector3.one / 10;
            otherCube.transform.localScale -= Vector3.one / 10;
            if (otherCube.transform.localScale.x <= _deathSize)
            {
                GameManager.Instance.KillCube(otherCube);
            }
            GameManager.Instance.SpawnHitPoint();
        }

        MovementDirection = Vector3.Reflect(MovementDirection, collision.contacts[0].normal);
        MovementDirection = new Vector3(Mathf.Sign(MovementDirection.x), 0, Mathf.Sign(MovementDirection.z)).normalized;
    }

    private void OnCollisionExit(Collision collision)
    {
        _currentCollisions.Remove(collision.gameObject.GetInstanceID());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, MovementDirection);
    }
}
