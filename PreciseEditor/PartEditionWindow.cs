using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

namespace PreciseEditor
{
    public class PartEditionWindow : MonoBehaviour
    {
        const int MAXLENGTH = 8;
        const float LABEL_WIDTH = 100;
        const float LINE_HEIGHT = 25f;
        const string FORMAT = "F4";
        const string CONTROL_LOCK_ID = "PartEditionWindowLock";

        private PopupDialog popupDialog = null;
        private Rect dialogRect = new Rect(0.5f, 0.5f, 540f, 200f);
        private float deltaPosition = 0.2f;
        private float deltaRotation = 15f;
        private Part part = null;
        private Part partUnderCursor = null;

        public void Update()
        {
            if (popupDialog != null)
            {
                bool partValid = ValidatePart();

                if (partValid)
                {
                    partUnderCursor = PreciseEditor.GetPartUnderCursor();
                }
            }
        }

        public void Show(Part part)
        {
            Vector3 position = part.transform.position;
            Vector3 localPosition = part.transform.localPosition;
            Quaternion rotation = part.transform.rotation;
            Quaternion localRotation = part.transform.localRotation;

            this.part = part;
            DismissDialog();

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
            DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
            DialogGUIButton buttonClose = new DialogGUIButton("Close Window", delegate { SaveWindowPosition(); }, 140f, LINE_HEIGHT, true);

            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase>
            {
                new DialogGUIHorizontalLayout(spaceAxisLeft, labelX, spaceAxisCenter, labelY, spaceAxisCenter, labelZ),
                new DialogGUIHorizontalLayout(labelPosition, buttonPosXMinus, inputPositionX, buttonPosXPlus, spaceTransform, buttonPosYMinus, inputPositionY, buttonPosYPlus, spaceTransform, buttonPosZMinus, inputPositionZ, buttonPosZPlus),
                new DialogGUIHorizontalLayout(labelLocalPosition, buttonLocPosXMinus, inputLocalPositionX, buttonLocPosXPlus, spaceTransform, buttonLocPosYMinus, inputLocalPositionY, buttonLocPosYPlus, spaceTransform, buttonLocPosZMinus, inputLocalPositionZ, buttonLocPosZPlus),
                new DialogGUIHorizontalLayout(labelRotation, buttonRotXMinus, inputRotationX, buttonRotXPlus, spaceTransform, buttonRotYMinus, inputRotationY, buttonRotYPlus, spaceTransform, buttonRotZMinus, inputRotationZ, buttonRotZPlus),
                new DialogGUIHorizontalLayout(labelLocalRotation, buttonLocRotXMinus, inputLocalRotationX, buttonLocRotXPlus, spaceTransform, buttonLocRotYMinus, inputLocalRotationY, buttonLocRotYPlus, spaceTransform, buttonLocRotZMinus, inputLocalRotationZ, buttonLocRotZPlus),
                new DialogGUIHorizontalLayout(labelDeltaPosition, inputDeltaPosition, spaceTransform, labelDeltaRotation, inputDeltaRotation)
            };

            List<DialogGUITextInput> partModuleInputList = new List<DialogGUITextInput>();

            foreach (var tweakable in part.GetTweakables())
            {
                var labelField = new DialogGUILabel(FormatLabel(tweakable.Label), LABEL_WIDTH, LINE_HEIGHT);
                var inputField = new DialogGUITextInput(
                    tweakable.GetValue(),
                    false, MAXLENGTH,
                    value => tweakable.SetValueWithSymmetry(value),
                    () => tweakable.GetValue(),
                    TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
                dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelField, inputField));
                partModuleInputList.Add(inputField);
            }

            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(GetBoxColliderBoundsDialogGUIBox(), GetPartUnderCursorDialogGUIBox()));
            dialogGUIBaseList.Add(new DialogGUIFlexibleSpace());
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(spaceToCenter, buttonClose, spaceToCenter));

            string windowTitle = FormatLabel("Precise Editor - ") + part.partInfo.title;
            MultiOptionDialog dialog = new MultiOptionDialog("partEditionDialog", "", windowTitle, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.OnDismiss = SaveWindowPosition;
            popupDialog.onDestroy.AddListener(OnPopupDialogDestroy);

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

            foreach (DialogGUITextInput partModuleInput in partModuleInputList)
            {
                SetTextInputListeners(partModuleInput);
            }
        }

        private DialogGUIBox GetBoxColliderBoundsDialogGUIBox()
        {
            DialogGUILabel labelColliderBounds = new DialogGUILabel(FormatLabel("Collider Bounds"), 100, 16);
            DialogGUILabel labelColliderCenter = new DialogGUILabel(FormatLabel("Center"), 50, 16);
            DialogGUILabel labelColliderCenterValue = new DialogGUILabel(delegate { return GetColliderCenter(); });
            DialogGUILabel labelColliderExtents = new DialogGUILabel(FormatLabel("Extents"), 50, 16);
            DialogGUILabel labelColliderExtentsValue = new DialogGUILabel(delegate { return GetColliderExtents(); });
            DialogGUILabel labelColliderSize = new DialogGUILabel(FormatLabel("Size"), 50, 16);
            DialogGUILabel labelColliderSizeValue = new DialogGUILabel(delegate { return GetColliderSize(); });

            DialogGUIVerticalLayout layoutColliderBounds = new DialogGUIVerticalLayout
            {
                children = {
                    labelColliderBounds,
                    new DialogGUIHorizontalLayout(labelColliderCenter, labelColliderCenterValue),
                    new DialogGUIHorizontalLayout(labelColliderExtents, labelColliderExtentsValue),
                    new DialogGUIHorizontalLayout(labelColliderSize, labelColliderSizeValue)
                },
                padding = new RectOffset(5, 5, 5, 5)
            };

            return new DialogGUIBox("", 217, 90, null, layoutColliderBounds);
        }

        private DialogGUIBox GetPartUnderCursorDialogGUIBox()
        {
            DialogGUILabel labelPartUnderCursor = new DialogGUILabel(delegate { return GetPartUnderCursorTitle(); }, 297, 16);
            DialogGUILabel labelDistanceToPart = new DialogGUILabel(FormatLabel("Distance To Part"), 150, 16);
            DialogGUILabel labelDistanceToPartValue = new DialogGUILabel(delegate { return GetDistanceToPart(); });
            DialogGUILabel labelDistanceBetweenColliders = new DialogGUILabel(FormatLabel("Distance Between Colliders"), 150, 16);
            DialogGUILabel labelDistanceBetweenCollidersValue = new DialogGUILabel(delegate { return GetDistanceBetweenColliders(); });
            DialogGUILabel labelColliderBoundsIntersects = new DialogGUILabel(FormatLabel("Collider Bounds Intersects"), 150, 16);
            DialogGUILabel labelColliderBoundsIntersectsValue = new DialogGUILabel(delegate { return GetColliderBoundsIntersects(); });

            DialogGUIVerticalLayout layoutPartUnderCursor = new DialogGUIVerticalLayout
            {
                children = {
                    new DialogGUIHorizontalLayout(labelPartUnderCursor),
                    new DialogGUIHorizontalLayout(labelDistanceToPart, labelDistanceToPartValue),
                    new DialogGUIHorizontalLayout(labelDistanceBetweenColliders, labelDistanceBetweenCollidersValue),
                    new DialogGUIHorizontalLayout(labelColliderBoundsIntersects, labelColliderBoundsIntersectsValue)
                },
                padding = new RectOffset(5, 5, 5, 5)
            };

            return new DialogGUIBox("", 307, 90, null, layoutPartUnderCursor);
        }

        private void SetTextInputListeners(DialogGUITextInput textInput)
        {
            TMP_InputField tmp_Input = textInput.uiItem.GetComponent<TMP_InputField>();
            tmp_Input.onSelect.AddListener(new UnityAction<string>(OnSelectTextInput));
            tmp_Input.onDeselect.AddListener(new UnityAction<string>(OnDeselectTextInput));
        }

        private void OnPopupDialogDestroy()
        {
            InputLockManager.RemoveControlLock(CONTROL_LOCK_ID);
        }

        private void OnSelectTextInput(string s)
        {
            InputLockManager.SetControlLock(ControlTypes.EDITOR_GIZMO_TOOLS | ControlTypes.EDITOR_ROOT_REFLOW, CONTROL_LOCK_ID);
        }

        private void OnDeselectTextInput(string s)
        {
            InputLockManager.RemoveControlLock(CONTROL_LOCK_ID);
        }

        private void SaveWindowPosition()
        {
            if (popupDialog)
            {
                RectTransform rectTransform = popupDialog.GetComponent<RectTransform>();
                Vector3 position = rectTransform.position / GameSettings.UI_SCALE;
                dialogRect.x = position.x / Screen.width + 0.5f;
                dialogRect.y = position.y / Screen.height + 0.5f;
            }
        }

        private void DismissDialog()
        {
            if (popupDialog)
            {
                SaveWindowPosition();
                popupDialog.Dismiss();
            }
        }

        private bool ValidatePart()
        {
            bool partValid = (part != null);

            if (!partValid)
            {
                DismissDialog();
            }

            return partValid;
        }

        private string GetPartName()
        {
            return part.partInfo.title;
        }

        private string GetColliderCenter()
        {
            return part.collider.bounds.center.ToString(FORMAT);
        }

        private string GetColliderExtents()
        {
            return part.collider.bounds.extents.ToString(FORMAT);
        }

        private string GetColliderSize()
        {
            return part.collider.bounds.size.ToString(FORMAT);
        }

        private float GetDistanceBetweenIntervals(float center1, float min1, float max1, float center2, float min2, float max2)
        {
            float distance = ((center2 - center1) > 0) ? (min2 - max1) : (min1 - max2);

            return (distance < 0) ? 0 : distance;
        }

        private Vector3 GetDistanceBetweenBounds(Bounds bounds1, Bounds bounds2)
        {
            float distanceX = GetDistanceBetweenIntervals(bounds1.center.x, bounds1.min.x, bounds1.max.x, bounds2.center.x, bounds2.min.x, bounds2.max.x);
            float distanceY = GetDistanceBetweenIntervals(bounds1.center.y, bounds1.min.y, bounds1.max.y, bounds2.center.y, bounds2.min.y, bounds2.max.y);
            float distanceZ = GetDistanceBetweenIntervals(bounds1.center.z, bounds1.min.z, bounds1.max.z, bounds2.center.z, bounds2.min.z, bounds2.max.z);

            return new Vector3(distanceX, distanceY, distanceZ);
        }

        private string GetPartUnderCursorTitle()
        {
            return partUnderCursor ? partUnderCursor.partInfo.title : "";
        }

        private string GetDistanceToPart()
        {
            return partUnderCursor ? (partUnderCursor.transform.position - part.transform.position).ToString(FORMAT) : "";
        }

        private string GetDistanceBetweenColliders()
        {
            return partUnderCursor ? GetDistanceBetweenBounds(part.collider.bounds, partUnderCursor.collider.bounds).ToString(FORMAT) : "";
        }

        private string GetColliderBoundsIntersects()
        {
            return partUnderCursor ? part.collider.bounds.Intersects(partUnderCursor.collider.bounds).ToString() : "";
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

        private string FormatLabel(string label)
        {
            return "<color=\"white\">" + label + "</color>";
        }
    }
}