using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace MoreSkins;
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    private class Patcher
    {
        private static bool CreateSkinOption(Customization customization, string name, Color color)
        {
            if (Array.Exists(customization.skins, skin => skin.name == name) || Array.Exists(customization.skins, skin => skin.color == color)) return false;
            var skinOption = ScriptableObject.CreateInstance<CustomizationOption>();
            skinOption.color = color;
            skinOption.name = name;
            skinOption.texture = customization.skins[0].texture;
            skinOption.type = Customization.Type.Skin;
            customization.skins = customization.skins.AddToArray(skinOption);
            return true;
        }
        
        [HarmonyPatch(typeof(PassportManager), "Awake")]
        [HarmonyPostfix]
        private static void PassportManagerAwakePostfix(PassportManager __instance)
        {
            var customization = __instance.GetComponent<Customization>();
            CreateSkinOption(customization, "Skin_White", Color.white);
            CreateSkinOption(customization, "Skin_Black", new Color(0.2f, 0.2f, 0.2f, 1));
            CreateSkinOption(customization, "Skin_SkinTone1", new Color32(141, 85, 36, 1));
            CreateSkinOption(customization, "Skin_SkinTone2", new Color32(198, 134, 66, 1));
            CreateSkinOption(customization, "Skin_SkinTone3", new Color32(224, 172, 105, 1));
            CreateSkinOption(customization, "Skin_SkinTone4", new Color32(241, 194, 125, 1));
            CreateSkinOption(customization, "Skin_SkinTone5", new Color32(255, 219, 172, 1));
            
        }
    }
    private readonly Harmony _harmony = new("com.ratherchaotic.moreskins");
    private void Awake()
    {
        _harmony.PatchAll(typeof(Patcher));
    }

}
