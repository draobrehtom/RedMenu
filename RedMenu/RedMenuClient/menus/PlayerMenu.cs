using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;
using RedMenuShared;
using RedMenuClient.util;
using System.Net;
using RedMenuClient.data;

namespace RedMenuClient.menus
{
    static class PlayerMenu
    {
        private const int maxSavedPeds = 100;

        private static Menu menu = new Menu("Player Menu", "Player Related Options");
        private static bool setupDone = false;
        private static Menu appearanceMenu = new Menu("Ped Appearance", "Player Customization");
        private static Menu scenarioMenu = new Menu("Scenarios", "Scenarios");

        private static Dictionary<int, uint> currentMpClothes = new Dictionary<int, uint>();
        private static Dictionary<uint, float> currentFacialFeatures = new Dictionary<uint, float>();
        private static Dictionary<int, int> currentBodySettings = new Dictionary<int, int>();

        private static MenuDynamicListItem playerOutfit;

        private static void AddScenarioSubmenu(Menu menu, List<string> hashes, string title, string description)
        {
            Menu submenu = new Menu(title, description);
            MenuItem button = new MenuItem(title, description) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            MenuController.AddSubmenu(menu, submenu);
            menu.AddMenuItem(button);
            MenuController.BindMenuItem(menu, submenu, button);

            foreach (var name in hashes)
            {
                MenuItem item = new MenuItem(name);
                submenu.AddMenuItem(item);
            }

            submenu.OnItemSelect += (m, item, index) =>
            {
                uint hash = (uint)GetHashKey(hashes[index]);
                TaskStartScenarioInPlace(PlayerPedId(), (int)hash, 0, 1, 0, 0, 0);
            };
        }

        private static void AddEmotesSubmenu(Menu menu, List<string> hashes, int category, string title, string description)
        {
            Menu submenu = new Menu(title, description);
            MenuItem button = new MenuItem(title, description) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            menu.AddMenuItem(button);
            MenuController.AddSubmenu(menu, submenu);
            MenuController.BindMenuItem(menu, submenu, button);

            foreach (var name in hashes)
            {
                MenuListItem item = new MenuListItem(name, new List<string>() { "0", "1", "2"}, 0);
                submenu.AddMenuItem(item);
            }

            submenu.OnListItemSelect += (m, listItem, selectedIndex, itemIndex) =>
            {
                TaskEmote(PlayerPedId(), category, selectedIndex, GetHashKey(hashes[itemIndex]), 1, 1, 1, 1);
            };
        }

        private static async Task<string> GetUserInput(string windowTitle, string defaultText, int maxInputLength)
        {
            var spacer = "\t";
            AddTextEntry($"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", $"{windowTitle ?? "Enter"}:{spacer}(MAX {maxInputLength} Characters)");
            DisplayOnscreenKeyboard(0, $"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", "", defaultText ?? "", "", "", "", maxInputLength); await BaseScript.Delay(0);
            while (true)
            {
                int keyboardStatus = UpdateOnscreenKeyboard();
                switch (keyboardStatus)
                {
                    case 3:
                    case 2:
                        return null;
                    case 1:
                        return GetOnscreenKeyboardResult();
                    default:
                        await BaseScript.Delay(0);
                        break;
                }
            }
        }

        private static void ResetCurrentMpClothes()
        {
            int[] keys = currentMpClothes.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                currentMpClothes[keys[i]] = 0;
            }
        }

        private static void ResetCurrentFacialFeatures()
        {
            uint[] keys = currentFacialFeatures.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                currentFacialFeatures[keys[i]] = 0;
            }
        }

        private static void ResetCurrentBodySettings()
        {
            int[] keys = currentBodySettings.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                currentBodySettings[keys[i]] = 0;
            }
        }

        public static async void LoadDefaultPed(int pedIndex)
        {
            if (!StorageManager.TryGet("SavedPeds_" + pedIndex + "_outfit", out int outfit))
            {
                outfit = 0;
            }

            if (StorageManager.TryGet("SavedPeds_" + pedIndex + "_model", out int model))
            {
                if (IsModelInCdimage((uint)model))
                {
                    RequestModel((uint)model, false);
                    while (!HasModelLoaded((uint)model))
                    {
                        await BaseScript.Delay(0);
                    }
                    SetPlayerModel(PlayerId(), model, 0);
                    SetPedOutfitPreset(PlayerPedId(), outfit, 0);
                    SetModelAsNoLongerNeeded((uint)model);
                    playerOutfit.CurrentItem = outfit.ToString();

                    // mp_male comes with non-MP ammo components that should be removed
                    if (model == GetHashKey("mp_male"))
                    {
                        Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), 0xDA0E2C55, 0);
                        Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), 0x3F1F01E5, 0);
                        Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                    }
                }
                else
                {
                    Debug.WriteLine($"^1[ERROR] This ped model is not present in the game files {model}.^7");
                }
            }

            await BaseScript.Delay(500);

            ResetCurrentMpClothes();
            ResetCurrentFacialFeatures();
            ResetCurrentBodySettings();

            int[] keys = currentMpClothes.Keys.ToArray();

            for (int j = 0; j < keys.Length; ++j)
            {
                if (StorageManager.TryGet("SavedPeds_" + pedIndex + "_mp_" + keys[j], out int hash))
                {
                    switch ((uint)hash)
                    {
                        case 0x18729F39:
                        case 0x1D4C528A:
                        case 0x2026C46D:
                        case 0x3107499B:
                        case 0x378AD10C:
                        case 0x3C1A74CD:
                        case 0x3F7F3587:
                        case 0x485EE834:
                        case 0x514ADCEA:
                        case 0x05E47CA6:
                        case 0x5FC29285:
                        case 0x0662AC34:
                        case 0x72E6EF74:
                        case 0x7505EF42:
                        case 0x777EC6EF:
                        case 0x79D7DF96:
                        case 0x7A6BBD0B:
                        case 0x7A96FACA:
                        case 0x7BC10759:
                        case 0x823687F5:
                        case 0x83887E88:
                        case 0x864B03AE:
                        case 0x877A2CF7:
                        case 0x91CE9B20:
                        case 0x94504D26:
                        case 0x96EDAE5C:
                        case 0x9925C067:
                        case 0x9B2C8B89:
                        case 0xA0E3AB7F:
                        case 0xA6D134C6:
                        case 0xAF14310B:
                        case 0x0B3966C9:
                        case 0xB6B6122D:
                        case 0xE06D30CE:
                        case 0xEA24B45E:
                        case 0xEABE0032:
                        case 0xF1542D11:
                        case 0xF16A1D23:
                        case 0xF8016BCA:
                        case 0xFAE9107F:
                            Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), hash, 0);
                            Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                            break;
                        default:
                            Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), (uint)hash, true, true, false);
                            break;
                    }
                    currentMpClothes[keys[j]] = (uint)hash;
                }
                else
                {
                    currentMpClothes[keys[j]] = 0;
                }
            }

            uint[] ffkeys = currentFacialFeatures.Keys.ToArray();

            for (int j = 0; j < ffkeys.Length; ++j)
            {
                if (StorageManager.TryGet("SavedPeds_" + pedIndex + "_ff_" + ffkeys[j], out float value))
                {
                    if (value != 0)
                    {
                        Function.Call((Hash)0x5653AB26C82938CF, PlayerPedId(), ffkeys[j], value);
                    }

                    currentFacialFeatures[ffkeys[j]] = value;
                }
                else
                {
                    currentFacialFeatures[ffkeys[j]] = 0;
                }
            }

            int[] bckeys = currentBodySettings.Keys.ToArray();

            for (int j = 0; j < bckeys.Length; ++j)
            {
                if (StorageManager.TryGet("SavedPeds_" + pedIndex + "_bc_" + bckeys[j], out int hash))
                {
                    if (hash != 0)
                    {
                        Function.Call((Hash)0x1902C4CFCC5BE57C, PlayerPedId(), hash);
                    }

                    currentBodySettings[bckeys[j]] = hash;
                }
                else
                {
                    currentBodySettings[bckeys[j]] = 0;
                }
            }

            Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
        }

        private static void SetMood(string mood)
        {
            int ped = PlayerPedId();

            switch (mood)
            {
                case "None":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_normal", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_normal", "face_human@gen_female@base");
                    }
                    ClearFacialIdleAnimOverride(ped);
                    break;
                case "Aiming":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_aiming_pistol", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_aiming_pistol", "face_human@gen_female@base");
                    }
                    break;
                case "Angry":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_angry", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_angry", "face_human@gen_female@base");
                    }
                    break;
                case "Bitchy":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_bitchy", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_bitchy", "face_human@gen_female@base");
                    }
                    break;
                case "Burning":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_burning", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_burning", "face_human@gen_female@base");
                    }
                    break;
                case "Cocky":
                    SetFacialIdleAnimOverride(ped, "mood_cocky", "face_human@gen_male@base");
                    break;
                case "Cold":
                    SetFacialIdleAnimOverride(ped, "mood_cold", "face_human@gen_male@base");
                    break;
                case "Confused":
                    SetFacialIdleAnimOverride(ped, "mood_confused", "face_human@gen_male@base");
                    break;
                case "Cower":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_cower", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_cower", "face_human@gen_female@base");
                    }
                    break;
                case "Disgust":
                    SetFacialIdleAnimOverride(ped, "mood_disgust", "face_human@gen_male@base");
                    break;
                case "Drunk":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_drunk_medium", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_drunk_medium", "face_human@gen_female@base");
                    }
                    break;
                case "Happy":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_happy", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_happy", "face_human@gen_female@base");
                    }
                    break;
                case "Hot":
                    SetFacialIdleAnimOverride(ped, "mood_hot", "face_human@gen_male@base");
                    break;
                case "Normal":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_normal", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_normal", "face_human@gen_female@base");
                    }
                    break;
                case "Sad":
                    SetFacialIdleAnimOverride(ped, "mood_sad", "face_human@gen_male@base");
                    break;
                case "Scared":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_scared", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_scared", "face_human@gen_female@base");
                    }
                    break;
                case "Seductive":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_seductive", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_seductive", "face_human@gen_female@base");
                    }
                    break;
                case "Sleeping":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_sleeping", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_sleeping", "face_human@gen_female@base");
                    }
                    break;
                case "Smug":
                    if (IsPedMale(ped))
                    {
                        SetFacialIdleAnimOverride(ped, "mood_smug", "face_human@gen_male@base");
                    }
                    else
                    {
                        SetFacialIdleAnimOverride(ped, "mood_smug", "face_human@gen_female@base");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuListItem restoreInnerCores = new MenuListItem("Restore Inner Core", new List<string>() { "All", "Health", "Stamina", "Dead Eye" }, 0, "Fully restores any or all inner cores to their max value.");
            MenuListItem restoreOuterCores = new MenuListItem("Restore Outer Core", new List<string>() { "All", "Health", "Stamina", "Dead Eye" }, 0, "Fully restores any or all outer cores to their max value.");
            MenuListItem fortifyCoresList = new MenuListItem("Fortify Core", new List<string>() { "All", "Health", "Stamina", "Dead Eye" }, 0, "Fortify inner cores.");
            MenuCheckboxItem godModeBox = new MenuCheckboxItem("God Mode", "Prevents you from taking damage.", UserDefaults.PlayerGodMode);
            MenuCheckboxItem infiniteStamina = new MenuCheckboxItem("Infinite Stamina", "Run forever!", UserDefaults.PlayerInfiniteStamina);
            MenuCheckboxItem infiniteDeadEye = new MenuCheckboxItem("Infinite DeadEye", "Useless?", UserDefaults.PlayerInfiniteDeadEye);
            MenuCheckboxItem everyoneIgnore = new MenuCheckboxItem("Everyone Ignore Player", "Currently, only prevents NPCs from shooting at you.", UserDefaults.PlayerEveryoneIgnore);
            MenuCheckboxItem disableRagdoll = new MenuCheckboxItem("Disable Ragdoll", "Prevent your character from ragdolling.", UserDefaults.PlayerDisableRagdoll);

            MenuItem clearPedTasks = new MenuItem("Clear Ped Tasks", "Clear all ped tasks immediately, breaking free of any animation.");
            MenuItem hogtieSelf = new MenuItem("Hogtie Yourself", "Knocks you to the ground and get hogtied.");
            MenuItem cleanPed = new MenuItem("Clean Ped", "Remove all dirt and other decals from the ped.");
            MenuItem dryPed = new MenuItem("Dry Ped", "Remove all wetness from the ped.");
            MenuItem killSelf = new MenuItem("Kill Yourself", "Kill yourself.");

            MenuListItem moods = new MenuListItem("Mood", new List<string>() { "None", "Aiming", "Angry", "Burning", "Cocky", "Cold", "Confused", "Cower", "Disgust", "Drunk", "Happy", "Hot", "Normal", "Sad", "Seductive", "Scared", "Sleeping", "Smug" }, 0, "Change your mood.");

            playerOutfit = new MenuDynamicListItem("Select Outfit", "0", new MenuDynamicListItem.ChangeItemCallback((item, left) =>
            {
                if (int.TryParse(item.CurrentItem, out int val))
                {
                    int newVal = val;
                    if (left)
                    {
                        newVal--;
                        if (newVal < 0)
                        {
                            newVal = 0;
                        }
                    }
                    else
                    {
                        newVal++;
                    }
                    SetPedOutfitPreset(PlayerPedId(), newVal, 0);
                    return newVal.ToString();
                }
                return "0";
            }), "Select a predefined outfit for this ped. Outfits are made by Rockstar. Note the selected value can go up indefinitely because we don't know how to check for the max amount of outfits yet, so more native research is needed.");

            if (PermissionsManager.IsAllowed(Permission.PMRestoreInnerCores))
            {
                menu.AddMenuItem(restoreInnerCores);
            }
            if (PermissionsManager.IsAllowed(Permission.PMRestoreOuterCores))
            {
                menu.AddMenuItem(restoreOuterCores);
            }
            if (PermissionsManager.IsAllowed(Permission.PMFortifyCores))
            {
                menu.AddMenuItem(fortifyCoresList);
            }
            if (PermissionsManager.IsAllowed(Permission.PMGodMode))
            {
                menu.AddMenuItem(godModeBox);
                if (UserDefaults.PlayerGodMode)
                {
                    SetEntityInvincible(PlayerPedId(), true);
                }
            }
            if (PermissionsManager.IsAllowed(Permission.PMInfiniteStamina))
            {
                menu.AddMenuItem(infiniteStamina);
            }
            if (PermissionsManager.IsAllowed(Permission.PMInfiniteDeadEye))
            {
                menu.AddMenuItem(infiniteDeadEye);
            }
            if (PermissionsManager.IsAllowed(Permission.PMEveryoneIgnore))
            {
                menu.AddMenuItem(everyoneIgnore);
            }
            if (PermissionsManager.IsAllowed(Permission.PMDisableRagdoll))
            {
                menu.AddMenuItem(disableRagdoll);
            }
            if (PermissionsManager.IsAllowed(Permission.PMClearTasks))
            {
                menu.AddMenuItem(clearPedTasks);
            }
            if (PermissionsManager.IsAllowed(Permission.PMHogtieSelf))
            {
                menu.AddMenuItem(hogtieSelf);
            }
            if (PermissionsManager.IsAllowed(Permission.PMCleanPed))
            {
                menu.AddMenuItem(cleanPed);
            }
            if (PermissionsManager.IsAllowed(Permission.PMDryPed))
            {
                menu.AddMenuItem(dryPed);
            }
            if (PermissionsManager.IsAllowed(Permission.PMKillSelf))
            {
                menu.AddMenuItem(killSelf);
            }
            if (PermissionsManager.IsAllowed(Permission.PMSelectPlayerModel) || PermissionsManager.IsAllowed(Permission.PMSelectOutfit))
            {
                MenuItem appearanceMenuBtn = new MenuItem("Player Appearance", "Player appearance options.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                MenuController.AddSubmenu(menu, appearanceMenu);
                menu.AddMenuItem(appearanceMenuBtn);
                MenuController.BindMenuItem(menu, appearanceMenu, appearanceMenuBtn);

                if (PermissionsManager.IsAllowed(Permission.PMSelectPlayerModel))
                {
                    List<string> males = new List<string>();
                    List<string> females = new List<string>();
                    List<string> cutscene = new List<string>();
                    List<string> animals = new List<string>();
                    List<string> horses = new List<string>();
                    List<string> other = new List<string>();
                    List<string> mp = new List<string>();
                    MenuListItem malePeds = new MenuListItem("Males", males, 0, "Select a male ped model.");
                    MenuListItem femalePeds = new MenuListItem("Females", females, 0, "Select a female ped model.");
                    MenuListItem cutscenePeds = new MenuListItem("Cutscene", cutscene, 0, "Select a cutscene ped model.");
                    MenuListItem animalPeds = new MenuListItem("Animals", animals, 0, "Select an animal ped model.");
                    MenuListItem horsePeds = new MenuListItem("Horses", horses, 0, "Select a horse ped model.");
                    MenuListItem otherPeds = new MenuListItem("Other", other, 0, "Select a ped model.");
                    MenuListItem mpPeds = new MenuListItem("MP", mp, 0, "Select an MP ped model.");
                    for (int i = 0; i < data.PedModels.MalePedHashes.Count(); i++)
                    {
                        males.Add($"{data.PedModels.MalePedHashes[i]} ({i + 1}/{data.PedModels.MalePedHashes.Count()})");
                    }
                    for (int i = 0; i < data.PedModels.FemalePedHashes.Count(); i++)
                    {
                        females.Add($"{data.PedModels.FemalePedHashes[i]} ({i + 1}/{data.PedModels.FemalePedHashes.Count()})");
                    }
                    for (int i = 0; i < data.PedModels.CutscenePedHashes.Count(); i++)
                    {
                        cutscene.Add($"{data.PedModels.CutscenePedHashes[i]} ({i + 1}/{data.PedModels.CutscenePedHashes.Count()})");
                    }
                    for (int i = 0; i < data.PedModels.AnimalHashes.Count(); i++)
                    {
                        animals.Add($"{data.PedModels.AnimalHashes[i]} ({i + 1}/{data.PedModels.AnimalHashes.Count()})");
                    }
                    for (int i = 0; i < data.PedModels.HorseHashes.Count(); i++)
                    {
                        horses.Add($"{data.PedModels.HorseHashes[i]} ({i + 1}/{data.PedModels.HorseHashes.Count()}");
                    }
                    for (int i = 0; i < data.PedModels.OtherPedHashes.Count(); i++)
                    {
                        other.Add($"{data.PedModels.OtherPedHashes[i]} ({i + 1}/{data.PedModels.OtherPedHashes.Count()})");
                    }
                    for (int i = 0; i < data.PedModels.MPPedHashes.Count(); i++)
                    {
                        mp.Add($"{data.PedModels.MPPedHashes[i]} ({i + 1}/{data.PedModels.MPPedHashes.Count()})");
                    }

                    appearanceMenu.AddMenuItem(malePeds);
                    appearanceMenu.AddMenuItem(femalePeds);
                    appearanceMenu.AddMenuItem(otherPeds);
                    appearanceMenu.AddMenuItem(cutscenePeds);
                    appearanceMenu.AddMenuItem(mpPeds);
                    appearanceMenu.AddMenuItem(animalPeds);
                    appearanceMenu.AddMenuItem(horsePeds);

                    appearanceMenu.OnListItemSelect += async (m, item, listIndex, itemIndex) =>
                    {
                        uint model = 0;
                        if (item == malePeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.MalePedHashes[listIndex]);
                        }
                        else if (item == femalePeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.FemalePedHashes[listIndex]);
                        }
                        else if (item == cutscenePeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.CutscenePedHashes[listIndex]);
                        }
                        else if (item == otherPeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.OtherPedHashes[listIndex]);
                        }
                        else if (item == mpPeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.MPPedHashes[listIndex]);
                        }
                        else if (item == animalPeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.AnimalHashes[listIndex]);
                        }
                        else if (item == horsePeds)
                        {
                            model = (uint)GetHashKey(data.PedModels.HorseHashes[listIndex]);
                        }

                        if (IsModelInCdimage(model))
                        {
                            RequestModel(model, false);
                            while (!HasModelLoaded(model))
                            {
                                await BaseScript.Delay(0);
                            }
                            SetPlayerModel(PlayerId(), (int)model, 0);
                            SetPedOutfitPreset(PlayerPedId(), 0, 0);
                            SetModelAsNoLongerNeeded(model);
                            playerOutfit.CurrentItem = "0";

                            // mp_male comes with non-MP ammo components that should be removed
                            if ((int)model == GetHashKey("mp_male"))
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), 0xDA0E2C55, 0);
                                Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), 0x3F1F01E5, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                            }

                            ResetCurrentMpClothes();
                            ResetCurrentFacialFeatures();
                            ResetCurrentBodySettings();
                        }
                        else
                        {
                            Debug.WriteLine($"^1[ERROR] This ped model is not present in the game files {model}.^7");
                        }
                    };
                }

                if (PermissionsManager.IsAllowed(Permission.PMSelectOutfit))
                {
                    appearanceMenu.AddMenuItem(playerOutfit);
                    MenuController.AddSubmenu(menu, appearanceMenu);
                }

                if (PermissionsManager.IsAllowed(Permission.PMCustomizeMpPeds))
                {
                    MenuItem femaleCustom = new MenuItem("MP Female Customization", "Customize your MP female ped.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    MenuItem maleCustom = new MenuItem("MP Male Customization", "Customize your MP male ped.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };

                    Menu femaleCustomMenu = new Menu("Customization", "MP Female Customization");
                    Menu maleCustomMenu = new Menu("Customization", "MP Male Customization");

                    MenuController.AddSubmenu(appearanceMenu, femaleCustomMenu);
                    MenuController.AddSubmenu(appearanceMenu, maleCustomMenu);

                    #region female
                    {
                        List<string> spurs = new List<string>();
                        List<string> pants = new List<string>();
                        List<string> shirts = new List<string>();
                        List<string> chaps = new List<string>();
                        List<string> faces = new List<string>();
                        List<string> ponchos = new List<string>();
                        List<string> badges = new List<string>();
                        List<string> vests = new List<string>();
                        List<string> legArmor = new List<string>();
                        List<string> glasses = new List<string>();
                        List<string> bandanas = new List<string>();
                        List<string> coats = new List<string>();
                        List<string> bodyArmor = new List<string>();
                        List<string> masks = new List<string>();
                        List<string> boots = new List<string>();
                        List<string> buckles = new List<string>();
                        List<string> rings = new List<string>();
                        List<string> neckwear = new List<string>();
                        List<string> wristbands = new List<string>();
                        List<string> feetStyle = new List<string>();
                        List<string> belts = new List<string>();
                        List<string> hair = new List<string>();
                        List<string> suspenders = new List<string>();
                        List<string> gauntlets = new List<string>();
                        List<string> bags = new List<string>();
                        List<string> teeth = new List<string>();
                        List<string> hats = new List<string>();
                        List<string> gunBelts = new List<string>();
                        List<string> skirts = new List<string>();
                        List<string> belts2 = new List<string>();
                        List<string> ponchos2 = new List<string>();
                        List<string> bodyStyle = new List<string>();
                        List<string> offHandHolsters = new List<string>();
                        List<string> coats2 = new List<string>();
                        List<string> eyes = new List<string>();
                        List<string> gloves = new List<string>();
                        List<string> gunBeltAccessory = new List<string>();
                        List<string> rings2 = new List<string>();
                        //List<string> beard = new List<string>();
                        List<string> buckles2 = new List<string>();
                        foreach (var k in data.FemaleCustomization.spurs) { spurs.Add($"({data.FemaleCustomization.spurs.IndexOf(k) + 1}/{data.FemaleCustomization.spurs.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.pants) { pants.Add($"({data.FemaleCustomization.pants.IndexOf(k) + 1}/{data.FemaleCustomization.pants.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.shirts) { shirts.Add($"({data.FemaleCustomization.shirts.IndexOf(k) + 1}/{data.FemaleCustomization.shirts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.chaps) { chaps.Add($"({data.FemaleCustomization.chaps.IndexOf(k) + 1}/{data.FemaleCustomization.chaps.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.faces) { faces.Add($"({data.FemaleCustomization.faces.IndexOf(k) + 1}/{data.FemaleCustomization.faces.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.ponchos) { ponchos.Add($"({data.FemaleCustomization.ponchos.IndexOf(k) + 1}/{data.FemaleCustomization.ponchos.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.badges) { badges.Add($"({data.FemaleCustomization.badges.IndexOf(k) + 1}/{data.FemaleCustomization.badges.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.vests) { vests.Add($"({data.FemaleCustomization.vests.IndexOf(k) + 1}/{data.FemaleCustomization.vests.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.legArmor) { legArmor.Add($"({data.FemaleCustomization.legArmor.IndexOf(k) + 1}/{data.FemaleCustomization.legArmor.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.glasses) { glasses.Add($"({data.FemaleCustomization.glasses.IndexOf(k) + 1}/{data.FemaleCustomization.glasses.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.bandanas) { bandanas.Add($"({data.FemaleCustomization.bandanas.IndexOf(k) + 1}/{data.FemaleCustomization.bandanas.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.coats) { coats.Add($"({data.FemaleCustomization.coats.IndexOf(k) + 1}/{data.FemaleCustomization.coats.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.bodyArmor) { bodyArmor.Add($"({data.FemaleCustomization.bodyArmor.IndexOf(k) + 1}/{data.FemaleCustomization.bodyArmor.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.masks) { masks.Add($"({data.FemaleCustomization.masks.IndexOf(k) + 1}/{data.FemaleCustomization.masks.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.boots) { boots.Add($"({data.FemaleCustomization.boots.IndexOf(k) + 1}/{data.FemaleCustomization.boots.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.buckles) { buckles.Add($"({data.FemaleCustomization.buckles.IndexOf(k) + 1}/{data.FemaleCustomization.buckles.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.rings) { rings.Add($"({data.FemaleCustomization.rings.IndexOf(k) + 1}/{data.FemaleCustomization.rings.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.neckwear) { neckwear.Add($"({data.FemaleCustomization.neckwear.IndexOf(k) + 1}/{data.FemaleCustomization.neckwear.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.wristbands) { wristbands.Add($"({data.FemaleCustomization.wristbands.IndexOf(k) + 1}/{data.FemaleCustomization.wristbands.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.feetStyle) { feetStyle.Add($"({data.FemaleCustomization.feetStyle.IndexOf(k) + 1}/{data.FemaleCustomization.feetStyle.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.belts) { belts.Add($"({data.FemaleCustomization.belts.IndexOf(k) + 1}/{data.FemaleCustomization.belts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.hair) { hair.Add($"({data.FemaleCustomization.hair.IndexOf(k) + 1}/{data.FemaleCustomization.hair.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.suspenders) { suspenders.Add($"({data.FemaleCustomization.suspenders.IndexOf(k) + 1}/{data.FemaleCustomization.suspenders.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.gauntlets) { gauntlets.Add($"({data.FemaleCustomization.gauntlets.IndexOf(k) + 1}/{data.FemaleCustomization.gauntlets.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.bags) { bags.Add($"({data.FemaleCustomization.bags.IndexOf(k) + 1}/{data.FemaleCustomization.bags.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.teeth) { teeth.Add($"({data.FemaleCustomization.teeth.IndexOf(k) + 1}/{data.FemaleCustomization.teeth.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.hats) { hats.Add($"({data.FemaleCustomization.hats.IndexOf(k) + 1}/{data.FemaleCustomization.hats.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.gunBelts) { gunBelts.Add($"({data.FemaleCustomization.gunBelts.IndexOf(k) + 1}/{data.FemaleCustomization.gunBelts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.skirts) { skirts.Add($"({data.FemaleCustomization.skirts.IndexOf(k) + 1}/{data.FemaleCustomization.skirts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.belts2) { belts2.Add($"({data.FemaleCustomization.belts2.IndexOf(k) + 1}/{data.FemaleCustomization.belts2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.ponchos2) { ponchos2.Add($"({data.FemaleCustomization.ponchos2.IndexOf(k) + 1}/{data.FemaleCustomization.ponchos2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.bodyStyle) { bodyStyle.Add($"({data.FemaleCustomization.bodyStyle.IndexOf(k) + 1}/{data.FemaleCustomization.bodyStyle.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.offHandHolsters) { offHandHolsters.Add($"({data.FemaleCustomization.offHandHolsters.IndexOf(k) + 1}/{data.FemaleCustomization.offHandHolsters.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.coats2) { coats2.Add($"({data.FemaleCustomization.coats2.IndexOf(k) + 1}/{data.FemaleCustomization.coats2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.eyes) { eyes.Add($"({data.FemaleCustomization.eyes.IndexOf(k) + 1}/{data.FemaleCustomization.eyes.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.gloves) { gloves.Add($"({data.FemaleCustomization.gloves.IndexOf(k) + 1}/{data.FemaleCustomization.gloves.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.gunBeltAccessory) { gunBeltAccessory.Add($"({data.FemaleCustomization.gunBeltAccessory.IndexOf(k) + 1}/{data.FemaleCustomization.gunBeltAccessory.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.rings2) { rings2.Add($"({data.FemaleCustomization.rings2.IndexOf(k) + 1}/{data.FemaleCustomization.rings2.Count()}) 0x{k.ToString("X08")}"); }
                        //foreach (var k in data.FemaleCustomization.beard) { buckles2.Add($"({data.FemaleCustomization.beard.IndexOf(k) + 1}/{data.FemaleCustomization.beard.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.FemaleCustomization.buckles2) { buckles2.Add($"({data.FemaleCustomization.buckles2.IndexOf(k) + 1}/{data.FemaleCustomization.buckles2.Count()}) 0x{k.ToString("X08")}"); }

                        femaleCustomMenu.AddMenuItem(new MenuListItem("Boot Accessories", spurs, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Pants", pants, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Shirts", shirts, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Chaps", chaps, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Heads", faces, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Cloaks", ponchos, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Badges", badges, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Vests", vests, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Spats", legArmor, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Eyewear", glasses, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Neckwear", bandanas, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Closed Coats", coats, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Armor", bodyArmor, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Masks", masks, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Boots", boots, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Accessories", buckles, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Right-Hand Rings", rings, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Neckties", neckwear, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Bracelets", wristbands, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Lower Bodies", feetStyle, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Loadouts", belts, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Hair", hair, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Suspenders", suspenders, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Gauntlets", gauntlets, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Satchels", bags, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Teeth", teeth, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Hats", hats, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Gun Belts", gunBelts, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Skirts", skirts, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Belts", belts2, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Ponchos", ponchos2, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Upper Bodies", bodyStyle, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Off-Hand Holsters", offHandHolsters, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Open Coats", coats2, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Eyes", eyes, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Gloves", gloves, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Gun Belt Accessory", gunBeltAccessory, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Left-Hand Rings", rings2, 0));
                        //femaleCustomMenu.AddMenuItem(new MenuListItem("Beard", beard, 0));
                        femaleCustomMenu.AddMenuItem(new MenuListItem("Belt Buckles", buckles2, 0));

                        femaleCustomMenu.OnListIndexChange += (m, item, oldIndex, newIndex, itemIndex) =>
                        {
                            uint hash;
                            switch (itemIndex)
                            {
                                case 0: hash = data.FemaleCustomization.spurs[newIndex]; break;
                                case 1: hash = data.FemaleCustomization.pants[newIndex]; break;
                                case 2: hash = data.FemaleCustomization.shirts[newIndex]; break;
                                case 3: hash = data.FemaleCustomization.chaps[newIndex]; break;
                                case 4: hash = data.FemaleCustomization.faces[newIndex]; break;
                                case 5: hash = data.FemaleCustomization.ponchos[newIndex]; break;
                                case 6: hash = data.FemaleCustomization.badges[newIndex]; break;
                                case 7: hash = data.FemaleCustomization.vests[newIndex]; break;
                                case 8: hash = data.FemaleCustomization.legArmor[newIndex]; break;
                                case 9: hash = data.FemaleCustomization.glasses[newIndex]; break;
                                case 10: hash = data.FemaleCustomization.bandanas[newIndex]; break;
                                case 11: hash = data.FemaleCustomization.coats[newIndex]; break;
                                case 12: hash = data.FemaleCustomization.bodyArmor[newIndex]; break;
                                case 13: hash = data.FemaleCustomization.masks[newIndex]; break;
                                case 14: hash = data.FemaleCustomization.boots[newIndex]; break;
                                case 15: hash = data.FemaleCustomization.buckles[newIndex]; break;
                                case 16: hash = data.FemaleCustomization.rings[newIndex]; break;
                                case 17: hash = data.FemaleCustomization.neckwear[newIndex]; break;
                                case 18: hash = data.FemaleCustomization.wristbands[newIndex]; break;
                                case 19: hash = data.FemaleCustomization.feetStyle[newIndex]; break;
                                case 20: hash = data.FemaleCustomization.belts[newIndex]; break;
                                case 21: hash = data.FemaleCustomization.hair[newIndex]; break;
                                case 22: hash = data.FemaleCustomization.suspenders[newIndex]; break;
                                case 23: hash = data.FemaleCustomization.gauntlets[newIndex]; break;
                                case 24: hash = data.FemaleCustomization.bags[newIndex]; break;
                                case 25: hash = data.FemaleCustomization.teeth[newIndex]; break;
                                case 26: hash = data.FemaleCustomization.hats[newIndex]; break;
                                case 27: hash = data.FemaleCustomization.gunBelts[newIndex]; break;
                                case 28: hash = data.FemaleCustomization.skirts[newIndex]; break;
                                case 29: hash = data.FemaleCustomization.belts2[newIndex]; break;
                                case 30: hash = data.FemaleCustomization.ponchos2[newIndex]; break;
                                case 31: hash = data.FemaleCustomization.bodyStyle[newIndex]; break;
                                case 32: hash = data.FemaleCustomization.offHandHolsters[newIndex]; break;
                                case 33: hash = data.FemaleCustomization.coats2[newIndex]; break;
                                case 34: hash = data.FemaleCustomization.eyes[newIndex]; break;
                                case 35: hash = data.FemaleCustomization.gloves[newIndex]; break;
                                case 36: hash = data.FemaleCustomization.gunBeltAccessory[newIndex]; break;
                                case 37: hash = data.FemaleCustomization.rings2[newIndex]; break;
                                //case 38: hash = data.FemaleCustomization.beard[newIndex]; break;
                                case 38: hash = data.FemaleCustomization.buckles2[newIndex]; break;
                                default:
                                    hash = 0;
                                    break;
                            }
                            if (hash != 0)
                            {
                                Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), hash, true, true, false);
                                currentMpClothes[itemIndex] = hash;
                            }
                        };

                        femaleCustomMenu.OnListItemSelect += (m, listItem, selectedIndex, itemIndex) =>
                        {
                            uint hash;
                            switch (itemIndex)
                            {
                                case 0: hash = 0x18729F39; break;
                                case 1: hash = 0x1D4C528A; break;
                                case 2: hash = 0x2026C46D; break;
                                case 3: hash = 0x3107499B; break;
                                case 4: hash = 0x378AD10C; break;
                                case 5: hash = 0x3C1A74CD; break;
                                case 6: hash = 0x3F7F3587; break;
                                case 7: hash = 0x485EE834; break;
                                case 8: hash = 0x514ADCEA; break;
                                case 9: hash = 0x05E47CA6; break;
                                case 10: hash = 0x5FC29285; break;
                                case 11: hash = 0x0662AC34; break;
                                case 12: hash = 0x72E6EF74; break;
                                case 13: hash = 0x7505EF42; break;
                                case 14: hash = 0x777EC6EF; break;
                                case 15: hash = 0x79D7DF96; break;
                                case 16: hash = 0x7A6BBD0B; break;
                                case 17: hash = 0x7A96FACA; break;
                                case 18: hash = 0x7BC10759; break;
                                case 19: hash = 0x823687F5; break;
                                case 20: hash = 0x83887E88; break;
                                case 21: hash = 0x864B03AE; break;
                                case 22: hash = 0x877A2CF7; break;
                                case 23: hash = 0x91CE9B20; break;
                                case 24: hash = 0x94504D26; break;
                                case 25: hash = 0x96EDAE5C; break;
                                case 26: hash = 0x9925C067; break;
                                case 27: hash = 0x9B2C8B89; break;
                                case 28: hash = 0xA0E3AB7F; break;
                                case 29: hash = 0xA6D134C6; break;
                                case 30: hash = 0xAF14310B; break;
                                case 31: hash = 0x0B3966C9; break;
                                case 32: hash = 0xB6B6122D; break;
                                case 33: hash = 0xE06D30CE; break;
                                case 34: hash = 0xEA24B45E; break;
                                case 35: hash = 0xEABE0032; break;
                                case 36: hash = 0xF1542D11; break;
                                case 37: hash = 0xF16A1D23; break;
                                case 38: hash = 0xFAE9107F; break;
                                default:
                                    hash = 0;
                                    break;
                            }

                            if (hash != 0)
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), hash, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                                currentMpClothes[itemIndex] = hash;
                            }
                        };
                    }
                    #endregion

                    #region male
                    {
                        List<string> spurs = new List<string>();
                        List<string> pants = new List<string>();
                        List<string> shirts = new List<string>();
                        List<string> chaps = new List<string>();
                        List<string> faces = new List<string>();
                        List<string> ponchos = new List<string>();
                        List<string> badges = new List<string>();
                        List<string> vests = new List<string>();
                        List<string> legArmor = new List<string>();
                        List<string> glasses = new List<string>();
                        List<string> bandanas = new List<string>();
                        List<string> coats = new List<string>();
                        List<string> bodyArmor = new List<string>();
                        List<string> masks = new List<string>();
                        List<string> boots = new List<string>();
                        List<string> buckles = new List<string>();
                        List<string> rings = new List<string>();
                        List<string> neckwear = new List<string>();
                        List<string> wristbands = new List<string>();
                        List<string> feetStyle = new List<string>();
                        List<string> belts = new List<string>();
                        List<string> hair = new List<string>();
                        List<string> suspenders = new List<string>();
                        List<string> gauntlets = new List<string>();
                        List<string> bags = new List<string>();
                        List<string> teeth = new List<string>();
                        List<string> hats = new List<string>();
                        List<string> gunBelts = new List<string>();
                        //List<string> skirts = new List<string>();
                        List<string> belts2 = new List<string>();
                        List<string> ponchos2 = new List<string>();
                        List<string> bodyStyle = new List<string>();
                        List<string> offHandHolsters = new List<string>();
                        List<string> coats2 = new List<string>();
                        List<string> eyes = new List<string>();
                        List<string> gloves = new List<string>();
                        List<string> gunBeltAccessory = new List<string>();
                        List<string> rings2 = new List<string>();
                        List<string> beard = new List<string>();
                        List<string> buckles2 = new List<string>();
                        foreach (var k in data.MaleCustomization.spurs) { spurs.Add($"({data.MaleCustomization.spurs.IndexOf(k) + 1}/{data.MaleCustomization.spurs.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.pants) { pants.Add($"({data.MaleCustomization.pants.IndexOf(k) + 1}/{data.MaleCustomization.pants.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.shirts) { shirts.Add($"({data.MaleCustomization.shirts.IndexOf(k) + 1}/{data.MaleCustomization.shirts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.chaps) { chaps.Add($"({data.MaleCustomization.chaps.IndexOf(k) + 1}/{data.MaleCustomization.chaps.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.faces) { faces.Add($"({data.MaleCustomization.faces.IndexOf(k) + 1}/{data.MaleCustomization.faces.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.ponchos) { ponchos.Add($"({data.MaleCustomization.ponchos.IndexOf(k) + 1}/{data.MaleCustomization.ponchos.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.badges) { badges.Add($"({data.MaleCustomization.badges.IndexOf(k) + 1}/{data.MaleCustomization.badges.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.vests) { vests.Add($"({data.MaleCustomization.vests.IndexOf(k) + 1}/{data.MaleCustomization.vests.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.legArmor) { legArmor.Add($"({data.MaleCustomization.legArmor.IndexOf(k) + 1}/{data.MaleCustomization.legArmor.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.glasses) { glasses.Add($"({data.MaleCustomization.glasses.IndexOf(k) + 1}/{data.MaleCustomization.glasses.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.bandanas) { bandanas.Add($"({data.MaleCustomization.bandanas.IndexOf(k) + 1}/{data.MaleCustomization.bandanas.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.coats) { coats.Add($"({data.MaleCustomization.coats.IndexOf(k) + 1}/{data.MaleCustomization.coats.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.bodyArmor) { bodyArmor.Add($"({data.MaleCustomization.bodyArmor.IndexOf(k) + 1}/{data.MaleCustomization.bodyArmor.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.masks) { masks.Add($"({data.MaleCustomization.masks.IndexOf(k) + 1}/{data.MaleCustomization.masks.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.boots) { boots.Add($"({data.MaleCustomization.boots.IndexOf(k) + 1}/{data.MaleCustomization.boots.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.buckles) { buckles.Add($"({data.MaleCustomization.buckles.IndexOf(k) + 1}/{data.MaleCustomization.buckles.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.rings) { rings.Add($"({data.MaleCustomization.rings.IndexOf(k) + 1}/{data.MaleCustomization.rings.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.neckwear) { neckwear.Add($"({data.MaleCustomization.neckwear.IndexOf(k) + 1}/{data.MaleCustomization.neckwear.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.wristbands) { wristbands.Add($"({data.MaleCustomization.wristbands.IndexOf(k) + 1}/{data.MaleCustomization.wristbands.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.feetStyle) { feetStyle.Add($"({data.MaleCustomization.feetStyle.IndexOf(k) + 1}/{data.MaleCustomization.feetStyle.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.belts) { belts.Add($"({data.MaleCustomization.belts.IndexOf(k) + 1}/{data.MaleCustomization.belts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.hair) { hair.Add($"({data.MaleCustomization.hair.IndexOf(k) + 1}/{data.MaleCustomization.hair.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.suspenders) { suspenders.Add($"({data.MaleCustomization.suspenders.IndexOf(k) + 1}/{data.MaleCustomization.suspenders.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.gauntlets) { gauntlets.Add($"({data.MaleCustomization.gauntlets.IndexOf(k) + 1}/{data.MaleCustomization.gauntlets.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.bags) { bags.Add($"({data.MaleCustomization.bags.IndexOf(k) + 1}/{data.MaleCustomization.bags.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.teeth) { teeth.Add($"({data.MaleCustomization.teeth.IndexOf(k) + 1}/{data.MaleCustomization.teeth.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.hats) { hats.Add($"({data.MaleCustomization.hats.IndexOf(k) + 1}/{data.MaleCustomization.hats.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.gunBelts) { gunBelts.Add($"({data.MaleCustomization.gunBelts.IndexOf(k) + 1}/{data.MaleCustomization.gunBelts.Count()}) 0x{k.ToString("X08")}"); }
                        //foreach (var k in data.MaleCustomization.skirts) { skirts.Add($"({data.MaleCustomization.skirts.IndexOf(k) + 1}/{data.MaleCustomization.skirts.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.belts2) { belts2.Add($"({data.MaleCustomization.belts2.IndexOf(k) + 1}/{data.MaleCustomization.belts2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.ponchos2) { ponchos2.Add($"({data.MaleCustomization.ponchos2.IndexOf(k) + 1}/{data.MaleCustomization.ponchos2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.bodyStyle) { bodyStyle.Add($"({data.MaleCustomization.bodyStyle.IndexOf(k) + 1}/{data.MaleCustomization.bodyStyle.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.offHandHolsters) { offHandHolsters.Add($"({data.MaleCustomization.offHandHolsters.IndexOf(k) + 1}/{data.MaleCustomization.offHandHolsters.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.coats2) { coats2.Add($"({data.MaleCustomization.coats2.IndexOf(k) + 1}/{data.MaleCustomization.coats2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.eyes) { eyes.Add($"({data.MaleCustomization.eyes.IndexOf(k) + 1}/{data.MaleCustomization.eyes.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.gloves) { gloves.Add($"({data.MaleCustomization.gloves.IndexOf(k) + 1}/{data.MaleCustomization.gloves.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.gunBeltAccessory) { gunBeltAccessory.Add($"({data.MaleCustomization.gunBeltAccessory.IndexOf(k) + 1}/{data.MaleCustomization.gunBeltAccessory.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.rings2) { rings2.Add($"({data.MaleCustomization.rings2.IndexOf(k) + 1}/{data.MaleCustomization.rings2.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.beard) { beard.Add($"({data.MaleCustomization.beard.IndexOf(k) + 1}/{data.MaleCustomization.beard.Count()}) 0x{k.ToString("X08")}"); }
                        foreach (var k in data.MaleCustomization.buckles2) { buckles2.Add($"({data.MaleCustomization.buckles2.IndexOf(k) + 1}/{data.MaleCustomization.buckles2.Count()}) 0x{k.ToString("X08")}"); }

                        maleCustomMenu.AddMenuItem(new MenuListItem("Boot Accessories", spurs, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Pants", pants, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Shirts", shirts, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Chaps", chaps, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Heads", faces, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Cloaks", ponchos, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Badges", badges, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Vests", vests, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Spats", legArmor, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Eyewear", glasses, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Neckwear", bandanas, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Closed Coats", coats, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Armor", bodyArmor, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Masks", masks, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Boots", boots, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Accessories", buckles, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Right-Hand Rings", rings, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Neckties", neckwear, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Bracelets", wristbands, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Lower Bodies", feetStyle, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Loadouts", belts, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Hair", hair, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Suspenders", suspenders, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Gauntlets", gauntlets, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Satchels", bags, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Teeth", teeth, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Hats", hats, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Gun Belts", gunBelts, 0));
                        //maleCustomMenu.AddMenuItem(new MenuListItem("Skirts", skirts, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Belts", belts2, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Ponchos", ponchos2, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Upper Bodies", bodyStyle, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Off-Hand Holsters", offHandHolsters, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Open Coats", coats2, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Eyes", eyes, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Gloves", gloves, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Gun Belt Accessory", gunBeltAccessory, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Left-Hand Rings", rings2, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Beard", beard, 0));
                        maleCustomMenu.AddMenuItem(new MenuListItem("Belt Buckles", buckles2, 0));

                        maleCustomMenu.OnListIndexChange += (m, item, oldIndex, newIndex, itemIndex) =>
                        {
                            uint hash;
                            switch (itemIndex)
                            {
                                case 0: hash = data.MaleCustomization.spurs[newIndex]; break;
                                case 1: hash = data.MaleCustomization.pants[newIndex]; break;
                                case 2: hash = data.MaleCustomization.shirts[newIndex]; break;
                                case 3: hash = data.MaleCustomization.chaps[newIndex]; break;
                                case 4: hash = data.MaleCustomization.faces[newIndex]; break;
                                case 5: hash = data.MaleCustomization.ponchos[newIndex]; break;
                                case 6: hash = data.MaleCustomization.badges[newIndex]; break;
                                case 7: hash = data.MaleCustomization.vests[newIndex]; break;
                                case 8: hash = data.MaleCustomization.legArmor[newIndex]; break;
                                case 9: hash = data.MaleCustomization.glasses[newIndex]; break;
                                case 10: hash = data.MaleCustomization.bandanas[newIndex]; break;
                                case 11: hash = data.MaleCustomization.coats[newIndex]; break;
                                case 12: hash = data.MaleCustomization.bodyArmor[newIndex]; break;
                                case 13: hash = data.MaleCustomization.masks[newIndex]; break;
                                case 14: hash = data.MaleCustomization.boots[newIndex]; break;
                                case 15: hash = data.MaleCustomization.buckles[newIndex]; break;
                                case 16: hash = data.MaleCustomization.rings[newIndex]; break;
                                case 17: hash = data.MaleCustomization.neckwear[newIndex]; break;
                                case 18: hash = data.MaleCustomization.wristbands[newIndex]; break;
                                case 19: hash = data.MaleCustomization.feetStyle[newIndex]; break;
                                case 20: hash = data.MaleCustomization.belts[newIndex]; break;
                                case 21: hash = data.MaleCustomization.hair[newIndex]; break;
                                case 22: hash = data.MaleCustomization.suspenders[newIndex]; break;
                                case 23: hash = data.MaleCustomization.gauntlets[newIndex]; break;
                                case 24: hash = data.MaleCustomization.bags[newIndex]; break;
                                case 25: hash = data.MaleCustomization.teeth[newIndex]; break;
                                case 26: hash = data.MaleCustomization.hats[newIndex]; break;
                                case 27: hash = data.MaleCustomization.gunBelts[newIndex]; break;
                                //case 28: hash = data.MaleCustomization.skirts[newIndex]; break;
                                case 28: hash = data.MaleCustomization.belts2[newIndex]; break;
                                case 29: hash = data.MaleCustomization.ponchos2[newIndex]; break;
                                case 30: hash = data.MaleCustomization.bodyStyle[newIndex]; break;
                                case 31: hash = data.MaleCustomization.offHandHolsters[newIndex]; break;
                                case 32: hash = data.MaleCustomization.coats2[newIndex]; break;
                                case 33: hash = data.MaleCustomization.eyes[newIndex]; break;
                                case 34: hash = data.MaleCustomization.gloves[newIndex]; break;
                                case 35: hash = data.MaleCustomization.gunBeltAccessory[newIndex]; break;
                                case 36: hash = data.MaleCustomization.rings2[newIndex]; break;
                                case 37: hash = data.MaleCustomization.beard[newIndex]; break;
                                case 38: hash = data.MaleCustomization.buckles2[newIndex]; break;
                                default:
                                    hash = 0;
                                    break;
                            }
                            if (hash != 0)
                            {
                                Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), hash, true, true, false);
                                currentMpClothes[itemIndex] = hash;
                            }
                        };

                        maleCustomMenu.OnListItemSelect += (m, listItem, selectedIndex, itemIndex) =>
                        {
                            uint hash;
                            switch (itemIndex)
                            {
                                case 0: hash = 0x18729F39; break;
                                case 1: hash = 0x1D4C528A; break;
                                case 2: hash = 0x2026C46D; break;
                                case 3: hash = 0x3107499B; break;
                                case 4: hash = 0x378AD10C; break;
                                case 5: hash = 0x3C1A74CD; break;
                                case 6: hash = 0x3F7F3587; break;
                                case 7: hash = 0x485EE834; break;
                                case 8: hash = 0x514ADCEA; break;
                                case 9: hash = 0x05E47CA6; break;
                                case 10: hash = 0x5FC29285; break;
                                case 11: hash = 0x0662AC34; break;
                                case 12: hash = 0x72E6EF74; break;
                                case 13: hash = 0x7505EF42; break;
                                case 14: hash = 0x777EC6EF; break;
                                case 15: hash = 0x79D7DF96; break;
                                case 16: hash = 0x7A6BBD0B; break;
                                case 17: hash = 0x7A96FACA; break;
                                case 18: hash = 0x7BC10759; break;
                                case 19: hash = 0x823687F5; break;
                                case 20: hash = 0x83887E88; break;
                                case 21: hash = 0x864B03AE; break;
                                case 22: hash = 0x877A2CF7; break;
                                case 23: hash = 0x91CE9B20; break;
                                case 24: hash = 0x94504D26; break;
                                case 25: hash = 0x96EDAE5C; break;
                                case 26: hash = 0x9925C067; break;
                                case 27: hash = 0x9B2C8B89; break;
                                //case 28: hash = 0xA0E3AB7F; break;
                                case 28: hash = 0xA6D134C6; break;
                                case 29: hash = 0xAF14310B; break;
                                case 30: hash = 0x0B3966C9; break;
                                case 31: hash = 0xB6B6122D; break;
                                case 32: hash = 0xE06D30CE; break;
                                case 33: hash = 0xEA24B45E; break;
                                case 34: hash = 0xEABE0032; break;
                                case 35: hash = 0xF1542D11; break;
                                case 36: hash = 0xF16A1D23; break;
                                case 37: hash = 0xF8016BCA; break;
                                case 38: hash = 0xFAE9107F; break;
                                default:
                                    hash = 0;
                                    break;
                            }

                            if (hash != 0)
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), hash, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                                currentMpClothes[itemIndex] = hash;
                            }
                        };
                    }
                    #endregion


                    appearanceMenu.AddMenuItem(femaleCustom);
                    appearanceMenu.AddMenuItem(maleCustom);

                    MenuController.BindMenuItem(appearanceMenu, femaleCustomMenu, femaleCustom);
                    MenuController.BindMenuItem(appearanceMenu, maleCustomMenu, maleCustom);


                    Menu bodyCustomizationMenu = new Menu("Body", "Customize MP character body");
                    MenuItem bodyCustomization = new MenuItem("MP Body Customization", "Customize MP character body") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    appearanceMenu.AddMenuItem(bodyCustomization);
                    MenuController.AddSubmenu(appearanceMenu, bodyCustomizationMenu);
                    MenuController.BindMenuItem(appearanceMenu, bodyCustomizationMenu, bodyCustomization);

                    List<string> bodySizes = new List<string>();
                    List<string> waistSizes = new List<String>();
                    foreach (var k in data.BodyCustomization.BodySizes) { bodySizes.Add($"({data.BodyCustomization.BodySizes.IndexOf(k) + 1}/{data.BodyCustomization.BodySizes.Count()}) 0x{k.ToString("X08")}"); }
                    foreach (var k in data.BodyCustomization.WaistSizes) { waistSizes.Add($"({data.BodyCustomization.WaistSizes.IndexOf(k) + 1}/{data.BodyCustomization.WaistSizes.Count()}) 0x{k.ToString("X08")}"); }

                    bodyCustomizationMenu.AddMenuItem(new MenuListItem("Body Size", bodySizes, 0));
                    bodyCustomizationMenu.AddMenuItem(new MenuListItem("Waist Size", waistSizes, 0));

                    bodyCustomizationMenu.OnListIndexChange += (m, item, oldIndex, newIndex, itemIndex) =>
                    {
                        int hash;
                        switch (itemIndex)
                        {
                            case 0: hash = data.BodyCustomization.BodySizes[newIndex]; break;
                            case 1: hash = data.BodyCustomization.WaistSizes[newIndex]; break;
                            default:
                                hash = 0;
                                break;
                        }

                        if (hash != 0)
                        {
                            Function.Call((Hash)0x1902C4CFCC5BE57C, PlayerPedId(), hash);
                            Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                            currentBodySettings[itemIndex] = hash;
                        }
                    };


                    Menu facialFeaturesMenu = new Menu("Facial Features", "Customize facial features");
                    MenuItem facialFeatures = new MenuItem("Facial Features", "Customize facial features") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    appearanceMenu.AddMenuItem(facialFeatures);
                    MenuController.AddSubmenu(appearanceMenu, facialFeaturesMenu);
                    MenuController.BindMenuItem(appearanceMenu, facialFeaturesMenu, facialFeatures);

                    foreach (data.FacialFeature feature in data.FacialFeatureData.FacialFeatures)
                    {
                        List<string> values = new List<string>();
                        int index = 0;
                        for (int f = feature.Min; f <= feature.Max; f = f + feature.Step)
                        {
                            values.Add((f * feature.Factor).ToString("0.0"));

                            if (f == feature.Value)
                            {
                                index = values.Count - 1;
                            }
                        }

                        MenuListItem item = new MenuListItem(feature.Name, values, index);
                        facialFeaturesMenu.AddMenuItem(item);

                        currentFacialFeatures[feature.Index] = feature.Value;
                    }

                    facialFeaturesMenu.OnListIndexChange += (m, listItem, oldIndex, newIndex, itemIndex) => {
                        float v = float.Parse(listItem.ListItems[newIndex]);
                        Function.Call((Hash)0x5653AB26C82938CF, PlayerPedId(), data.FacialFeatureData.FacialFeatures[itemIndex].Index, v);
                        Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), false, true, true, true, false);
                        currentFacialFeatures[data.FacialFeatureData.FacialFeatures[itemIndex].Index] = v;
                    };
                }

                if (PermissionsManager.IsAllowed(Permission.PMSavedPeds))
                {
                    MenuItem savedPeds = new MenuItem("Saved Peds", "Save and load peds.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    Menu savedPedsMenu = new Menu("Saved Peds", "Save and load peds.");
                    MenuController.AddSubmenu(appearanceMenu, savedPedsMenu);
                    appearanceMenu.AddMenuItem(savedPeds);
                    MenuController.BindMenuItem(appearanceMenu, savedPedsMenu, savedPeds);
                    List<MenuItem> savedPedSlots = new List<MenuItem>();
                    List<MenuCheckboxItem> defaultSavedPedCheckboxes = new List<MenuCheckboxItem>();

                    for (int i = 0; i <= 38; ++i)
                    {
                        currentMpClothes[i] = 0;
                    }

                    foreach (FacialFeature feature in data.FacialFeatureData.FacialFeatures)
                    {
                        currentFacialFeatures[feature.Index] = 0;
                    }
                    
                    for (int i = 0; i <= 1; ++i)
                    {
                        currentBodySettings[i] = 0;
                    }

                    for (int i = 1; i <= maxSavedPeds; ++i)
                    {
                        int pedIndex = i;

                        if (!StorageManager.TryGet("SavedPeds_" + pedIndex + "_name", out string pedName))
                        {
                            pedName = "Ped " + pedIndex;
                        }

                        MenuItem savedPed = new MenuItem(pedName) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                        if (pedIndex == UserDefaults.PlayerDefaultSavedPed)
                        {
                            savedPed.LeftIcon = MenuItem.Icon.STAR;
                        }
                        savedPedsMenu.AddMenuItem(savedPed);
                        savedPedSlots.Add(savedPed);

                        Menu savedPedOptionsMenu = new Menu(pedName);
                        MenuController.AddSubmenu(savedPedsMenu, savedPedOptionsMenu);
                        MenuController.BindMenuItem(savedPedsMenu, savedPedOptionsMenu, savedPed);

                        MenuItem load = new MenuItem("Load", "Load this ped.");
                        MenuItem save = new MenuItem("Save", "Save current ped to this slot.");
                        MenuCheckboxItem isDefault = new MenuCheckboxItem("Default", "Load this ped automatically when you respawn.", pedIndex == UserDefaults.PlayerDefaultSavedPed);
                        defaultSavedPedCheckboxes.Add(isDefault);
                        savedPedOptionsMenu.AddMenuItem(load);
                        savedPedOptionsMenu.AddMenuItem(save);
                        savedPedOptionsMenu.AddMenuItem(isDefault);

                        savedPedOptionsMenu.OnItemSelect += async (m, item, index) =>
                        {
                            if (item == load)
                            {
                                LoadDefaultPed(pedIndex);
                            }
                            else if (item == save)
                            {
                                string newName = await GetUserInput("Enter ped name", pedName, 20);

                                if (newName != null)
                                {
                                    StorageManager.Save("SavedPeds_" + pedIndex + "_model", GetEntityModel(PlayerPedId()), true);
                                    StorageManager.Save("SavedPeds_" + pedIndex + "_outfit", Int32.Parse(playerOutfit.CurrentItem), true);
                                    foreach (KeyValuePair<int, uint> entry in currentMpClothes)
                                    {
                                        StorageManager.Save("SavedPeds_" + pedIndex + "_mp_" + entry.Key, (int)entry.Value, true);
                                    }
                                    foreach (KeyValuePair<uint, float> entry in currentFacialFeatures)
                                    {
                                        StorageManager.Save("SavedPeds_" + pedIndex + "_ff_" + entry.Key, entry.Value, true);
                                    }
                                    foreach (KeyValuePair<int, int> entry in currentBodySettings)
                                    {
                                        StorageManager.Save("SavedPeds_" + pedIndex + "_bc_" + entry.Key, entry.Value, true);
                                    }
                                    StorageManager.Save("SavedPeds_" + pedIndex + "_name", newName, true);
                                    savedPed.Text = newName;
                                    savedPedOptionsMenu.MenuTitle = newName;
                                    pedName = newName;
                                }
                            }
                        };

                        savedPedOptionsMenu.OnCheckboxChange += (m, item, index, _checked) =>
                        {
                            if (item == isDefault)
                            {
                                if (_checked)
                                {
                                    UserDefaults.PlayerDefaultSavedPed = pedIndex;

                                    foreach (var cb in defaultSavedPedCheckboxes)
                                    {
                                        if (cb != isDefault)
                                        {
                                            cb.Checked = false;
                                        }
                                    }
                                }
                                else
                                {
                                    UserDefaults.PlayerDefaultSavedPed = 0;
                                }

                                for (int slot = 0; slot < savedPedSlots.Count; ++slot)
                                {
                                    if (_checked && slot + 1 == pedIndex)
                                    {
                                        savedPedSlots[slot].LeftIcon = MenuItem.Icon.STAR;
                                    }
                                    else
                                    {
                                        savedPedSlots[slot].LeftIcon = MenuItem.Icon.NONE;
                                    }
                                }
                            }
                        };
                    }
                }
            }

            if (PermissionsManager.IsAllowed(Permission.PMScenarios))
            {
                MenuItem scenarioMenuBtn = new MenuItem("Scenarios", "Player scenario options.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                MenuController.AddSubmenu(menu, scenarioMenu);
                menu.AddMenuItem(scenarioMenuBtn);
                MenuController.BindMenuItem(menu, scenarioMenu, scenarioMenuBtn);

                MenuItem stopScenario = new MenuItem("Stop Scenario", "Stop any active scenarios.");
                scenarioMenu.AddMenuItem(stopScenario);

                scenarioMenu.OnItemSelect += (m, item, index) =>
                {
                    if (item == stopScenario)
                    {
                        ClearPedTasks(PlayerPedId(), 0, 0);
                    }
                };

                AddScenarioSubmenu(scenarioMenu, data.ScenarioData.ScenarioHashes, "All Scenarios", "A list of all scenarios.");

                Menu animalScenariosMenu = new Menu("Animal Scenarios", "Scenarios for animal peds.");
                MenuItem animalScenarios = new MenuItem("Animal Scenarios", "Scenarios for animal peds.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                MenuController.AddSubmenu(scenarioMenu, animalScenariosMenu);
                scenarioMenu.AddMenuItem(animalScenarios);
                MenuController.BindMenuItem(scenarioMenu, animalScenariosMenu, animalScenarios);

                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.BearScenarioHashes, "Bear Scenarios", "Scenarios for bear peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.CatScenarioHashes, "Cat Scenarios", "Scenarios for cat peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.CoyoteScenarioHashes, "Coyote Scenarios", "Scenarios for coyote peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.DeerScenarioHashes, "Deer Scenarios", "Scenarios for deer peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.DogScenarioHashes, "Dog Scenarios", "Scenarios for dog peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.EagleScenarioHashes, "Eagle Scenarios", "Scenarios for eagle peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.ElkScenarioHashes, "Elk Scenarios", "Scenarios for elk peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.FoxScenarioHashes, "Fox Scenarios", "Scenarios for fox peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.HorseScenarioHashes, "Horse Scenarios", "Scenarios for horse peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.GoatScenarioHashes, "Goat Scenarios", "Scenarios for goat peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.MooseScenarioHashes, "Moose Scenarios", "Scenarios for moose peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.PigScenarioHashes, "Pig Scenarios", "Scenarios for pig peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.SheepScenarioHashes, "Sheep Scenarios", "Scenarios for sheep peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.SkunkScenarioHashes, "Skunk Scenarios", "Scenarios for skunk peds.");
                AddScenarioSubmenu(animalScenariosMenu, data.ScenarioData.WolfScenarioHashes, "Wolf Scenarios", "Scenarios for wolf peds.");
            }

            if (PermissionsManager.IsAllowed(Permission.PMEmotes))
            {
                Menu emotesMenu = new Menu("Emotes", "Player emotes.");
                MenuItem emotes = new MenuItem("Emotes", "Player emotes.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(emotes);
                MenuController.AddSubmenu(menu, emotesMenu);
                MenuController.BindMenuItem(menu, emotesMenu, emotes);

                AddEmotesSubmenu(emotesMenu, data.ScenarioData.ReactionEmotes, 0, "Reactions", "Reaction emotes.");
                AddEmotesSubmenu(emotesMenu, data.ScenarioData.ActionEmotes, 1, "Actions", "Action emotes.");
                AddEmotesSubmenu(emotesMenu, data.ScenarioData.TauntEmotes, 2, "Taunts", "Taunt emotes.");
                AddEmotesSubmenu(emotesMenu, data.ScenarioData.GreetEmotes, 3, "Greetings", "Greeting emotes.");
                AddEmotesSubmenu(emotesMenu, data.ScenarioData.DanceEmotes, 4, "Dances", "Dance emotes.");
            }

            if (PermissionsManager.IsAllowed(Permission.PMWalkingStyle))
            {
                Menu walkingStyleMenu = new Menu("Walk Style", "Change your walking style.");
                MenuItem walkingStyle = new MenuItem("Walk Style", "Change your walking style.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(walkingStyle);
                MenuController.AddSubmenu(menu, walkingStyleMenu);
                MenuController.BindMenuItem(menu, walkingStyleMenu, walkingStyle);

                MenuListItem bases = new MenuListItem("Base", data.ScenarioData.StanceBases, 0);
                MenuListItem styles = new MenuListItem("Style", data.ScenarioData.StanceStyles, 0);

                walkingStyleMenu.AddMenuItem(bases);
                walkingStyleMenu.AddMenuItem(styles);

                walkingStyleMenu.OnListItemSelect += (m, listItem, selectedIndex, itemIndex) =>
                {
                    if (listItem == bases)
                    {
                        Function.Call((Hash)0x923583741DC87BCE, PlayerPedId(), data.ScenarioData.StanceBases[selectedIndex]);
                    }
                    else if (listItem == styles)
                    {
                        Function.Call((Hash)0x89F5E7ADECCCB49C, PlayerPedId(), data.ScenarioData.StanceStyles[selectedIndex]);
                    }
                };
            }

            if (PermissionsManager.IsAllowed(Permission.PMMood))
            {
                menu.AddMenuItem(moods);
            }

            menu.OnDynamicListItemSelect += (m, item, currentItem) =>
            {
                if (item == playerOutfit)
                {
                    if (int.TryParse(currentItem, out int val))
                    {
                        SetPedOutfitPreset(PlayerPedId(), val, 0);
                    }
                }
            };

            menu.OnCheckboxChange += (m, item, index, _checked) =>
            {
                if (item == godModeBox)
                {
                    UserDefaults.PlayerGodMode = _checked;
                    SetEntityInvincible(PlayerPedId(), _checked);
                }
                else if (item == infiniteStamina)
                {
                    UserDefaults.PlayerInfiniteStamina = _checked;
                }
                else if (item == infiniteDeadEye)
                {
                    UserDefaults.PlayerInfiniteDeadEye = _checked;
                }
                else if (item == everyoneIgnore)
                {
                    UserDefaults.PlayerEveryoneIgnore = _checked;
                    SetEveryoneIgnorePlayer(PlayerId(), _checked);
                }
                else if (item == disableRagdoll)
                {
                    UserDefaults.PlayerDisableRagdoll = _checked;
                    SetPedCanRagdoll(PlayerPedId(), !_checked);
                }
            };

            menu.OnItemSelect += (m, item, index) =>
            {
                if (item == clearPedTasks)
                {
                    Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, PlayerPedId(), true, false);
                }
                else if (item == hogtieSelf)
                {
                    Function.Call(Hash.TASK_KNOCKED_OUT_AND_HOGTIED, PlayerPedId(), false, false);
                }
                else if (item == cleanPed)
                {
                    int ped = PlayerPedId();
                    ClearPedEnvDirt(ped);
                    ClearPedDamageDecalByZone(ped, 10, "ALL");
                    ClearPedBloodDamage(ped);
                }
                else if (item == dryPed)
                {
                    ClearPedWetness(PlayerPedId());
                }
                else if (item == killSelf)
                {
                    SetEntityInvincible(PlayerPedId(), false);
                    ApplyDamageToPed(PlayerPedId(), 500000, 0, 1, 1);
                }
            };

            menu.OnListItemSelect += (m, item, listIndex, itemIndex) =>
            {
                if (item == restoreInnerCores)
                {
                    switch (listIndex)
                    {
                        case 0:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 0, 100);
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 1, 100);
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 2, 100);
                            break;
                        case 1:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 0, 100);
                            break;
                        case 2:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 1, 100);
                            break;
                        case 3:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, PlayerPedId(), 2, 100);
                            break;
                        default:
                            break;
                    }
                }
                else if (item == restoreOuterCores)
                {
                    switch (listIndex)
                    {
                        case 0: // all
                            Function.Call((Hash)0xAC2767ED8BDFAB15, PlayerPedId(), 100.0, 0);
                            RestorePlayerStamina(PlayerId(), 100.0f);
                            // deadeye?
                            break;
                        case 1: // health
                            Function.Call((Hash)0xAC2767ED8BDFAB15, PlayerPedId(), 100.0, 0);
                            break;
                        case 2: // stamina
                            RestorePlayerStamina(PlayerId(), 100.0f);
                            break;
                        case 3: //deadeye
                            // deadeye?
                            break;
                        default: // invalid index
                            break;
                    }
                }
                else if (item == fortifyCoresList)
                {
                    switch (listIndex)
                    {
                        case 0: // all
                                // API definition incorrectly has p2 as int when it must be a float
                                //EnableAttributeOverpower(PlayerPedId(), 0, 100, 1);
                                //EnableAttributeOverpower(PlayerPedId(), 1, 100, 1);
                                //EnableAttributeOverpower(PlayerPedId(), 2, 100, 1);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 0, 100.0f, true);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 1, 100.0f, true);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 2, 100.0f, true);
                            break;
                        case 1: // health
                            //EnableAttributeOverpower(PlayerPedId(), 0, 100, 1);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 0, 100.0f, true);
                            break;
                        case 2: // stamina
                            //EnableAttributeOverpower(PlayerPedId(), 1, 100, 1);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 1, 100.0f, true);
                            break;
                        case 3: //dead eye
                            //EnableAttributeOverpower(PlayerPedId(), 2, 100, 1);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, PlayerPedId(), 2, 100.0f, true);
                            break;
                        default: // invalid index
                            break;
                    }
                }
                else if (item == moods)
                {
                    SetMood(item.GetCurrentSelection());
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }

    }
}
