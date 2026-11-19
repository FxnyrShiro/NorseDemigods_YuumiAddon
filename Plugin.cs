using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NorseDemigods;
using NorseDemigods.Yuumi.Abilities;
using UnityEngine;
using NorseDemigods.StatusEffects;

namespace NorseDemigods.Yuumi
{
    [BepInPlugin("com.yuumiaddon.plugin", "YuumiAddon", "1.0.0")]
    [BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("NorseDemigods", BepInDependency.DependencyFlags.HardDependency)]

    public class YuumiPlugin : BaseUnityPlugin
    {
        public static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = base.Logger;

            YuumiIcons.Load();
            YuumiLocalization.Register();
			YuumiLocalization.EnsureSubscribed();

            var harmony = new Harmony("com.yuumiaddon.plugin");
            harmony.PatchAll();

            // Пробуем зарегистрировать сразу (если NorseDemigods уже загрузил дефолты)
            YuumiRegistration.Register();
        }

        private void Start()
        {
            Log.LogInfo("Yuumi Addon loaded successfully!");
        }
    }

     [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public class ObjectDBAwakePatch
    {
        public static void Postfix(ObjectDB __instance)
        {
            RegisterStatusEffect(__instance);
        }

        internal static void RegisterStatusEffect(ObjectDB db)
        {
            if (db == null || db.m_StatusEffects == null)
            {
                return;
            }

            if (!db.m_StatusEffects.Exists(se => se != null && se.name == "SE_DemigodYuumi"))
            {
                var se = ScriptableObject.CreateInstance<SE_DemigodYuumi>();
                se.name = "SE_DemigodYuumi";
                db.m_StatusEffects.Add(se);
            }
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
    public class ObjectDBCopyPatch
    {
        public static void Postfix(ObjectDB __instance)
        {
            ObjectDBAwakePatch.RegisterStatusEffect(__instance);
        }
    }

    [HarmonyPatch(typeof(Demigod), "SetupDefaults")]
    public class DemigodDefaultsPatch
    {
        public static void Postfix()
        {
            YuumiRegistration.Register();
        }
    }

    [HarmonyPatch(typeof(Localization), "SetupLanguage")]
    public class LocalizationSetupPatch
    {
        public static void Postfix()
        {
            YuumiLocalization.ApplyToGame();
        }
    }

    [HarmonyPatch(typeof(Character), "Awake")]
    public class CharacterAwakePatch
    {
        public static void Postfix(Character __instance)
        {
            if (__instance != null)
            {
                YuumiRpc.Register(__instance.GetComponent<ZNetView>());
            }
        }
    }

    [HarmonyPatch(typeof(Projectile), "OnHit")]
    public class ProjectileOnHitPatch
    {
        public static void Prefix(Projectile __instance, Collider collider)
        {
            if (__instance == null || collider == null)
            {
                return;
            }

            if (__instance.TryGetComponent(out ProwlingProjectileTracker tracker) && tracker.Ability != null)
            {
                GameObject hitObject = Projectile.FindHitObject(collider);
                if (hitObject != null && hitObject.TryGetComponent(out Character victim))
                {
                    tracker.Ability.OnProjectileHitEnemy(victim);
                }
            }
        }
    }
}
