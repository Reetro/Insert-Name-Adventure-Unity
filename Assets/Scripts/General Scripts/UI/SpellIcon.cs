using UnityEngine;
using UnityEngine.UI;
using Spells;
using TMPro;
using UnityEngine.EventSystems;

namespace PlayerUI
{
    public class SpellIcon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Image spellIcon = null;
        private Button spellButton = null;
        private ActionButton actionButton = null;
        private Transform actionBarContainerTransfrom = null;
        private CanvasGroup canvasGroup = null;

        /// <summary>
        /// Where this spell is located on the Actionbar
        /// </summary>
        public int SpellIndex { get; private set; }
        /// <summary>
        /// The actual spell assigned to this slot
        /// </summary>
        public ScriptableSpell MyScriptableSpell { get; private set; }

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private Image cooldownImage = null;

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private TextMeshProUGUI coolDownText = null;

        /// <summary>
        /// Set default values and disable button in parent object
        /// </summary>
        /// <param name="spellToCast"></param>
        /// <param name="index"></param>
        public void SetupIcon(ScriptableSpell spellToCast, int index, ActionButton actionButton)
        {
            MyScriptableSpell = spellToCast;
            SpellIndex = index;
            this.actionButton = actionButton;

            spellIcon = GetComponent<Image>();

            spellIcon.sprite = spellToCast.Artwork;

            spellButton = GetComponent<Button>();

            DisplayCooldown(false);

            spellButton.onClick.AddListener(CastSpell);

            spellIcon.transform.SetAsLastSibling();

            canvasGroup = GetComponent<CanvasGroup>();

            actionBarContainerTransfrom = GameObject.FindGameObjectWithTag("Actionbar Container").transform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
        /// <summary>
        /// Parent icon to the action bar container and start drag
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            actionButton.RemoveIcon();

            transform.SetParent(actionBarContainerTransfrom);

            transform.position = eventData.position;

            transform.position += (Vector3)eventData.delta;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;

            canvasGroup.alpha = 0.6f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            canvasGroup.alpha = 1f;
        }

        #region Spell Functions
        /// <summary>
        /// Hide / Show Cooldown info
        /// </summary>
        /// <param name="show"></param>
        public void DisplayCooldown(bool show)
        {
            cooldownImage.enabled = show;

            coolDownText.enabled = show;
        }
        /// <summary>
        /// Update the cooldown countdown timer text
        /// </summary>
        /// <param name="cooldown"></param>
        public void UpdateCooldownText(float cooldown)
        {
            coolDownText.text = cooldown.ToString("f1");
        }
        /// <summary>
        /// Update the fill amount of the cooldown overlay image
        /// </summary>
        /// <param name="cooldown"></param>
        public void UpdateCooldownFillAmount(float cooldown, bool updateFill)
        {
            if (updateFill)
            {
                cooldownImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            }
        }
        /// <summary>
        /// Reset fill amount of the cooldown overlay image
        /// </summary>
        public void ResetCooldownFilAmount()
        {
            cooldownImage.fillAmount = 1f;
        }
        /// <summary>
        /// Cast the spell at the given index on the action bar
        /// </summary>
        /// <param name="spellIndex"></param>
        public void CastSpell()
        {
            GeneralFunctions.CastSpell(MyScriptableSpell, this);
        }
        #endregion
    }
}