using UnityEngine;
using System.Collections.Generic;

namespace PreciseEditor
{
    public class ColliderWindow : BaseWindow
    {
        const string FORMAT = "F4";
        private Part part = null;
        private Part partUnderCursor = null;

        public ColliderWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 320f, 10f);
        }

        public void Update()
        {
            partUnderCursor = PreciseEditor.GetPartUnderCursor();
        }

        public void Show(Part part)
        {
            Hide();

            this.part = part;
            string title = FormatLabel("Precise Editor - ") + "Colliders";
            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase> {
                GetBoxColliderBoundsDialogGUIBox(), GetPartUnderCursorDialogGUIBox()
            };

            dialog = new MultiOptionDialog("colliderWindowDialog", "", title, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);
        }

        private DialogGUIBox GetBoxColliderBoundsDialogGUIBox()
        {
            DialogGUILabel labelColliderBounds = new DialogGUILabel(FormatLabel("Collider Bounds"), 100, 16);
            DialogGUILabel labelColliderCenter = new DialogGUILabel(FormatLabel("Center"), 150, 16);
            DialogGUILabel labelColliderCenterValue = new DialogGUILabel(delegate { return GetColliderCenter(); });
            DialogGUILabel labelColliderExtents = new DialogGUILabel(FormatLabel("Extents"), 150, 16);
            DialogGUILabel labelColliderExtentsValue = new DialogGUILabel(delegate { return GetColliderExtents(); });
            DialogGUILabel labelColliderSize = new DialogGUILabel(FormatLabel("Size"), 150, 16);
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

            return new DialogGUIBox("", 307, 90, null, layoutColliderBounds);
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
    }
}