using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

namespace PreciseEditor
{
    public class PartEditionWindow : BaseWindow
    {
        const int MAXLENGTH = 8;
        const float LABEL_WIDTH = 75f;
        const float LINE_HEIGHT = 25f;
        const string FORMAT = "F4";

        private float deltaPosition = 0.2f;
        private float deltaRotation = 15f;
        private Part part = null;
        private Space referenceSpace = Space.World;
        private bool showTweakables = false;
        private bool showColliders = false;
        private TweakableWindow tweakableWindow = null;
        private ColliderWindow colliderWindow = null;

        public PartEditionWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 600f, 175f);
        }

        public void Start()
        {
            tweakableWindow = gameObject.AddComponent<TweakableWindow>();
            colliderWindow = gameObject.AddComponent<ColliderWindow>();
        }

        public void Update()
        {
            if (IsVisible() || tweakableWindow.IsVisible() || colliderWindow.IsVisible())
            {
                ValidatePart();
            }
        }

        public void Show(Part part)
        {
            Hide();

			this.part = part;
            if (!part)
            {
                return;
            }

            DialogGUISpace spaceAxisLeft = new DialogGUISpace(30f);
            DialogGUISpace spaceAxisCenter = new DialogGUISpace(115f);
            DialogGUISpace spaceAxisRight = new DialogGUISpace(80f);
            DialogGUISpace spaceTransform = new DialogGUISpace(15f);
            DialogGUIButton buttonReferenceSpace = new DialogGUIButton(GetReferenceSpaceLabel, ReferenceSpaceToggle, 100f, LINE_HEIGHT, false);
            DialogGUILabel labelX = new DialogGUILabel(FormatLabel("X"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelY = new DialogGUILabel(FormatLabel("Y"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelZ = new DialogGUILabel(FormatLabel("Z"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelMinusPlus = new DialogGUILabel(FormatLabel("-/+"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelPosition = new DialogGUILabel(FormatLabel("Position"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelRotation = new DialogGUILabel(FormatLabel("Rotation"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaPosition = new DialogGUILabel(FormatLabel("-/+ Position"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaRotation = new DialogGUILabel(FormatLabel("-/+ Rotation"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUITextInput inputPositionX = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(0, value, referenceSpace); }, delegate { return GetPosition(0, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionY = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(1, value, referenceSpace); }, delegate { return GetPosition(1, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionZ = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(2, value, referenceSpace); }, delegate { return GetPosition(2, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaPosition = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetDeltaPosition(value); }, delegate { return deltaPosition.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationX = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(0, value, referenceSpace); }, delegate { return GetRotation(0, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationY = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(1, value, referenceSpace); }, delegate { return GetRotation(1, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationZ = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(2, value, referenceSpace); }, delegate { return GetRotation(2, referenceSpace); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaRotation = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetDeltaRotation(value); }, delegate { return deltaRotation.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUIButton buttonPosXMinus = new DialogGUIButton("-", delegate { Translate(0, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosXPlus = new DialogGUIButton("+", delegate { Translate(0, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYMinus = new DialogGUIButton("-", delegate { Translate(1, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYPlus = new DialogGUIButton("+", delegate { Translate(1, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZMinus = new DialogGUIButton("-", delegate { Translate(2, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZPlus = new DialogGUIButton("+", delegate { Translate(2, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXMinus = new DialogGUIButton("-", delegate { Rotate(0, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXPlus = new DialogGUIButton("+", delegate { Rotate(0, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYMinus = new DialogGUIButton("-", delegate { Rotate(1, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYPlus = new DialogGUIButton("+", delegate { Rotate(1, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZMinus = new DialogGUIButton("-", delegate { Rotate(2, true, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZPlus = new DialogGUIButton("+", delegate { Rotate(2, false, referenceSpace); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIToggleButton toggleButtonTweakables = new DialogGUIToggleButton(showTweakables, "Tweakables", delegate { ToggleTweakables(); }, -1, LINE_HEIGHT);
            DialogGUIToggleButton toggleButtonColliders = new DialogGUIToggleButton(showColliders, "Colliders", delegate { ToggleColliders(); }, -1, LINE_HEIGHT);
            DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
            DialogGUIButton buttonClose = new DialogGUIButton("Close", delegate { CloseWindow(); }, 140f, LINE_HEIGHT, true);

            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase>
            {
                new DialogGUIHorizontalLayout(buttonReferenceSpace, spaceAxisLeft, labelX, spaceAxisCenter, labelY, spaceAxisCenter, labelZ, spaceAxisRight, labelMinusPlus),
                new DialogGUIHorizontalLayout(labelPosition, buttonPosXMinus, inputPositionX, buttonPosXPlus, spaceTransform, buttonPosYMinus, inputPositionY, buttonPosYPlus, spaceTransform, buttonPosZMinus, inputPositionZ, buttonPosZPlus, spaceTransform, inputDeltaPosition),
                new DialogGUIHorizontalLayout(labelRotation, buttonRotXMinus, inputRotationX, buttonRotXPlus, spaceTransform, buttonRotYMinus, inputRotationY, buttonRotYPlus, spaceTransform, buttonRotZMinus, inputRotationZ, buttonRotZPlus, spaceTransform, inputDeltaRotation),
                new DialogGUIHorizontalLayout(toggleButtonTweakables, toggleButtonColliders)
            };
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(spaceToCenter, buttonClose, spaceToCenter));

            string windowTitle = FormatLabel("Precise Editor - ") + part.partInfo.title;
            dialog = new MultiOptionDialog("partEditionDialog", "", windowTitle, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
            popupDialog.onDestroy.AddListener(RemoveControlLock);

            Color axisColorRed = new Color(1.0f, 0.21f, 0f, 1.0f);
            Color axisColorGreen = new Color(0f, 0.98f, 0f, 1.0f);
            Color axisColorCyan = new Color(0f, 0.85f, 1.0f, 1.0f);
            SetTextInputColor(inputPositionX, axisColorRed);
            SetTextInputColor(inputPositionY, axisColorGreen);
            SetTextInputColor(inputPositionZ, axisColorCyan);
            SetTextInputColor(inputRotationX, axisColorRed);
            SetTextInputColor(inputRotationY, axisColorGreen);
            SetTextInputColor(inputRotationZ, axisColorCyan);

            SetTextInputListeners(inputPositionX);
            SetTextInputListeners(inputPositionY);
            SetTextInputListeners(inputPositionZ);
            SetTextInputListeners(inputRotationX);
            SetTextInputListeners(inputRotationY);
            SetTextInputListeners(inputRotationZ);
            SetTextInputListeners(inputDeltaPosition);
            SetTextInputListeners(inputDeltaRotation);

            if (showTweakables)
            {
                tweakableWindow.Show(part.GetTweakables());
            }

            if (showColliders)
            {
                colliderWindow.Show(part);
            }
        }

        private string GetReferenceSpaceLabel()
        {
            return (referenceSpace == Space.Self) ? "Local" : "Absolute";
        }

        private void ReferenceSpaceToggle()
        {
            referenceSpace = (referenceSpace == Space.World) ? Space.Self : Space.World;
        }

        private bool ToggleTweakables()
        {
            showTweakables = !showTweakables;

            if (showTweakables)
            {
                tweakableWindow.Show(part.GetTweakables());
            }
            else
            {
                tweakableWindow.Hide();
            }

            return showTweakables;
        }

        private bool ToggleColliders()
        {
            showColliders = !showColliders;

            if (showColliders)
            {
                colliderWindow.Show(part);
            }
            else
            {
                colliderWindow.Hide();
            }

            return showColliders;
        }

        private void CloseWindow()
        {
            part = null;
            Hide();
        }

        private bool ValidatePart()
        {
            bool partValid = (part != null);

            if (!partValid)
            {
                Hide();
                tweakableWindow.Hide();
                colliderWindow.Hide();
            }

            return partValid;
        }

        private string GetPartName()
        {
            return part.partInfo.title;
        }

        private string SetPosition(int vectorIndex, string value, Space space)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Vector3 position = space == Space.Self ? part.transform.localPosition : part.transform.position;
            position[vectorIndex] = fValue;
            PartTransform.SetPosition(part, position, space);
            return value;
        }

        private string GetPosition(int vectorIndex, Space space)
        {
            if (space == Space.Self)
            {
                return part.transform.localPosition[vectorIndex].ToString(FORMAT);
            }
            else
            {
                return part.transform.position[vectorIndex].ToString(FORMAT);
            }
        }

        private string SetRotation(int vectorIndex, string value, Space space)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Vector3 partEulerAngles = (space == Space.Self) ? part.transform.localRotation.eulerAngles : part.transform.rotation.eulerAngles;
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = fValue - partEulerAngles[vectorIndex];
            PartTransform.Rotate(part, eulerAngles, space);
            return value;
        }

        private string GetRotation(int vectorIndex, Space space)
        {
            if (space == Space.Self)
            {
                return part.transform.localRotation.eulerAngles[vectorIndex].ToString(FORMAT);
            }
            else
            {
                return part.transform.rotation.eulerAngles[vectorIndex].ToString(FORMAT);
            }
        }

        private string SetDeltaPosition(string value)
        {
            deltaPosition = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            return value;
        }

        private string SetDeltaRotation(string value)
        {
            deltaRotation = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            return value;
        }

        private void Translate(int vectorIndex, bool inverse, Space space)
        {
            float offset = inverse ? -deltaPosition : deltaPosition;
            Transform transform = part.transform;
            float currentValue = (space == Space.Self) ? transform.localPosition[vectorIndex] : transform.position[vectorIndex];
            float newValue = currentValue + offset;
            SetPosition(vectorIndex, newValue.ToString(), space);
        }

        private void Rotate(int vectorIndex, bool inverse, Space space)
        {
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = inverse ? -deltaRotation : deltaRotation;

            PartTransform.Rotate(part, eulerAngles, space);
        }
    }
}