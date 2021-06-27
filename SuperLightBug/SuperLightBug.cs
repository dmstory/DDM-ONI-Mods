using HarmonyLib;
using UnityEngine;

namespace SuperLightBug
{
    [HarmonyPatch(typeof(BaseLightBugConfig), "BaseLightBug")]
    public class SuperLightBugMod : KMod.UserMod2
    {
        [HarmonyPostfix]
        public static void BaseLightBugPost(GameObject __result)
        {
            Light2D light2D = __result.AddOrGet<Light2D>();
            if (light2D == null)
                return;
            light2D.Lux = 50000;
        }
    }
}
