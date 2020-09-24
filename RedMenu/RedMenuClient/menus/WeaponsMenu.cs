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

namespace RedMenuClient.menus
{
    class WeaponsMenu
    {
        private static Menu menu = new Menu("Weapons Menu", $"Weapon & Ammo Options");
        private static bool setupDone = false;
        private static Menu allWeaponsMenu = new Menu("All Weapons", "A list of all weapons");

        private static void AddWeaponsSubmenu(List<string> hashes, string title, string description)
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
                uint model = (uint)GetHashKey(hashes[index]);
                GiveWeaponToPed_2(PlayerPedId(), model, 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);
            };
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuItem dropWeaponBtn = new MenuItem("Drop Weapon", "Remove the currently selected weapon from your inventory.");
            MenuItem dropAllWeaponsBtn = new MenuItem("Drop All Weapons", "Removes all weapons from your inventory.");
            MenuItem refillAmmo = new MenuItem("Refill Ammo", "Get the maximum amount of ammo for the currently selected weapon.");

            if (PermissionsManager.IsAllowed(Permission.WMDropWeapon))
            {
                menu.AddMenuItem(dropWeaponBtn);
                menu.AddMenuItem(dropAllWeaponsBtn);
            }

            if (PermissionsManager.IsAllowed(Permission.WMRefillAmmo))
            {
                menu.AddMenuItem(refillAmmo);
            }

            AddWeaponsSubmenu(data.WeaponsData.ItemHashes, "Items", "A list of equippable items.");
            AddWeaponsSubmenu(data.WeaponsData.BowHashes, "Bows", "A list of bows.");
            AddWeaponsSubmenu(data.WeaponsData.MeleeHashes, "Melee", "A list of melee weapons.");
            AddWeaponsSubmenu(data.WeaponsData.PistolHashes, "Pistols", "A list of pistols.");
            AddWeaponsSubmenu(data.WeaponsData.RepeaterHashes, "Repeaters", "A list of repeaters.");
            AddWeaponsSubmenu(data.WeaponsData.RevolverHashes, "Revolvers", "A list of revolvers.");
            AddWeaponsSubmenu(data.WeaponsData.RifleHashes, "Rifles", "A list of rifles.");
            AddWeaponsSubmenu(data.WeaponsData.ShotgunHashes, "Shotguns", "A list of shotguns.");
            AddWeaponsSubmenu(data.WeaponsData.SniperHashes, "Sniper Rifles", "A list of sniper rifles.");
            AddWeaponsSubmenu(data.WeaponsData.ThrownHashes, "Throwables", "A list of throwable weapons.");

            menu.OnItemSelect += (m, item, index) =>
            {
                if (item == dropWeaponBtn)
                {
                    int ped = PlayerPedId();
                    uint weapon = 0;
                    GetCurrentPedWeapon(ped, ref weapon, true, 0, true);
                    RemoveWeaponFromPed(ped, weapon, true, 0);
                }
                else if (item == dropAllWeaponsBtn)
                {
                    RemoveAllPedWeapons(PlayerPedId(), true, true);
                }
                else if (item == refillAmmo)
                {
                    int ped = PlayerPedId();
                    uint weapon = 0;
                    GetCurrentPedWeapon(ped, ref weapon, true, 0, true);
                    SetPedAmmo(ped, weapon, 500);
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
