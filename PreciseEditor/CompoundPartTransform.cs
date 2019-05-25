using UnityEngine;

namespace PreciseEditor
{
    public static class CompoundPartTransform
    {
        public static void SetWorldPosition(CompoundPart part, Vector3 newPosition, bool targetSelected)
        {
            bool targetActive = IsTargetActive(part, targetSelected);
            Vector3 localPosition = part.transform.InverseTransformPoint(newPosition);

            if (targetActive)
            {
                SetTargetPosition(part, newPosition);
            }
            else
            {
                part.transform.position = newPosition;
            }

            UpdateEditorGizmo(part, targetActive);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, part);

            if (part.symMethod == SymmetryMethod.Mirror)
            {
                UpdateMirrorSymmetryCounterpartsPosition(part, targetActive);
            }
            else if (part.symMethod == SymmetryMethod.Radial)
            {
                UpdateRadialSymmetryCounterpartsPosition(part, localPosition, targetActive);
            }

            EditorLogic.fetch.SetBackup();
        }

        public static void Rotate(CompoundPart part, Vector3 eulerAngles, Space space, bool targetSelected)
        {
            bool targetActive = IsTargetActive(part, targetSelected);
            CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
            if (targetActive)
            {
                RotateTarget(part, eulerAngles, space);
            }
            else
            {
                part.transform.Rotate(eulerAngles, space);
                part.onEditorEndTweak();
            }
            UpdateEditorGizmo(part, targetActive);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, part);

            if (part.symMethod == SymmetryMethod.Mirror)
            {
                RotateMirrorSymmetryCounterparts(part, eulerAngles, space, targetActive);
            }
            else if (part.symMethod == SymmetryMethod.Radial)
            {
                RotateRadialSymmetryCounterparts(part, eulerAngles, space, targetActive);
            }

            EditorLogic.fetch.SetBackup();
        }

        private static bool IsTargetActive(CompoundPart part, bool targetSelected)
        {
            return targetSelected && part.target != null;
        }

        private static void SetTargetPosition(CompoundPart part, Vector3 position)
        {
            CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
            bool oldTweakingTarget = module.TweakingTarget;
            module.TweakingTarget = true;
            module.targetAnchor.position = position;
            module.targetAnchor.hasChanged = true;
            module.OnTargetUpdate();
            module.TweakingTarget = oldTweakingTarget;
        }

        private static void RotateTarget(CompoundPart part, Vector3 eulerAngles, Space space)
        {
            CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
            bool oldTweakingTarget = module.TweakingTarget;
            module.TweakingTarget = true;
            module.targetAnchor.Rotate(eulerAngles, space);
            module.targetAnchor.hasChanged = true;
            module.OnTargetUpdate();
            module.TweakingTarget = oldTweakingTarget;
        }

        private static void RotateTarget(CompoundPart part, Vector3 axis, float angle, Space space)
        {
            CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
            bool oldTweakingTarget = module.TweakingTarget;
            module.TweakingTarget = true;
            module.targetAnchor.Rotate(axis, angle, space);
            module.targetAnchor.hasChanged = true;
            module.OnTargetUpdate();
            module.TweakingTarget = oldTweakingTarget;
        }

        private static Vector3 MirrorPosition(Vector3 position)
        {
            Vector3 localPosition = position - EditorLogic.RootPart.transform.position;
            Vector3 projection = Vector3.ProjectOnPlane(localPosition, EditorLogic.RootPart.transform.right);
            Vector3 projectedPoint = EditorLogic.RootPart.transform.position + projection;
            Vector3 offset = projectedPoint - position;

            return EditorLogic.RootPart.transform.position + projection + offset;
        }

        private static void UpdateMirrorSymmetryCounterpartsPosition(CompoundPart part, bool targetActive)
        {
            Vector3 mirrorPosition;
            if (targetActive)
            {
                Vector3 targetPosition = PartUtil.GetPosition(part, Space.World, targetActive);
                mirrorPosition = MirrorPosition(targetPosition);
            }
            else
            {
                mirrorPosition = MirrorPosition(part.transform.position);
            }

            foreach (CompoundPart symmetryCounterpart in part.symmetryCounterparts)
            {
                if (targetActive)
                {
                    SetTargetPosition(symmetryCounterpart, mirrorPosition);
                }
                else
                {
                    symmetryCounterpart.transform.position = mirrorPosition;
                }
                UpdateEditorGizmo(symmetryCounterpart, targetActive);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
            }
        }

        private static void UpdateRadialSymmetryCounterpartsPosition(CompoundPart part, Vector3 localPosition, bool targetActive)
        {
            foreach (CompoundPart symmetryCounterpart in part.symmetryCounterparts)
            {
                if (targetActive)
                {
                    Vector3 newPosition = symmetryCounterpart.transform.TransformPoint(localPosition);
                    SetTargetPosition(symmetryCounterpart, newPosition);
                }
                else
                {
                    symmetryCounterpart.transform.Translate(localPosition);
                }
                UpdateEditorGizmo(symmetryCounterpart, targetActive);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
            }
        }

        private static void RotateMirrorSymmetryCounterparts(CompoundPart part, Vector3 eulerAngles, Space space, bool targetActive)
        {
            if (space == Space.Self)
            {
                if (targetActive)
                {
                    CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
                    eulerAngles = module.targetAnchor.TransformDirection(eulerAngles);
                }
                else
                {
                    eulerAngles = part.transform.TransformDirection(eulerAngles);
                }
            }

            eulerAngles = EditorLogic.RootPart.transform.InverseTransformDirection(eulerAngles);
            Quaternion rotation = Quaternion.Euler(eulerAngles);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            Vector3 mirrorAxis = new Vector3(-axis.x, axis.y, axis.z);
            mirrorAxis = EditorLogic.RootPart.transform.TransformDirection(mirrorAxis);

            foreach (CompoundPart symmetryCounterpart in part.symmetryCounterparts)
            {
                if (targetActive)
                {
                    RotateTarget(symmetryCounterpart, mirrorAxis, -angle, Space.World);
                }
                else
                {
                    symmetryCounterpart.transform.Rotate(mirrorAxis, -angle, Space.World);
                    symmetryCounterpart.onEditorEndTweak();
                }
                UpdateEditorGizmo(symmetryCounterpart, targetActive);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        private static void RotateRadialSymmetryCounterparts(CompoundPart part, Vector3 eulerAngles, Space space, bool targetActive)
        {
            if (space == Space.World)
            {
                if (targetActive)
                {
                    CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
                    eulerAngles = module.targetAnchor.InverseTransformDirection(eulerAngles);
                }
                else
                {
                    eulerAngles = part.transform.InverseTransformDirection(eulerAngles);
                }
            }

            foreach (CompoundPart symmetryCounterpart in part.symmetryCounterparts)
            {
                if (targetActive)
                {
                    RotateTarget(symmetryCounterpart, eulerAngles, Space.Self);
                }
                else
                {
                    symmetryCounterpart.transform.Rotate(eulerAngles, Space.Self);
                    symmetryCounterpart.onEditorEndTweak();
                }

                UpdateEditorGizmo(symmetryCounterpart, targetActive);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        private static void UpdateEditorGizmo(CompoundPart part, bool targetActive)
        {
            if (part == EditorLogic.SelectedPart)
            {
                Vector3 position;
                Quaternion rotation;
                if (targetActive)
                {
                    CompoundParts.CModuleLinkedMesh module = part.FindModuleImplementing<CompoundParts.CModuleLinkedMesh>();
                    position = module.targetAnchor.position;
                    rotation = module.targetAnchor.rotation;
                }
                else
                {
                    position = part.transform.position;
                    rotation = part.transform.rotation;
                }

                var gizmoOffset = HighLogic.FindObjectOfType<EditorGizmos.GizmoOffset>();
                if (gizmoOffset != null)
                {
                    gizmoOffset.transform.position = position;
                    if (gizmoOffset.CoordSpace == Space.Self)
                    {
                        gizmoOffset.transform.rotation = rotation;
                    }
                }

                var gizmoRotate = HighLogic.FindObjectOfType<EditorGizmos.GizmoRotate>();
                if (gizmoRotate != null)
                {
                    gizmoRotate.transform.position = position;
                    if (gizmoRotate.CoordSpace == Space.Self)
                    {
                        gizmoRotate.transform.rotation = rotation;
                    }
                }
            }
        }
    }
}