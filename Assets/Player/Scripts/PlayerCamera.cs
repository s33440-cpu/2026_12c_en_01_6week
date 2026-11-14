using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (_player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject == null)
                return;

            _player = playerObject.transform;
        }

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