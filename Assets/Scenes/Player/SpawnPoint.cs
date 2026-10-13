using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;

    private void Start()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
