
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PotionInventory : MonoBehaviour
{
    private PotionType storedPotion;
    private bool hasPotion = false;

    [Header("Inventory UI")]
    [SerializeField] UnityEngine.UI.Image Hotkey;
    public UnityEngine.UI.Image potionIcon;
    public Sprite healingPotionIcon;
    public Sprite speedPotionIcon;
    public UnityEngine.UI.Slider durationBar;
    public GameObject durationBarObject;

    private void Start()
    {
        foreach (Transform child in durationBar.transform)
        {
            child.gameObject.SetActive(false);
        }
        UpdateInventoryUI();
    }

    public bool HasPotion()
    {
        return hasPotion;
    }

    public bool TryStorePotion(PotionType potion)
    {
        if (hasPotion)
            return false;

        storedPotion = potion;
        hasPotion = true;

        UpdateInventoryUI();

        Debug.Log("Stored potion: " + storedPotion);

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UsePotion();
        }
    }

    private void UsePotion()
    {
        if (!hasPotion)
            return;

        switch (storedPotion)
        {
            case PotionType.Healing:
                Debug.Log("Using healing potion");
                CollectableController.Instance.AddHP(FindAnyObjectByType<HealthPot>().amountOfHealing);
                break;

            case PotionType.Speed:
                Debug.Log("Using speed potion");
                foreach (Transform child in durationBar.transform)
                {
                    child.gameObject.SetActive(true);
                }
                StartCoroutine(ApplySpeedBoost());
                durationBar.value = 1f;
                break;
        }
        hasPotion = false;

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        if (potionIcon == null)
            return;

        if (!hasPotion)
        {
            potionIcon.enabled = false;
            Hotkey.enabled = false;
            return;
        }

        switch (storedPotion)
        {
            case PotionType.Healing:
                potionIcon.sprite = healingPotionIcon;
                break;

            case PotionType.Speed:
                potionIcon.sprite = speedPotionIcon;
                break;
        }

        potionIcon.enabled = true;
        Hotkey.enabled = true;
    }
    private IEnumerator ApplySpeedBoost()
    {
        float speedDuration = FindAnyObjectByType<SpeedPotion>().duration;
        float elapsedTime = 0f;
        float originalSpeed = FindAnyObjectByType<PlayerMovement>()._speed;

        while (elapsedTime < speedDuration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = 1f - (elapsedTime / speedDuration);
            durationBar.value = remainingTime;
            yield return null;
        }

        CollectableController.Instance.AddSpeed(FindAnyObjectByType<SpeedPotion>().amountOfBoost);
        yield return new WaitForSeconds(speedDuration);
        FindAnyObjectByType<PlayerMovement>()._speed = originalSpeed;
        durationBar.value = 0f;
        foreach (Transform child in durationBar.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
