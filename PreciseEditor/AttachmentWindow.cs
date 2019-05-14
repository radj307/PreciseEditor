using UnityEngine;
using System.Collections.Generic;

namespace PreciseEditor
{
    public class AttachmentWindow : BaseWindow
    {
        const float LINE_HEIGHT = 25f;

        private AttachRules attachRules = null;

        public AttachmentWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 325f, 10f);
        }

        public void Show(AttachRules attachRules)
        {
            Hide();

            this.attachRules = attachRules;

            DialogGUIToggle toggleSurfaceAttach = new DialogGUIToggle(delegate { return GetSurfaceAttach(); }, "Allow surface attachment to other parts", delegate { ToggleSurfaceAttach(); }, -1, LINE_HEIGHT);
            DialogGUIToggle toggleAllowSurfaceAttach = new DialogGUIToggle(delegate { return GetAllowSurfaceAttach(); }, "Allow other parts to be surface attached to this part", delegate { ToggleAllowSurfaceAttach(); }, -1, LINE_HEIGHT);

            string title = FormatLabel("Precise Editor - ") + "Attachment Rules";
            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase> {
                toggleSurfaceAttach, toggleAllowSurfaceAttach
            };

            dialog = new MultiOptionDialog("attachmentWindowDialog", "", title, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
        }

        private bool GetSurfaceAttach()
        {
            return attachRules.srfAttach;
        }

        private void ToggleSurfaceAttach()
        {
            attachRules.srfAttach = !attachRules.srfAttach;
        }

        private bool GetAllowSurfaceAttach()
        {
            return attachRules.allowSrfAttach;
        }

        private void ToggleAllowSurfaceAttach()
        {
            attachRules.allowSrfAttach = !attachRules.allowSrfAttach;
        }
    }
}