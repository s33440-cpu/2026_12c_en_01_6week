using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;

    public void Toggle()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
