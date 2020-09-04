using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SaveSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int slotNumber = 1;
    [SerializeField] private TextMeshProUGUI activeText = null;

    private int clickCount = 0;
    private bool isSelected = false;

    private void OnEnable()
    {
        activeText.enabled = GeneralFunctions.IsSlotActive(slotNumber);
    }

    public void OnClicked()
    {
        isSelected = true;


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount = eventData.clickCount;

        if (clickCount == 2)
        {
            clickCount = 0;

            if (GeneralFunctions.DoesSaveExistInSlot(slotNumber))
            {
                GeneralFunctions.LoadGameFromSlot(slotNumber);
            }
            else
            {
                GeneralFunctions.CreateNewSave(slotNumber);
            }
        }
    }
}
