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

    class SavedWeapon
    {
        public string Name { get;  }
        public List<data.WeaponComponent> Components { get; }

        public List<data.WeaponComponent> Colours { get; }

        public SavedWeapon(string name, List<data.WeaponComponent> components, List<data.WeaponComponent> colours)
        {
            Name = name;
            Components = components;
            Colours = colours;
        }
    }

    class WeaponsMenu
    {
        private static Menu menu = new Menu("Weapons Menu", $"Weapon & Ammo Options");
        private static bool setupDone = false;
        private static Menu allWeaponsMenu = new Menu("All Weapons", "A list of all weapons");

        private const int maxSavedLoadouts = 50;

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

        private static List<data.WeaponComponent> GetKnownComponents()
        {
            var knownComponents = new List<data.WeaponComponent>();
            knownComponents.AddRange(data.WeaponsData.Variants);
            knownComponents.AddRange(data.WeaponsData.LongarmBarrelDecals);
            knownComponents.AddRange(data.WeaponsData.SidearmBarrelDecals);
            knownComponents.AddRange(data.WeaponsData.LongarmFrameDecals);
            knownComponents.AddRange(data.WeaponsData.SidearmFrameDecals);
            knownComponents.AddRange(data.WeaponsData.CarbineSights);
            knownComponents.AddRange(data.WeaponsData.EvansSights);
            knownComponents.AddRange(data.WeaponsData.LitchfieldSights);
            knownComponents.AddRange(data.WeaponsData.LancasterSights);
            knownComponents.AddRange(data.WeaponsData.BoltActionSights);
            knownComponents.AddRange(data.WeaponsData.SpringfieldSights);
            knownComponents.AddRange(data.WeaponsData.CarcanoSights);
            knownComponents.AddRange(data.WeaponsData.RollingBlockSights);
            knownComponents.AddRange(data.WeaponsData.DoubleBarrelShotgunSights);
            knownComponents.AddRange(data.WeaponsData.RepeatingShotgunSights);
            knownComponents.AddRange(data.WeaponsData.PumpShotgunSights);
            knownComponents.AddRange(data.WeaponsData.SemiAutoShotgunSights);
            knownComponents.AddRange(data.WeaponsData.SawedOffShotgunSights);
            knownComponents.AddRange(data.WeaponsData.M1899Sights);
            knownComponents.AddRange(data.WeaponsData.MauserSights);
            knownComponents.AddRange(data.WeaponsData.SemiAutoSights);
            knownComponents.AddRange(data.WeaponsData.VolcanicSights);
            knownComponents.AddRange(data.WeaponsData.CattlemanSights);
            knownComponents.AddRange(data.WeaponsData.DoubleActionSights);
            knownComponents.AddRange(data.WeaponsData.LematSights);
            knownComponents.AddRange(data.WeaponsData.SchofieldSights);
            knownComponents.AddRange(data.WeaponsData.Scopes);
            knownComponents.AddRange(data.WeaponsData.CarbineWraps);
            knownComponents.AddRange(data.WeaponsData.EvansWraps);
            knownComponents.AddRange(data.WeaponsData.LitchfieldWraps);
            knownComponents.AddRange(data.WeaponsData.LancasterWraps);
            knownComponents.AddRange(data.WeaponsData.BoltActionWraps);
            knownComponents.AddRange(data.WeaponsData.SpringfieldWraps);
            knownComponents.AddRange(data.WeaponsData.VarmintWraps);
            knownComponents.AddRange(data.WeaponsData.CarcanoWraps);
            knownComponents.AddRange(data.WeaponsData.RollingBlockWraps);
            knownComponents.AddRange(data.WeaponsData.DoubleBarrelShotgunWraps);
            knownComponents.AddRange(data.WeaponsData.RepeatingShotgunWraps);
            knownComponents.AddRange(data.WeaponsData.PumpShotgunWraps);
            knownComponents.AddRange(data.WeaponsData.SemiAutoShotgunWraps);
            knownComponents.AddRange(data.WeaponsData.SawedOffShotgunWraps);
            knownComponents.AddRange(data.WeaponsData.CarbineStocks);
            knownComponents.AddRange(data.WeaponsData.EvansStocks);
            knownComponents.AddRange(data.WeaponsData.LitchfieldStocks);
            knownComponents.AddRange(data.WeaponsData.LancasterStocks);
            knownComponents.AddRange(data.WeaponsData.BoltActionStocks);
            knownComponents.AddRange(data.WeaponsData.SpringfieldStocks);
            knownComponents.AddRange(data.WeaponsData.CarcanoStocks);
            knownComponents.AddRange(data.WeaponsData.RollingBlockStocks);
            knownComponents.AddRange(data.WeaponsData.DoubleBarrelShotgunStocks);
            knownComponents.AddRange(data.WeaponsData.RepeatingShotgunStocks);
            knownComponents.AddRange(data.WeaponsData.PumpShotgunStocks);
            knownComponents.AddRange(data.WeaponsData.SemiAutoShotgunStocks);
            knownComponents.AddRange(data.WeaponsData.SawedOffShotgunStocks);
            knownComponents.AddRange(data.WeaponsData.GripCarvings);
            knownComponents.AddRange(data.WeaponsData.Rifling);
            knownComponents.AddRange(data.WeaponsData.M1899Grips);
            knownComponents.AddRange(data.WeaponsData.MauserGrips);
            knownComponents.AddRange(data.WeaponsData.SemiAutoGrips);
            knownComponents.AddRange(data.WeaponsData.VolcanicGrips);
            knownComponents.AddRange(data.WeaponsData.CattlemanGrips);
            knownComponents.AddRange(data.WeaponsData.DoubleActionGrips);
            knownComponents.AddRange(data.WeaponsData.LematGrips);
            knownComponents.AddRange(data.WeaponsData.SchofieldGrips);
            return knownComponents;
        }

        private static List<data.WeaponComponent> GetKnownComponentColours()
        {
            var knownColours = new List<data.WeaponComponent>();
            knownColours.AddRange(data.WeaponsData.LongarmBarrelColours);
            knownColours.AddRange(data.WeaponsData.SidearmBarrelColours);
            knownColours.AddRange(data.WeaponsData.LongarmBarrelDecalColours);
            knownColours.AddRange(data.WeaponsData.SidearmBarrelDecalColours);
            knownColours.AddRange(data.WeaponsData.LongarmFrameColours);
            knownColours.AddRange(data.WeaponsData.SidearmFrameColours);
            knownColours.AddRange(data.WeaponsData.LongarmFrameDecalColours);
            knownColours.AddRange(data.WeaponsData.SidearmFrameDecalColours);
            knownColours.AddRange(data.WeaponsData.ScopeSightColours);
            knownColours.AddRange(data.WeaponsData.WrapColours);
            knownColours.AddRange(data.WeaponsData.LeverColours);
            knownColours.AddRange(data.WeaponsData.LongarmHammerColours);
            knownColours.AddRange(data.WeaponsData.SidearmHammerColours);
            knownColours.AddRange(data.WeaponsData.TriggerColours);
            return knownColours;
        }

        public static async void LoadSavedLoadout(int loadoutIndex)
        {
            int ped = PlayerPedId();

            RemoveAllPedWeapons(ped, true, true);

            if (!StorageManager.TryGet("SavedLoadouts_" + loadoutIndex + "_weapons", out string json))
            {
                json = "[]";
            }

            List<SavedWeapon> weapons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SavedWeapon>>(json);

            foreach (var weapon in weapons)
            {
                uint weaponHash = (uint)GetHashKey(weapon.Name);

                GiveWeaponToPed_2(ped, weaponHash, 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);

                var knownComponents = GetKnownComponents();
                var knownColours = GetKnownComponentColours();

                foreach (var component in knownComponents)
                {
                    RemoveWeaponComponentFromPed(ped, component.Hash, weaponHash);
                }

                foreach (var component in weapon.Components)
                {
                    GiveWeaponComponentToEntity(ped, component.Hash, weaponHash, true);
                }

                foreach (var component in knownColours)
                {
                    RemoveWeaponComponentFromPed(ped, component.Hash, weaponHash);
                }

                foreach (var component in weapon.Colours)
                {
                    GiveWeaponComponentToEntity(ped, component.Hash, weaponHash, true);
                }
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

        private static bool HasPedGotWeaponComponent(int ped, int componentHash, int weaponHash)
        {
            return Function.Call<bool>((Hash)0xBBC67A6F965C688A, ped, componentHash, weaponHash);
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

            if (PermissionsManager.IsAllowed(Permission.WMSavedLoadouts))
            {
                Menu savedLoadoutsMenu = new Menu("Saved Loadouts", "Save and load weapon loadouts");
                MenuItem savedLoadouts = new MenuItem("Saved Loadouts", "Save and load weapon loadouts.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(savedLoadouts);
                MenuController.AddSubmenu(menu, savedLoadoutsMenu);
                MenuController.BindMenuItem(menu, savedLoadoutsMenu, savedLoadouts);
                List<MenuItem> savedLoadoutSlots = new List<MenuItem>();
                List<MenuCheckboxItem> defaultSavedLoadoutCheckboxes = new List<MenuCheckboxItem>();

                for (int i = 1; i <= maxSavedLoadouts; ++i)
                {
                    int loadoutIndex = i;

                    if (!StorageManager.TryGet("SavedLoadouts_" + loadoutIndex + "_name", out string loadoutName))
                    {
                        loadoutName = "Loadout " + loadoutIndex;
                    }

                    MenuItem savedLoadout = new MenuItem(loadoutName) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                    if (loadoutIndex == UserDefaults.WeaponDefaultSavedLoadout)
                    {
                        savedLoadout.LeftIcon = MenuItem.Icon.STAR;
                    }
                    savedLoadoutsMenu.AddMenuItem(savedLoadout);
                    savedLoadoutSlots.Add(savedLoadout);

                    Menu savedLoadoutOptionsMenu = new Menu(loadoutName);
                    MenuController.AddSubmenu(savedLoadoutsMenu, savedLoadoutOptionsMenu);
                    MenuController.BindMenuItem(savedLoadoutsMenu, savedLoadoutOptionsMenu, savedLoadout);

                    MenuItem load = new MenuItem("Load", "Load this loadout.");
                    MenuItem save = new MenuItem("Save", "Save current loadout to this slot.");
                    MenuCheckboxItem isDefault = new MenuCheckboxItem("Default", "Load this loadout automatically when you respawn.", loadoutIndex == UserDefaults.WeaponDefaultSavedLoadout);
                    defaultSavedLoadoutCheckboxes.Add(isDefault);
                    savedLoadoutOptionsMenu.AddMenuItem(load);
                    savedLoadoutOptionsMenu.AddMenuItem(save);
                    savedLoadoutOptionsMenu.AddMenuItem(isDefault);

                    savedLoadoutOptionsMenu.OnItemSelect += async (m, item, index) =>
                    {
                        if (item == load)
                        {
                            LoadSavedLoadout(loadoutIndex);
                        }
                        else if (item == save)
                        {
                            string newName = await GetUserInput("Enter loadout name", loadoutName, 20);

                            if (newName != null)
                            {
                                int ped = PlayerPedId();

                                List<string> knownWeapons = new List<string>();
                                knownWeapons.AddRange(data.WeaponsData.ItemHashes);
                                knownWeapons.AddRange(data.WeaponsData.MeleeHashes);
                                knownWeapons.AddRange(data.WeaponsData.RevolverHashes);
                                knownWeapons.AddRange(data.WeaponsData.PistolHashes);
                                knownWeapons.AddRange(data.WeaponsData.SniperHashes);
                                knownWeapons.AddRange(data.WeaponsData.RifleHashes);
                                knownWeapons.AddRange(data.WeaponsData.RepeaterHashes);
                                knownWeapons.AddRange(data.WeaponsData.ThrownHashes);
                                knownWeapons.AddRange(data.WeaponsData.ShotgunHashes);
                                knownWeapons.AddRange(data.WeaponsData.BowHashes);

                                var knownComponents = GetKnownComponents();
                                var knownColours = GetKnownComponentColours();

                                List <SavedWeapon> weapons = new List<SavedWeapon>();

                                foreach (var weaponName in knownWeapons)
                                {
                                    int weaponHash = GetHashKey(weaponName);

                                    if (HasPedGotWeapon(ped, weaponHash, 0, 0))
                                    {
                                        SetCurrentPedWeapon(ped, (uint)weaponHash, true, 0, false, false);

                                        int weapon = GetCurrentPedWeaponEntityIndex(ped, 0);

                                        var components = new List<data.WeaponComponent>();
                                        var colours = new List<data.WeaponComponent>();

                                        foreach (var component in knownComponents)
                                        {
                                            if (HasPedGotWeaponComponent(ped, component.Hash, weaponHash))
                                            {
                                                components.Add(component);
                                            }
                                        }

                                        foreach (var component in knownColours)
                                        {
                                            if (HasWeaponGotWeaponComponent(weapon, (uint)component.Hash))
                                            {
                                                colours.Add(component);
                                            }
                                        }

                                        weapons.Add(new SavedWeapon(weaponName, components, colours));
                                    }
                                }

                                string json = Newtonsoft.Json.JsonConvert.SerializeObject(weapons);
                                StorageManager.Save("SavedLoadouts_" + loadoutIndex + "_weapons", json, true);

                                StorageManager.Save("SavedLoadouts_" + loadoutIndex + "_name", newName, true);
                                savedLoadout.Text = newName;
                                savedLoadoutOptionsMenu.MenuTitle = newName;
                                loadoutName = newName;
                            }
                        }
                    };

                    savedLoadoutOptionsMenu.OnCheckboxChange += (m, item, index, _checked) =>
                    {
                        if (item == isDefault)
                        {
                            if (_checked)
                            {
                                UserDefaults.WeaponDefaultSavedLoadout = loadoutIndex;

                                foreach (var cb in defaultSavedLoadoutCheckboxes)
                                {
                                    if (cb != item)
                                    {
                                        cb.Checked = false;
                                    }
                                }
                            }
                            else
                            {
                                UserDefaults.WeaponDefaultSavedLoadout = 0;
                            }

                            for (int slot = 0; slot < savedLoadoutSlots.Count; ++slot)
                            {
                                if (_checked && slot + 1 == loadoutIndex)
                                {
                                    savedLoadoutSlots[slot].LeftIcon = MenuItem.Icon.STAR;
                                }
                                else
                                {
                                    savedLoadoutSlots[slot].LeftIcon = MenuItem.Icon.NONE;
                                }
                            }
                        }
                    };
                }
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
