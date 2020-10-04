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

        private static int GetLastMount(int ped)
        {
            return Function.Call<int>((Hash)0x4C8B59171957BCF7, ped);
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

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

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
