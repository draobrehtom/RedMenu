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

            if (PermissionsManager.IsAllowed(Permission.WMDropWeapon))
            {
                menu.AddMenuItem(dropWeaponBtn);
                menu.AddMenuItem(dropAllWeaponsBtn);
            }

            Menu ammoMenu = new Menu("Ammo", "Get ammo.");
            MenuItem ammo = new MenuItem("Ammo", "Get ammo.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            menu.AddMenuItem(ammo);
            MenuController.AddSubmenu(menu, ammoMenu);
            MenuController.BindMenuItem(menu, ammoMenu, ammo);

            MenuItem refillAmmo = new MenuItem("Refill Ammo", "Get the maximum amount of ammo for the currently selected weapon.");
            MenuItem refillAllAmmo = new MenuItem("Refill All Ammo", "Get the maximum amount of ammo for all weapons.");

            MenuCheckboxItem infiniteAmmo = new MenuCheckboxItem("Infinite Ammo", "Never run out of ammo.");

            if (PermissionsManager.IsAllowed(Permission.WMRefillAmmo))
            {
                ammoMenu.AddMenuItem(refillAmmo);
                ammoMenu.AddMenuItem(refillAllAmmo);
            }

            if (PermissionsManager.IsAllowed(Permission.WMInfiniteAmmo))
            {
                ammoMenu.AddMenuItem(infiniteAmmo);
            }

            ammoMenu.OnItemSelect += (m, item, index) =>
            {
                if (item == refillAmmo)
                {
                    int ped = PlayerPedId();
                    uint weapon = 0;
                    GetCurrentPedWeapon(ped, ref weapon, true, 0, true);
                    SetPedAmmo(ped, weapon, 500);
                }
                else if (item == refillAllAmmo)
                {
                    foreach (var name in data.WeaponsData.AmmoHashes)
                    {
                        SetPedAmmoByType(PlayerPedId(), GetHashKey(name), 500);
                    }
                }
            };

            ammoMenu.OnCheckboxChange += (m, item, index, _checked) =>
            {
                if (item == infiniteAmmo)
                {
                    SetPedInfiniteAmmoClip(PlayerPedId(), _checked);
                }
            };

            Menu ammoTypesMenu = new Menu("Ammo Types", "Get ammo by type.");
            MenuItem ammoTypes = new MenuItem("Ammo Types", "Get ammo by type.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            ammoMenu.AddMenuItem(ammoTypes);
            MenuController.AddSubmenu(ammoMenu, ammoTypesMenu);
            MenuController.BindMenuItem(ammoMenu, ammoTypesMenu, ammoTypes);

            foreach (var name in data.WeaponsData.AmmoHashes)
            {
                MenuItem item = new MenuItem(name);
                ammoTypesMenu.AddMenuItem(item);
            }

            ammoTypesMenu.OnItemSelect += (m, item, index) =>
            {
                int hash = GetHashKey(data.WeaponsData.AmmoHashes[index]);
                SetPedAmmoByType(PlayerPedId(), hash, 500);
            };

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
            };


        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
