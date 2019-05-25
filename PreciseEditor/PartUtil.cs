using UnityEngine;

namespace PreciseEditor
{
    static class PartUtil
    {
        public static bool IsTargetActive(Part part, bool compoundTargetSelected)
        {
            return (part.isCompund && compoundTargetSelected && ((CompoundPart)part).target != null);
        }

        public static Vector3 GetPosition(Part part, Space space, bool compoundTargetSelected)
        {
            bool targetActive = IsTargetActive(part, compoundTargetSelected);
            CompoundPart compoundPart = null;
            if (targetActive)
            {
                compoundPart = (CompoundPart)part;
            }

            if (space == Space.Self)
            {
                if (targetActive)
                {
                    Vector3 worldPosition = compoundPart.transform.TransformPoint(compoundPart.targetPosition);
                    return (compoundPart.parent != null) ? compoundPart.parent.transform.InverseTransformPoint(worldPosition) : worldPosition;
                }
                else
                {
                    return part.transform.localPosition;
                }
            }
            else
            {
                if (targetActive)
                {
                    return compoundPart.transform.TransformPoint(compoundPart.targetPosition);
                }
                else
                {
                    return part.transform.position;
                }
            }
        }

        public static  Quaternion GetRotation(Part part, Space space, bool compoundTargetSelected)
        {
            bool targetActive = IsTargetActive(part, compoundTargetSelected);
            CompoundPart compoundPart = null;
            if (targetActive)
            {
                compoundPart = (CompoundPart)part;
            }

            if (space == Space.Self)
            {
                if (targetActive)
                {
                    return compoundPart.transform.localRotation * compoundPart.targetRotation;
                }
                else
                {
                    return part.transform.localRotation;
                }
            }
            else
            {
                if (targetActive)
                {
                    return compoundPart.transform.rotation * compoundPart.targetRotation;
                }
                else
                {
                    return part.transform.rotation;
                }
            }
        }
    }
}
