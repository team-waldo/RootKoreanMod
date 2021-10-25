using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using HarmonyLib;

namespace RootKoreanMod
{
    public static class Patch
    {
        [HarmonyPatch(typeof(dwd.core.localization.LocalizationDB), "SetPairs", argumentTypes: new Type[] { typeof(IEnumerable<KeyValuePair<string, string>>) })]
        public class LocalizationDB_SetPairs_Patch
        {
            public static bool Prefix(ref IEnumerable<KeyValuePair<string, string>> pairs)
            {
                if (ModMain.Translation != null)
                {
                    var dict = (Dictionary<string, string>) pairs;

                    foreach (var item in ModMain.Translation)
                    {
                        dict[item.Key] = item.Value;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(TMP_FontAsset), "Awake")]
        public class TMP_FontAsset_Awake_Patch
        {
            public static void Postfix(TMP_FontAsset __instance)
            {
                ModMain.AddFallbackFont(__instance);
            }
        }
    }
}
