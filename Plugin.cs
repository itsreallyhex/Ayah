using System;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx;
using TMPro;
using UnityEngine;

namespace Ayah
{
    [BepInPlugin("comiItsreallyhex.ayah", "Ayah", "1.0.0"), HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        // ═══════════════════════════════════════════════════════════════
        //  WANT TO ADD MORE CONTENT? READ THIS FIRST
        // ═══════════════════════════════════════════════════════════════
        //
        //  Add new entries inside the Ayahs array below.
        //  Each entry must follow this exact format:
        //
        //  FORMAT FOR QUR'AN VERSE:
        //  "\"[verse text]\" – Quran [surah]:[ayah]",
        //
        //  FORMAT FOR HADITH:
        //  "\"[hadith text]\" – Prophet Muhammad SAW ([source])",
        //
        //  RULES:
        //  1. Hadiths MUST be Sahih (authentic) — preferred sources: Bukhari, Muslim, Tirmidhi
        //  2. Do NOT add weak (da'if) or fabricated hadiths
        //  3. Keep entries short — long text will overflow in-game
        //  4. Check for duplicates before adding
        //  5. This project is licensed under GPL v3 — keep it open source
        //
        // ═══════════════════════════════════════════════════════════════
        private static readonly string[] Ayahs = new string[]
        {
              // ─── QUR'AN ───────────────────────────────────────────────
            "\"Allah does not burden a soul beyond that it can bear.\" – Quran 2:286",
            "\"Indeed, with hardship comes ease.\" – Quran 94:6",
            "\"And He is with you wherever you are.\" – Quran 57:4",
            "\"So remember Me; I will remember you.\" – Quran 2:152",
            "\"Do not despair of the mercy of Allah.\" – Quran 39:53",
            "\"And Allah is the best of planners.\" – Quran 8:30",
            "\"Indeed, Allah is with the patient.\" – Quran 2:153",
            "\"And He found you lost and guided you.\" – Quran 93:7",
            "\"Whoever does an atom's weight of good will see it.\" – Quran 99:7",
            "\"And worship your Lord until certainty comes to you.\" – Quran 15:99",
            "\"Indeed I am near. I respond to the call of the caller when he calls.\" – Quran 2:186",
            "\"Allah will not change the condition of a people until they change themselves.\" – Quran 13:11",
            "\"And do good; indeed, Allah loves the doers of good.\" – Quran 2:195",
            "\"Every soul will taste death.\" – Quran 21:35",
            "\"He is the Forgiving, the Loving.\" – Quran 85:14",
            "\"And rely upon Allah; sufficient is Allah as Disposer of affairs.\" – Quran 33:3",
            "\"Does He who created not know, while He is the Subtle, the Acquainted?\" – Quran 67:14",
            "\"And strive for Allah with the striving due to Him.\" – Quran 22:78",
            "\"My Lord, increase me in knowledge.\" – Quran 20:114",
            "\"Verily, the most noble of you in the sight of Allah is the most righteous.\" – Quran 49:13",

            // ─── HADITH ───────────────────────────────────────────────
            "\"Actions are judged by intentions.\" – Prophet Muhammad SAW (Bukhari)",
            "\"The strong man is not the one who wrestles others down. The strong man is the one who controls himself when angry.\" – Prophet Muhammad SAW (Bukhari)",
            "\"None of you truly believes until he loves for his brother what he loves for himself.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Make things easy; do not make them difficult.\" – Prophet Muhammad SAW (Bukhari)",
            "\"The best of you are those with the best character.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Speak good or remain silent.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Smiling at your brother is an act of charity.\" – Prophet Muhammad SAW (Tirmidhi)",
            "\"If Allah wants good for someone, He gives him understanding of the religion.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Leave what makes you doubt for what does not make you doubt.\" – Prophet Muhammad SAW (Tirmidhi)",
            "\"The most beloved deeds to Allah are the most consistent, even if small.\" – Prophet Muhammad SAW (Bukhari)",
            "\"He who does not show mercy will not be shown mercy.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Feed the hungry, visit the sick, and free the captive.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Whoever believes in Allah and the Last Day, let him speak good or remain silent.\" – Prophet Muhammad SAW (Bukhari)",
            "\"Tie your camel, then put your trust in Allah.\" – Prophet Muhammad SAW (Tirmidhi)",
            "\"Live in this world as if you are a stranger or a wayfarer.\" – Prophet Muhammad SAW (Bukhari)",
            "\"The cure for ignorance is to question.\" – Prophet Muhammad SAW (Abu Dawud)",
            "\"Whoever removes a worldly hardship from a believer, Allah will remove from him a hardship on the Day of Resurrection.\" – Prophet Muhammad SAW (Muslim)",
            "\"Do not belittle any act of kindness, even greeting your brother with a cheerful face.\" – Prophet Muhammad SAW (Muslim)",
            "\"Part of the perfection of one's Islam is leaving what does not concern him.\" – Prophet Muhammad SAW (Tirmidhi)",
            "\"Whoever takes a path seeking knowledge, Allah makes easy for him a path to Paradise.\" – Prophet Muhammad SAW (Muslim)",
        };

        private void Awake()
        {
            Logger.LogInfo("Ayah plugin loaded.");
            Harmony.CreateAndPatchAll(GetType().Assembly, Info.Metadata.GUID);
        }
         
        
        [HarmonyPatch(typeof(PlayFabTitleDataTextDisplay), "OnTitleDataRequestComplete")]
        private static bool Prefix(ref PlayFabTitleDataTextDisplay __instance)
        {
            if (!__instance.name.Contains("motd")) return true;

            var textBox = Traverse.Create(__instance)
                                  .Field("textBox")
                                  .GetValue<TextMeshPro>();
            foreach (var field in __instance.GetType().GetFields(
          System.Reflection.BindingFlags.NonPublic |
          System.Reflection.BindingFlags.Instance |
          System.Reflection.BindingFlags.Public))
            {
                BepInEx.Logging.Logger.CreateLogSource("Ayah").LogInfo($"Field: {field.Name}");
            }


            if (textBox == null) return true;

            textBox.richText = true;

            string verse = Ayahs[UnityEngine.Random.Range(0, Ayahs.Length)];
            textBox.text = "<b>AYAH OF THE DAY</b>\n" + verse;

            return false;
        }
    }
}
