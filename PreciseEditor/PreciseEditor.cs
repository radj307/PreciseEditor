﻿using UnityEngine;

namespace PreciseEditor
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class PreciseEditor : MonoBehaviour
    {
        public static PreciseEditor Instance { get; private set; }

        public PartEditionWindow partEditionWindow = null;
        public VesselWindow vesselWindow = null;

        public void Start()
        {
            Instance = this;
            partEditionWindow = gameObject.AddComponent<PartEditionWindow>();
            vesselWindow = gameObject.AddComponent<VesselWindow>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Part part = GetPartUnderCursor();
                if (part)
                {
                    partEditionWindow.Show(part);
                }
            }
        }

        public static Part GetPartUnderCursor()
        {
            if (EditorLogic.SelectedPart)
            { // use the currently selected part
                return EditorLogic.SelectedPart;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            { // use the root part if shift is pressed
                return EditorLogic.RootPart;
            }

            // else, use a raycast to find the part
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            EditorLogic editorLogic = EditorLogic.fetch;

            if (editorLogic && Physics.Raycast(ray, out RaycastHit rayCastHit))
            {
                return editorLogic.ship.Parts.Find(p => p.gameObject == rayCastHit.transform.gameObject);
            }

            return null;
        }
    }
}