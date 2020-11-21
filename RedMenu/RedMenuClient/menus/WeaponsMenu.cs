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
    static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }

        }
    }
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

        private static void GiveAllWeaponsInCategory(List<string> hashes)
        {
            foreach (var name in hashes)
            {
                GiveWeaponToPed_2(PlayerPedId(), (uint)GetHashKey(name), 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);
            }
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuItem dropWeaponBtn = new MenuItem("Drop Weapon", "Remove the currently selected weapon from your inventory.");
            MenuItem dropAllWeaponsBtn = new MenuItem("Drop All Weapons", "Removes all weapons from your inventory.");
            MenuItem getAllWeapons = new MenuItem("Get All Weapons", "Add all the weapons you can carry to your inventory.");

            if (PermissionsManager.IsAllowed(Permission.WMDropWeapon))
            {
                menu.AddMenuItem(dropWeaponBtn);
                menu.AddMenuItem(dropAllWeaponsBtn);
            }

            if (PermissionsManager.IsAllowed(Permission.WMGetAll))
            {
                menu.AddMenuItem(getAllWeapons);
            }

            Menu ammoMenu = new Menu("Ammo", "Get ammo.");
            MenuItem ammo = new MenuItem("Ammo", "Get ammo.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            menu.AddMenuItem(ammo);
            MenuController.AddSubmenu(menu, ammoMenu);
            MenuController.BindMenuItem(menu, ammoMenu, ammo);

            MenuItem refillAmmo = new MenuItem("Refill Ammo", "Get the maximum amount of ammo for the currently selected weapon.");
            MenuItem refillAllAmmo = new MenuItem("Refill All Ammo", "Get the maximum amount of ammo for all weapons.");

            MenuCheckboxItem infiniteAmmo = new MenuCheckboxItem("Infinite Ammo", "Never run out of ammo.", UserDefaults.WeaponInfiniteAmmo);

            if (PermissionsManager.IsAllowed(Permission.WMRefillAmmo))
            {
                ammoMenu.AddMenuItem(refillAmmo);
                ammoMenu.AddMenuItem(refillAllAmmo);
            }

            if (PermissionsManager.IsAllowed(Permission.WMInfiniteAmmo))
            {
                ammoMenu.AddMenuItem(infiniteAmmo);
                if (UserDefaults.WeaponInfiniteAmmo)
                {
                    SetPedInfiniteAmmoClip(PlayerPedId(), true);
                }
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
                    UserDefaults.WeaponInfiniteAmmo = _checked;
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

            if (PermissionsManager.IsAllowed(Permission.WMDualWield))
            {
                Menu dualWieldMenu = new Menu("Dual Wield", "Dual wield weapons");
                MenuItem dualWield = new MenuItem("Dual Wield", "Dual wield weapons") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(dualWield);
                MenuController.AddSubmenu(menu, dualWieldMenu);
                MenuController.BindMenuItem(menu, dualWieldMenu, dualWield);

                List<string> dualWieldWeapons = new List<string>();
                dualWieldWeapons.AddRange(data.WeaponsData.PistolHashes);
                dualWieldWeapons.AddRange(data.WeaponsData.RevolverHashes);
                dualWieldWeapons.Add("WEAPON_SHOTGUN_SAWEDOFF");

                MenuListItem rHandWeapon = new MenuListItem("Right Hand", dualWieldWeapons, 0, "Weapon held in right hand.");
                MenuListItem lHandWeapon = new MenuListItem("Left Hand", dualWieldWeapons, 0, "Weapon held in left hand.");
                MenuItem equip = new MenuItem("Equip", "Equip the selected weapons.");

                dualWieldMenu.AddMenuItem(rHandWeapon);
                dualWieldMenu.AddMenuItem(lHandWeapon);
                dualWieldMenu.AddMenuItem(equip);

                dualWieldMenu.OnItemSelect += (m, item, index) =>
                {
                    if (item == equip)
                    {
                        uint hash1 = (uint)GetHashKey(rHandWeapon.GetCurrentSelection());
                        uint hash2 = (uint)GetHashKey(lHandWeapon.GetCurrentSelection());
                        Function.Call((Hash)0x5E3BDDBCB83F3D84, PlayerPedId(), hash1, 500, 1, 1, 2, 0, 0, 0.5, 1.0, 752097756, 0, 0, 0, false);
                        Function.Call((Hash)0x5E3BDDBCB83F3D84, PlayerPedId(), hash2, 500, 1, 1, 3, 0, 0, 0.5, 1.0, 752097756, 1, 0, 0, false);
                        Function.Call((Hash)0xADF692B254977C0C, PlayerPedId(), hash1, false, 0, false, false);
                        Function.Call((Hash)0xADF692B254977C0C, PlayerPedId(), hash2, false, 1, false, false);
                    }
                };
            }

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
                else if (item == getAllWeapons)
                {
                    GiveAllWeaponsInCategory(data.WeaponsData.MeleeHashes);
                    GiveAllWeaponsInCategory(data.WeaponsData.ThrownHashes);

                    List<string> longarms = new List<string>();
                    longarms.AddRange(data.WeaponsData.BowHashes);
                    longarms.AddRange(data.WeaponsData.RepeaterHashes);
                    longarms.AddRange(data.WeaponsData.RifleHashes);
                    longarms.AddRange(data.WeaponsData.SniperHashes);
                    longarms.Add("WEAPON_SHOTGUN_DOUBLEBARREL");
                    longarms.Add("WEAPON_SHOTGUN_PUMP");
                    longarms.Add("WEAPON_SHOTGUN_REPEATING");
                    longarms.Add("WEAPON_SHOTGUN_SEMIAUTO");
                    longarms.Shuffle();

                    GiveWeaponToPed_2(PlayerPedId(), (uint)GetHashKey(longarms[0]), 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);
                    GiveWeaponToPed_2(PlayerPedId(), (uint)GetHashKey(longarms[1]), 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);

                    List<string> sidearms = new List<string>();
                    sidearms.AddRange(data.WeaponsData.PistolHashes);
                    sidearms.AddRange(data.WeaponsData.RevolverHashes);
                    sidearms.Add("WEAPON_SHOTGUN_SAWEDOFF");
                    sidearms.Shuffle();

                    GiveWeaponToPed_2(PlayerPedId(), (uint)GetHashKey(sidearms[0]), 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);

                    GiveAllWeaponsInCategory(data.WeaponsData.ItemHashes);

                    foreach (var name in data.WeaponsData.AmmoHashes)
                    {
                        SetPedAmmoByType(PlayerPedId(), GetHashKey(name), 500);
                    }
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
