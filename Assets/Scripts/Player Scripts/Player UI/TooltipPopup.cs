using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

namespace PlayerUI.ToolTipUI
{
    public class TooltipPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject = null;
        [SerializeField] private RectTransform popupObject = null;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] Vector3 offset = Vector3.zero;
        [SerializeField] private float padding = 0f;

        private Canvas popupCanvas = null;

        private void Awake()
        {
            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
        }

        private void Update()
        {
            FollowCursor();
        }

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;
            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            popupObject.transform.position = newPos;
        }

        public void DisplayInfo(ScriptableItem item)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(item.GetToolTipInfoText());

            infoText.text = stringBuilder.ToString();

            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }

        public void HideInfo()
        {
            popupCanvasObject.SetActive(false);
        }
    }
}