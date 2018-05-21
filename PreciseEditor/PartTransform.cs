using UnityEngine;

namespace PreciseEditor
{
    public static class PartTransform
    {
        /// <summary>
        /// Set one of position component (x, y, z).
        /// </summary>
        /// <param name="part">Part to position</param>
        /// <param name="newPosition">Part new position</param>
        /// <param name="space">The reference space</param>
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

        /// <summary>
        /// Set one of rotation component (x, y, z).
        /// </summary>
        /// <param name="part">Part to rotate.</param>
        /// <param name="eulerAngles">The part new euler angles</param>
        /// <param name="space">The reference space</param>
        public static void SetRotation(Part part, Vector3 eulerAngles, Space space)
        {
            Vector3 srcEulerAngles = (space == Space.Self) ? part.transform.localRotation.eulerAngles : part.transform.rotation.eulerAngles;
            Vector3 deltaEuler = eulerAngles - srcEulerAngles;
            Quaternion angle = Quaternion.Euler(deltaEuler);

            if (space == Space.Self)
            {
                part.transform.localRotation = Quaternion.Euler(eulerAngles);
            }
            else
            {
                part.transform.rotation = Quaternion.Euler(eulerAngles);
            }
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, part);

            if (part.symMethod == SymmetryMethod.Mirror)
            {
                PartTransform.UpdateMirrorSymmetryCounterpartsRotation(part);
            }
            else if (part.symMethod == SymmetryMethod.Radial)
            {
                PartTransform.UpdateRadialSymmetryCounterpartsRotation(part, angle);
            }

            EditorLogic.fetch.SetBackup();
        }

        /// <summary>
        /// Apply a rotation to a part and its symmetry counterparts.
        /// </summary>
        /// <param name="part">The part to rotate.</param>
        /// <param name="eulerAngles">The rotation euler angles</param>
        /// <param name="space">The reference space</param>
        public static void Rotate(Part part, Vector3 eulerAngles, Space space)
        {
            part.transform.Rotate(eulerAngles, space);
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

        /// <summary>
        /// Update the position of every mirror symmetry counter part of a given part after it has been moved.
        /// </summary>
        /// <param name="part"></param>
        private static void UpdateMirrorSymmetryCounterpartsPosition(Part part)
        {
            if (part.parent)
            {
                Vector3 localPosition = part.transform.position - part.parent.transform.position;
                Vector3 projection = Vector3.ProjectOnPlane(localPosition, EditorLogic.RootPart.transform.right);
                Vector3 projectedPoint = part.parent.transform.position + projection;
                Vector3 offset = projectedPoint - part.transform.position;

                foreach (Part symmetryCounterpart in part.symmetryCounterparts)
                {
                    symmetryCounterpart.transform.position = symmetryCounterpart.parent.transform.position + projection + offset;
                    GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
                }
            }
        }

        /// <summary>
        /// Update the position of every radial symmetry counter part of a given part after it has been moved.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="offset">The translation vector.</param>
        private static void UpdateRadialSymmetryCounterpartsPosition(Part part, Vector3 offset)
        {
            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.Translate(offset);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, symmetryCounterpart);
            }
        }

        /// <summary>
        /// Update the rotation of every mirror symmetry counterpart of a given part after it has been rotated.
        /// </summary>
        /// <param name="part">The part that has rotated.</param>
        private static void UpdateMirrorSymmetryCounterpartsRotation(Part part)
        {
            Quaternion rotation = part.transform.rotation;
            Quaternion mirrorRotation = new Quaternion(-rotation.z, -rotation.w, -rotation.x, -rotation.y);

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.rotation = mirrorRotation;
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        /// <summary>
        /// Update the rotation of every radial symmetry counterpart of a given part after it has been rotated.
        /// </summary>
        /// <param name="part">The part that has rotated</param>
        /// <param name="angle">The rotation to apply</param>
        private static void UpdateRadialSymmetryCounterpartsRotation(Part part, Quaternion rotation)
        {
            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.rotation *= rotation;
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }

        /// <summary>
        /// Update the rotation of every radial symmetry counter part of a given part after it has been rotated.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="eulerAngle"></param>
        /// <param name="space"></param>
        private static void RotateRadialSymmetryCounterparts(Part part, Vector3 eulerAngles, Space space)
        {
            if (space == Space.World)
            {
                eulerAngles = part.transform.InverseTransformDirection(eulerAngles);
            }

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                symmetryCounterpart.transform.Rotate(eulerAngles, Space.Self);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, symmetryCounterpart);
            }
        }
    }
}