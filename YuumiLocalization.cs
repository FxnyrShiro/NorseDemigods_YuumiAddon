using System.Collections.Generic;
using Jotunn.Managers;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace NorseDemigods.Yuumi;

internal static class YuumiLocalization
{
    private static readonly Dictionary<string, string> English = new Dictionary<string, string>
    {
        { "demigod_yuumi_name", "Yuumi" },
        {
            "demigod_yuumi_description",
            "A magical cat from another world, bound to the Book of Thresholds. Yuumi is a support demigod: she heals and empowers her allies, bonds with them through 'You and Me!', and finishes fights with 'Final Chapter'."
        },

        { "ability_prowlingprojectile_name", "Prowling Projectile" },
        {
            "ability_prowlingprojectile_tooltip",
            "Fire a homing bolt of feline magic that deals <color=orange>{0}</color> spirit damage and slows struck enemies by <color=orange>{1}%</color> for <color=orange>{2}</color> seconds."
        },
        { "ability_prowlingprojectile_status_tooltip", "Damage: <color=orange>{0}</color>" },

        { "ability_youandme_name", "You and Me!" },
        {
            "ability_youandme_tooltip",
            "Leap to an ally and bond with them for <color=orange>{0}</color> seconds. The ally gains <color=orange>{1}%</color> damage, <color=orange>{2}%</color> movement speed and <color=orange>{3}</color> health per second, while you gain armor and regeneration. Recast to break the bond."
        },
        { "ability_youandme_status_tooltip", "Bonded with an ally" },

        { "ability_zoomies_name", "Zoomies" },
        {
            "ability_zoomies_tooltip",
            "Surge with energy, healing yourself and your bonded ally for <color=orange>{0}</color> and granting <color=orange>{1}%</color> movement speed and <color=orange>{2}%</color> damage for <color=orange>{3}</color> seconds."
        },
        { "ability_zoomies_status_tooltip", "Healing surge ready" },

        { "ability_finalchapter_name", "Final Chapter" },
        {
            "ability_finalchapter_tooltip",
            "Release <color=orange>{0}</color> waves of feline magic, each dealing <color=orange>{1}</color> spirit damage, slowing enemies by <color=orange>{2}%</color> for <color=orange>{3}</color> seconds and healing allies for <color=orange>{4}</color>."
        },
        { "ability_finalchapter_status_tooltip", "Waves: <color=orange>{0}</color>" },

        { "se_yuumi_attached_name", "You and Me!" },
        {
            "se_yuumi_attached_tooltip",
            "Bonded with Yuumi: <color=orange>+{0}%</color> damage, <color=orange>+{1}%</color> movement speed and <color=orange>{2}</color> health per second."
        },
        { "se_yuumi_attached_self_name", "You and Me!" },
        {
            "se_yuumi_attached_self_tooltip",
            "Attached to an ally: <color=orange>+{0}</color> armor and <color=orange>{1}</color> health per second."
        },
        { "se_yuumi_zoomies_name", "Zoomies" },
        {
            "se_yuumi_zoomies_tooltip",
            "Zoomies: <color=orange>+{0}%</color> movement speed and <color=orange>+{1}%</color> damage."
        },
        { "se_yuumi_slow_name", "Prowling Projectile" },
        { "se_yuumi_slow_tooltip", "Slowed by <color=orange>{0}%</color>." },
        { "se_yuumi_bop_name", "Feline Friendship" },
        { "se_yuumi_bop_tooltip", "Feline Friendship: <color=orange>+{0}</color> armor." },

        { "yuumi_bop_ready_message", "Feline Friendship is ready!" },
        { "yuumi_bop_triggered_message", "Feline Friendship heals you and your allies!" },
        { "yuumi_no_ally_message", "There is no ally nearby to attach to." },
        { "yuumi_attach_message", "You bond with {0}." },
        { "yuumi_attach_broken_message", "Your bond with the ally has broken." }
    };

    private static readonly Dictionary<string, string> Russian = new Dictionary<string, string>
    {
        { "demigod_yuumi_name", "Юми" },
        {
            "demigod_yuumi_description",
            "Волшебная кошка из другого мира, привязанная к Книге Порогов. Юми — бог поддержки: она лечит и усиливает союзников, привязывается к ним способностью «Ты и я!» и завершает бой «Последней главой»."
        },

        { "ability_prowlingprojectile_name", "Крадущийся снаряд" },
        {
            "ability_prowlingprojectile_tooltip",
            "Выпускает самонаводящийся снаряд кошачьей магии, наносящий <color=orange>{0}</color> урона духом и замедляющий задетых врагов на <color=orange>{1}%</color> на <color=orange>{2}</color> сек."
        },
        { "ability_prowlingprojectile_status_tooltip", "Урон: <color=orange>{0}</color>" },

        { "ability_youandme_name", "Ты и я!" },
        {
            "ability_youandme_tooltip",
            "Прыгает к союзнику и связывает себя с ним на <color=orange>{0}</color> сек. Союзник получает <color=orange>{1}%</color> урона, <color=orange>{2}%</color> скорости передвижения и <color=orange>{3}</color> здоровья в секунду, а вы — броню и регенерацию. Повторное применение разрывает связь."
        },
        { "ability_youandme_status_tooltip", "Связь с союзником" },

        { "ability_zoomies_name", "Скорость" },
        {
            "ability_zoomies_tooltip",
            "Всплеск энергии: лечит вас и связанного союзника на <color=orange>{0}</color>, даёт <color=orange>{1}%</color> скорости передвижения и <color=orange>{2}%</color> урона на <color=orange>{3}</color> сек."
        },
        { "ability_zoomies_status_tooltip", "Всплеск энергии готов" },

        { "ability_finalchapter_name", "Последняя глава" },
        {
            "ability_finalchapter_tooltip",
            "Выпускает <color=orange>{0}</color> волн кошачьей магии; каждая наносит <color=orange>{1}</color> урона духом, замедляет врагов на <color=orange>{2}%</color> на <color=orange>{3}</color> сек. и лечит союзников на <color=orange>{4}</color>."
        },
        { "ability_finalchapter_status_tooltip", "Волн: <color=orange>{0}</color>" },

        { "se_yuumi_attached_name", "Ты и я!" },
        {
            "se_yuumi_attached_tooltip",
            "Связь с Юми: <color=orange>+{0}%</color> урона, <color=orange>+{1}%</color> скорости передвижения и <color=orange>{2}</color> здоровья в секунду."
        },
        { "se_yuumi_attached_self_name", "Ты и я!" },
        {
            "se_yuumi_attached_self_tooltip",
            "Вы привязаны к союзнику: <color=orange>+{0}</color> брони и <color=orange>{1}</color> здоровья в секунду."
        },
        { "se_yuumi_zoomies_name", "Скорость" },
        {
            "se_yuumi_zoomies_tooltip",
            "Скорость: <color=orange>+{0}%</color> к скорости передвижения и <color=orange>+{1}%</color> к урону."
        },
        { "se_yuumi_slow_name", "Крадущийся снаряд" },
        { "se_yuumi_slow_tooltip", "Замедление на <color=orange>{0}%</color>." },
        { "se_yuumi_bop_name", "Кошачья дружба" },
        { "se_yuumi_bop_tooltip", "Кошачья дружба: <color=orange>+{0}</color> брони." },

        { "yuumi_bop_ready_message", "Кошачья дружба готова!" },
        { "yuumi_bop_triggered_message", "Кошачья дружба лечит вас и союзников!" },
        { "yuumi_no_ally_message", "Рядом нет союзника, к которому можно привязаться." },
        { "yuumi_attach_message", "Вы привязались к {0}." },
        { "yuumi_attach_broken_message", "Связь с союзником разорвана." }
    };

    internal static void Register()
    {
        NorseDemigods.Localization.AddTranslation("English", English);
        NorseDemigods.Localization.AddTranslation("Russian", Russian);
    }

internal static void ApplyToGame()
        {
            Localization instance = Localization.instance;
            if (instance == null)
            {
                return;
            }

            MethodInfo addWord = AccessTools.Method(typeof(Localization), "AddWord");
            if (addWord == null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> translation in English)
            {
                addWord.Invoke(instance, new object[] { translation.Key, translation.Value });
            }

            string language = PlayerPrefs.GetString("language", "English");
            if (!string.Equals(language, "English", System.StringComparison.OrdinalIgnoreCase))
            {
                Dictionary<string, string> translations = null;
                if (string.Equals(language, "Russian", System.StringComparison.OrdinalIgnoreCase))
                {
                    translations = Russian;
                }

                if (translations != null)
                {
                    foreach (KeyValuePair<string, string> translation in translations)
                    {
                        addWord.Invoke(instance, new object[] { translation.Key, translation.Value });
                    }
                }
            }
        }

    internal static void EnsureSubscribed()
    {
        LocalizationManager.OnLocalizationAdded -= ApplyToGame;
        LocalizationManager.OnLocalizationAdded += ApplyToGame;
    }

    internal static string T(string key)
    {
        return NorseDemigods.Localization.TryTranslate(key);
    }

}
