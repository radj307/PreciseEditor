using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

namespace PreciseEditor
{
    public class PartEditionWindow : BaseWindow
    {
        const int MAXLENGTH = 8;
        const float LABEL_WIDTH = 100;
        const float LINE_HEIGHT = 25f;
        const string FORMAT = "F4";

        private float deltaPosition = 0.2f;
        private float deltaRotation = 15f;
        private Part part = null;
        private bool showTweakables = false;
        private bool showColliders = false;
        private TweakableWindow tweakableWindow = null;
        private ColliderWindow colliderWindow = null;

        public PartEditionWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 540f, 200f);
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

            Vector3 position = part.transform.position;
            Vector3 localPosition = part.transform.localPosition;
            Quaternion rotation = part.transform.rotation;
            Quaternion localRotation = part.transform.localRotation;

            DialogGUISpace spaceAxisLeft = new DialogGUISpace(160f);
            DialogGUISpace spaceAxisCenter = new DialogGUISpace(115f);
            DialogGUILabel labelX = new DialogGUILabel(FormatLabel("X"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelY = new DialogGUILabel(FormatLabel("Y"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelZ = new DialogGUILabel(FormatLabel("Z"), LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelPosition = new DialogGUILabel(FormatLabel("Absolute Position"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelLocalPosition = new DialogGUILabel(FormatLabel("Local Position"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelRotation = new DialogGUILabel(FormatLabel("Absolute Rotation"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelLocalRotation = new DialogGUILabel(FormatLabel("Local Rotation"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaPosition = new DialogGUILabel(FormatLabel("-/+ Position"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaRotation = new DialogGUILabel(FormatLabel("-/+ Rotation"), LABEL_WIDTH, LINE_HEIGHT);
            DialogGUISpace spaceTransform = new DialogGUISpace(15f);
            DialogGUITextInput inputPositionX = new DialogGUITextInput(position.x.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(0, value, Space.World); }, delegate { return GetPosition(0, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionY = new DialogGUITextInput(position.y.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(1, value, Space.World); }, delegate { return GetPosition(1, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionZ = new DialogGUITextInput(position.z.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(2, value, Space.World); }, delegate { return GetPosition(2, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionX = new DialogGUITextInput(localPosition.x.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(0, value, Space.Self); }, delegate { return GetPosition(0, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionY = new DialogGUITextInput(localPosition.y.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(1, value, Space.Self); }, delegate { return GetPosition(1, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionZ = new DialogGUITextInput(localPosition.z.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetPosition(2, value, Space.Self); }, delegate { return GetPosition(2, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationX = new DialogGUITextInput(rotation.eulerAngles.x.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(0, value, Space.World); }, delegate { return GetRotation(0, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationY = new DialogGUITextInput(rotation.eulerAngles.y.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(1, value, Space.World); }, delegate { return GetRotation(1, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationZ = new DialogGUITextInput(rotation.eulerAngles.z.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(2, value, Space.World); }, delegate { return GetRotation(2, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationX = new DialogGUITextInput(localRotation.eulerAngles.x.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(0, value, Space.Self); }, delegate { return GetRotation(0, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationY = new DialogGUITextInput(localRotation.eulerAngles.y.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(1, value, Space.Self); }, delegate { return GetRotation(1, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationZ = new DialogGUITextInput(localRotation.eulerAngles.z.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetRotation(2, value, Space.Self); }, delegate { return GetRotation(2, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaPosition = new DialogGUITextInput(deltaPosition.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetDeltaPosition(value); }, delegate { return deltaPosition.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaRotation = new DialogGUITextInput(deltaRotation.ToString(FORMAT), false, MAXLENGTH, delegate (string value) { return SetDeltaRotation(value); }, delegate { return deltaRotation.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUIButton buttonPosXMinus = new DialogGUIButton("-", delegate { Translate(0, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosXPlus = new DialogGUIButton("+", delegate { Translate(0, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYMinus = new DialogGUIButton("-", delegate { Translate(1, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYPlus = new DialogGUIButton("+", delegate { Translate(1, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZMinus = new DialogGUIButton("-", delegate { Translate(2, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZPlus = new DialogGUIButton("+", delegate { Translate(2, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosXMinus = new DialogGUIButton("-", delegate { Translate(0, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosXPlus = new DialogGUIButton("+", delegate { Translate(0, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosYMinus = new DialogGUIButton("-", delegate { Translate(1, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosYPlus = new DialogGUIButton("+", delegate { Translate(1, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosZMinus = new DialogGUIButton("-", delegate { Translate(2, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosZPlus = new DialogGUIButton("+", delegate { Translate(2, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXMinus = new DialogGUIButton("-", delegate { Rotate(0, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXPlus = new DialogGUIButton("+", delegate { Rotate(0, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYMinus = new DialogGUIButton("-", delegate { Rotate(1, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYPlus = new DialogGUIButton("+", delegate { Rotate(1, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZMinus = new DialogGUIButton("-", delegate { Rotate(2, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZPlus = new DialogGUIButton("+", delegate { Rotate(2, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotXMinus = new DialogGUIButton("-", delegate { Rotate(0, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotXPlus = new DialogGUIButton("+", delegate { Rotate(0, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotYMinus = new DialogGUIButton("-", delegate { Rotate(1, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotYPlus = new DialogGUIButton("+", delegate { Rotate(1, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotZMinus = new DialogGUIButton("-", delegate { Rotate(2, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotZPlus = new DialogGUIButton("+", delegate { Rotate(2, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIToggleButton toggleButtonTweakables = new DialogGUIToggleButton(showTweakables, "Tweakables", delegate { ToggleTweakables(); }, -1, LINE_HEIGHT);
            DialogGUIToggleButton toggleButtonColliders = new DialogGUIToggleButton(showColliders, "Colliders", delegate { ToggleColliders(); }, -1, LINE_HEIGHT);
            DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
            DialogGUIButton buttonClose = new DialogGUIButton("Close Window", delegate { CloseWindow(); }, 140f, LINE_HEIGHT, true);

            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase>
            {
                new DialogGUIHorizontalLayout(spaceAxisLeft, labelX, spaceAxisCenter, labelY, spaceAxisCenter, labelZ),
                new DialogGUIHorizontalLayout(labelPosition, buttonPosXMinus, inputPositionX, buttonPosXPlus, spaceTransform, buttonPosYMinus, inputPositionY, buttonPosYPlus, spaceTransform, buttonPosZMinus, inputPositionZ, buttonPosZPlus),
                new DialogGUIHorizontalLayout(labelLocalPosition, buttonLocPosXMinus, inputLocalPositionX, buttonLocPosXPlus, spaceTransform, buttonLocPosYMinus, inputLocalPositionY, buttonLocPosYPlus, spaceTransform, buttonLocPosZMinus, inputLocalPositionZ, buttonLocPosZPlus),
                new DialogGUIHorizontalLayout(labelRotation, buttonRotXMinus, inputRotationX, buttonRotXPlus, spaceTransform, buttonRotYMinus, inputRotationY, buttonRotYPlus, spaceTransform, buttonRotZMinus, inputRotationZ, buttonRotZPlus),
                new DialogGUIHorizontalLayout(labelLocalRotation, buttonLocRotXMinus, inputLocalRotationX, buttonLocRotXPlus, spaceTransform, buttonLocRotYMinus, inputLocalRotationY, buttonLocRotYPlus, spaceTransform, buttonLocRotZMinus, inputLocalRotationZ, buttonLocRotZPlus),
                new DialogGUIHorizontalLayout(labelDeltaPosition, inputDeltaPosition, spaceTransform, labelDeltaRotation, inputDeltaRotation),
                new DialogGUIHorizontalLayout(toggleButtonTweakables, toggleButtonColliders)
            };
            dialogGUIBaseList.Add(new DialogGUIFlexibleSpace());
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(spaceToCenter, buttonClose, spaceToCenter));

            string windowTitle = FormatLabel("Precise Editor - ") + part.partInfo.title;
            dialog = new MultiOptionDialog("partEditionDialog", "", windowTitle, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
            popupDialog.onDestroy.AddListener(RemoveControlLock);

            SetTextInputListeners(inputPositionX);
            SetTextInputListeners(inputPositionY);
            SetTextInputListeners(inputPositionZ);
            SetTextInputListeners(inputLocalPositionX);
            SetTextInputListeners(inputLocalPositionY);
            SetTextInputListeners(inputLocalPositionZ);
            SetTextInputListeners(inputRotationX);
            SetTextInputListeners(inputRotationY);
            SetTextInputListeners(inputRotationZ);
            SetTextInputListeners(inputLocalRotationX);
            SetTextInputListeners(inputLocalRotationY);
            SetTextInputListeners(inputLocalRotationZ);
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