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
        private Part newPart = null;
        private Vector3 position;
        private Quaternion rotation;
        private Space referenceSpace = Space.World;
        private bool compoundTargetSelected = false;
        private bool showAttachment = false;
        private bool showColliders = false;
        private AttachmentWindow attachmentWindow = null;
        private ColliderWindow colliderWindow = null;
        private AxisLines axisLines = null;

        public PartEditionWindow()
        {
            dialogRect = new Rect(0.5f, 0.75f, 600f, 175f);
        }

        public void Start()
        {
            attachmentWindow = gameObject.AddComponent<AttachmentWindow>();
            colliderWindow = gameObject.AddComponent<ColliderWindow>();
            axisLines = gameObject.AddComponent<AxisLines>();
        }

        public void Update()
        {
            if (IsVisible() || attachmentWindow.IsVisible() || colliderWindow.IsVisible())
            {
                if (ValidatePart())
                {
                    position = PartUtil.GetPosition(part, referenceSpace, compoundTargetSelected);
                    rotation = PartUtil.GetRotation(part, referenceSpace, compoundTargetSelected);
                    axisLines.UpdateAxis(part, referenceSpace, compoundTargetSelected);
                }
            }
        }

        public void Show(Part part)
        {
            if (IsVisible())
            {
                Hide();
                newPart = part;
                return;
            }

            this.part = part;

            DialogGUISpace spaceAxisLeft = new DialogGUISpace(30f);
            DialogGUISpace spaceAxisCenter = new DialogGUISpace(115f);
            DialogGUISpace spaceAxisRight = new DialogGUISpace(80f);
            DialogGUISpace spaceTransform = new DialogGUISpace(15f);
            DialogGUIButton buttonReferenceSpace = new DialogGUIButton(GetReferenceSpaceLabel, ToggleReferenceSpace, 100f, LINE_HEIGHT, false);
            DialogGUILabel labelX = new DialogGUILabel(FormatLabel("X"), LINE_HEIGHT);
            DialogGUILabel labelY = new DialogGUILabel(FormatLabel("Y"), LINE_HEIGHT);
            DialogGUILabel labelZ = new DialogGUILabel(FormatLabel("Z"), LINE_HEIGHT);
            DialogGUILabel labelMinusPlus = new DialogGUILabel(FormatLabel("-/+"), LINE_HEIGHT);
            DialogGUILabel labelPosition = new DialogGUILabel(FormatLabel("Position"), LABEL_WIDTH);
            DialogGUILabel labelRotation = new DialogGUILabel(FormatLabel("Rotation"), LABEL_WIDTH);
            DialogGUITextInput inputPositionX = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(0, value); }, delegate { return GetPosition(0); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionY = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(1, value); }, delegate { return GetPosition(1); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputPositionZ = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetPosition(2, value); }, delegate { return GetPosition(2); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaPosition = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetDeltaPosition(value); }, delegate { return deltaPosition.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationX = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(0, value); }, delegate { return GetRotation(0); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationY = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(1, value); }, delegate { return GetRotation(1); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputRotationZ = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetRotation(2, value); }, delegate { return GetRotation(2); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUITextInput inputDeltaRotation = new DialogGUITextInput("", false, MAXLENGTH, delegate (string value) { return SetDeltaRotation(value); }, delegate { return deltaRotation.ToString(FORMAT); }, TMP_InputField.ContentType.DecimalNumber, LINE_HEIGHT);
            DialogGUIButton buttonPosXMinus = new DialogGUIButton("-", delegate { Translate(0, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosXPlus = new DialogGUIButton("+", delegate { Translate(0, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYMinus = new DialogGUIButton("-", delegate { Translate(1, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosYPlus = new DialogGUIButton("+", delegate { Translate(1, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZMinus = new DialogGUIButton("-", delegate { Translate(2, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonPosZPlus = new DialogGUIButton("+", delegate { Translate(2, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXMinus = new DialogGUIButton("-", delegate { Rotate(0, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotXPlus = new DialogGUIButton("+", delegate { Rotate(0, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYMinus = new DialogGUIButton("-", delegate { Rotate(1, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotYPlus = new DialogGUIButton("+", delegate { Rotate(1, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZMinus = new DialogGUIButton("-", delegate { Rotate(2, true); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIButton buttonRotZPlus = new DialogGUIButton("+", delegate { Rotate(2, false); }, LINE_HEIGHT, LINE_HEIGHT, false);
            DialogGUIToggleButton toggleButtonAttachment = new DialogGUIToggleButton(showAttachment, "Attachment Rules", delegate { ToggleAttachment(); }, -1, LINE_HEIGHT);
            DialogGUIToggleButton toggleButtonColliders = new DialogGUIToggleButton(showColliders, "Colliders", delegate { ToggleColliders(); }, -1, LINE_HEIGHT);
            DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
            DialogGUIButton buttonClose = new DialogGUIButton("Close", delegate { CloseWindow(); }, 140f, LINE_HEIGHT, true);

            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase>
            {
                new DialogGUIHorizontalLayout(TextAnchor.MiddleCenter, buttonReferenceSpace, spaceAxisLeft, labelX, spaceAxisCenter, labelY, spaceAxisCenter, labelZ, spaceAxisRight, labelMinusPlus),
                new DialogGUIHorizontalLayout(TextAnchor.MiddleCenter, labelPosition, buttonPosXMinus, inputPositionX, buttonPosXPlus, spaceTransform, buttonPosYMinus, inputPositionY, buttonPosYPlus, spaceTransform, buttonPosZMinus, inputPositionZ, buttonPosZPlus, spaceTransform, inputDeltaPosition),
                new DialogGUIHorizontalLayout(TextAnchor.MiddleCenter, labelRotation, buttonRotXMinus, inputRotationX, buttonRotXPlus, spaceTransform, buttonRotYMinus, inputRotationY, buttonRotYPlus, spaceTransform, buttonRotZMinus, inputRotationZ, buttonRotZPlus, spaceTransform, inputDeltaRotation)
            };
            if (part.isCompund)
            {
                DialogGUILabel labelCompound = new DialogGUILabel(FormatLabel("Anchor"), LABEL_WIDTH);
                DialogGUIButton buttonCompound = new DialogGUIButton(GetCompoundLabel, ToggleCompound, 100f, LINE_HEIGHT, false);
                dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleCenter, labelCompound, buttonCompound));
            }
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(toggleButtonAttachment, toggleButtonColliders));
            dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(spaceToCenter, buttonClose, spaceToCenter));

            string windowTitle = FormatLabel("Precise Editor - ") + part.partInfo.title;
            dialog = new MultiOptionDialog("partEditionDialog", "", windowTitle, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
            popupDialog.onDestroy.AddListener(RemoveControlLock);
            popupDialog.onDestroy.AddListener(OnPopupDialogDestroy);

            SetTextInputColor(inputPositionX, axisLines.red);
            SetTextInputColor(inputPositionY, axisLines.green);
            SetTextInputColor(inputPositionZ, axisLines.cyan);
            SetTextInputColor(inputRotationX, axisLines.red);
            SetTextInputColor(inputRotationY, axisLines.green);
            SetTextInputColor(inputRotationZ, axisLines.cyan);

            SetTextInputListeners(inputPositionX);
            SetTextInputListeners(inputPositionY);
            SetTextInputListeners(inputPositionZ);
            SetTextInputListeners(inputRotationX);
            SetTextInputListeners(inputRotationY);
            SetTextInputListeners(inputRotationZ);
            SetTextInputListeners(inputDeltaPosition);
            SetTextInputListeners(inputDeltaRotation);

            if (showAttachment)
            {
                attachmentWindow.Show(part.attachRules);
            }

            if (showColliders)
            {
                colliderWindow.Show(part);
            }

            axisLines.Show(part, referenceSpace, compoundTargetSelected);
        }

        private void OnPopupDialogDestroy()
        {
            axisLines.Hide();
            if (newPart)
            {
                Show(newPart);
                newPart = null;
            }
        }

        private string GetReferenceSpaceLabel()
        {
            return (referenceSpace == Space.Self) ? "Local" : "Absolute";
        }

        private void ToggleReferenceSpace()
        {
            referenceSpace = (referenceSpace == Space.World) ? Space.Self : Space.World;
        }

        private string GetCompoundLabel()
        {
            return compoundTargetSelected ? "Target" : "Source";
        }

        private void ToggleCompound()
        {
            CompoundPart compoundPart = (CompoundPart)part;
            if (compoundTargetSelected || compoundPart.target != null)
            {
                compoundTargetSelected = !compoundTargetSelected;
            }
        }

        private bool ToggleAttachment()
        {
            showAttachment = !showAttachment;

            if (showAttachment)
            {
                attachmentWindow.Show(part.attachRules);
            }
            else
            {
                attachmentWindow.Hide();
            }

            return showAttachment;
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
            axisLines.Hide();
            attachmentWindow.Hide();
            colliderWindow.Hide();
            Hide();
        }

        private bool ValidatePart()
        {
            bool partValid = (part != null);

            if (!partValid)
            {
                axisLines.Hide();
                attachmentWindow.Hide();
                colliderWindow.Hide();
                Hide();
            }

            return partValid;
        }

        private string GetPartName()
        {
            return part.partInfo.title;
        }

        private GameObject FindChildGameObjectByName(GameObject parent, string name)
        {
            Transform[] transforms = parent.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform transform in transforms)
            {
                if (transform.gameObject.name == name)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        private string SetPosition(int vectorIndex, string value)
        {
            Vector3 newPosition = GetWorldPositionToBeSet(vectorIndex, value);
            Vector3 oldPosition = PartUtil.GetPosition(part, Space.World, compoundTargetSelected);
            Bounds partBounds;

            if (PartUtil.IsTargetActive(part, compoundTargetSelected))
            {
                partBounds = FindChildGameObjectByName(part.gameObject, "obj_targetCollider").GetComponent<Collider>().bounds;
            }
            else
            {
                partBounds = part.collider.bounds;
            }

            if (!AreBoundsOutOfHangarBounds(partBounds))
            {
                Vector3 boundsOffset = partBounds.center - oldPosition;

                partBounds.center = newPosition + boundsOffset;
                if (AreBoundsOutOfHangarBounds(partBounds))
                {
                    return value;
                }
            }
            if (newPosition == oldPosition)
            {
                return value;
            }

            if (part.isCompund)
            {
                CompoundPartTransform.SetWorldPosition((CompoundPart)part, newPosition, compoundTargetSelected);
            }
            else
            {
                PartTransform.SetWorldPosition(part, newPosition);
            }

            return value;
        }

        private Vector3 GetWorldPositionToBeSet(int vectorIndex, string value)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Vector3 position = PartUtil.GetPosition(part, referenceSpace, compoundTargetSelected);
            position[vectorIndex] = fValue;

            return (referenceSpace == Space.Self && part.parent != null) ? part.parent.transform.TransformPoint(position) : position;
        }

        private bool AreBoundsOutOfHangarBounds(Bounds bounds)
        {
            Bounds editorBounds = EditorBounds.Instance.constructionBounds;

            return !(editorBounds.Contains(bounds.min) && editorBounds.Contains(bounds.max));
        }

        private string GetPosition(int vectorIndex)
        {
            return position[vectorIndex].ToString(FORMAT);
        }

        private string SetRotation(int vectorIndex, string value)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            Quaternion rotation = PartUtil.GetRotation(part, referenceSpace, compoundTargetSelected);
            Vector3 partEulerAngles = rotation.eulerAngles;
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = fValue - partEulerAngles[vectorIndex];
            if (part.isCompund)
            {
                CompoundPartTransform.Rotate((CompoundPart)part, eulerAngles, referenceSpace, compoundTargetSelected);
            }
            else
            {
                PartTransform.Rotate(part, eulerAngles, referenceSpace);
            }
            return value;
        }

        private string GetRotation(int vectorIndex)
        {
            return rotation.eulerAngles[vectorIndex].ToString(FORMAT);
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

        private void Translate(int vectorIndex, bool inverse)
        {
            float offset = inverse ? -deltaPosition : deltaPosition;
            Vector3 position = PartUtil.GetPosition(part, referenceSpace, compoundTargetSelected);
            float currentValue = position[vectorIndex];
            float newValue = currentValue + offset;
            SetPosition(vectorIndex, newValue.ToString());
        }

        private void Rotate(int vectorIndex, bool inverse)
        {
            Vector3 eulerAngles = new Vector3(0, 0, 0);
            eulerAngles[vectorIndex] = inverse ? -deltaRotation : deltaRotation;

            if (part.isCompund)
            {
                CompoundPartTransform.Rotate((CompoundPart)part, eulerAngles, referenceSpace, compoundTargetSelected);
            }
            else
            {
                PartTransform.Rotate(part, eulerAngles, referenceSpace);
            }
        }
    }
}