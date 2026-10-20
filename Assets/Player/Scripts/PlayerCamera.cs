using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            _player.position.x,
            _player.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            _smoothSpeed * Time.deltaTime
        );
    }
}