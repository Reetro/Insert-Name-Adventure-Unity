using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerUI.ToolTipUI
{
    public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ScriptableItem item = null;
        private TooltipPopup tooltipPopup = null;

        private void Awake()
        {
            tooltipPopup = FindObjectOfType<TooltipPopup>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipPopup.DisplayInfo(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipPopup.HideInfo();
        }

        public void SetItem(ScriptableItem item)
        {
            this.item = item;
        }
    }
}
