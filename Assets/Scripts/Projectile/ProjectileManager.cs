using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _prewarmCount = 20;
    #endregion

    #region 내부 변수
    public static ProjectileManager Instance { get; private set; }
    private readonly Queue<GameObject> _pools = new Queue<GameObject>();
    private Transform _poolRoot;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        _poolRoot = new GameObject("Bullet_Pool").transform;

        for (int i = 0; i < _prewarmCount; ++i)
        {
            GameObject b = Instantiate(_bulletPrefab, _poolRoot);
            b.SetActive(false);
            _pools.Enqueue(b);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }
    }

    #region 외부 호출 함수
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Bullet_Pool").transform;
        }

        if (_pools.Count > 0)
        {
            GameObject b = _pools.Dequeue();
            b.transform.SetPositionAndRotation(position, rotation);
            b.transform.SetParent(null);
            b.SetActive(true);
            return b;
        }

        GameObject extra = Instantiate(_bulletPrefab, position, rotation);
        extra.SetActive(true);
        return extra;
    }

    public void Despawn(GameObject bullet)
    {
        if (bullet == null)
        {
            return;
        }

        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Bullet_Pool").transform;
        }

        bullet.SetActive(false);
        bullet.transform.SetParent(_poolRoot);

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        _pools.Enqueue(bullet);
    }
    #endregion
}