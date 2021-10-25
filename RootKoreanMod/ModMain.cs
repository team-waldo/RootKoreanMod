using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace RootKoreanMod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class ModMain : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "akintos.rootkoreanmod";
        const string PLUGIN_NAME = "RootKoreanMod";
        const string PLUGIN_VERSION = "1.0.0";

        const string ASSETBUNDLE_FILENAME = "sourcehanserifk.unity3d";
        const string TRANSLATION_FILENAME = "translation.csv";

        public string PluginDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static AssetBundle FontBundle { get; private set; }
        public static IDictionary<string, string> Translation { get; private set; }

        public void Awake()
        {
            Translation = LoadTranslation();
        }

        public IDictionary<string, string> LoadTranslation()
        {
            string csvPath = Path.Combine(PluginDirectory, TRANSLATION_FILENAME);
            var csvOptions = new Csv.CsvOptions() { AllowNewLineInEnclosedFieldValues = true };

            var dict = new Dictionary<string, string>();

            using (var sr = new StreamReader(csvPath))
            {
                foreach (var item in Csv.CsvReader.Read(sr, csvOptions))
                {
                    if (item.ColumnCount >= 3 && !string.IsNullOrWhiteSpace(item[2]))
                    {
                        dict[item[0]] = item[2];
                    }
                }
            }
            return dict;
        }

        public void Start()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            try
            {
                LoadFontBundle();
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
            }
        }

        public void LoadFontBundle()
        {
            if (FontBundle == null)
            {
                string assetBundlePath = Path.Combine(PluginDirectory, ASSETBUNDLE_FILENAME);

                Logger.LogInfo($"Assetbundle file path : {assetBundlePath}");
                Logger.LogInfo($"Assetbundle file exists : {File.Exists(assetBundlePath)}");

                FontBundle = AssetBundle.LoadFromFile(assetBundlePath);
                Logger.LogInfo($"Assetbundle loaded : {FontBundle != null}");
            }
        }
    }
}
