using UnityEngine;

namespace PreciseEditor
{
    public class VesselWindow : BaseWindow
    {
        public VesselWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 275f, 200f);
        }

        public void Show()
        {
            Hide();

            string title = FormatLabel("Precise Editor - ") + "Vessel";
            const float LABEL_WIDTH = 100;
            const float VALUE_WIDTH = 150;
            const float HEIGHT = 25f;

            DialogGUILabel labelCenterOfMass = new DialogGUILabel(FormatLabel("Center of Mass"), LABEL_WIDTH, HEIGHT);
            DialogGUILabel labelCenterOfLift = new DialogGUILabel(FormatLabel("Center of Lift"), LABEL_WIDTH, HEIGHT);
            DialogGUILabel labelCenterOfThrust = new DialogGUILabel(FormatLabel("Center of Thrust"), LABEL_WIDTH, HEIGHT);
            DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
            DialogGUILabel valueCenterOfMass = new DialogGUILabel(GetCenterOfMass, VALUE_WIDTH, HEIGHT);
            DialogGUILabel valueCenterOfLift = new DialogGUILabel(GetCenterOfLift, VALUE_WIDTH, HEIGHT);
            DialogGUILabel valueCenterOfThrust = new DialogGUILabel(GetCenterOfThrust, VALUE_WIDTH, HEIGHT);
            DialogGUIButton buttonClose = new DialogGUIButton("Close", delegate { }, 140f, HEIGHT, true);

            dialog = new MultiOptionDialog("vesselWindowDialog", "", title, HighLogic.UISkin, dialogRect,
                new DialogGUIFlexibleSpace(),
                new DialogGUIHorizontalLayout(labelCenterOfMass, valueCenterOfMass),
                new DialogGUIHorizontalLayout(labelCenterOfLift, valueCenterOfLift),
                new DialogGUIHorizontalLayout(labelCenterOfThrust, valueCenterOfThrust),
                new DialogGUIVerticalLayout(
                    new DialogGUIFlexibleSpace(),
                    new DialogGUIHorizontalLayout(spaceToCenter, buttonClose, spaceToCenter)
                )
            );

            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
        }

        public void Toggle()
        {
            if (popupDialog)
            {
                Hide();
            }
            else
            {
                Show();
            }
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