using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int slotNumber = 1;
    [SerializeField] private TextMeshProUGUI activeText = null;

    private void OnEnable()
    {
        activeText.enabled = GeneralFunctions.IsSlotActive(slotNumber);
    }
}
