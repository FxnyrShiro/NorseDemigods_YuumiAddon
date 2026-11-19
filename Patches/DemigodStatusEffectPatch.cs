using System;
using HarmonyLib;
using NorseDemigods;
using NorseDemigods.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi.Patches
{
    [HarmonyPatch(typeof(Demigod), "get_DemigodStatusEffect")]
    public class DemigodStatusEffectPatch
    {
        private static readonly AccessTools.FieldRef<Demigod, SE_Demigod> StatusEffectField =
            AccessTools.FieldRefAccess<Demigod, SE_Demigod>("demigodStatusEffect");

        public static bool Prefix(Demigod __instance, ref SE_Demigod __result)
        {
            if (!string.Equals(__instance.DemigodName, YuumiConfig.DemigodName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            int hash = "SE_DemigodYuumi".GetStableHashCode();
            __instance.DemigodStatusEffectHash = hash;

            if (__instance.Owner != null && __instance.Owner.GetSEMan() != null)
            {
                var active = __instance.Owner.GetSEMan().GetStatusEffect(hash) as SE_Demigod;
                if (active != null)
                {
                    StatusEffectField(__instance) = active;
                    __result = active;
                    return false;
                }

                if (ObjectDB.instance != null)
                {
                    var prefabSe = ObjectDB.instance.GetStatusEffect(hash) as SE_Demigod;
                    if (prefabSe != null)
                    {
                        var added = __instance.Owner.GetSEMan().AddStatusEffect(prefabSe) as SE_Demigod;
                        StatusEffectField(__instance) = added;
                        __result = added;
                        return false;
                    }
                }
            }

            SE_Demigod current = StatusEffectField(__instance);
            if (current == null && ObjectDB.instance != null)
            {
                current = ObjectDB.instance.GetStatusEffect(hash) as SE_Demigod;
                StatusEffectField(__instance) = current;
            }

            __result = current;
            return false;
        }
    }

    [HarmonyPatch(typeof(SE_Demigod), "GetEnergyPassiveRegenerationTimePeriod")]
    public class YuumiEnergyRegenPatch
    {
        public static bool Prefix(SE_Demigod __instance, Player player, ref float __result)
        {
            if (__instance is SE_DemigodYuumi)
            {
                float skillLevel = player != null ? player.GetSkillLevel(NorseDemigods.Instance.SkillDemigod) : 0f;
                // От 4 сек на 0 уровне до 2 сек на 100 уровне навыка
                __result = Mathf.Lerp(4f, 2f, skillLevel / 100f);
                return false;
            }
            return true;
        }
    }
}


