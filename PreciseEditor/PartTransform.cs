using UnityEngine;

namespace PreciseEditor
{
    public static class PartTransform
    {
        public static void SetPosition(Part part, Vector3 newPosition, Space space)
        {
            Vector3 offset;

            if (space == Space.Self)
            {
                Vector3 sourcePosition = part.transform.position;
                part.transform.localPosition = newPosition;
                offset = -part.transform.InverseTransformPoint(sourcePosition);
            }
            else
            {
                offset = part.transform.InverseTransformPoint(newPosition);
                part.transform.position = newPosition;
            }
            PartTransform.UpdateEditorGizmo(part);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, part);

            if (part.symMethod == SymmetryMethod.Mirror)
            {
                PartTransform.UpdateMirrorSymmetryCounterpartsPosition(part);
            }
            else if (part.symMethod == SymmetryMethod.Radial)
            {
                PartTransform.UpdateRadialSymmetryCounterpartsPosition(part, offset);
            }

            EditorLogic.fetch.SetBackup();
        }

        public static void Rotate(Part part, Vector3 eulerAngles, Space space)
        {
            part.transform.Rotate(eulerAngles, space);
            PartTransform.UpdateEditorGizmo(part);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, part);

            if (part.symMethod == SymmetryMethod.Mirror)
            {
                PartTransform.UpdateMirrorSymmetryCounterpartsRotation(part);
            }
            else if (part.symMethod == SymmetryMethod.Radial)
            {
                PartTransform.RotateRadialSymmetryCounterparts(part, eulerAngles, space);
            }

            EditorLogic.fetch.SetBackup();
        }

        private static void UpdateMirrorSymmetryCounterpartsPosition(Part part)
        {
            Vector3 localPosition = part.transform.position - EditorLogic.RootPart.transform.position;
            Vector3 projection = Vector3.ProjectOnPlane(localPosition, EditorLogic.RootPart.transform.right);
            Vector3 projectedPoint = EditorLogic.RootPart.transform.position + projection;
            Vector3 offset = projectedPoint - part.transform.position;

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.position = EditorLogic.RootPart.transform.position + projection + offset;
                PartTransform.UpdateEditorGizmo(symmetryCounterpart);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
            }
        }

        private static void UpdateRadialSymmetryCounterpartsPosition(Part part, Vector3 offset)
        {
            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.Translate(offset);
                PartTransform.UpdateEditorGizmo(symmetryCounterpart);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
            }
        }

        private static void UpdateMirrorSymmetryCounterpartsRotation(Part part)
        {
            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.rotation = EditorGeometryUtil.MirrorRotation(part.transform.rotation, part.transform, EditorLogic.RootPart.transform);

                if (part.attRotation0 == symmetryCounterpart.attRotation0)
                {
                    symmetryCounterpart.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
                }
                PartTransform.UpdateEditorGizmo(symmetryCounterpart);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        private static void UpdateRadialSymmetryCounterpartsRotation(Part part, Quaternion rotation)
        {
            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.rotation *= rotation;
                PartTransform.UpdateEditorGizmo(symmetryCounterpart);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        private static void RotateRadialSymmetryCounterparts(Part part, Vector3 eulerAngles, Space space)
        {
            if (space == Space.World)
            {
                eulerAngles = part.transform.InverseTransformDirection(eulerAngles);
            }

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.Rotate(eulerAngles, Space.Self);
                PartTransform.UpdateEditorGizmo(symmetryCounterpart);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        private static void UpdateEditorGizmo(Part part)
        {
            if (part == EditorLogic.SelectedPart)
            {
                var gizmoOffset = HighLogic.FindObjectOfType<EditorGizmos.GizmoOffset>();
                if (gizmoOffset != null)
                {
                    gizmoOffset.transform.position = part.transform.position;
                    if (gizmoOffset.CoordSpace == Space.Self)
                    {
                        gizmoOffset.transform.rotation = part.transform.rotation;
                    }
                }

                var gizmoRotate = HighLogic.FindObjectOfType<EditorGizmos.GizmoRotate>();
                if (gizmoRotate != null)
                {
                    gizmoRotate.transform.position = part.transform.position;
                    if (gizmoRotate.CoordSpace == Space.Self)
                    {
                        gizmoRotate.transform.rotation = part.transform.rotation;
                    }
                }
            }
        }
    }
}