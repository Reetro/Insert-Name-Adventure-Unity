﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerUI.ToolTipUI
{
    public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ScriptableItem item = null;
        private TooltipPopup tooltipPopup = null;
        private bool mouseOnButton = false;

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

        public void SetItem(ScriptableItem item)
        {
            this.item = item;
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