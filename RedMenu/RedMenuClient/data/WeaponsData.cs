using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMenuClient.data
{
    public static class WeaponsData
    {
        public static List<string> ItemHashes { get; } = new List<string>()
        {
            "WEAPON_FISHINGROD",
            "WEAPON_KIT_BINOCULARS",
            "WEAPON_KIT_CAMERA",
            "WEAPON_LASSO",
            "WEAPON_MELEE_LANTERN",
            "WEAPON_MELEE_DAVY_LANTERN",
            "WEAPON_MELEE_LANTERN_ELECTRIC",
            "WEAPON_MELEE_TORCH"
        };
        public static List<string> MeleeHashes { get; } = new List<string>()
        {
            "WEAPON_MELEE_ANCIENT_HATCHET",
            "WEAPON_MELEE_BROKEN_SWORD",
            "WEAPON_MELEE_CLEAVER",
            "WEAPON_MELEE_HATCHET",
            "WEAPON_MELEE_HATCHET_DOUBLE_BIT",
            "WEAPON_MELEE_HATCHET_DOUBLE_BIT_RUSTED",
            "WEAPON_MELEE_HATCHET_HEWING",
            "WEAPON_MELEE_HATCHET_HUNTER",
            "WEAPON_MELEE_HATCHET_HUNTER_RUSTED",
            "WEAPON_MELEE_HATCHET_VIKING",
            "WEAPON_MELEE_KNIFE",
            "WEAPON_MELEE_KNIFE_BEAR",
            "WEAPON_MELEE_KNIFE_CIVIL_WAR",
            "WEAPON_MELEE_KNIFE_JAWBONE",
            "WEAPON_MELEE_KNIFE_JOHN",
            "WEAPON_MELEE_KNIFE_MINER",
            "WEAPON_MELEE_KNIFE_VAMPIRE",
            "WEAPON_MELEE_MACHETE"
        };

        public static List<string> RevolverHashes { get; } = new List<string>()
        {
            "WEAPON_REVOLVER_CATTLEMAN",
            "WEAPON_REVOLVER_CATTLEMAN_JOHN",
            "WEAPON_REVOLVER_CATTLEMAN_MEXICAN",
            "WEAPON_REVOLVER_CATTLEMAN_PIG",
            "WEAPON_REVOLVER_DOUBLEACTION",
            "WEAPON_REVOLVER_DOUBLEACTION_EXOTIC",
            "WEAPON_REVOLVER_LEMAT",
            "WEAPON_REVOLVER_SCHOFIELD",
            "WEAPON_REVOLVER_SCHOFIELD_CALLOWAY"
        };

        public static List<string> PistolHashes { get; } = new List<string>()
        {
            "WEAPON_PISTOL_M1899",
            "WEAPON_PISTOL_MAUSER",
            "WEAPON_PISTOL_MAUSER_DRUNK",
            "WEAPON_PISTOL_SEMIAUTO",
            "WEAPON_PISTOL_VOLCANIC"
        };
        public static List<string> SniperHashes { get; } = new List<string>()
        {
            "WEAPON_SNIPERRIFLE_CARCANO",
            "WEAPON_SNIPERRIFLE_ROLLINGBLOCK"
        };

        public static List<string> RifleHashes { get; } = new List<string>()
        {
            "WEAPON_RIFLE_BOLTACTION",
            "WEAPON_RIFLE_SPRINGFIELD",
            "WEAPON_RIFLE_VARMINT"
        };

        public static List<string> RepeaterHashes { get; } = new List<string>()
        {
            "WEAPON_REPEATER_CARBINE",
            "WEAPON_REPEATER_EVANS",
            "WEAPON_REPEATER_HENRY",
            "WEAPON_REPEATER_WINCHESTER"
        };

        public static List<string> ThrownHashes { get; } = new List<string>()
        {
            "WEAPON_THROWN_DYNAMITE",
            "WEAPON_THROWN_MOLOTOV",
            "WEAPON_THROWN_THROWING_KNIVES",
            "WEAPON_THROWN_TOMAHAWK",
            "WEAPON_THROWN_TOMAHAWK_ANCIENT"
        };
        public static List<string> ShotgunHashes { get; } = new List<string>()
        {
            "WEAPON_SHOTGUN_DOUBLEBARREL",
            "WEAPON_SHOTGUN_PUMP",
            "WEAPON_SHOTGUN_REPEATING",
            "WEAPON_SHOTGUN_SAWEDOFF",
            "WEAPON_SHOTGUN_SEMIAUTO",
            "WEAPON_SHOTGUN_SEMIAUTO_HOSEA"
        };
        public static List<string> BowHashes { get; } = new List<string>()
        {
            "WEAPON_BOW"
        };
    }
}
