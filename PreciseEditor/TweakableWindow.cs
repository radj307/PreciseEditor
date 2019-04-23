using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace PreciseEditor
{
    public class TweakableWindow : BaseWindow
    {
        const int MAXLENGTH = 8;

        public TweakableWindow()
        {
            dialogRect = new Rect(0.5f, 0.5f, 225f, 25f);
        }

        public void Show(Tweakable[] tweakables)
        {
            Hide();

            string title = FormatLabel("Precise Editor - ") + "Tweakables";
            List<DialogGUIBase> dialogGUIBaseList = new List<DialogGUIBase> { };
            List<DialogGUITextInput> partModuleInputList = new List<DialogGUITextInput>();

            if (tweakables.Length > 0)
            {
                foreach (var tweakable in tweakables)
                {
                    var labelField = new DialogGUILabel(FormatLabel(tweakable.Label), 100f, 25f);
                    var inputField = new DialogGUITextInput(
                        tweakable.GetValue(),
                        false, MAXLENGTH,
                        value => tweakable.SetValueWithSymmetry(value),
                        () => tweakable.GetValue(),
                        TMP_InputField.ContentType.DecimalNumber, 25f);
                    dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(labelField, inputField));
                    partModuleInputList.Add(inputField);
                }
            }
            else
            {
                DialogGUISpace spaceToCenter = new DialogGUISpace(-1);
                DialogGUILabel labelNoTweakables = new DialogGUILabel("<color=\"white\">This part has no tweakables.</color>");
                dialogGUIBaseList.Add(new DialogGUIHorizontalLayout(spaceToCenter, labelNoTweakables, spaceToCenter));
            }

            dialog = new MultiOptionDialog("tweakableWindowDialog", "", title, HighLogic.UISkin, dialogRect, new DialogGUIVerticalLayout(dialogGUIBaseList.ToArray()));
            popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, false, HighLogic.UISkin, false);
            popupDialog.onDestroy.AddListener(SaveWindowPosition);

            foreach (DialogGUITextInput partModuleInput in partModuleInputList)
            {
                SetTextInputListeners(partModuleInput);
            }
        }
    }
}