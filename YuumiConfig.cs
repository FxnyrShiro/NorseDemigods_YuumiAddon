using BepInEx.Configuration;
using NorseDemigods;

namespace NorseDemigods.Yuumi;

/// <summary>
///     All settings of the addon. They are bound to the NorseDemigods config file (if available)
///     so that they show up next to the settings of the original mod.
/// </summary>
internal static class YuumiConfig
{
    internal const string DemigodName = "Yuumi";

    private static bool _bound;

    // Prowling Projectile (Q)
    internal static ConfigEntry<int> QCooldown;
    internal static ConfigEntry<int> QEnergyCost;
    internal static ConfigEntry<float> QBaseDamage;
    internal static ConfigEntry<float> QDamagePerLevel;
    internal static ConfigEntry<int> QProjectileSpeed;
    internal static ConfigEntry<int> QProjectileRange;
    internal static ConfigEntry<int> QSeekRadius;
    internal static ConfigEntry<int> QAoeRadius;
    internal static ConfigEntry<int> QSlowAmount;
    internal static ConfigEntry<float> QSlowDuration;

    // You and Me! (W)
    internal static ConfigEntry<int> WCooldown;
    internal static ConfigEntry<int> WEnergyCost;
    internal static ConfigEntry<int> WDuration;
    internal static ConfigEntry<int> WRange;
    internal static ConfigEntry<int> WAllyDamageBonus;
    internal static ConfigEntry<int> WAllySpeedBonus;
    internal static ConfigEntry<int> WAllyHealPerSec;
    internal static ConfigEntry<int> WSelfArmor;
    internal static ConfigEntry<int> WSelfHealPerSec;
    internal static ConfigEntry<int> WBreakRange;

    // Zoomies (E)
    internal static ConfigEntry<int> ECooldown;
    internal static ConfigEntry<int> EEnergyCost;
    internal static ConfigEntry<int> EDuration;
    internal static ConfigEntry<int> EHealAtBaseLevel;
    internal static ConfigEntry<int> EHealAtMaxLevel;
    internal static ConfigEntry<int> ESpeedBonus;
    internal static ConfigEntry<int> EDamageBonus;

    // Final Chapter (R)
    internal static ConfigEntry<int> RCooldown;
    internal static ConfigEntry<int> REnergyCost;
    internal static ConfigEntry<int> RWaves;
    internal static ConfigEntry<float> RWaveInterval;
    internal static ConfigEntry<int> RDamageAtBaseLevel;
    internal static ConfigEntry<int> RDamageAtMaxLevel;
    internal static ConfigEntry<int> RSlowAmount;
    internal static ConfigEntry<float> RSlowDuration;
    internal static ConfigEntry<int> RHealAtBaseLevel;
    internal static ConfigEntry<int> RHealAtMaxLevel;
    internal static ConfigEntry<int> RWaveRadius;
    internal static ConfigEntry<int> RWaveStartDistance;
    internal static ConfigEntry<int> RWaveDistance;

    // Feline Friendship (passive)
    internal static ConfigEntry<int> PBopInterval;
    internal static ConfigEntry<int> PBopRadius;
    internal static ConfigEntry<int> PBopHealAtBaseLevel;
    internal static ConfigEntry<int> PBopHealAtMaxLevel;
    internal static ConfigEntry<int> PBopArmor;
    internal static ConfigEntry<int> PBopArmorDuration;

    internal static void Bind(ConfigFile config)
    {
        if (_bound)
        {
            return;
        }

        _bound = true;

        BindQ(config);
        BindW(config);
        BindE(config);
        BindR(config);
        BindPassive(config);
    }

    private static void BindQ(ConfigFile config)
    {
        string category = Configs.GenerateConfigKey(nameof(Abilities.AbilityProwlingProjectile));
        QCooldown = config.Bind(category, Configs.GenerateConfigKey("AbilityCooldown", "seconds"), 6,
            new ConfigDescription("Determines the time, in seconds, that the player must wait before using the ability again.",
                new AcceptableValueRange<int>(0, 600), Configs.IsAdminOnlyAndAdvanced));
        QEnergyCost = config.Bind(category, Configs.GenerateConfigKey("AbilityEnergyCost"), 1,
            new ConfigDescription("Determines the amount of energy required and consumed in order to cast the ability.",
                new AcceptableValueRange<int>(0, 20), Configs.IsAdminOnlyAndAdvanced));
        QBaseDamage = config.Bind(category, Configs.GenerateConfigKey("BaseDamage"), 8f,
            new ConfigDescription("Spirit damage of the projectile at demigod skill level zero.",
                new AcceptableValueRange<float>(0f, 500f), Configs.IsAdminOnlyAndAdvanced));
        QDamagePerLevel = config.Bind(category, Configs.GenerateConfigKey("DamagePerLevel"), 0.7f,
            new ConfigDescription("Additional spirit damage of the projectile per demigod skill level.",
                new AcceptableValueRange<float>(0f, 10f), Configs.IsAdminOnlyAndAdvanced));
        QProjectileSpeed = config.Bind(category, Configs.GenerateConfigKey("ProjectileSpeed"), 30,
            new ConfigDescription("Determines the speed of the projectile.",
                new AcceptableValueRange<int>(5, 200), Configs.IsAdminOnlyAndAdvanced));
        QProjectileRange = config.Bind(category, Configs.GenerateConfigKey("AbilityRange"), 30,
            new ConfigDescription("Determines how far away the ability can be aimed at.",
                new AcceptableValueRange<int>(1, 100), Configs.IsAdminOnlyAndAdvanced));
        QSeekRadius = config.Bind(category, Configs.GenerateConfigKey("ProjectileTargetSeekRadius"), 30,
            new ConfigDescription("Determines the maximum distance from an enemy the projectile can home in on.",
                new AcceptableValueRange<int>(1, 100), Configs.IsAdminOnlyAndAdvanced));
        QAoeRadius = config.Bind(category, Configs.GenerateConfigKey("AoeDamageRadius"), 2,
            new ConfigDescription("Determines the radius within which nearby enemies will be struck by the projectile.",
                new AcceptableValueRange<int>(1, 10), Configs.IsAdminOnlyAndAdvanced));
        QSlowAmount = config.Bind(category, Configs.GenerateConfigKey("SlowAmount", "%"), 40,
            new ConfigDescription("Determines how much the movement speed of struck enemies is reduced, as a percentage.",
                new AcceptableValueRange<int>(0, 90), Configs.IsAdminOnlyAndAdvanced));
        QSlowDuration = config.Bind(category, Configs.GenerateConfigKey("SlowDuration", "seconds"), 2.5f,
            new ConfigDescription("Determines the time, in seconds, that struck enemies remain slowed.",
                new AcceptableValueRange<float>(0f, 30f), Configs.IsAdminOnlyAndAdvanced));
    }

    private static void BindW(ConfigFile config)
    {
        string category = Configs.GenerateConfigKey(nameof(Abilities.AbilityYouAndMe));
        WCooldown = config.Bind(category, Configs.GenerateConfigKey("AbilityCooldown", "seconds"), 12,
            new ConfigDescription("Determines the time, in seconds, that the player must wait before using the ability again.",
                new AcceptableValueRange<int>(0, 600), Configs.IsAdminOnlyAndAdvanced));
        WEnergyCost = config.Bind(category, Configs.GenerateConfigKey("AbilityEnergyCost"), 1,
            new ConfigDescription("Determines the amount of energy required and consumed in order to cast the ability.",
                new AcceptableValueRange<int>(0, 20), Configs.IsAdminOnlyAndAdvanced));
        WDuration = config.Bind(category, Configs.GenerateConfigKey("AbilityDuration", "seconds"), 30,
            new ConfigDescription("Determines the time, in seconds, that the bond with the ally lasts.",
                new AcceptableValueRange<int>(1, 300), Configs.IsAdminOnlyAndAdvanced));
        WRange = config.Bind(category, Configs.GenerateConfigKey("AbilityRange"), 20,
            new ConfigDescription("Determines how far away an ally can be in order to attach to them.",
                new AcceptableValueRange<int>(1, 50), Configs.IsAdminOnlyAndAdvanced));
        WAllyDamageBonus = config.Bind(category, Configs.GenerateConfigKey("AllyDamageBonus", "%"), 25,
            new ConfigDescription("Determines the damage bonus given to the attached ally, as a percentage.",
                new AcceptableValueRange<int>(0, 200), Configs.IsAdminOnlyAndAdvanced));
        WAllySpeedBonus = config.Bind(category, Configs.GenerateConfigKey("AllySpeedBonus", "%"), 10,
            new ConfigDescription("Determines the movement speed bonus given to the attached ally, as a percentage.",
                new AcceptableValueRange<int>(0, 100), Configs.IsAdminOnlyAndAdvanced));
        WAllyHealPerSec = config.Bind(category, Configs.GenerateConfigKey("AllyHealPerSecond"), 3,
            new ConfigDescription("Determines how much health the attached ally regenerates every second.",
                new AcceptableValueRange<int>(0, 100), Configs.IsAdminOnlyAndAdvanced));
        WSelfArmor = config.Bind(category, Configs.GenerateConfigKey("SelfArmor"), 25,
            new ConfigDescription("Determines the additional armor you gain while attached to an ally.",
                new AcceptableValueRange<int>(0, 200), Configs.IsAdminOnlyAndAdvanced));
        WSelfHealPerSec = config.Bind(category, Configs.GenerateConfigKey("SelfHealPerSecond"), 3,
            new ConfigDescription("Determines how much health you regenerate every second while attached to an ally.",
                new AcceptableValueRange<int>(0, 100), Configs.IsAdminOnlyAndAdvanced));
        WBreakRange = config.Bind(category, Configs.GenerateConfigKey("BreakRange"), 30,
            new ConfigDescription("Determines how far away you can move from the attached ally before the bond breaks.",
                new AcceptableValueRange<int>(1, 100), Configs.IsAdminOnlyAndAdvanced));
    }

    private static void BindE(ConfigFile config)
    {
        string category = Configs.GenerateConfigKey(nameof(Abilities.AbilityZoomies));
        ECooldown = config.Bind(category, Configs.GenerateConfigKey("AbilityCooldown", "seconds"), 8,
            new ConfigDescription("Determines the time, in seconds, that the player must wait before using the ability again.",
                new AcceptableValueRange<int>(0, 600), Configs.IsAdminOnlyAndAdvanced));
        EEnergyCost = config.Bind(category, Configs.GenerateConfigKey("AbilityEnergyCost"), 2,
            new ConfigDescription("Determines the amount of energy required and consumed in order to cast the ability.",
                new AcceptableValueRange<int>(0, 20), Configs.IsAdminOnlyAndAdvanced));
        EDuration = config.Bind(category, Configs.GenerateConfigKey("AbilityDuration", "seconds"), 8,
            new ConfigDescription("Determines the time, in seconds, that the surge of energy lasts.",
                new AcceptableValueRange<int>(1, 60), Configs.IsAdminOnlyAndAdvanced));
        EHealAtBaseLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtBaseLevel"), 20,
            new ConfigDescription("Determines the amount healed at demigod skill level zero.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        EHealAtMaxLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtMaxLevel"), 60,
            new ConfigDescription("Determines the amount healed at the maximum demigod skill level.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        ESpeedBonus = config.Bind(category, Configs.GenerateConfigKey("SpeedBonus", "%"), 30,
            new ConfigDescription("Determines the movement speed bonus, as a percentage.",
                new AcceptableValueRange<int>(0, 100), Configs.IsAdminOnlyAndAdvanced));
        EDamageBonus = config.Bind(category, Configs.GenerateConfigKey("DamageBonus", "%"), 15,
            new ConfigDescription("Determines the damage bonus, as a percentage.",
                new AcceptableValueRange<int>(0, 200), Configs.IsAdminOnlyAndAdvanced));
    }

    private static void BindR(ConfigFile config)
    {
        string category = Configs.GenerateConfigKey(nameof(Abilities.AbilityFinalChapter));
        RCooldown = config.Bind(category, Configs.GenerateConfigKey("AbilityCooldown", "seconds"), 60,
            new ConfigDescription("Determines the time, in seconds, that the player must wait before using the ability again.",
                new AcceptableValueRange<int>(0, 600), Configs.IsAdminOnlyAndAdvanced));
        REnergyCost = config.Bind(category, Configs.GenerateConfigKey("AbilityEnergyCost"), 3,
            new ConfigDescription("Determines the amount of energy required and consumed in order to cast the ability.",
                new AcceptableValueRange<int>(0, 30), Configs.IsAdminOnlyAndAdvanced));
        RWaves = config.Bind(category, Configs.GenerateConfigKey("Waves"), 5,
            new ConfigDescription("Determines the number of waves.",
                new AcceptableValueRange<int>(1, 10), Configs.IsAdminOnlyAndAdvanced));
        RWaveInterval = config.Bind(category, Configs.GenerateConfigKey("WaveInterval", "seconds"), 0.45f,
            new ConfigDescription("Determines the time, in seconds, between waves.",
                new AcceptableValueRange<float>(0.1f, 5f), Configs.IsAdminOnlyAndAdvanced));
        RDamageAtBaseLevel = config.Bind(category, Configs.GenerateConfigKey("DamageAtBaseLevel"), 10,
            new ConfigDescription("Determines the spirit damage of each wave at demigod skill level zero.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        RDamageAtMaxLevel = config.Bind(category, Configs.GenerateConfigKey("DamageAtMaxLevel"), 35,
            new ConfigDescription("Determines the spirit damage of each wave at the maximum demigod skill level.",
                new AcceptableValueRange<int>(0, 1000), Configs.IsAdminOnlyAndAdvanced));
        RSlowAmount = config.Bind(category, Configs.GenerateConfigKey("SlowAmount", "%"), 60,
            new ConfigDescription("Determines how much the movement speed of struck enemies is reduced, as a percentage.",
                new AcceptableValueRange<int>(0, 90), Configs.IsAdminOnlyAndAdvanced));
        RSlowDuration = config.Bind(category, Configs.GenerateConfigKey("SlowDuration", "seconds"), 1.5f,
            new ConfigDescription("Determines the time, in seconds, that struck enemies remain slowed.",
                new AcceptableValueRange<float>(0f, 30f), Configs.IsAdminOnlyAndAdvanced));
        RHealAtBaseLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtBaseLevel"), 8,
            new ConfigDescription("Determines the amount healed to allies by each wave at demigod skill level zero.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        RHealAtMaxLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtMaxLevel"), 30,
            new ConfigDescription("Determines the amount healed to allies by each wave at the maximum demigod skill level.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        RWaveRadius = config.Bind(category, Configs.GenerateConfigKey("WaveRadius"), 3,
            new ConfigDescription("Determines the radius of each wave.",
                new AcceptableValueRange<int>(1, 10), Configs.IsAdminOnlyAndAdvanced));
        RWaveStartDistance = config.Bind(category, Configs.GenerateConfigKey("WaveStartDistance"), 2,
            new ConfigDescription("Determines the distance of the first wave from the caster.",
                new AcceptableValueRange<int>(0, 20), Configs.IsAdminOnlyAndAdvanced));
        RWaveDistance = config.Bind(category, Configs.GenerateConfigKey("WaveDistance"), 3,
            new ConfigDescription("Determines the distance between waves.",
                new AcceptableValueRange<int>(1, 20), Configs.IsAdminOnlyAndAdvanced));
    }

    private static void BindPassive(ConfigFile config)
    {
        string category = Configs.GenerateConfigKey("YuumiPassive");
        PBopInterval = config.Bind(category, Configs.GenerateConfigKey("BopInterval", "seconds"), 20,
            new ConfigDescription("Determines how often Feline Friendship becomes ready.",
                new AcceptableValueRange<int>(1, 300), Configs.IsAdminOnlyAndAdvanced));
        PBopRadius = config.Bind(category, Configs.GenerateConfigKey("BopRadius"), 12,
            new ConfigDescription("Determines the radius within which Feline Friendship heals allies.",
                new AcceptableValueRange<int>(1, 50), Configs.IsAdminOnlyAndAdvanced));
        PBopHealAtBaseLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtBaseLevel"), 10,
            new ConfigDescription("Determines the amount healed by Feline Friendship at demigod skill level zero.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        PBopHealAtMaxLevel = config.Bind(category, Configs.GenerateConfigKey("HealAmountAtMaxLevel"), 40,
            new ConfigDescription("Determines the amount healed by Feline Friendship at the maximum demigod skill level.",
                new AcceptableValueRange<int>(0, 500), Configs.IsAdminOnlyAndAdvanced));
        PBopArmor = config.Bind(category, Configs.GenerateConfigKey("ArmorBonus"), 15,
            new ConfigDescription("Determines the additional armor granted by Feline Friendship.",
                new AcceptableValueRange<int>(0, 200), Configs.IsAdminOnlyAndAdvanced));
        PBopArmorDuration = config.Bind(category, Configs.GenerateConfigKey("ArmorBonusDuration", "seconds"), 5,
            new ConfigDescription("Determines the time, in seconds, that the armor bonus lasts.",
                new AcceptableValueRange<int>(1, 60), Configs.IsAdminOnlyAndAdvanced));
    }
}
