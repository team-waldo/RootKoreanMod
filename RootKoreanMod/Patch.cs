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

        [HarmonyPatch(typeof(Canis.utils.localization.L), "LT", argumentTypes: new Type[] { typeof(string) })]
        public class L_LT_Patch
        {
            public static void Postfix(string key, ref string __result)
            {
                if (ModMain.Translation == null)
                {
                    return;
                }

                if (ModMain.Translation.TryGetValue(key, out string translation))
                {
                    __result = translation;
                }
            }
        }

        private static IDictionary<string, string> FontMapping = new Dictionary<string, string>()
        {
            { "SourceHanSerifSC-Regular SDF", "SourceHanSerifK-Regular SDF" },
            { "SourceHanSerifSC-Bold SDF", "SourceHanSerifK-Bold SDF" },
            { "SourceHanSerifSC-Regular SDF_BlackStroke", "SourceHanSerifK-Bold SDF" },
        };

        [HarmonyPatch(typeof(TMP_FontAsset), "Awake")]
        public class TMP_FontAsset_Awake_Patch
        {
            public static void Postfix(TMP_FontAsset __instance)
            {
                if (ModMain.FontBundle == null)
                {
                    return;
                }

                if (FontMapping.TryGetValue(__instance.name, out var krfontname))
                {
                    var krfont = ModMain.FontBundle.LoadAsset<TMP_FontAsset>(krfontname);
                    if (!__instance.fallbackFontAssetTable.Contains(krfont))
                    {
                        __instance.fallbackFontAssetTable.Add(krfont);
                    }
                }
            }
        }
    }
}
