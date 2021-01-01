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
    class VoiceMenu
    {
        private static Menu menu = new Menu("Voice Menu", "Voice related options");
        private static bool setupDone = false;

        private static void SetVoiceRange(float range)
        {
            Function.Call((Hash)0x08797A8C03868CB8, range);
            Function.Call((Hash)0xEC8703E4536A9952);
            Function.Call((Hash)0x58125B691F6827D5, range);
        }

        public static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            List<float> proximityRange = new List<float>()
            {
                5f,
                10f,
                15f,
                20f,
                100f,
                300f,
                1000f,
                2000f,
                0f
            };

            List<string> proximity = new List<string>()
            {
                "5 m",
                "10 m",
                "15 m",
                "20 m",
                "100 m",
                "300 m",
                "1 km",
                "2 km",
                "Global"
            };

            MenuListItem range = new MenuListItem("Range", proximity, proximityRange.IndexOf(UserDefaults.VoiceRange), "Set the range of voice chat.");

            if (PermissionsManager.IsAllowed(Permission.VORange))
            {
                menu.AddMenuItem(range);
                SetVoiceRange(UserDefaults.VoiceRange);
            }

            menu.OnListIndexChange += (m, listItem, oldIndex, newIndex, itemIndex) =>
            {
                if (listItem == range)
                {
                    float r = proximityRange[range.ListIndex];
                    SetVoiceRange(r);
                    UserDefaults.VoiceRange = r;
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
