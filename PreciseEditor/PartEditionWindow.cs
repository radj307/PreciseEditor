using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

namespace PreciseEditor
{
    public class PartEditionWindow : MonoBehaviour
    {
        const string FORMAT_POSITION = "F4";
        const string FORMAT_ANGLE    = "F4";
        const string CONTROL_LOCK_ID = "PartEditionWindowLock";
        const float LINE_HEIGHT = 25f;
        const float LINE_SPACER = 4f;

        PopupDialog popupDialog = null;
        protected Rect dialogRect = new Rect(0.5f, 0.5f, 540f, 200f);
        protected Part part = null;
        protected float deltaPosition = 0.2f;
        protected float deltaRotation = 15f;

        public void Show(Part part)
        {
            this.part = part;

            DismissDialog();

            const string MESSAGE = "";
            const int MAXLENGTH = 14;
            const float LABEL_WIDTH = 100;
            const float TRANSFORM_SPACER_WIDTH = 15f;

            Vector3 position = part.transform.position;
            Vector3 localPosition = part.transform.localPosition;
            Quaternion rotation = part.transform.rotation;
            Quaternion localRotation = part.transform.localRotation;

            DialogGUILabel labelAxisLeftSpacer = new DialogGUILabel("", 160f, LINE_HEIGHT);
            DialogGUILabel labelAxisCenterSpacer = new DialogGUILabel("", 115f, LINE_HEIGHT);
            DialogGUILabel labelX = new DialogGUILabel("X", LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelY = new DialogGUILabel("Y", LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelZ = new DialogGUILabel("Z", LINE_HEIGHT, LINE_HEIGHT);
            DialogGUILabel labelPosition = new DialogGUILabel("Absolute Position", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelLocalPosition = new DialogGUILabel("Local Position", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelRotation = new DialogGUILabel("Absolute Rotation", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelLocalRotation = new DialogGUILabel("Local Rotation", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaPosition = new DialogGUILabel("-/+ Position", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelDeltaAngle = new DialogGUILabel("-/+ Angle", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelTransformSpacer = new DialogGUILabel("", TRANSFORM_SPACER_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelCloseButtonSpacer = new DialogGUILabel("", 185f, LINE_HEIGHT);
            DialogGUITextInput inputPositionX = new DialogGUITextInput(position.x.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(0, value, Space.World); }, delegate { return this.GetPosition(0, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionY = new DialogGUITextInput(position.y.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(1, value, Space.World); }, delegate { return this.GetPosition(1, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionZ = new DialogGUITextInput(position.z.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(2, value, Space.World); }, delegate { return this.GetPosition(2, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionX = new DialogGUITextInput(localPosition.x.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(0, value, Space.Self); }, delegate { return this.GetPosition(0, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionY = new DialogGUITextInput(localPosition.y.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(1, value, Space.Self); }, delegate { return this.GetPosition(1, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalPositionZ = new DialogGUITextInput(localPosition.z.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetPosition(2, value, Space.Self); }, delegate { return this.GetPosition(2, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationX = new DialogGUITextInput(rotation.eulerAngles.x.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(0, value, Space.World); }, delegate { return this.GetRotation(0, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationY = new DialogGUITextInput(rotation.eulerAngles.y.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(1, value, Space.World); }, delegate { return this.GetRotation(1, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationZ = new DialogGUITextInput(rotation.eulerAngles.z.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(2, value, Space.World); }, delegate { return this.GetRotation(2, Space.World); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationX = new DialogGUITextInput(localRotation.eulerAngles.x.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(0, value, Space.Self); }, delegate { return this.GetRotation(0, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationY = new DialogGUITextInput(localRotation.eulerAngles.y.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(1, value, Space.Self); }, delegate { return this.GetRotation(1, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputLocalRotationZ = new DialogGUITextInput(localRotation.eulerAngles.z.ToString(FORMAT_ANGLE), false, MAXLENGTH, delegate (string value) { return this.SetRotation(2, value, Space.Self); }, delegate { return this.GetRotation(2, Space.Self); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaPosition = new DialogGUITextInput(this.deltaPosition.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetDeltaPosition(value); }, delegate { return this.deltaPosition.ToString(FORMAT_POSITION); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaRotation = new DialogGUITextInput(this.deltaRotation.ToString(FORMAT_POSITION), false, MAXLENGTH, delegate (string value) { return this.SetDeltaRotation(value); }, delegate { return this.deltaRotation.ToString(FORMAT_POSITION); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);

            DialogGUIButton buttonPosXMinus = new DialogGUIButton("-", delegate { this.Translate(0, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosXPlus = new DialogGUIButton("+", delegate { this.Translate(0, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYMinus = new DialogGUIButton("-", delegate { this.Translate(1, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYPlus = new DialogGUIButton("+", delegate { this.Translate(1, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZMinus = new DialogGUIButton("-", delegate { this.Translate(2, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZPlus = new DialogGUIButton("+", delegate { this.Translate(2, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosXMinus = new DialogGUIButton("-", delegate { this.Translate(0, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosXPlus = new DialogGUIButton("+", delegate { this.Translate(0, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosYMinus = new DialogGUIButton("-", delegate { this.Translate(1, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosYPlus = new DialogGUIButton("+", delegate { this.Translate(1, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosZMinus = new DialogGUIButton("-", delegate { this.Translate(2, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocPosZPlus = new DialogGUIButton("+", delegate { this.Translate(2, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXMinus = new DialogGUIButton("-", delegate { this.Rotate(0, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXPlus = new DialogGUIButton("+", delegate { this.Rotate(0, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYMinus = new DialogGUIButton("-", delegate { this.Rotate(1, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYPlus = new DialogGUIButton("+", delegate { this.Rotate(1, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZMinus = new DialogGUIButton("-", delegate { this.Rotate(2, true, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZPlus = new DialogGUIButton("+", delegate { this.Rotate(2, false, Space.World); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotXMinus = new DialogGUIButton("-", delegate { this.Rotate(0, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotXPlus = new DialogGUIButton("+", delegate { this.Rotate(0, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotYMinus = new DialogGUIButton("-", delegate { this.Rotate(1, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotYPlus = new DialogGUIButton("+", delegate { this.Rotate(1, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotZMinus = new DialogGUIButton("-", delegate { this.Rotate(2, true, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonLocRotZPlus = new DialogGUIButton("+", delegate { this.Rotate(2, false, Space.Self); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUILabel labelColliderBounds = new DialogGUILabel("Collider Bounds", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelColliderBoundsValue = new DialogGUILabel(delegate { return this.GetColliderBounds(); });
            DialogGUILabel labelPartUnderCursor = new DialogGUILabel("Part Under Cursor", LABEL_WIDTH, LINE_HEIGHT);
            DialogGUILabel labelPartUnderCursorValue = new DialogGUILabel(delegate { return this.GetPartUnderCursorValue(); }, 415, LINE_HEIGHT * 2);
            DialogGUIButton buttonClose = new DialogGUIButton("Close Window", delegate { SaveWindowPosition(); }, 140f, LINE_HEIGHT, true);

            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase> {
                new DialogGUIHorizontalLayout(labelAxisLeftSpacer, labelX, labelAxisCenterSpacer, labelY, labelAxisCenterSpacer, labelZ),
                new DialogGUIHorizontalLayout(labelPosition, buttonPosXMinus, inputPositionX, buttonPosXPlus, labelTransformSpacer, buttonPosYMinus, inputPositionY, buttonPosYPlus, labelTransformSpacer, buttonPosZMinus, inputPositionZ, buttonPosZPlus),
                new DialogGUIHorizontalLayout(labelLocalPosition, buttonLocPosXMinus, inputLocalPositionX, buttonLocPosXPlus, labelTransformSpacer, buttonLocPosYMinus, inputLocalPositionY, buttonLocPosYPlus, labelTransformSpacer, buttonLocPosZMinus, inputLocalPositionZ, buttonLocPosZPlus),
                new DialogGUIHorizontalLayout(labelRotation, buttonRotXMinus, inputRotationX, buttonRotXPlus, labelTransformSpacer, buttonRotYMinus, inputRotationY, buttonRotYPlus, labelTransformSpacer, buttonRotZMinus, inputRotationZ, buttonRotZPlus),
                new DialogGUIHorizontalLayout(labelLocalRotation, buttonLocRotXMinus, inputLocalRotationX, buttonLocRotXPlus, labelTransformSpacer, buttonLocRotYMinus, inputLocalRotationY, buttonLocRotYPlus, labelTransformSpacer, buttonLocRotZMinus, inputLocalRotationZ, buttonLocRotZPlus),
                new DialogGUIHorizontalLayout(labelDeltaPosition, inputDeltaPosition, labelTransformSpacer, labelDeltaAngle, inputDeltaRotation)
            };

            List<DialogGUITextInput> partModuleInputList = new List<DialogGUITextInput>();

            foreach (var tweakable in part.GetTweakables())
            {
                var labelField = new DialogGUILabel(tweakable.Label, LABEL_WIDTH, LINE_HEIGHT);
                var inputField = new DialogGUITextInput(
                    tweakable.GetValue(),
                    false, MAXLENGTH,
                    value => tweakable.SetValueWithSymmetry(value),
                    () => tweakable.GetValue(),
                    TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
                dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelField, inputField));
                partModuleInputList.Add(inputField);
            }

            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelColliderBounds, labelColliderBoundsValue));
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelPartUnderCursor, labelPartUnderCursorValue));
            dialogGUIBaseList.Add(new DialogGUIFlexibleSpace());
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelCloseButtonSpacer, buttonClose));

            MultiOptionDialog dialog = new MultiOptionDialog("partEditionDialog", MESSAGE, GetWindowTitle(), HighLogic.UISkin, this.dialogRect, new DialogGUIFlexibleSpace(),
                new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));

            this.popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            this.popupDialog.OnDismiss = this.SaveWindowPosition;
            this.popupDialog.onDestroy.AddListener(this.OnPopupDialogDestroy);

            TMP_InputField tmp_InputPositionX = inputPositionX.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputPositionY = inputPositionY.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputPositionZ = inputPositionZ.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalPositionX = inputLocalPositionX.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalPositionY = inputLocalPositionY.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalPositionZ = inputLocalPositionZ.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputRotationX = inputRotationX.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputRotationY = inputRotationY.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputRotationZ = inputRotationZ.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalRotationX = inputLocalRotationX.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalRotationY = inputLocalRotationY.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputLocalRotationZ = inputLocalRotationZ.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputDeltaPosition = inputDeltaPosition.uiItem.GetComponent<TMP_InputField>();
            TMP_InputField tmp_InputDeltaRotation = inputDeltaRotation.uiItem.GetComponent<TMP_InputField>();

            tmp_InputPositionX.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputPositionX.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputPositionY.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputPositionY.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputPositionZ.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputPositionZ.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));

            tmp_InputLocalPositionX.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalPositionX.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputLocalPositionY.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalPositionY.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputLocalPositionZ.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalPositionZ.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));

            tmp_InputRotationX.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputRotationX.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputRotationY.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputRotationY.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputRotationZ.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputRotationZ.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));

            tmp_InputLocalRotationX.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalRotationX.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputLocalRotationY.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalRotationY.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputLocalRotationZ.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputLocalRotationZ.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));

            tmp_InputDeltaPosition.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputDeltaPosition.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            tmp_InputDeltaRotation.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
            tmp_InputDeltaRotation.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));

            foreach (DialogGUITextInput partModuleInput in partModuleInputList)
            {
                TMP_InputField tmp_InputField = partModuleInput.uiItem.GetComponent<TMP_InputField>();
                tmp_InputField.onSelect.AddListener(new UnityAction<string>(this.OnSelectTextInput));
                tmp_InputField.onDeselect.AddListener(new UnityAction<string>(this.OnDeselectTextInput));
            }
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
            if (this.popupDialog)
            {
                Vector3 dialogPosition = this.popupDialog.RTrf.position;
                this.dialogRect.x = dialogPosition.x / Screen.width + 0.5f;
                this.dialogRect.y = dialogPosition.y / Screen.height + 0.5f;
            }
        }

        private void DismissDialog()
        {
            if (this.popupDialog)
            {
                this.SaveWindowPosition();
                this.popupDialog.Dismiss();
            }
        }

        private bool ValidatePart()
        {
            bool partValid = (this.part != null);

            if (!partValid)
            {
                this.DismissDialog();
            }

            return partValid;
        }

        private string GetWindowTitle()
        {
            return "Precise Editor - " + this.part.partInfo.title;
        }

        private string GetPartName()
        {
            if (!this.ValidatePart())
            {
                return "";
            }

            return this.part.partInfo.title;
        }

        private string GetColliderBounds()
        {
            if (!this.ValidatePart())
            {
                return "";
            }

            string center = this.part.collider.bounds.center.ToString(FORMAT_POSITION);
            string extents = this.part.collider.bounds.extents.ToString(FORMAT_POSITION);
            string size = this.part.collider.bounds.size.ToString(FORMAT_POSITION);

            return "Center: " + center + "    Extents: " + extents + "\nSize: " + size;
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

        private string GetPartUnderCursorValue()
        {
            if (!this.ValidatePart())
            {
                return "";
            }

            Part targetPart = PreciseEditor.GetPartUnderCursor();

            if (targetPart)
            {
                Vector3 distanceToPart = targetPart.transform.position - part.transform.position;
                Bounds srcBounds = part.collider.bounds;
                Bounds targetBounds = targetPart.collider.bounds;
                Vector3 distanceToCollider = GetDistanceBetweenBounds(srcBounds, targetBounds);
                bool boundsIntersect = srcBounds.Intersects(targetBounds);

                string partUnderCursorValue = targetPart.partInfo.title + "\n";
                partUnderCursorValue += "Distance To Part: " + distanceToPart.ToString(FORMAT_POSITION) + "\n";
                partUnderCursorValue += "Distance Between Collider Bounds: " + distanceToCollider.ToString(FORMAT_POSITION) + "\n";
                partUnderCursorValue += "Collider Bounds Intersects: " + boundsIntersect.ToString();

                return partUnderCursorValue;
            }

            return "";
        }

        private string SetPosition(int vectorIndex, string value, Space space)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Vector3 position = space == Space.Self ? this.part.transform.localPosition : this.part.transform.position;
            position[vectorIndex] = fValue;
            PartTransform.SetPosition(this.part, position, space);
            return value;
        }

        private string GetPosition(int vectorIndex, Space space)
        {
            if (!this.ValidatePart())
            {
                return "";
            }
            if (space == Space.Self)
            {
                return this.part.transform.localPosition[vectorIndex].ToString(FORMAT_POSITION);
            }
            else
            {
                return this.part.transform.position[vectorIndex].ToString(FORMAT_POSITION);
            }
        }

        private string SetRotation(int vectorIndex, string value, Space space)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Vector3 partEulerAngles = (space == Space.Self) ? this.part.transform.localRotation.eulerAngles : this.part.transform.rotation.eulerAngles;
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = fValue - partEulerAngles[vectorIndex];
            PartTransform.Rotate(this.part, eulerAngles, space);
            return value;
        }

        private string GetRotation(int vectorIndex, Space space)
        {
            if (!this.ValidatePart())
            {
                return "";
            }
            if (space == Space.Self)
            {
                return this.part.transform.localRotation.eulerAngles[vectorIndex].ToString(FORMAT_ANGLE);
            }
            else
            {
                return this.part.transform.rotation.eulerAngles[vectorIndex].ToString(FORMAT_ANGLE);
            }
        }

        private string SetDeltaPosition(string value)
        {
            this.deltaPosition = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            return value;
        }

        private string SetDeltaRotation(string value)
        {
            this.deltaRotation = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            return value;
        }

        private void Translate(int vectorIndex, bool inverse, Space space)
        {
            float offset = inverse ? -this.deltaPosition : this.deltaPosition;
            Transform transform = this.part.transform;
            float currentValue = (space == Space.Self) ? transform.localPosition[vectorIndex] : transform.position[vectorIndex];
            float newValue = currentValue + offset;
            this.SetPosition(vectorIndex, newValue.ToString(), space);
        }

        private void Rotate(int vectorIndex, bool inverse, Space space)
        {
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = inverse ? -this.deltaRotation : this.deltaRotation;

            PartTransform.Rotate(this.part, eulerAngles, space);
        }
    }
}