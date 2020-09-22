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
    class OnlinePlayersMenu
    {
        private static Menu menu = new Menu("Online Players", "Players in the server");
        private static bool setupDone = false;

        private static int GetTeleportTarget()
        {
            int ped = PlayerPedId();
            int veh = GetVehiclePedIsIn(ped, false);
            int mnt = GetMount(ped);

            if (veh != 0)
            {
                return veh;
            }
            else if (mnt != 0)
            {
                return mnt;
            }
            else
            {
                return ped;
            }
        }

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            var players = API.GetActivePlayers();

            foreach (var player in players)
            {
                int id = GetPlayerServerId(player);
                string name = GetPlayerName(player);
                MenuItem item = new MenuItem(id.ToString() + ": " + name);
                menu.AddMenuItem(item);
            }

            menu.OnItemSelect += (m, item, index) =>
            {
                int ped = GetTeleportTarget();
                int target = GetPlayerPed(players[index]);
                Vector3 coords = GetEntityCoords(target, true, true);
                float h = GetEntityHeading(target);
                FreezeEntityPosition(ped, true);
                SetEntityCoords(ped, coords.X, coords.Y, coords.Z, false, false, false, false);
                SetEntityHeading(ped, h);
                FreezeEntityPosition(ped, false);
            };
        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
