using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [field: SerializeField] public float OrbitalSpeed { get; set; } = 10f;
    private float _orbitDistance;
    public Transform Target
    {
        get { return _target; }
        set
        {
            _target = value;
            SetOrbitActive();
        }
    }

    private void Start()
    {
        try
        {
            _orbitDistance = Vector2.Distance(transform.position, _target.position);
        }
        catch
        {
            Debug.LogWarning("Orbit script requires a target to function properly.");
            SetOrbitActive();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        try
        {
            Move();
        }
        catch
        {
            SetOrbitActive();
        }
    }

    private void Move()
    {
        float moveX = _orbitDistance * Mathf.Cos(Time.time * OrbitalSpeed);
        float moveY = _orbitDistance * Mathf.Sin(Time.time * OrbitalSpeed);
        transform.position = new Vector2(_target.position.x + moveX, _target.position.y + moveY);
    }

    private void SetOrbitActive()
    {
        if (_target != null)
        {
            this.enabled = true;
            gameObject.SetActive(true);
            _orbitDistance = Vector2.Distance(transform.position, _target.position);
        }
        else
        {
            this.enabled = false;
            gameObject.SetActive(false);
            _orbitDistance = 0f;
        }
    }
}
