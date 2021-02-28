using GameplayScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GeneralScripts.UI
{
    public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ScriptableItem item;
        private TooltipPopup tooltipPopup;
        private bool mouseOnButton;

        public void OnSceneCreated()
        {
            tooltipPopup = FindObjectOfType<TooltipPopup>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOnButton = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipPopup.HideInfo();

            mouseOnButton = false;
        }

        public void SetItem(ScriptableItem newItem)
        {
            item = newItem;
        }

        private void Update()
        {
            if (mouseOnButton)
            {
                tooltipPopup.DisplayInfo(item);
            }
        }
    }
}