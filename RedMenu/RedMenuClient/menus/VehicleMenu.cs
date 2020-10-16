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

                foreach (var name in data.VehicleData.VehicleHashes)
                {
                    MenuItem item = new MenuItem(name);
                    spawnVehicleMenu.AddMenuItem(item);
                }

                spawnVehicleMenu.OnItemSelect += async (m, item, index) =>
                {
                    if (currentVehicle != 0)
                    {
                        DeleteVehicle(ref currentVehicle);
                        currentVehicle = 0;
                    }

                    uint model = (uint)GetHashKey(data.VehicleData.VehicleHashes[index]);

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
                    }
                    else
                    {
                        Debug.WriteLine($"^1[ERROR] This vehicle model is not present in the game files {model}.^7");
                    }
                };
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
