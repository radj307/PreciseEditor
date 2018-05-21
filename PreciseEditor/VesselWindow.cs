using UnityEngine;

namespace PreciseEditor
{
    public class VesselWindow : MonoBehaviour
    {
        const string FORMAT_POSITION = "F4";

        PopupDialog popupDialog = null;
        protected Rect dialogRect = new Rect(0.5f, 0.5f, 275f, 200f);

        public void Show()
        {
            const string TITLE = "Precise Editor - Vessel Window";
            const string MESSAGE = "";
            const float LABEL_WIDTH = 100;
            const float VALUE_WIDTH = 150;
            const float HEIGHT = 25f;

            DialogGUILabel labelCenterOfMass = new DialogGUILabel("Center of Mass", LABEL_WIDTH, HEIGHT);
            DialogGUILabel labelCenterOfLift = new DialogGUILabel("Center of Lift", LABEL_WIDTH, HEIGHT);
            DialogGUILabel labelCenterOfThrust = new DialogGUILabel("Center of Thrust", LABEL_WIDTH, HEIGHT);
            DialogGUILabel labelCloseButtonSpacer = new DialogGUILabel("", 60f, HEIGHT);
            DialogGUILabel valueCenterOfMass = new DialogGUILabel(this.GetCenterOfMass, VALUE_WIDTH, HEIGHT);
            DialogGUILabel valueCenterOfLift = new DialogGUILabel(this.GetCenterOfLift, VALUE_WIDTH, HEIGHT);
            DialogGUILabel valueCenterOfThrust = new DialogGUILabel(this.GetCenterOfThrust, VALUE_WIDTH, HEIGHT);
            DialogGUIButton buttonClose = new DialogGUIButton("Close Window", delegate { }, 140f, HEIGHT, true);

            MultiOptionDialog dialog = new MultiOptionDialog("vesselWindowDialog", MESSAGE, TITLE, HighLogic.UISkin, this.dialogRect,
                new DialogGUIFlexibleSpace(),
                new DialogGUIHorizontalLayout(labelCenterOfMass, valueCenterOfMass),
                new DialogGUIHorizontalLayout(labelCenterOfLift, valueCenterOfLift),
                new DialogGUIHorizontalLayout(labelCenterOfThrust, valueCenterOfThrust),
                new DialogGUIVerticalLayout(
                    new DialogGUIFlexibleSpace(),
                    new DialogGUIHorizontalLayout(labelCloseButtonSpacer, buttonClose)
                )
            );

            this.popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            this.popupDialog.onDestroy.AddListener(this.OnPopupDialogDestroy);
        }

        public void Hide()
        {
            if (this.popupDialog)
            {
                this.popupDialog.Dismiss();
            }
        }

        public void Toggle()
        {
            if (this.popupDialog)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        private void OnPopupDialogDestroy()
        {
            Vector3 dialogPosition = this.popupDialog.RTrf.position;
            this.dialogRect.x = dialogPosition.x / Screen.width + 0.5f;
            this.dialogRect.y = dialogPosition.y / Screen.height + 0.5f;
        }

        private string GetCenterOfMass()
        {
            if (EditorLogic.RootPart)
            {
                return EditorMarker_CoM.findCenterOfMass(EditorLogic.RootPart).ToString("F4");
            }
            else
            {
                return "No vessel";
            }
        }

        private string GetCenterOfLift()
        {
            if (EditorLogic.RootPart)
            {
                Vector3 refVelocity = new Vector3(0.0f, -1.7f, 100.0f);
                double refAltitude = 100;
                double refStp = 99.8323745727539f;
                double refDensity = 1.21034456867256f;
                Ray CoL = EditorMarker_CoL.FindCoL(refVelocity, refAltitude, refStp, refDensity);

                return CoL.origin.ToString("F4");
            }
            else
            {
                return "No vessel";
            }            
        }

        private string GetCenterOfThrust()
        {
            if (EditorLogic.RootPart)
            {
                Ray CoT = EditorMarker_CoT.FindCoT();

                return CoT.origin.ToString("F4");
            }
            else
            {
                return "No vessel";
            }
        }
    }
}