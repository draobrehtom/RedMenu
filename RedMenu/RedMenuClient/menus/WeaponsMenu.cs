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

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuItem dropWeaponBtn = new MenuItem("Drop Weapon", "Remove the currently selected weapon from your inventory.");

            if (PermissionsManager.IsAllowed(Permission.WMDropWeapon))
            {
                menu.AddMenuItem(dropWeaponBtn);
            }

            MenuItem allWeapons = new MenuItem("All Weapons", "A list of all weapons.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            MenuController.AddSubmenu(menu, allWeaponsMenu);
            menu.AddMenuItem(allWeapons);
            MenuController.BindMenuItem(menu, allWeaponsMenu, allWeapons);

            foreach (var name in data.WeaponsData.WeaponHashes)
            {
                MenuItem item = new MenuItem(name);
                allWeaponsMenu.AddMenuItem(item);
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
            };

            allWeaponsMenu.OnItemSelect += (m, item, index) =>
            {
                uint model = (uint)GetHashKey(data.WeaponsData.WeaponHashes[index]);
                GiveWeaponToPed_2(PlayerPedId(), model, 500, true, false, 0, false, 0.5f, 1.0f, 0, false, 0f, false);
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
