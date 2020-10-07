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

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            if (PermissionsManager.IsAllowed(Permission.MMSpawn))
            {
                List<string> mounts = new List<string>();
                MenuListItem mountPeds = new MenuListItem("Spawn Mount", mounts, 0, "Spawn a mount.");
                for (int i = 0; i < data.PedModels.HorseHashes.Count(); i++)
                {
                    mounts.Add($"{data.PedModels.HorseHashes[i]} ({i + 1}/{data.PedModels.HorseHashes.Count()}");
                }
                menu.AddMenuItem(mountPeds);

                menu.OnListItemSelect += async (m, item, listIndex, itemIndex) =>
                {
                    if (item == mountPeds)
                    {
                        if (currentMount != 0)
                        {
                            DeleteEntity(ref currentMount);
                            currentMount = 0;
                        }

                        uint model = (uint)GetHashKey(data.PedModels.HorseHashes[listIndex]);

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
                };
            }

            if (PermissionsManager.IsAllowed(Permission.MMTack))
            {
                Menu tackMenu = new Menu("Tack", "Customize mount tack.");
                MenuItem tack = new MenuItem("Tack", "Customize mount tack.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(tack);
                MenuController.AddSubmenu(menu, tackMenu);
                MenuController.BindMenuItem(menu, tackMenu, tack);

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
                        Function.Call((Hash)0xD3A7B003ED343FD9, GetLastMount(PlayerPedId()), hash, true, true, false);
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
                        Function.Call((Hash)0xD710A5007C2AC539, GetLastMount(PlayerPedId()), hash, 0);
                        Function.Call((Hash)0xCC8CA3E88256E58F, GetLastMount(PlayerPedId()), false, true, true, true, false);
                    }
                };
            }
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
