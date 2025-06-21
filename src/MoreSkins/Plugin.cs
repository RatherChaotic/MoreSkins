using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using static Photon.Pun.PhotonNetwork;

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

        /*private static bool CreateTab(PassportManager passportManager, string name)
        {
            if (Array.Exists(passportManager.tabs, tab => tab.name == name)) return false;
            var tab = passportManager.uiObject.AddComponent<PassportTab>();
            tab.type = Customization.Type.Fit;
            tab.name = name;
            tab.anim = passportManager.anim;
            return true;
        }*/
        
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
    
    
    private MethodInfo? getCustomizationMethod;
    private MethodInfo? setCustomizationMethod;
    private readonly Harmony _harmony = new("com.ratherchaotic.moreskins");
    private void Awake()
    {
        var customizationType = typeof(CharacterCustomization);
        getCustomizationMethod = customizationType.GetMethod("GetCustomization", BindingFlags.Public | BindingFlags.Static);
        setCustomizationMethod = customizationType.GetMethod("SetCustomization", BindingFlags.Public | BindingFlags.Static);
        _harmony.PatchAll(typeof(Patcher));
    }

    private void Update()
    {
        {
            var player = LocalPlayer;
            var customizationData = (CharacterCustomizationData)getCustomizationMethod!.Invoke(null, new object[] { player});
            customizationData.currentSkin = 9;
            
            setCustomizationMethod!.Invoke(null, new object[] { customizationData, player });
            
        }
    }

}
