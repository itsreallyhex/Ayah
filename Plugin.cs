using System;
using System.Collections;
using System.Linq;
using HarmonyLib;
using BepInEx;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Ayah
{
    [BepInPlugin("comiItsreallyhex.ayah", "Ayah", "1.0.0"), HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string ApiUrl = "https://itsreallyhex.github.io/HexoAPI/ayahs.json";

        [Serializable]
        private class AyahData
        {
            public string[] quran = Array.Empty<string>();
            public string[] hadith = Array.Empty<string>();
        }

        // 10 hardcoded fallback entries used when the web request fails
        private static readonly string[] FallbackAyahs =
        {
            "\"Allah does not burden a soul beyond that it can bear.\" – Quran 2:286",
            "\"Indeed, with hardship comes ease.\" – Quran 94:6",
            "\"And He is with you wherever you are.\" – Quran 57:4",
            "\"Do not despair of the mercy of Allah.\" – Quran 39:53",
            "\"Call upon Me; I will respond to you.\" – Quran 40:60",
            "\"Actions are judged by intentions.\" – Prophet Muhammad SAW (Bukhari)",
            "\"The best of you are those with the best character.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Speak good or remain silent.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Make things easy; do not make them difficult.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Every act of goodness is charity.\" – Prophet Muhammad SAW (Muslim)"
        };

        private static string[] _ayahs = Array.Empty<string>();

        private void Awake()
        {
            Logger.LogInfo("Ayah plugin loaded.");
            Harmony.CreateAndPatchAll(GetType().Assembly, Info.Metadata.GUID);
            StartCoroutine(FetchAyahsFromApi());
        }

        private IEnumerator FetchAyahsFromApi()
        {
            using (UnityWebRequest req = UnityWebRequest.Get(ApiUrl))
            {
                req.timeout = 10;
                yield return req.SendWebRequest();

#pragma warning disable CS0618
                bool failed = req.isNetworkError || req.isHttpError;
#pragma warning restore CS0618

                if (failed)
                {
                    Logger.LogWarning($"Failed to fetch ayahs from HexoAPI ({req.error}). Using fallback.");
                    _ayahs = FallbackAyahs;
                    yield break;
                }

                try
                {
                    AyahData data = JsonUtility.FromJson<AyahData>(req.downloadHandler.text);
                    string[] combined = (data.quran ?? Array.Empty<string>())
                        .Concat(data.hadith ?? Array.Empty<string>())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray();

                    if (combined.Length == 0)
                    {
                        Logger.LogWarning("HexoAPI returned empty data. Using fallback.");
                        _ayahs = FallbackAyahs;
                    }
                    else
                    {
                        _ayahs = combined;
                        Logger.LogInfo($"Loaded {_ayahs.Length} entries from HexoAPI.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to parse HexoAPI response: {ex.Message}. Using fallback.");
                    _ayahs = FallbackAyahs;
                }
            }
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