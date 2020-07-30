using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using PlayerControls;

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
        private Controls controls = null;

        /// <summary>
        /// Create new player controls object
        /// </summary>
        private void Awake()
        {
            controls = new Controls();

            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
        }
        /// <summary>
        /// Enable player input
        /// </summary>
        private void OnEnable()
        {
            controls.Player.Enable();
        }
        /// <summary>
        /// Disable player input
        /// </summary>
        private void OnDisable()
        {
            controls.Player.Disable();
        }
        /// <summary>
        /// Make Tooltip follow mouse position
        /// </summary>
        private void Update()
        {
            FollowCursor();
        }
        /// <summary>
        /// Calculate screen edges
        /// </summary>
        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = (Vector3)controls.Player.MousePostion.ReadValue<Vector2>() + offset;
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
        /// <summary>
        /// Get Tooltip info and display tooltip
        /// </summary>
        /// <param name="item"></param>
        public void DisplayInfo(ScriptableItem item)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(item.GetToolTipInfoText());

            infoText.text = stringBuilder.ToString();

            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }
        /// <summary>
        /// Hide Tooltip
        /// </summary>
        public void HideInfo()
        {
            popupCanvasObject.SetActive(false);
        }
    }
}