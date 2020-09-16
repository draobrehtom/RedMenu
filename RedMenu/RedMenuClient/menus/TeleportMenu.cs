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
    class TeleportMenu
    {
        private static Menu menu = new Menu("Player Menu", "Player Related Options");
        private static Menu locationsMenu = new Menu("Locations", "A list of locations to teleport to.");
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            MenuItem teleportToWaypoint = new MenuItem("Teleport to Waypoint", "Teleport to the currently set waypoint.");

            if (PermissionsManager.IsAllowed(Permission.TMTeleportToWaypoint))
            {
                menu.AddMenuItem(teleportToWaypoint);
            }

            if (PermissionsManager.IsAllowed(Permission.TMLocations))
            {
                MenuItem locations = new MenuItem("Locations", "A list of locations to teleport to.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                MenuController.AddSubmenu(menu, locationsMenu);
                menu.AddMenuItem(locations);
                MenuController.BindMenuItem(menu, locationsMenu, locations);

                foreach (var location in data.TeleportData.TeleportLocations)
                {
                    MenuItem item = new MenuItem(location.Name);
                    locationsMenu.AddMenuItem(item);
                }

                locationsMenu.OnItemSelect += (m, item, index) =>
                {
                    TeleportLocation loc = data.TeleportData.TeleportLocations[index];
                    int ped = PlayerPedId();
                    FreezeEntityPosition(ped, true);
                    SetEntityCoords(ped, loc.X, loc.Y, loc.Z, false, false, false, false);
                    SetEntityHeading(ped, loc.H);
                    FreezeEntityPosition(ped, false);
                };
            }

            menu.OnItemSelect += (m, item, index) =>
            {
                if (item == teleportToWaypoint)
                {
                    if (IsWaypointActive())
                    {
                        int ped = PlayerPedId();
                        FreezeEntityPosition(ped, true);
                        Vector3 waypoint = GetWaypointCoords();
                        SetEntityCoords(ped, waypoint.X, waypoint.Y, 1000.0f, false, false, false, false);
                        Vector3 coords = GetEntityCoords(ped, false, false);
                        float groundz = GetHeightmapBottomZForPosition(coords.X, coords.Y);
                        SetEntityInvincible(ped, true);
                        SetEntityCoords(ped, coords.X, coords.Y, groundz + 10.0f, false, false, false, false);
                        FreezeEntityPosition(ped, false);
                        Wait(3000);
                        SetEntityInvincible(ped, false);
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
