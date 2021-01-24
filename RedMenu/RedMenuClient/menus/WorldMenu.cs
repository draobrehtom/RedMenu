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
    class WorldMenu : BaseScript
    {
        private static Menu menu = new Menu("World Menu", "World related options");
        private static bool setupDone = false;

        private static void NetworkOverrideClockTime(int hour, int minute, int second, int transitionTime, bool freezeTime)
        {
            Function.Call((Hash)0x669E223E64B1903C, hour, minute, second, transitionTime, freezeTime);
        }

        private static void SetWeatherType(int weatherHash, bool p1, bool p2, bool p3, float p4, bool p5)
        {
            Function.Call((Hash)0x59174F1AFE095B5A, weatherHash, p1, p2, p3, p4, p5);
        }

        private static void SetSnowCoverageType(int type)
        {
            Function.Call((Hash)0xF02A9C330BBFC5C7, type);
        }

        public static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;

            if (PermissionsManager.IsAllowed(Permission.WOTime))
            {
                Menu timeOptionsMenu = new Menu("Time Options", "Set the current time");
                MenuItem timeOptions = new MenuItem("Time Options", "Set the current time.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };

                menu.AddMenuItem(timeOptions);
                MenuController.AddSubmenu(menu, timeOptionsMenu);
                MenuController.BindMenuItem(menu, timeOptionsMenu, timeOptions);

                List<string> hourRange = new List<string>();
                for (int i = 0; i <= 23; ++i)
                {
                    hourRange.Add(i.ToString());
                }

                List<string> minuteSecondRange = new List<string>();
                for (int i = 0; i <= 59; ++i)
                {
                    minuteSecondRange.Add(i.ToString());
                }

                MenuListItem hour = new MenuListItem("Hour", hourRange, 0, "The hour to set.");
                MenuListItem minute = new MenuListItem("Minute", minuteSecondRange, 0, "The minute to set.");
                MenuListItem second = new MenuListItem("Second", minuteSecondRange, 0, "The second to set.");
                MenuListItem transition = new MenuListItem("Transition", new List<string>() { "0", "1000", "5000", "10000", "30000" }, 1, "Transition time in milliseconds.");
                MenuCheckboxItem freeze = new MenuCheckboxItem("Freeze", "Whether to freeze time.");
                MenuItem apply = new MenuItem("Apply", "Apply the selected time settings.");

                timeOptionsMenu.AddMenuItem(hour);
                timeOptionsMenu.AddMenuItem(minute);
                timeOptionsMenu.AddMenuItem(second);
                timeOptionsMenu.AddMenuItem(transition);
                timeOptionsMenu.AddMenuItem(freeze);
                timeOptionsMenu.AddMenuItem(apply);

                timeOptionsMenu.OnItemSelect += (menu, item, index) =>
                {
                    if (item == apply)
                    {
                        TriggerEvent("weatherSync:toggleSync", false);
                        NetworkOverrideClockTime(hour.ListIndex, minute.ListIndex, second.ListIndex, Int32.Parse(transition.GetCurrentSelection()), freeze.Checked);
                    }
                };
            }

            if (PermissionsManager.IsAllowed(Permission.WOWeather))
            {
                Menu weatherOptionsMenu = new Menu("Weather Options", "Set the current weather");
                MenuItem weatherOptions = new MenuItem("Weather Options", "Set the current weather.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(weatherOptions);
                MenuController.AddSubmenu(menu, weatherOptionsMenu);
                MenuController.BindMenuItem(menu, weatherOptionsMenu, weatherOptions);

                MenuListItem weatherType = new MenuListItem("Weather Type", data.WorldData.WeatherTypes, 0, "The main type of weather.");
                MenuListItem transition = new MenuListItem("Transition", new List<string>() { "1.0", "5.0", "10.0", "30.0" }, 0, "Transition time in seconds.");
                MenuListItem snowCoverageType = new MenuListItem("Snow Coverage", new List<string>() { "Primary", "Secondary", "Xmas", "XmasSecondary" }, 0, "Type of ground snow coverage.");
                MenuItem apply = new MenuItem("Apply", "Apply the selected weather settings.");

                weatherOptionsMenu.AddMenuItem(weatherType);
                weatherOptionsMenu.AddMenuItem(transition);
                weatherOptionsMenu.AddMenuItem(snowCoverageType);
                weatherOptionsMenu.AddMenuItem(apply);

                weatherOptionsMenu.OnItemSelect += (menu, item, index) =>
                {
                    if (item == apply)
                    {
                        TriggerEvent("weatherSync:toggleSync", false);
                        SetWeatherType(GetHashKey(weatherType.GetCurrentSelection()), true, false, true, float.Parse(transition.GetCurrentSelection()), false);
                        SetSnowCoverageType(snowCoverageType.ListIndex);
                    }
                };
            }

            if (PermissionsManager.IsAllowed(Permission.WOTimecycleModifiers))
            {
                Menu timecycleModifiersMenu = new Menu("Timecycle", "Set/Clear timecycle modifiers");
                MenuItem timeCycleModifiers = new MenuItem("Timecycle Modifiers", "Set/Clear timecycle modifiers.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(timeCycleModifiers);
                MenuController.AddSubmenu(menu, timecycleModifiersMenu);
                MenuController.BindMenuItem(menu, timecycleModifiersMenu, timeCycleModifiers);

                MenuListItem modifier = new MenuListItem("Modifier", data.WorldData.TimecycleModifiers, 0, "Set a timecycle modifier.");
                MenuItem clear = new MenuItem("Clear Timecycle Modifier", "Clear the timecycle modifier.");

                timecycleModifiersMenu.AddMenuItem(modifier);
                timecycleModifiersMenu.AddMenuItem(clear);

                timecycleModifiersMenu.OnListItemSelect += (menu, listItem, selectedIndex, itemIndex) =>
                {
                    if (listItem == modifier)
                    {
                        SetTimecycleModifier(modifier.GetCurrentSelection());
                    }
                };

                timecycleModifiersMenu.OnListIndexChange += (menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
                {
                    if (listItem == modifier)
                    {
                        SetTimecycleModifier(modifier.GetCurrentSelection());
                    }
                };

                timecycleModifiersMenu.OnItemSelect += (menu, item, index) =>
                {
                    if (item == clear)
                    {
                        ClearTimecycleModifier();
                    }
                };
            }

            if (PermissionsManager.IsAllowed(Permission.WOAnimpostfx))
            {
                Menu animpostfxMenu = new Menu("Animpostfx", "Play animated post-effects");
                MenuItem animpostfx = new MenuItem("Animpostfx", "Play animated post-effects.") { RightIcon = MenuItem.Icon.ARROW_RIGHT };
                menu.AddMenuItem(animpostfx);
                MenuController.AddSubmenu(menu, animpostfxMenu);
                MenuController.BindMenuItem(menu, animpostfxMenu, animpostfx);

                MenuListItem effect = new MenuListItem("Effect", data.WorldData.AnimpostfxEffects, 0, "Choose an effect.");
                MenuItem play = new MenuItem("Play", "Start playing the selected effect.");
                MenuItem stop = new MenuItem("Stop", "Stop playing the selected effect.");
                MenuItem stopAll = new MenuItem("Stop All", "Stop playing all effects.");

                animpostfxMenu.AddMenuItem(effect);
                animpostfxMenu.AddMenuItem(play);
                animpostfxMenu.AddMenuItem(stop);
                animpostfxMenu.AddMenuItem(stopAll);

                animpostfxMenu.OnItemSelect += (menu, item, index) =>
                {
                    if (item == play)
                    {
                        AnimpostfxPlay(effect.GetCurrentSelection());
                    }
                    else if (item == stop)
                    {
                        AnimpostfxStop(effect.GetCurrentSelection());
                    }
                    else if (item == stopAll)
                    {
                        AnimpostfxStopAll();
                    }
                };
            }
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return menu;
        }
    }
}
