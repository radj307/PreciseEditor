using System.Globalization;
using System.Linq;
using System.Reflection;

namespace PreciseEditor
{
    public class Tweakable
    {
        public Part Part;
        public PartModule Module;
        public FieldInfo FieldInfo;

        public string Label;

        //todo: use RangeInfo to set min/max limits
        public UI_FloatRange RangeInfo;

        public string GetValue()
        {
            return FieldInfo.GetValue(Module).ToString();
        }

        public string SetValueWithSymmetry(string valueStr)
        {
            var value = float.Parse(valueStr, CultureInfo.InvariantCulture.NumberFormat);

            FieldInfo.SetValue(Module, value);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, Part);
            var moduleIndex = Part.Modules.IndexOf(Module);

            foreach (var symmetryPart in Part.symmetryCounterparts)
            {
                var symmetryModule = symmetryPart.Modules[moduleIndex];
                FieldInfo.SetValue(symmetryModule, value);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, symmetryPart);
            }

            return valueStr;
        }
    }

    public static class TweakableExtension
    {
        public static Tweakable[] GetTweakables(this Part part)
        {
            return part.Modules.Cast<PartModule>()
                .SelectMany(module => module.GetType().GetFields()
                    .Select(f => new
                    {
                        fieldInfo = f,
                        kspFieldInfo = (KSPField) f.GetCustomAttributes(typeof(KSPField), false).FirstOrDefault(),
                        rangeInfo = (UI_FloatRange) f.GetCustomAttributes(typeof(UI_FloatRange), false).FirstOrDefault()
                    })
                    .Where(a => a.kspFieldInfo != null && a.kspFieldInfo.guiActiveEditor && a.rangeInfo != null)
                    .Select(a => new Tweakable
                    {
                        Part = part,
                        Module = module,
                        FieldInfo = a.fieldInfo,
                        //todo: test if Localizer.Format() is needed on these
                        Label = a.kspFieldInfo.guiName,
                        RangeInfo = a.rangeInfo
                    }))
                .ToArray();
        }
    }
}