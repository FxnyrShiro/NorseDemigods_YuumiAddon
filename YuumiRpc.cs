using NorseDemigods.Yuumi.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi;

/// <summary>
///     RPC methods used to apply buffs to allies (and their own client). Every character
///     registers these methods on its <see cref="ZNetView" />.
/// </summary>
internal static class YuumiRpc
{
    internal const string ApplyAttach = "NorseDemigodsYuumi-ApplyAttach";
    internal const string ApplyZoomies = "NorseDemigodsYuumi-ApplyZoomies";

    internal static void Register(ZNetView nview)
    {
        if (nview == null)
        {
            return;
        }

        nview.Register<ZDOID, float, float, float, float>(ApplyAttach, ApplyAttachRpc);
        nview.Register<ZDOID, float, float, float, float>(ApplyZoomies, ApplyZoomiesRpc);
    }

    private static void ApplyAttachRpc(long sender, ZDOID characterId, float duration, float damageBonus,
        float speedBonus, float healPerSecond)
    {
        Character target = FindOwnedCharacter(characterId);
        if (target == null)
        {
            return;
        }

        SE_YuumiAttached.ApplyToAlly(target, duration, damageBonus, speedBonus, healPerSecond);
    }

    private static void ApplyZoomiesRpc(long sender, ZDOID characterId, float duration, float heal,
        float speedBonus, float damageBonus)
    {
        Character target = FindOwnedCharacter(characterId);
        if (target == null)
        {
            return;
        }

        SE_YuumiZoomies.Add(target, duration, heal, speedBonus, damageBonus);
    }

    private static Character FindOwnedCharacter(ZDOID characterId)
    {
        if (ZNetScene.instance == null)
        {
            return null;
        }

        GameObject instance = ZNetScene.instance.FindInstance(characterId);
        if (instance == null || !instance.TryGetComponent(out Character character))
        {
            return null;
        }

        ZNetView nview = instance.GetComponent<ZNetView>();
        if (nview == null || !nview.IsOwner())
        {
            return null;
        }

        return character;
    }
}
