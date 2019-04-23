using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace PreciseEditor
{
    public abstract class BaseWindow : MonoBehaviour
    {
        const string FORMAT_POSITION = "F4";
        const string CONTROL_LOCK_ID = "PreciseEditorLock";

        protected MultiOptionDialog dialog = null;
        protected PopupDialog popupDialog = null;
        protected Rect dialogRect;

        public bool IsVisible()
        {
            return popupDialog != null;
        }

        public void Hide()
        {
            if (popupDialog != null)
            {
                SaveWindowPosition();
                popupDialog.Dismiss();
                popupDialog = null;
            }
        }

        protected void SaveWindowPosition()
        {
            if (popupDialog)
            {
                RectTransform rectTransform = popupDialog.GetComponent<RectTransform>();
                Vector3 position = rectTransform.position / GameSettings.UI_SCALE;
                dialogRect.x = position.x / Screen.width + 0.5f;
                dialogRect.y = position.y / Screen.height + 0.5f;
            }
        }

        protected string FormatLabel(string label)
        {
            return "<color=\"white\">" + label + "</color>";
        }

        protected void RemoveControlLock()
        {
            InputLockManager.RemoveControlLock(CONTROL_LOCK_ID);
        }

        protected void SetTextInputListeners(DialogGUITextInput textInput)
        {
            TMP_InputField tmp_Input = textInput.uiItem.GetComponent<TMP_InputField>();
            tmp_Input.onSelect.AddListener(new UnityAction<string>(OnSelectTextInput));
            tmp_Input.onDeselect.AddListener(new UnityAction<string>(OnDeselectTextInput));
        }

        private void OnSelectTextInput(string s)
        {
            InputLockManager.SetControlLock(ControlTypes.EDITOR_GIZMO_TOOLS | ControlTypes.EDITOR_ROOT_REFLOW, CONTROL_LOCK_ID);
        }

        private void OnDeselectTextInput(string s)
        {
            InputLockManager.RemoveControlLock(CONTROL_LOCK_ID);
        }
    }
}