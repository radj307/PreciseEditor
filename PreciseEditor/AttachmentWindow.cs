using UnityEngine;
using System.Collections.Generic;

namespace PreciseEditor
{
    public class AttachmentWindow : BaseWindow
    {
        const float LINE_HEIGHT = 25f;

        public AttachmentWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 325f, 10f);
        }

        public void Show(Part part)
        {
            Hide();

            DialogGUIToggle toggleSurfaceAttach = new DialogGUIToggle(delegate { return GetSurfaceAttach(part); }, "Allow surface attachment to other parts", delegate { ToggleSurfaceAttach(part); }, -1, LINE_HEIGHT);
            DialogGUIToggle toggleAllowSurfaceAttach = new DialogGUIToggle(delegate { return GetAllowSurfaceAttach(part); }, "Allow other parts to be surface attached to this part", delegate { ToggleAllowSurfaceAttach(part); }, -1, LINE_HEIGHT);

            string title = FormatLabel("Precise Editor - ") + "Attachment Rules";
            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase> {
                toggleSurfaceAttach, toggleAllowSurfaceAttach
            };

            dialog = new MultiOptionDialog("attachmentWindowDialog", "", title, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
        }

        private bool GetSurfaceAttach(Part part)
        {
            return part.attachRules.srfAttach;
        }

        private void ToggleSurfaceAttach(Part part)
        {
            part.attachRules.srfAttach = !part.attachRules.srfAttach;

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.attachRules.srfAttach = part.attachRules.srfAttach;
            }
        }

        private bool GetAllowSurfaceAttach(Part part)
        {
            return part.attachRules.allowSrfAttach;
        }

        private void ToggleAllowSurfaceAttach(Part part)
        {
            part.attachRules.allowSrfAttach = !part.attachRules.allowSrfAttach;

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.attachRules.allowSrfAttach = part.attachRules.allowSrfAttach;
            }
        }
    }
}