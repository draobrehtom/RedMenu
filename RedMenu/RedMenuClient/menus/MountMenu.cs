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
    class MountMenu
    {
        private static Menu menu = new Menu("Mount", "Mount related options.");
        private static bool setupDone = false;
        private static int currentMount = 0;
        private static Random rng = new Random();

        private const int maxSavedMounts = 100;

        private static Dictionary<int, uint> currentMountComponents = new Dictionary<int, uint>();

        private static int GetLastMount(int ped)
        {
            return Function.Call<int>((Hash)0x4C8B59171957BCF7, ped);
        }

        private static int CreatePed_2(uint model, float x, float y, float z, float heading, bool isNetwork, bool netMissionEntity, bool p7, bool p8)
        {
            return Function.Call<int>((Hash)0xD49F9B0955C367DE, model, x, y, z, heading, isNetwork, netMissionEntity, p7, p8);
        }

        private static int BlipAddForEntity(int blipHash, int entity)
        {
            return Function.Call<int>((Hash)0x23F74C2FDA6E7C61, blipHash, entity);
        }

        private static void SetPedFaceFeature(int ped, int index, float value)
        {
            Function.Call((Hash)0x5653AB26C82938CF, ped, index, value);
        }

        private static void UpdatePedVariation(int ped, bool p1, bool p2, bool p3, bool p4, bool p5)
        {
            Function.Call((Hash)0xCC8CA3E88256E58F, ped, p1, p2, p3, p4, p5);
        }

        private static bool IsThisModelAHorse(int ped)
        {
            return Function.Call<bool>((Hash)0x772A1969F649E902, ped);
        }

        private static int GetTargetMount(int ped)
        {
            if (IsThisModelAHorse(GetEntityModel(ped)))
            {
                return ped;
            }
            else
            {
                int lastMount = GetLastMount(ped);

                if (DoesEntityExist(lastMount))
                {
                    return GetLastMount(ped);
                }
                else
                {
                    return currentMount;
                }
            }
        }

        private async static void SpawnMount(uint model)
        {
            if (currentMount != 0)
            {
                DeleteEntity(ref currentMount);
                currentMount = 0;
            }

            int ped = PlayerPedId();
            Vector3 coords = GetEntityCoords(ped, false, false);
            float h = GetEntityHeading(ped);

            // Get a point in front of the player
            float r = -h * (float)(Math.PI / 180);
            float x2 = coords.X + (float)(5 * Math.Sin(r));
            float y2 = coords.Y + (float)(5 * Math.Cos(r));

            if (IsModelInCdimage(model))
            {
                RequestModel(model, false);
                while (!HasModelLoaded(model))
                {
                    RequestModel(model, false);
                    await BaseScript.Delay(0);
                }

                currentMount = CreatePed_2(model, x2, y2, coords.Z, 0.0f, true, true, true, true);
                SetModelAsNoLongerNeeded(model);
                SetPedOutfitPreset(currentMount, 0, 0);
                SetPedConfigFlag(currentMount, 297, true); // Enable leading
                SetPedConfigFlag(currentMount, 312, true); // Won't flee when shooting
                BlipAddForEntity(-1230993421, currentMount);
            }
            else
            {
                Debug.WriteLine($"^1[ERROR] This ped model is not present in the game files {model}.^7");
            }
        }

        private static void ResetCurrentMountComponents()
        {
            int[] keys = currentMountComponents.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                currentMountComponents[keys[i]] = 0;
            }
        }

        private static async Task<string> GetUserInput(string windowTitle, string defaultText, int maxInputLength)
        {
            var spacer = "\t";
            AddTextEntry($"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", $"{windowTitle ?? "Enter"}:{spacer}(MAX {maxInputLength} Characters)");
            DisplayOnscreenKeyboard(1, $"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", "", defaultText ?? "", "", "", "", maxInputLength); await BaseScript.Delay(0);
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

        private static void SetMountSex(int mount, int sex)
        {
            switch (sex)
            {
                case 0:
                    SetPedFaceFeature(mount, 41611, 0.0f);
                    break;
                case 1:
                    SetPedFaceFeature(mount, 41611, 1.0f);
                    break;
                default:
                    break;
            }

            UpdatePedVariation(mount, false, true, true, true, false);
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuListItem restoreCores = new MenuListItem("Restore Cores", new List<string>() { "All", "Health", "Stamina" }, 0, "Restore horse inner cores.");
            MenuListItem fortifyCores = new MenuListItem("Fortify Cores", new List<string>() { "All", "Health", "Stamina" }, 0, "Fortify horse inner cores.");
            MenuItem cleanMount = new MenuItem("Clean Mount", "Remove all dirt and other decals from the mount you are currently riding.");
            MenuItem deleteMount = new MenuItem("Delete Mount", "Delete the mount you are currently riding.");

            List<string> mounts = new List<string>();
            MenuListItem mountPeds = new MenuListItem("Spawn Mount", mounts, 0, "Spawn a mount.");
            for (int i = 0; i < data.MountData.MountHashes.Count(); i++)
            {
                mounts.Add($"{data.MountData.MountHashes[i]} ({i + 1}/{data.MountData.MountHashes.Count()}");
            }

            MenuListItem sex = new MenuListItem("Sex", new List<string>() { "Male", "Female" }, 0, "Set the sex of your mount.");

            MenuItem randomMount = new MenuItem("Spawn Random Mount", "Spawn a random mount.");
            MenuItem randomTack = new MenuItem("Randomize Tack", "Add random tack to your horse.");

            if (PermissionsManager.IsAllowed(Permission.MMSavedMounts))
            {
                MenuItem savedMounts = new MenuItem("Saved Mounts", "Save and load mounts.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                Menu savedMountsMenu = new Menu("Saved Mounts", "Save and load mounts");
                menu.AddMenuItem(savedMounts);
                MenuController.AddSubmenu(menu, savedMountsMenu);
                MenuController.BindMenuItem(menu, savedMountsMenu, savedMounts);

                for (int i = 0; i <= 9; ++i)
                {
                    currentMountComponents[i] = 0;
                }

                for (int i = 1; i <= maxSavedMounts; ++i)
                {
                    int mountIndex = i;
                    
                    if (!StorageManager.TryGet("SavedMounts_" + mountIndex + "_name", out string mountName))
                    {
                        mountName = "Mount " + mountIndex;
                    }

                    MenuItem savedMount = new MenuItem(mountName) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    savedMountsMenu.AddMenuItem(savedMount);

                    Menu savedMountOptionsMenu = new Menu(mountName);
                    MenuController.AddSubmenu(savedMountsMenu, savedMountOptionsMenu);
                    MenuController.BindMenuItem(savedMountsMenu, savedMountOptionsMenu, savedMount);

                    MenuItem load = new MenuItem("Load", "Load this mount.");
                    MenuItem save = new MenuItem("Save", "Save current mount to this slot.");
                    savedMountOptionsMenu.AddMenuItem(load);
                    savedMountOptionsMenu.AddMenuItem(save);

                    savedMountOptionsMenu.OnItemSelect += async (m, item, index) =>
                    {
                        if (item == load)
                        {
                            if (StorageManager.TryGet("SavedMounts_" + mountIndex + "_model", out int model))
                            {
                                SpawnMount((uint)model);
                            }

                            await BaseScript.Delay(500);

                            ResetCurrentMountComponents();

                            int[] keys = currentMountComponents.Keys.ToArray();

                            for (int j = 0; j < keys.Length; ++j)
                            {
                                if (StorageManager.TryGet("SavedMounts_" + mountIndex + "_c_" + keys[j], out int hash))
                                {
                                    switch ((uint)hash)
                                    {
                                        case 0x17CEB41A:
                                        case 0x5447332:
                                        case 0x80451C25:
                                        case 0xA63CAE10:
                                        case 0xAA0217AB:
                                        case 0xBAA7E618:
                                        case 0xDA6DADCA:
                                        case 0xEFB31921:
                                        case 0x1530BE1C:
                                            Function.Call((Hash)0xD710A5007C2AC539, currentMount, hash, 0);
                                            Function.Call((Hash)0xCC8CA3E88256E58F, currentMount, false, true, true, true, false);
                                            break;
                                        default:
                                            Function.Call((Hash)0xD3A7B003ED343FD9, currentMount, (uint)hash, true, true, false);
                                            break;
                                    }
                                    currentMountComponents[keys[j]] = (uint)hash;
                                }
                                else
                                {
                                    currentMountComponents[keys[j]] = 0;
                                }
                            }

                            if (StorageManager.TryGet("SavedMounts_" + mountIndex + "_sex", out int mountSex))
                            {
                                SetMountSex(currentMount, mountSex);
                            }

                            SetPedPromptName(currentMount, mountName);
                        }
                        else if (item == save)
                        {
                            string newName = await GetUserInput("Enter mount name", mountName, 20);

                            if (newName != null)
                            {
                                StorageManager.Save("SavedMounts_" + mountIndex + "_model", GetEntityModel(currentMount), true);
                                foreach (KeyValuePair<int, uint> entry in currentMountComponents)
                                {
                                    StorageManager.Save("SavedMounts_" + mountIndex + "_c_" + entry.Key, (int)entry.Value, true);
                                }
                                StorageManager.Save("SavedMounts_" + mountIndex + "_sex", sex.ListIndex, true);
                                StorageManager.Save("SavedMounts_" + mountIndex + "_name", newName, true);
                                savedMount.Text = newName;
                                savedMountOptionsMenu.MenuTitle = newName;
                                mountName = newName;
                            }
                        }
                    };
                }
            }

            if (PermissionsManager.IsAllowed(Permission.MMSpawn))
            {
                menu.AddMenuItem(mountPeds);
                menu.AddMenuItem(randomMount);
            }

            if (PermissionsManager.IsAllowed(Permission.MMSex))
            {
                menu.AddMenuItem(sex);
            }

            if (PermissionsManager.IsAllowed(Permission.MMTack))
            {
                Menu tackMenu = new Menu("Tack", "Customize mount tack.");
                MenuItem tack = new MenuItem("Tack", "Customize mount tack.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(tack);
                MenuController.AddSubmenu(menu, tackMenu);
                MenuController.BindMenuItem(menu, tackMenu, tack);

                menu.AddMenuItem(randomTack);

                List<string> blankets = new List<string>();
                List<string> grips = new List<string>();
                List<string> bags = new List<string>();
                List<string> tails = new List<string>();
                List<string> manes = new List<string>();
                List<string> saddles = new List<string>();
                List<string> stirrups = new List<string>();
                List<string> rolls = new List<string>();
                List<string> lanterns = new List<string>();
                foreach (var k in data.MountData.BlanketHashes) { blankets.Add($"({data.MountData.BlanketHashes.IndexOf(k) + 1}/{data.MountData.BlanketHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.GripHashes) { grips.Add($"({data.MountData.GripHashes.IndexOf(k) + 1}/{data.MountData.GripHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.BagHashes) { bags.Add($"({data.MountData.BagHashes.IndexOf(k) + 1}/{data.MountData.BagHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.TailHashes) { tails.Add($"({data.MountData.TailHashes.IndexOf(k) + 1}/{data.MountData.TailHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.ManeHashes) { manes.Add($"({data.MountData.ManeHashes.IndexOf(k) + 1}/{data.MountData.ManeHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.SaddleHashes) { saddles.Add($"({data.MountData.SaddleHashes.IndexOf(k) + 1}/{data.MountData.SaddleHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.StirrupHashes) { stirrups.Add($"({data.MountData.StirrupHashes.IndexOf(k) + 1}/{data.MountData.StirrupHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.RollHashes) { rolls.Add($"({data.MountData.RollHashes.IndexOf(k) + 1}/{data.MountData.RollHashes.Count()}) 0x{k.ToString("X08")}"); }
                foreach (var k in data.MountData.LanternHashes) { lanterns.Add($"({data.MountData.LanternHashes.IndexOf(k) + 1}/{data.MountData.LanternHashes.Count()}) 0x{k.ToString("X08")}"); }
                tackMenu.AddMenuItem(new MenuListItem("Blanket", blankets, 0));
                tackMenu.AddMenuItem(new MenuListItem("Grip", grips, 0));
                tackMenu.AddMenuItem(new MenuListItem("Bag", bags, 0));
                tackMenu.AddMenuItem(new MenuListItem("Tail", tails, 0));
                tackMenu.AddMenuItem(new MenuListItem("Mane", manes, 0));
                tackMenu.AddMenuItem(new MenuListItem("Saddle", saddles, 0));
                tackMenu.AddMenuItem(new MenuListItem("Stirrups", stirrups, 0));
                tackMenu.AddMenuItem(new MenuListItem("Roll", rolls, 0));
                tackMenu.AddMenuItem(new MenuListItem("Lantern", lanterns, 0));

                tackMenu.OnListIndexChange += (m, item, oldIndex, newIndex, itemIndex) =>
                {
                    uint hash;
                    switch (itemIndex)
                    {
                        case 0: hash = data.MountData.BlanketHashes[newIndex]; break;
                        case 1: hash = data.MountData.GripHashes[newIndex]; break;
                        case 2: hash = data.MountData.BagHashes[newIndex]; break;
                        case 3: hash = data.MountData.TailHashes[newIndex]; break;
                        case 4: hash = data.MountData.ManeHashes[newIndex]; break;
                        case 5: hash = data.MountData.SaddleHashes[newIndex]; break;
                        case 6: hash = data.MountData.StirrupHashes[newIndex]; break;
                        case 7: hash = data.MountData.RollHashes[newIndex]; break;
                        case 8: hash = data.MountData.LanternHashes[newIndex]; break;
                        default: hash = 0; break;
                    }
                    if (hash != 0)
                    {
                        Function.Call((Hash)0xD3A7B003ED343FD9, GetTargetMount(PlayerPedId()), hash, true, true, false);
                        currentMountComponents[itemIndex] = hash;
                    }
                };

                tackMenu.OnListItemSelect += (m, item, selectedIndex, itemIndex) =>
                {
                    uint hash;
                    switch (itemIndex)
                    {
                        case 0: hash = 0x17CEB41A; break;
                        case 1: hash = 0x5447332; break;
                        case 2: hash = 0x80451C25; break;
                        case 3: hash = 0xA63CAE10; break;
                        case 4: hash = 0xAA0217AB; break;
                        case 5: hash = 0xBAA7E618; break;
                        case 6: hash = 0xDA6DADCA; break;
                        case 7: hash = 0xEFB31921; break;
                        case 8: hash = 0x1530BE1C; break;
                        default: hash = 0; break;
                    }
                    if (hash != 0)
                    {
                        Function.Call((Hash)0xD710A5007C2AC539, GetTargetMount(PlayerPedId()), hash, 0);
                        Function.Call((Hash)0xCC8CA3E88256E58F, GetTargetMount(PlayerPedId()), false, true, true, true, false);
                        currentMountComponents[itemIndex] = hash;
                    }
                };
            }

            if (PermissionsManager.IsAllowed(Permission.MMRestoreCores))
            {
                menu.AddMenuItem(restoreCores);
            }

            if (PermissionsManager.IsAllowed(Permission.MMFortifyCores))
            {
                menu.AddMenuItem(fortifyCores);
            }

            if (PermissionsManager.IsAllowed(Permission.MMClean))
            {
                menu.AddMenuItem(cleanMount);
            }

            if (PermissionsManager.IsAllowed(Permission.MMDelete))
            {
                menu.AddMenuItem(deleteMount);
            }

            menu.OnItemSelect += (m, item, index) =>
            {
                if (item == cleanMount)
                {
                    int ped = GetTargetMount(PlayerPedId());
                    ClearPedEnvDirt(ped);
                    ClearPedDamageDecalByZone(ped, 10, "ALL");
                    ClearPedBloodDamage(ped);
                }
                else if (item == deleteMount)
                {
                    int mount = GetMount(PlayerPedId());

                    if (mount != 0)
                    {
                        DeleteEntity(ref mount);
                    }
                }
                else if (item == randomMount)
                {
                    int r = rng.Next(data.MountData.MountHashes.Count);
                    uint model = (uint)GetHashKey(data.MountData.MountHashes[r]);
                    SpawnMount(model);
                }
                else if (item == randomTack)
                {
                    int rBlanket = rng.Next(data.MountData.BlanketHashes.Count);
                    int rGrip = rng.Next(data.MountData.GripHashes.Count);
                    int rBag = rng.Next(data.MountData.BagHashes.Count);
                    int rSaddle = rng.Next(data.MountData.SaddleHashes.Count);
                    int rStirrup = rng.Next(data.MountData.StirrupHashes.Count);
                    int rRoll = rng.Next(data.MountData.RollHashes.Count);

                    int mount = GetTargetMount(PlayerPedId());

                    uint hBlanket = data.MountData.BlanketHashes[rBlanket];
                    uint hGrip = data.MountData.GripHashes[rGrip];
                    uint hBag = data.MountData.BagHashes[rBag];
                    uint hSaddle = data.MountData.SaddleHashes[rSaddle];
                    uint hStirrup = data.MountData.StirrupHashes[rStirrup];
                    uint hRoll = data.MountData.RollHashes[rRoll];

                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hBlanket, true, true, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hGrip, true, true, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hBag, true, true, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hSaddle, true, true, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hStirrup, true, true, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, mount, hRoll, true, true, false);

                    currentMountComponents[0] = hBlanket;
                    currentMountComponents[1] = hGrip;
                    currentMountComponents[2] = hBag;
                    currentMountComponents[5] = hSaddle;
                    currentMountComponents[6] = hStirrup;
                    currentMountComponents[7] = hRoll;
                }
            };

            menu.OnListItemSelect += async (m, item, listIndex, itemIndex) =>
            {
                if (item == mountPeds)
                {
                    uint model = (uint)GetHashKey(data.MountData.MountHashes[listIndex]);
                    SpawnMount(model);
                }
                else if (item == sex)
                {
                    SetMountSex(GetTargetMount(PlayerPedId()), listIndex);
                }
                else if (item == restoreCores)
                {
                    switch (listIndex)
                    {
                        case 0:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, GetTargetMount(PlayerPedId()), 0, 100);
                            Function.Call<int>((Hash)0xC6258F41D86676E0, GetTargetMount(PlayerPedId()), 1, 100);
                            break;
                        case 1:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, GetTargetMount(PlayerPedId()), 0, 100);
                            break;
                        case 2:
                            Function.Call<int>((Hash)0xC6258F41D86676E0, GetTargetMount(PlayerPedId()), 1, 100);
                            break;
                        default:
                            break;
                    }
                }
                else if (item == fortifyCores)
                {
                    switch (listIndex)
                    {
                        case 0:
                            Function.Call((Hash)0x4AF5A4C7B9157D14, GetTargetMount(PlayerPedId()), 0, 100.0f, true);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, GetTargetMount(PlayerPedId()), 1, 100.0f, true);
                            break;
                        case 1:
                            Function.Call((Hash)0x4AF5A4C7B9157D14, GetTargetMount(PlayerPedId()), 0, 100.0f, true);
                            break;
                        case 2:
                            Function.Call((Hash)0x4AF5A4C7B9157D14, GetTargetMount(PlayerPedId()), 1, 100.0f, true);
                            break;
                        default:
                            break;
                    }
                }
            };

            menu.OnListIndexChange += (m, listItem, oldIndex, newIndex, itemIndex) =>
            {
                if (listItem == sex)
                {
                    SetMountSex(GetTargetMount(PlayerPedId()), newIndex);
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
