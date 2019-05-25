using UnityEngine;

namespace PreciseEditor
{
    public class AxisLines : MonoBehaviour
    {
        public Color red, green, cyan;
        protected GameObject[] axis;
        protected Space axisSpace;
        protected bool axisCompoundTargetSelected;
        protected bool visible = false;

        public void Start()
        {
            red = new Color(1.0f, 0.21f, 0f, 1.0f);
            green = new Color(0f, 0.98f, 0f, 1.0f);
            cyan = new Color(0f, 0.85f, 1.0f, 1.0f);
        }

        public void Show(Part part, Space space, bool compoundTargetSelected)
        {
            Material material = new Material(Shader.Find("KSP/UnlitColor"));
            GameObject axisX = CreateAxis("AxisX", material, red);
            GameObject axisY = CreateAxis("AxisY", material, green);
            GameObject axisZ = CreateAxis("AxisZ", material, cyan);
            axis = new GameObject[] { axisX, axisY, axisZ };
            UpdateAxis(part, space, compoundTargetSelected);
            visible = true;
        }

        public void UpdateAxis(Part part, Space space, bool compoundTargetSelected)
        {
            if (part == null)
            {
                Hide();
                return;
            }

            bool isRootPart = (part.parent == null);
            if (visible && space == axisSpace && compoundTargetSelected == axisCompoundTargetSelected && part.transform.position == transform.position &&
                (isRootPart || part.parent.transform.rotation == transform.rotation))
            {
                return;
            }

            bool isTargetActive = PartUtil.IsTargetActive(part, compoundTargetSelected);
            transform.position = PartUtil.GetPosition(part, Space.World, isTargetActive);
            transform.rotation = isRootPart ? part.transform.rotation : part.parent.transform.rotation;
            axisSpace = space;
            axisCompoundTargetSelected = compoundTargetSelected;
            Vector3[] directions = new Vector3[] { transform.right, transform.up, transform.forward };

            for (int index = 0; index < axis.Length; index++)
            {
                LineRenderer lineRenderer = axis[index].GetComponent<LineRenderer>();
                Vector3[] axisBounds = (axisSpace == Space.World || isRootPart) ? GetWorldSpaceAxisBounds(transform.position, index) : GetSelfSpaceAxisBounds(transform.position, directions[index]);
                lineRenderer.SetPosition(0, axisBounds[0]);
                lineRenderer.SetPosition(1, axisBounds[1]);
            }
        }

        public void Hide()
        {
            for (int index = 0; index < axis.Length; index++)
            {
                Destroy(axis[index]);
                axis[index] = null;
            }
            visible = false;
        }

        private GameObject CreateAxis(string name, Material material, Color color)
        {
            GameObject axis = new GameObject(name);
            LineRenderer lineRenderer = axis.AddComponent<LineRenderer>();
            float width = 0.03f;

            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.material = material;
            lineRenderer.positionCount = 2;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            return axis;
        }

        private Vector3[] GetWorldSpaceAxisBounds(Vector3 position, int vectorIndex)
        {
            Bounds bounds = EditorBounds.Instance.constructionBounds;
            Vector3 start, end;
            start = end = position;
            start[vectorIndex] = bounds.min[vectorIndex];
            end[vectorIndex] = bounds.max[vectorIndex];

            return new Vector3[] { start, end };
        }

        private Vector3[] GetSelfSpaceAxisBounds(Vector3 axisPosition, Vector3 axisDirection)
        {
            Ray rayToStart = new Ray(axisPosition, -axisDirection);
            Ray rayToEnd = new Ray(axisPosition, axisDirection);
            Bounds bounds = EditorBounds.Instance.constructionBounds;
            bool startFits = bounds.IntersectRay(rayToStart, out float lengthToStart);
            bool endFits = bounds.IntersectRay(rayToEnd, out float lengthToEnd);
            Vector3 start = rayToStart.GetPoint(lengthToStart);
            Vector3 end = rayToEnd.GetPoint(lengthToEnd);

            return new Vector3[] { start, end };
        }
    }
}