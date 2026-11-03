using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float movementSpeed = 0.5f;
    public float movementHeight = 0.15f;
    private Vector3 startingPosition;
    private float randomSpeed;
    private float randomHeight;
    private float randomOffset;

    private void Start()
    {
        startingPosition = transform.localPosition;

        randomSpeed = Random.Range(0.7f, 1.3f);
        randomHeight = Random.Range(0.7f, 1.3f);
        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }
    private void Update()
    {
        float verticalMovement = Mathf.Sin(Time.time * movementSpeed* randomSpeed +randomOffset)* movementHeight * randomHeight;
        transform.localPosition = startingPosition + new Vector3(0f, verticalMovement, 0f);
    }
}