using System.Collections.Generic;
using System.Globalization;
using KSP.Localization;

namespace PreciseEditor
{
    public static class TweakablePartModules
    {
        public static PartModuleList GetPartModules(Part part)
        {
            PartModuleList partModuleList = new PartModuleList(part);

            foreach (PartModule partModule in part.Modules)
            {
                if (!TweakablePartModules.IsTweakablePartModule(partModule))
                {
                    partModuleList.Remove(partModule);
                }
            }

            return partModuleList;
        }

        public static string[] GetTweakableFieldNames(string partModuleName)
        {
            IDictionary<string, string[]> dictionary = new Dictionary<string, string[]>()
            {
                { "ModuleReactionWheel", new string[] { "authorityLimiter" } },
                { "ModuleControlSurface", new string[] { "authorityLimiter" } },
                { "ModuleAnimateGeneric", new string[] { "deployPercent" } },
                { "ModuleEngines", new string[] { "thrustPercentage" } },
                { "ModuleRCSFX", new string[] { "thrustPercentage" } },
                { "ModuleDecouple", new string[] { "ejectionForcePercent" } },
                { "ModuleLight", new string[] { "lightR", "lightG", "lightB" } },
                { "ModuleProceduralFairing", new string[] { "ejectionForce", "nArcs" } },
                { "ModuleParachute", new string[] { "minAirPressureToOpen", "deployAltitude", "spreadAngle" } },
                { "ModuleWheelBase", new string[] { "frictionMultiplier" } },
                { "ModuleWheelBrakes", new string[] { "brakeTweakable" } },
                { "ModuleWheelSuspension", new string[] { "springTweakable", "damperTweakable" } },
                { "ModuleWheelMotor", new string[] { "driveLimiter", "tractionControlScale" } }
            };

            return dictionary[partModuleName];
        }

        public static string[] GetTweakableFieldLabels(string partModuleName)
        {
            IDictionary<string, string[]> dictionary = new Dictionary<string, string[]>()
            {
                { "ModuleReactionWheel", new string[] { Localizer.Format("#autoLOC_6001309") } },
                { "ModuleControlSurface", new string[] { Localizer.Format("#autoLOC_6001336") } },
                { "ModuleAnimateGeneric", new string[] { Localizer.Format("#autoLOC_6001353") } },
                { "ModuleEngines", new string[] { Localizer.Format("#autoLOC_6001363") } },
                { "ModuleRCSFX", new string[] { Localizer.Format("#autoLOC_6001363") } },
                { "ModuleDecouple", new string[] { Localizer.Format("#autoLOC_6001442") } },
                { "ModuleLight", new string[] { Localizer.Format("#autoLOC_6001402"), Localizer.Format("#autoLOC_6001403"), Localizer.Format("#autoLOC_6001404") } },
                { "ModuleProceduralFairing", new string[] { Localizer.Format("#autoLOC_6001395"), Localizer.Format("#autoLOC_6001394") } },
                { "ModuleParachute", new string[] { Localizer.Format("#autoLOC_6001340"), Localizer.Format("#autoLOC_463493"), Localizer.Format("#autoLOC_6001341") } },
                { "ModuleWheelBase", new string[] { Localizer.Format("#autoLOC_6001457") } },
                { "ModuleWheelBrakes", new string[] { Localizer.Format("#autoLOC_6003006") } },
                { "ModuleWheelSuspension", new string[] { Localizer.Format("#autoLOC_6001469"), Localizer.Format("#autoLOC_6001470") } },
                { "ModuleWheelMotor", new string[] { Localizer.Format("#autoLOC_6001463"), Localizer.Format("#autoLOC_6001464") } }
            };

            return dictionary[partModuleName];
        }

        public static string GetTweakableFieldValue(PartModule partModule, string fieldName)
        {
            string value = "";

            if (partModule is ModuleReactionWheel moduleReactionWheel)
            {
                value = moduleReactionWheel.GetType().GetField(fieldName).GetValue(moduleReactionWheel).ToString();
            }
            if (partModule is ModuleControlSurface moduleControlSurface)
            {
                value = moduleControlSurface.GetType().GetField(fieldName).GetValue(moduleControlSurface).ToString();
            }
            if (partModule is ModuleAnimateGeneric moduleAnimateGeneric)
            {
                value = moduleAnimateGeneric.GetType().GetField(fieldName).GetValue(moduleAnimateGeneric).ToString();
            }
            if (partModule is ModuleEngines moduleEngines)
            {
                value = moduleEngines.GetType().GetField(fieldName).GetValue(moduleEngines).ToString();
            }
            if (partModule is ModuleRCSFX moduleRcsFx)
            {
                value = moduleRcsFx.GetType().GetField(fieldName).GetValue(moduleRcsFx).ToString();
            }
            if (partModule is ModuleDecouple moduleDecouple)
            {
                value = moduleDecouple.GetType().GetField(fieldName).GetValue(moduleDecouple).ToString();
            }
            if (partModule is ModuleLight moduleLight)
            {
                value = moduleLight.GetType().GetField(fieldName).GetValue(moduleLight).ToString();
            }
            if (partModule is ModuleProceduralFairing moduleProceduralFairing)
            {
                value = moduleProceduralFairing.GetType().GetField(fieldName).GetValue(moduleProceduralFairing).ToString();
            }
            if (partModule is ModuleParachute moduleParachute)
            {
                value = moduleParachute.GetType().GetField(fieldName).GetValue(moduleParachute).ToString();
            }
            if (partModule is ModuleWheelBase moduleWheelBase)
            {
                value = moduleWheelBase.GetType().GetField(fieldName).GetValue(moduleWheelBase).ToString();
            }
            if (partModule is ModuleWheels.ModuleWheelBrakes moduleWheelBrakes)
            {
                value = moduleWheelBrakes.GetType().GetField(fieldName).GetValue(moduleWheelBrakes).ToString();
            }
            if (partModule is ModuleWheels.ModuleWheelSuspension moduleWheelSuspension)
            {
                value = moduleWheelSuspension.GetType().GetField(fieldName).GetValue(moduleWheelSuspension).ToString();
            }
            if (partModule is ModuleWheels.ModuleWheelMotor moduleWheelMotor)
            {
                value = moduleWheelMotor.GetType().GetField(fieldName).GetValue(moduleWheelMotor).ToString();
            }

            return value;
        }

        public static string SetTweakableFieldValue(PartModule partModule, string fieldName, string value)
        {
            float fValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);

            if (partModule is ModuleReactionWheel moduleReactionWheel)
            {
                moduleReactionWheel.GetType().GetField(fieldName).SetValue(moduleReactionWheel, fValue);
            }
            if (partModule is ModuleControlSurface moduleControlSurface)
            {
                moduleControlSurface.GetType().GetField(fieldName).SetValue(moduleControlSurface, fValue);
            }
            if (partModule is ModuleAnimateGeneric moduleAnimateGeneric)
            {
                moduleAnimateGeneric.GetType().GetField(fieldName).SetValue(moduleAnimateGeneric, fValue);
            }
            if (partModule is ModuleEngines moduleEngines)
            {
                moduleEngines.GetType().GetField(fieldName).SetValue(moduleEngines, fValue);
            }
            if (partModule is ModuleRCSFX moduleRcsFx)
            {
                moduleRcsFx.GetType().GetField(fieldName).SetValue(moduleRcsFx, fValue);
            }
            if (partModule is ModuleDecouple moduleDecouple)
            {
                moduleDecouple.GetType().GetField(fieldName).SetValue(moduleDecouple, fValue);
            }
            if (partModule is ModuleLight moduleLight)
            {
                moduleLight.GetType().GetField(fieldName).SetValue(moduleLight, fValue);
            }
            if (partModule is ModuleProceduralFairing moduleProceduralFairing)
            {
                moduleProceduralFairing.GetType().GetField(fieldName).SetValue(moduleProceduralFairing, fValue);
            }
            if (partModule is ModuleParachute moduleParachute)
            {
                moduleParachute.GetType().GetField(fieldName).SetValue(moduleParachute, fValue);
            }
            if (partModule is ModuleWheelBase moduleWheelBase)
            {
                moduleWheelBase.GetType().GetField(fieldName).SetValue(moduleWheelBase, fValue);
            }
            if (partModule is ModuleWheels.ModuleWheelBrakes moduleWheelBrakes)
            {
                moduleWheelBrakes.GetType().GetField(fieldName).SetValue(moduleWheelBrakes, fValue);
            }
            if (partModule is ModuleWheels.ModuleWheelSuspension moduleWheelSuspension)
            {
                moduleWheelSuspension.GetType().GetField(fieldName).SetValue(moduleWheelSuspension, fValue);
            }
            if (partModule is ModuleWheels.ModuleWheelMotor moduleWheelMotor)
            {
                moduleWheelMotor.GetType().GetField(fieldName).SetValue(moduleWheelMotor, fValue);
            }

            return value;
        }

        private static bool IsTweakablePartModule(PartModule partModule)
        {
            return
                partModule is ModuleReactionWheel ||
                partModule is ModuleControlSurface ||
                partModule is ModuleAnimateGeneric ||
                partModule is ModuleEngines ||
                partModule is ModuleRCSFX ||
                partModule is ModuleDecouple ||
                partModule is ModuleLight ||
                partModule is ModuleProceduralFairing ||
                partModule is ModuleParachute ||
                partModule is ModuleWheelBase ||
                partModule is ModuleWheels.ModuleWheelBrakes ||
                partModule is ModuleWheels.ModuleWheelSuspension ||
                partModule is ModuleWheels.ModuleWheelMotor;
        }
    }
}