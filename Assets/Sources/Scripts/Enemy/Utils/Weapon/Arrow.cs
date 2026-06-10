using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem _crackEffect;
    [SerializeField] private ParticleSystem _flyEffect;
    [SerializeField] private TrailRenderer _trailEffect;
    [SerializeField] private float _spinSpeed = 500f;

    private ObjectPoolService _poolService;
    private MeshRenderer _mesh;
    private Collider _collider;
    private GameObject _gameObject;
    private Transform _transform;

    private bool _isActive = true;

    [Inject]
    public void Construct(ObjectPoolService poolService)
    {
        _poolService = poolService;
    }
    
    private void Awake()
    {
        _transform = transform;
        
        _mesh = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _gameObject = gameObject;
    }

    private void Update()
    {
        if (_isActive)
        {
            _transform.Rotate(0, 0, _spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnEnable()
    {
        _isActive = true;
        
        _crackEffect.Stop();        
        _flyEffect.Play();
        _trailEffect.emitting = true;

        _mesh.enabled = true;
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _isActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        HitEffectSpawner effect = collision.collider.GetComponent<HitEffectSpawner>();
        ContactPoint hitPoint = collision.contacts[0];

        if (effect != null)
            effect.Perform(hitPoint);
        

        if (player != null)
        {
            player.Die(hitPoint);
        }

        _mesh.enabled = false;
        _collider.enabled = false;

        _crackEffect.Play();
        _flyEffect.Stop(true);
        _trailEffect.emitting = false;

        StartCoroutine(ReturnToPoolAfterParticles());
    }

    private IEnumerator ReturnToPoolAfterParticles()
    {
        yield return new WaitForSeconds(_crackEffect.main.duration);

        _poolService.Release(ObjectPoolService.PoolObjectTypes.Arrow, _gameObject);
    }
}
