using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace MoreSkins;
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    private class CharacterAwakePatch
    {
        private static bool CreateSkinOption(Customization customization, string name, Color color)
        {
            if (Array.Exists(customization.skins, skin => skin.name == name) || Array.Exists(customization.skins, skin => skin.color == color)) return false;
            var skinOption = ScriptableObject.CreateInstance<CustomizationOption>();
            skinOption.color = color;
            skinOption.name = name;
            skinOption.type = Customization.Type.Skin;
            customization.skins = customization.skins.AddToArray(skinOption);
            return true;
        }


        [HarmonyPatch(typeof(Character), "Awake")]
        [HarmonyPostfix]
        private static void CharacterAwakePostfix(Character __instance)
        {
            
            CharacterCustomization.SetCharacterSkinColor(8);
            // Your code here, runs after Character.Awake
        }
        [HarmonyPatch(typeof(PassportManager), "Awake")]
        [HarmonyPostfix]
        private static void PassportManagerAwakePostfix(PassportManager __instance)
        {
            var customization = __instance.GetComponent<Customization>();
            Log.LogInfo("Running PassportManager Awake Patch");
            CreateSkinOption(customization, "Skin_White", Color.white);
            CreateSkinOption(customization, "Skin_Black", new Color(0.2f, 0.2f, 0.2f, 1));
            CreateSkinOption(customization, "Skin_SkinTone1", new Color32(141, 85, 36, 1));
            CreateSkinOption(customization, "Skin_SkinTone2", new Color32(198, 134, 66, 1));
            CreateSkinOption(customization, "Skin_SkinTone3", new Color32(224, 172, 105, 1));
            CreateSkinOption(customization, "Skin_SkinTone4", new Color32(241, 194, 125, 1));
            CreateSkinOption(customization, "Skin_SkinTone5", new Color32(255, 219, 172, 1));
        }
        
        /*[HarmonyPatch(typeof(Customization), "Awake")]
        [HarmonyPostfix]
        private static void CustomizationAwakePostfix(Customization __instance)
        {
            Log.LogInfo("Running Customization Awake Patch");
            if (__instance.skins.Length < 10)
            {
                Log.LogInfo("Adding skin option to Customization");
                var skinWhite = ScriptableObject.CreateInstance<CustomizationOption>();
                skinWhite.color = Color.white;
                skinWhite.name = "Skin_White";
                skinWhite.type = Customization.Type.Skin;
                __instance.skins = __instance.skins.AddToArray(skinWhite);
                Log.LogInfo("Added White Skin to Customization");
            }
        }*/
    }

    internal static ManualLogSource Log;
    private readonly Harmony _harmony = new("com.ratherchaotic.moreskins");
    private void Awake()
    {
        Log = Logger;
        _harmony.PatchAll(typeof(CharacterAwakePatch));
        Log.LogInfo($"Plugin MoreSkins is loaded!");
    }

}
