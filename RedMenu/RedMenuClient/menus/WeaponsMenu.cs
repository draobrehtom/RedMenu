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

        private static void RemoveAmmoFromPedByType(int ped, int ammoHash, int amount, uint reason)
        {
            Function.Call((Hash)0xB6CFEC32E3742779, ped, ammoHash, amount, reason);
        }

        private static void GiveWeaponComponentToEntity(int entity, int componentHash, uint weaponHash, bool p3)
        {
            Function.Call((Hash)0x74C9090FDD1BB48E, entity, componentHash, weaponHash, p3);
        }

        private static void RemoveWeaponComponentFromPed(int ped, int componentHash, uint weaponHash)
        {
            Function.Call((Hash)0x19F70C4D80494FF8, ped, componentHash, weaponHash);
        }
        
        private static void ClearWeaponComponentCategory(int ped, uint weaponHash, List<data.WeaponComponent> category)
        {
            foreach (var component in category)
            {
                RemoveWeaponComponentFromPed(ped, component.Hash, weaponHash);
            }
        }

        private static void CreateWeaponComponentCategoryListItem(Menu menu, string name, List<data.WeaponComponent> category)
        {
            List<string> items = new List<string>();
            foreach (var component in category)
            {
                items.Add(component.Label);
            }
            MenuListItem listItem = new MenuListItem(name, items, 0);
            listItem.ItemData = category;
            menu.AddMenuItem(listItem);
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuItem cleanWeapon = new MenuItem("Clean Weapon", "Clean the currently selected weapon.");
            MenuItem dirtyWeapon = new MenuItem("Dirty Weapon", "Dirty the currently selected weapon.");
            MenuItem dropWeaponBtn = new MenuItem("Drop Weapon", "Remove the currently selected weapon from your inventory.");
            MenuItem dropAllWeaponsBtn = new MenuItem("Drop All Weapons", "Removes all weapons from your inventory.");
            MenuItem getAllWeapons = new MenuItem("Get All Weapons", "Add all the weapons you can carry to your inventory.");

            if (PermissionsManager.IsAllowed(Permission.WMCleanWeapon))
            {
                menu.AddMenuItem(cleanWeapon);
            }

            if (PermissionsManager.IsAllowed(Permission.WMDirtyWeapon))
            {
                menu.AddMenuItem(dirtyWeapon);
            }

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

            MenuItem removeAmmo = new MenuItem("Remove Ammo", "Remove all ammo for the currently selected weapon.");
            MenuItem removeAllAmmo = new MenuItem("Remove All Ammo", "Remove all ammo for all weapons.");

            MenuCheckboxItem infiniteAmmo = new MenuCheckboxItem("Infinite Ammo", "Never run out of ammo.", UserDefaults.WeaponInfiniteAmmo);

            if (PermissionsManager.IsAllowed(Permission.WMRefillAmmo))
            {
                ammoMenu.AddMenuItem(refillAmmo);
                ammoMenu.AddMenuItem(refillAllAmmo);
            }

            if (PermissionsManager.IsAllowed(Permission.WMRemoveAmmo))
            {
                ammoMenu.AddMenuItem(removeAmmo);
                ammoMenu.AddMenuItem(removeAllAmmo);
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
                else if (item == removeAmmo)
                {
                    int ped = PlayerPedId();
                    uint weapon = 0;
                    GetCurrentPedWeapon(ped, ref weapon, true, 0, true);
                    int ammoType = GetPedAmmoTypeFromWeapon(ped, weapon);
                    int amount = GetPedAmmoByType(ped, ammoType);
                    RemoveAmmoFromPedByType(ped, ammoType, amount, 0x2188E0A3);

                }
                else if (item == removeAllAmmo)
                {
                    foreach (var name in data.WeaponsData.AmmoHashes)
                    {
                        int ped = PlayerPedId();
                        int ammoType = GetHashKey(name);
                        int amount = GetPedAmmoByType(ped, ammoType);
                        RemoveAmmoFromPedByType(ped, ammoType, amount, 0x2188E0A3);
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

            Menu ammoTypesMenu = new Menu("Ammo Types", "Get ammo by type");
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

            if (PermissionsManager.IsAllowed(Permission.WMCustomize))
            {
                Menu customizationMenu = new Menu("Customization", "Customize weapon components");
                MenuItem customization = new MenuItem("Customize Weapon", "Customize weapon components for your current weapon.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(customization);
                MenuController.AddSubmenu(menu, customizationMenu);
                MenuController.BindMenuItem(menu, customizationMenu, customization);

                CreateWeaponComponentCategoryListItem(customizationMenu, "Variants", data.WeaponsData.Variants);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Barrel Colour", data.WeaponsData.LongarmBarrelColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Barrel Decal", data.WeaponsData.LongarmBarrelDecals);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Barrel Decal Colour", data.WeaponsData.LongarmBarrelDecalColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Frame Colour", data.WeaponsData.LongarmFrameColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Frame Decal", data.WeaponsData.LongarmFrameDecals);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Frame Decal Colour", data.WeaponsData.LongarmFrameDecalColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Longarm Hammer Colour", data.WeaponsData.LongarmHammerColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Lever Colour", data.WeaponsData.LeverColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Trigger Colour", data.WeaponsData.TriggerColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Barrel Colour", data.WeaponsData.SidearmBarrelColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Barrel Decal", data.WeaponsData.SidearmBarrelDecals);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Barrel Decal Colour", data.WeaponsData.SidearmBarrelDecalColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Frame Colour", data.WeaponsData.SidearmFrameColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Frame Decal", data.WeaponsData.SidearmFrameDecals);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Frame Decal Colour", data.WeaponsData.SidearmFrameDecalColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sidearm Hammer Colour", data.WeaponsData.SidearmHammerColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Carbine)", data.WeaponsData.CarbineSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Evans)", data.WeaponsData.EvansSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Litchfield)", data.WeaponsData.LitchfieldSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Lancaster)", data.WeaponsData.LancasterSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Bolt-action)", data.WeaponsData.BoltActionSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Springfield)", data.WeaponsData.SpringfieldSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Carcano)", data.WeaponsData.CarcanoSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Rolling Block)", data.WeaponsData.RollingBlockSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Double-barrel Shotgun)", data.WeaponsData.DoubleBarrelShotgunSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Repeating Shotgun)", data.WeaponsData.RepeatingShotgunSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Pump Shotgun)", data.WeaponsData.PumpShotgunSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Semi-auto Shotgun)", data.WeaponsData.SemiAutoShotgunSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Sawed-off Shotgun)", data.WeaponsData.SawedOffShotgunSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (M1899)", data.WeaponsData.M1899Sights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Mauser)", data.WeaponsData.MauserSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Semi-auto)", data.WeaponsData.SemiAutoSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Volcanic)", data.WeaponsData.VolcanicSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Cattleman)", data.WeaponsData.CattlemanSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Double-action)", data.WeaponsData.DoubleActionSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (LeMat)", data.WeaponsData.LematSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Sights (Schofield)", data.WeaponsData.SchofieldSights);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Scope", data.WeaponsData.Scopes);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Scope/Sight Colour", data.WeaponsData.ScopeSightColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Carbine)", data.WeaponsData.CarbineWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Evans)", data.WeaponsData.EvansWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Litchfield)", data.WeaponsData.LitchfieldWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Lancaster)", data.WeaponsData.LancasterWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Bolt-action)", data.WeaponsData.BoltActionWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Springfield)", data.WeaponsData.SpringfieldWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Varmint)", data.WeaponsData.VarmintWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Carcano)", data.WeaponsData.CarcanoWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Rolling Block)", data.WeaponsData.RollingBlockWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Double-barrel Shotgun)", data.WeaponsData.DoubleBarrelShotgunWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Repeating Shotgun)", data.WeaponsData.RepeatingShotgunWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Pump Shotgun)", data.WeaponsData.PumpShotgunWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Semi-auto Shotgun)", data.WeaponsData.SemiAutoShotgunWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap (Sawed-off Shotgun)", data.WeaponsData.SawedOffShotgunWraps);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Wrap Colour", data.WeaponsData.WrapColours);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Carbine)", data.WeaponsData.CarbineStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Evans)", data.WeaponsData.EvansStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Litchfield)", data.WeaponsData.LitchfieldStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Lancaster)", data.WeaponsData.LancasterStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Bolt-action)", data.WeaponsData.BoltActionStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Springfield)", data.WeaponsData.SpringfieldStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Carcano)", data.WeaponsData.CarcanoStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Rolling Block)", data.WeaponsData.RollingBlockStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Double-barrel Shotgun)", data.WeaponsData.DoubleBarrelShotgunStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Repeating Shotgun)", data.WeaponsData.RepeatingShotgunStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Pump Shotgun)", data.WeaponsData.PumpShotgunStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Semi-auto Shotgun)", data.WeaponsData.SemiAutoShotgunStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Stock (Sawed-off Shotgun)", data.WeaponsData.SawedOffShotgunStocks);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (M1899)", data.WeaponsData.M1899Grips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Mauser)", data.WeaponsData.MauserGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Semi Auto)", data.WeaponsData.SemiAutoGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Volcanic)", data.WeaponsData.VolcanicGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Cattleman)", data.WeaponsData.CattlemanGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Double-action)", data.WeaponsData.DoubleActionGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (LeMat)", data.WeaponsData.LematGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip (Schofield)", data.WeaponsData.SchofieldGrips);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Grip Carving", data.WeaponsData.GripCarvings);
                CreateWeaponComponentCategoryListItem(customizationMenu, "Rifling", data.WeaponsData.Rifling);

                customizationMenu.OnListIndexChange += (m, item, oldIndex, newIndex, listIndex) =>
                {
                    int ped = PlayerPedId();
                    uint wep = 0;

                    GetCurrentPedWeapon(ped, ref wep, true, 0, true);

                    ClearWeaponComponentCategory(ped, wep, item.ItemData);
                    GiveWeaponComponentToEntity(ped, item.ItemData[newIndex].Hash, wep, true);
                };

                customizationMenu.OnListItemSelect += (m, item, selectedIndex, listIndex) =>
                {
                    int ped = PlayerPedId();
                    uint wep = 0;

                    GetCurrentPedWeapon(ped, ref wep, true, 0, true);
                    
                    ClearWeaponComponentCategory(ped, wep, item.ItemData);
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
                else if (item == cleanWeapon)
                {
                    int wep = GetCurrentPedWeaponEntityIndex(PlayerPedId(), 0);
                    Function.Call((Hash)0xA7A57E89E965D839, wep, 0.0);
                    Function.Call((Hash)0xA9EF4AD10BDDDB57, wep, 0.0);
                    Function.Call((Hash)0x812CE61DEBCAB948, wep, 0.0);
                    Function.Call((Hash)0xE22060121602493B, wep, 0.0);
                }
                else if (item == dirtyWeapon)
                {
                    int wep = GetCurrentPedWeaponEntityIndex(PlayerPedId(), 0);
                    Function.Call((Hash)0xA7A57E89E965D839, wep, 1.0);
                    Function.Call((Hash)0xA9EF4AD10BDDDB57, wep, 1.0);
                    Function.Call((Hash)0x812CE61DEBCAB948, wep, 1.0);
                    Function.Call((Hash)0xE22060121602493B, wep, 1.0);
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
