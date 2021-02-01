using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using RedMenuShared;
using RedMenuClient.util;
using System.Net;
using CitizenFX.Core.Native;

namespace RedMenuClient
{
    class TickFunctions : BaseScript
    {
        public TickFunctions() {
            EventHandlers["playerSpawned"] += new Action(Update);
        }

        private static void Update()
        {
            // Update godmode.
            if (PermissionsManager.IsAllowed(Permission.PMGodMode) && UserDefaults.PlayerGodMode)
            {
                SetEntityInvincible(PlayerPedId(), true);
            }
            else
            {
                SetEntityInvincible(PlayerPedId(), false);
            }

            if (PermissionsManager.IsAllowed(Permission.PMEveryoneIgnore) && UserDefaults.PlayerEveryoneIgnore)
            {
                SetEveryoneIgnorePlayer(PlayerId(), true);
            }
            else
            {
                SetEveryoneIgnorePlayer(PlayerId(), false);
            }

            if (PermissionsManager.IsAllowed(Permission.PMDisableRagdoll) && UserDefaults.PlayerDisableRagdoll)
            {
                SetPedCanRagdoll(PlayerPedId(), false);
            }
            else
            {
                SetPedCanRagdoll(PlayerPedId(), true);
            }

            if (PermissionsManager.IsAllowed(Permission.WMInfiniteAmmo) && UserDefaults.WeaponInfiniteAmmo)
            {
                SetPedInfiniteAmmoClip(PlayerPedId(), true);
            }
            else
            {
                SetPedInfiniteAmmoClip(PlayerPedId(), false);
            }

            // This needs more native research for the outer cores.
            //if (ConfigManager.EnableMaxStats)
            //{
            //    SetAttribute(PlayerPedId(), 0, GetMaxAttributePoints(PlayerPedId(), 0));
            //    SetAttributePoints(PlayerPedId(), 1, GetMaxAttributePoints(PlayerPedId(), 1));
            //    SetAttributePoints(PlayerPedId(), 2, GetMaxAttributePoints(PlayerPedId(), 2));
            //}
        }

        private static int lastPed = 0;

        [Tick]
        internal static async Task PedChangeDetectionTick()
        {
            int ped = PlayerPedId();

            if (ped != lastPed)
            {
                Update();
                lastPed = ped;
            }

            await Delay(1000);
            await Task.FromResult(0);
        }

        [Tick]
        internal static async Task InfiniteStatsTick()
        {
            if (!IsPlayerDead(PlayerId())) // Allows respawning after killing yourself
            {
                int ped = PlayerPedId();

                if (PermissionsManager.IsAllowed(Permission.PMGodMode) && UserDefaults.PlayerGodMode)
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 0, 100.0f);
                }

                if (PermissionsManager.IsAllowed(Permission.PMInfiniteStamina) && UserDefaults.PlayerInfiniteStamina)
                {
                    RestorePlayerStamina(PlayerId(), 100.0f);
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, 100.0f);
                }

                if (PermissionsManager.IsAllowed(Permission.PMInfiniteDeadEye) && UserDefaults.PlayerInfiniteDeadEye)
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 2, 100.0f);
                }

                int mount = GetMount(ped);

                if (mount != 0)
                {
                    if (PermissionsManager.IsAllowed(Permission.MMGodMode) && UserDefaults.MountGodMode)
                    {
                        Function.Call((Hash)0xC6258F41D86676E0, mount, 0, 100);
                    }
                    if (PermissionsManager.IsAllowed(Permission.MMInfiniteStamina) && UserDefaults.MountInfiniteStamina)
                    {
                        Function.Call((Hash)0x675680D089BFA21F, mount, 100.0f);
                        Function.Call((Hash)0xC6258F41D86676E0, mount, 1, 100);
                    }
                }
            }

            await Delay(1000);
            await Task.FromResult(0);
        }

        /// <summary>
        /// Manages the radar toggle when holding down the select radar mode button.
        /// Until more radar natives are discovered, this will have to do with only an on/off toggle.
        /// </summary>
        /// <returns></returns>
        [Tick]
        internal static async Task RadarToggleTick()
        {
            if (UserDefaults.MiscMinimapControls)
            {
                if (Util.IsControlPressed(Control.SelectRadarMode))
                {
                    //UiPrompt promptY = new UiPrompt(new Control[1] { Control.ContextY }, "Compass");
                    //UiPrompt promptX = new UiPrompt(new Control[1] { Control.ContextX }, "Expanded");
                    UiPrompt promptA = new UiPrompt(new Control[1] { Control.ContextA }, "Regular");
                    UiPrompt promptB = new UiPrompt(new Control[1] { Control.ContextB }, "Off", "BRT2MountPrompt");
                    //promptY.Prepare();
                    //promptX.Prepare();
                    promptA.Prepare();
                    promptB.Prepare();
                    bool enabled = false;
                    while (Util.IsControlPressed(Control.SelectRadarMode))
                    {
                        if (!enabled)
                        {
                            //promptY.SetEnabled(true, true);
                            //promptX.SetEnabled(true, true);
                            promptA.SetEnabled(true, true);
                            promptB.SetEnabled(true, true);
                            enabled = true;
                        }

                        if (Util.IsControlJustReleased(Control.ContextB))
                        {
                            DisplayRadar(false);
                        }
                        if (Util.IsControlJustReleased(Control.ContextA))
                        {
                            DisplayRadar(true);
                        }
                        //if (Util.IsControlJustReleased(Control.ContextX))
                        //{
                        //    DisplayRadar(true);
                        //}
                        //if (Util.IsControlJustReleased(Control.ContextY))
                        //{
                        //    DisplayRadar(true);
                        //}

                        await Delay(0);
                    }
                    //promptY.Dispose();
                    //promptX.Dispose();
                    promptA.Dispose();
                    promptB.Dispose();
                    while (Util.IsControlPressed(Control.SelectRadarMode))
                    {
                        await Delay(0);
                    }
                }
            }
            else
            {
                await Delay(1000);
            }
            await Task.FromResult(0);
        }
    }
}
