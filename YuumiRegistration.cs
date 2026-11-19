using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using NorseDemigods.Attributes;
using NorseDemigods.Yuumi.Abilities;
using NorseDemigods.Yuumi.Demigods;
using UnityEngine;

namespace NorseDemigods.Yuumi;

internal static class YuumiRegistration
{
    internal static void Register()
    {
        try
        {
            List<DemigodDefaults> defaults = Demigod.DemigodDefaults;
            if (defaults == null)
            {
                YuumiPlugin.Log.LogWarning("NorseDemigods has no demigod definitions loaded; Yuumi was not registered.");
                return;
            }

            if (defaults.Exists(x => string.Equals(x.Name, YuumiConfig.DemigodName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            ConfigFile config = NorseDemigods.Instance != null ? NorseDemigods.Instance.Config : null;
            if (config == null)
            {
                YuumiPlugin.Log.LogError("NorseDemigods instance not available; cannot bind Yuumi settings.");
                return;
            }

            YuumiConfig.Bind(config);

            List<DemigodAbility> abilities = new List<DemigodAbility>
            {
                new AbilityProwlingProjectile(
                    new DemigodAbility.AbilityParametersStruct(DemigodAbility.AbilityType.SLOT_1)),
                new AbilityYouAndMe(new DemigodAbility.AbilityParametersStruct(DemigodAbility.AbilityType.SLOT_2)),
                new AbilityZoomies(new DemigodAbility.AbilityParametersStruct(DemigodAbility.AbilityType.SLOT_3)),
                new AbilityFinalChapter(new DemigodAbility.AbilityParametersStruct(DemigodAbility.AbilityType.SLOT_4))
            };

            List<DemigodAttribute> attributes = new List<DemigodAttribute>
            {
                (DemigodAttribute)Activator.CreateInstance(typeof(AttributePurity)),
                (DemigodAttribute)Activator.CreateInstance(typeof(AttributeRegenerativeFlesh)),
                (DemigodAttribute)Activator.CreateInstance(typeof(AttributeFoxLegs)),
                (DemigodAttribute)Activator.CreateInstance(typeof(AttributeDeerEndurance)),
                (DemigodAttribute)Activator.CreateInstance(typeof(AttributeGlide))
            };

            defaults.Add(new DemigodDefaults(YuumiConfig.DemigodName, typeof(DemigodYuumi), abilities, attributes));
            YuumiPlugin.Log.LogInfo($"Registered demigod '{YuumiConfig.DemigodName}' with {abilities.Count} abilities and {attributes.Count} attributes.");
        }
        catch (Exception exception)
        {
            YuumiPlugin.Log.LogError($"Failed to register Yuumi: {exception}");
        }
    }
}
