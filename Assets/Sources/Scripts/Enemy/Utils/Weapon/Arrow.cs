using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem _crackEffect;

    private ObjectPoolService _poolService;
    private MeshRenderer _mesh;
    private Collider _collider;
    private GameObject _gameObject;

    [Inject]
    public void Construct(ObjectPoolService poolService)
    {
        _poolService = poolService;
    }
    
    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _gameObject = gameObject;
    }

    private void OnEnable()
    {
        _crackEffect.Stop();
        _mesh.enabled = true;
        _collider.enabled = true;
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
        StartCoroutine(ReturnToPoolAfterParticles());
    }

    private IEnumerator ReturnToPoolAfterParticles()
    {
        yield return new WaitForSeconds(_crackEffect.main.duration);

        _poolService.Release(ObjectPoolService.PoolObjectTypes.Arrow, _gameObject);
    }
}
