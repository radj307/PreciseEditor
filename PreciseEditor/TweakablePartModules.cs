using System.Collections.Generic;
using System.Globalization;
using KSP.Localization;

namespace PreciseEditor
{
    public static class TweakablePartModules
    {
        public static string[] GetTweakableFieldNames(PartModule partModule)
        {
            if (partModule is ModuleReactionWheel moduleReactionWheel)
            {
                return new string[] { "authorityLimiter" };
            }
            if (partModule is ModuleControlSurface moduleControlSurface)
            {
                return new string[] { "authorityLimiter" };
            }
            if (partModule is ModuleAeroSurface moduleAeroSurface)
            {
                return new string[] { "aeroAuthorityLimiter" };
            }
            if (partModule is ModuleAnimateGeneric moduleAnimateGeneric)
            {
                return new string[] { "deployPercent" };
            }
            if (partModule is ModuleEngines moduleEngines)
            {
                return new string[] { "thrustPercentage" };
            }
            if (partModule is ModuleRCSFX moduleRcsFx)
            {
                return new string[] { "thrustPercentage" };
            }
            if (partModule is ModuleGimbal moduleGimbal)
            {
                return new string[] { "gimbalLimiter" };
            }
            if (partModule is ModuleDecouple moduleDecouple)
            {
                return new string[] { "ejectionForcePercent" };
            }
            if (partModule is ModuleLight moduleLight)
            {
                return new string[] { "lightR", "lightG", "lightB" };
            }
            if (partModule is ModuleProceduralFairing moduleProceduralFairing)
            {
                return new string[] { "ejectionForce" };
            }
            if (partModule is ModuleParachute moduleParachute)
            {
                return new string[] { "minAirPressureToOpen", "deployAltitude", "spreadAngle" };
            }
            if (partModule is ModuleWheelBase moduleWheelBase)
            {
                return new string[] { "frictionMultiplier" };
            }
            if (partModule is ModuleWheels.ModuleWheelBrakes moduleWheelBrakes)
            {
                return new string[] { "brakeTweakable" };
            }
            if (partModule is ModuleWheels.ModuleWheelSuspension moduleWheelSuspension)
            {
                return new string[] { "springTweakable", "damperTweakable" };
            }
            if (partModule is ModuleWheels.ModuleWheelMotor moduleWheelMotor)
            {
                return new string[] { "driveLimiter", "tractionControlScale" };
            }

            return new string[] { };
        }

        public static string[] GetTweakableFieldLabels(PartModule partModule)
        {
            if (partModule is ModuleReactionWheel moduleReactionWheel)
            {
                return new string[] { Localizer.Format("#autoLOC_6001309") };
            }
            if (partModule is ModuleControlSurface moduleControlSurface)
            {
                return new string[] { Localizer.Format("#autoLOC_6001336") };
            }
            if (partModule is ModuleAeroSurface moduleAeroSurface)
            {
                return new string[] { Localizer.Format("#autoLOC_6001336") };
            }
            if (partModule is ModuleAnimateGeneric moduleAnimateGeneric)
            {
                return new string[] { Localizer.Format("#autoLOC_6001353") };
            }
            if (partModule is ModuleEngines moduleEngines)
            {
                return new string[] { Localizer.Format("#autoLOC_6001363") };
            }
            if (partModule is ModuleRCSFX moduleRcsFx)
            {
                return new string[] { Localizer.Format("#autoLOC_6001363") };
            }
            if (partModule is ModuleGimbal moduleGimbal)
            {
                return new string[] { Localizer.Format("#autoLOC_6001383") };
            }
            if (partModule is ModuleDecouple moduleDecouple)
            {
                return new string[] { Localizer.Format("#autoLOC_6001442") };
            }
            if (partModule is ModuleLight moduleLight)
            {
                return new string[] { Localizer.Format("#autoLOC_6001402"), Localizer.Format("#autoLOC_6001403"), Localizer.Format("#autoLOC_6001404") };
            }
            if (partModule is ModuleProceduralFairing moduleProceduralFairing)
            {
                return new string[] { Localizer.Format("#autoLOC_6001395") };
            }
            if (partModule is ModuleParachute moduleParachute)
            {
                return new string[] { Localizer.Format("#autoLOC_6001340"), Localizer.Format("#autoLOC_463493"), Localizer.Format("#autoLOC_6001341") };
            }
            if (partModule is ModuleWheelBase moduleWheelBase)
            {
                return new string[] { Localizer.Format("#autoLOC_6001457") };
            }
            if (partModule is ModuleWheels.ModuleWheelBrakes moduleWheelBrakes)
            {
                return new string[] { Localizer.Format("#autoLOC_6003006") };
            }
            if (partModule is ModuleWheels.ModuleWheelSuspension moduleWheelSuspension)
            {
                return new string[] { Localizer.Format("#autoLOC_6001469"), Localizer.Format("#autoLOC_6001470") };
            }
            if (partModule is ModuleWheels.ModuleWheelMotor moduleWheelMotor)
            {
                return new string[] { Localizer.Format("#autoLOC_6001463"), Localizer.Format("#autoLOC_6001464") };
            }

            return new string[] { };
        }

        public static string GetPartModuleFieldValue(PartModule partModule, string fieldName)
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
            if (partModule is ModuleAeroSurface moduleAeroSurface)
            {
                value = moduleAeroSurface.GetType().GetField(fieldName).GetValue(moduleAeroSurface).ToString();
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
            if (partModule is ModuleGimbal moduleGimbal)
            {
                value = moduleGimbal.GetType().GetField(fieldName).GetValue(moduleGimbal).ToString();
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

        public static string SetPartModuleFieldValue(PartModule partModule, string fieldName, string value)
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
            if (partModule is ModuleAeroSurface moduleAeroSurface)
            {
                moduleAeroSurface.GetType().GetField(fieldName).SetValue(moduleAeroSurface, fValue);
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
            if (partModule is ModuleGimbal moduleGimbal)
            {
                moduleGimbal.GetType().GetField(fieldName).SetValue(moduleGimbal, fValue);
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

        public static string SetPartFieldValue(Part part, PartModule partModule, string fieldName, string value)
        {
            SetPartModuleFieldValue(partModule, fieldName, value);
            GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, part);
            int partModuleIndex = part.Modules.IndexOf(partModule);

            foreach (Part symmetryCounterpart in part.symmetryCounterparts)
            {
                SetPartModuleFieldValue(symmetryCounterpart.Modules[partModuleIndex], fieldName, value);
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, symmetryCounterpart);
            }

            return value;
        }

        public static bool IsTweakablePartModule(PartModule partModule)
        {
            return partModule.isEnabled && (
                partModule is ModuleReactionWheel ||
                partModule is ModuleControlSurface ||
                partModule is ModuleAeroSurface ||
                partModule is ModuleAnimateGeneric ||
                partModule is ModuleEngines ||
                partModule is ModuleRCSFX ||
                partModule is ModuleGimbal ||
                partModule is ModuleDecouple ||
                partModule is ModuleLight ||
                partModule is ModuleProceduralFairing ||
                partModule is ModuleParachute ||
                partModule is ModuleWheelBase ||
                partModule is ModuleWheels.ModuleWheelBrakes ||
                partModule is ModuleWheels.ModuleWheelSuspension ||
                partModule is ModuleWheels.ModuleWheelMotor);
        }
    }
}