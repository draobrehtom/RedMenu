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
    class VehicleMenu
    {
        private static Menu menu = new Menu("Vehicle Menu", "Vehicle related options.");
        private static bool setupDone = false;
        private static int currentVehicle = 0;

        private static int BlipAddForEntity(int blipHash, int entity)
        {
            return Function.Call<int>((Hash)0x23F74C2FDA6E7C61, blipHash, entity);
        }

        private static void AddVehicleSubmenu(Menu menu, List<string> hashes, string name, string description)
        {
            Menu submenu = new Menu(name, description);
            MenuItem submenuBtn = new MenuItem(name, description) { RightIcon = MenuItem.Icon.ARROW_RIGHT };
            menu.AddMenuItem(submenuBtn);
            MenuController.AddSubmenu(menu, submenu);
            MenuController.BindMenuItem(menu, submenu, submenuBtn);

            foreach (var hash in hashes)
            {
                submenu.AddMenuItem(new MenuItem(hash));
            }

            submenu.OnItemSelect += async (m, item, index) =>
            {
                if (currentVehicle != 0)
                {
                    DeleteVehicle(ref currentVehicle);
                    currentVehicle = 0;
                }

                uint model = (uint)GetHashKey(hashes[index]);

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
                        await BaseScript.Delay(0);
                    }

                    currentVehicle = CreateVehicle(model, x2, y2, coords.Z, h, true, true, false, true);
                    SetModelAsNoLongerNeeded(model);
                    SetVehicleOnGroundProperly(currentVehicle, 0);
                    SetEntityVisible(currentVehicle, true);
                    BlipAddForEntity(631964804, currentVehicle);

                    if (UserDefaults.VehicleSpawnInside)
                    {
                        TaskWarpPedIntoVehicle(ped, currentVehicle, -1);
                    }

                    // If this isn't done, the hot air balloon won't move with the wind for some reason
                    int c = currentVehicle;
                    SetVehicleAsNoLongerNeeded(ref currentVehicle);
                    currentVehicle = c;
                }
                else
                {
                    Debug.WriteLine($"^1[ERROR] This vehicle model is not present in the game files {model}.^7");
                }
            };
        }

        public static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuCheckboxItem spawnInside = new MenuCheckboxItem("Spawn Inside Vehicle", "Automatically spawn inside vehicles.", UserDefaults.VehicleSpawnInside);
            MenuItem deleteVehicle = new MenuItem("Delete Vehicle", "Delete the vehicle you are currently in.");

            if (PermissionsManager.IsAllowed(Permission.VMSpawn))
            {
                Menu spawnVehicleMenu = new Menu("Spawn Vehicle", "Spawn a vehicle.");
                MenuItem spawnVehicle = new MenuItem("Spawn Vehicle", "Spawn a vehicle.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(spawnVehicle);
                MenuController.AddSubmenu(menu, spawnVehicleMenu);
                MenuController.BindMenuItem(menu, spawnVehicleMenu, spawnVehicle);

                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.BuggyHashes, "Buggies", "Spawn a buggy.");
                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.BoatHashes, "Boats", "Spawn a boat.");
                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.CartHashes, "Carts", "Spawn a cart.");
                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.CoachHashes, "Coaches", "Spawn a coach.");
                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.WagonHashes, "Wagons", "Spawn a wagon.");
                AddVehicleSubmenu(spawnVehicleMenu, data.VehicleData.MiscHashes, "Misc", "Spawn a miscellaneous vehicle.");
            }

            if (PermissionsManager.IsAllowed(Permission.VMSpawnInside))
            {
                menu.AddMenuItem(spawnInside);
            }

            if (PermissionsManager.IsAllowed(Permission.VMDelete))
            {
                menu.AddMenuItem(deleteVehicle);
            }

            menu.OnItemSelect += (m, item, index) =>
            {
                if (item == deleteVehicle)
                {
                    int veh = GetVehiclePedIsIn(PlayerPedId(), true);

                    if (veh != 0)
                    {
                        DeleteVehicle(ref veh);
                    }
                }
            };

            menu.OnCheckboxChange += (m, item, index, _checked) =>
            {
                if (item == spawnInside)
                {
                    UserDefaults.VehicleSpawnInside = _checked;
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
