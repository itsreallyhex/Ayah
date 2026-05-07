using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using BepInEx;
using TMPro;
using UnityEngine;

namespace Ayah
{
    [BepInPlugin("comiItsreallyhex.ayah", "Ayah", "1.0.0"), HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        [Serializable]
        private class AyahData
        {
            public string[] quran = Array.Empty<string>();
            public string[] hadith = Array.Empty<string>();
        }

        private static string[] _ayahs = Array.Empty<string>();

        private void Awake()
        {
            Logger.LogInfo("Ayah plugin loaded.");

            string filePath = Path.Combine(Path.GetDirectoryName(Info.Location), "ayahs.json");

            if (File.Exists(filePath))
            {
                try
                {
                    AyahData data = JsonUtility.FromJson<AyahData>(File.ReadAllText(filePath));
                    _ayahs = (data.quran ?? Array.Empty<string>())
                        .Concat(data.hadith ?? Array.Empty<string>())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray();
                    Logger.LogInfo($"Loaded {_ayahs.Length} entries from ayahs.json.");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to parse ayahs.json: {ex.Message}");
                }
            }
            else
            {
                Logger.LogWarning($"ayahs.json not found at: {filePath}");
            }

            Harmony.CreateAndPatchAll(GetType().Assembly, Info.Metadata.GUID);
        }

        [HarmonyPatch(typeof(PlayFabTitleDataTextDisplay), "OnTitleDataRequestComplete")]
        private static bool Prefix(ref PlayFabTitleDataTextDisplay __instance)
        {
            if (!__instance.name.Contains("motd")) return true;

            var textBox = Traverse.Create(__instance)
                                  .Field("textBox")
                                  .GetValue<TextMeshPro>();

            if (textBox == null || _ayahs.Length == 0) return true;

            textBox.richText = true;

            string verse = _ayahs[UnityEngine.Random.Range(0, _ayahs.Length)];
            textBox.text = "<b>AYAH OF THE DAY</b>\n" + verse;

            return false;
        }
    }
}