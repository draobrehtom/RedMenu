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
            "WEAPON_KIT_BINOCULARS_IMPROVED",
            "WEAPON_KIT_CAMERA",
            "WEAPON_KIT_CAMERA_ADVANCED",
            "WEAPON_KIT_METAL_DETECTOR",
            "WEAPON_LASSO",
            "WEAPON_LASSO_REINFORCED",
            "WEAPON_MELEE_LANTERN",
            "WEAPON_MELEE_LANTERN_HALLOWEEN",
            "WEAPON_MELEE_DAVY_LANTERN",
            //"WEAPON_MELEE_LANTERN_ELECTRIC",
            "WEAPON_MELEE_TORCH",
            //"WEAPON_MOONSHINEJUG",
            "WEAPON_MOONSHINEJUG_MP"
        };
        public static List<string> MeleeHashes { get; } = new List<string>()
        {
            //"WEAPON_MELEE_ANCIENT_HATCHET",
            //"WEAPON_MELEE_BROKEN_SWORD",
            "WEAPON_MELEE_CLEAVER",
            "WEAPON_MELEE_HAMMER",
            "WEAPON_MELEE_HATCHET",
            //"WEAPON_MELEE_HATCHET_DOUBLE_BIT",
            //"WEAPON_MELEE_HATCHET_DOUBLE_BIT_RUSTED",
            //"WEAPON_MELEE_HATCHET_HEWING",
            "WEAPON_MELEE_HATCHET_HUNTER",
            //"WEAPON_MELEE_HATCHET_HUNTER_RUSTED",
            //"WEAPON_MELEE_HATCHET_VIKING",
            "WEAPON_MELEE_KNIFE",
            //"WEAPON_MELEE_KNIFE_BEAR",
            //"WEAPON_MELEE_KNIFE_CIVIL_WAR",
            "WEAPON_MELEE_KNIFE_HORROR",
            "WEAPON_MELEE_KNIFE_JAWBONE",
            "WEAPON_MELEE_KNIFE_RUSTIC",
            "WEAPON_MELEE_KNIFE_TRADER",
            //"WEAPON_MELEE_KNIFE_JOHN",
            //"WEAPON_MELEE_KNIFE_MINER",
            //"WEAPON_MELEE_KNIFE_VAMPIRE",
            "WEAPON_MELEE_MACHETE",
            "WEAPON_MELEE_MACHETE_COLLECTOR",
            "WEAPON_MELEE_MACHETE_HORROR"
        };

        public static List<string> RevolverHashes { get; } = new List<string>()
        {
            "WEAPON_REVOLVER_CATTLEMAN",
            //"WEAPON_REVOLVER_CATTLEMAN_JOHN",
            "WEAPON_REVOLVER_CATTLEMAN_MEXICAN",
            //"WEAPON_REVOLVER_CATTLEMAN_PIG",
            "WEAPON_REVOLVER_DOUBLEACTION",
            //"WEAPON_REVOLVER_DOUBLEACTION_EXOTIC",
            "WEAPON_REVOLVER_DOUBLEACTION_GAMBLER",
            "WEAPON_REVOLVER_LEMAT",
            "WEAPON_REVOLVER_NAVY",
            "WEAPON_REVOLVER_NAVY_CROSSOVER",
            "WEAPON_REVOLVER_SCHOFIELD",
            //"WEAPON_REVOLVER_SCHOFIELD_CALLOWAY"
        };

        public static List<string> PistolHashes { get; } = new List<string>()
        {
            "WEAPON_PISTOL_M1899",
            "WEAPON_PISTOL_MAUSER",
            //"WEAPON_PISTOL_MAUSER_DRUNK",
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
            "WEAPON_RIFLE_ELEPHANT",
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
            "WEAPON_THROWN_BOLAS",
            "WEAPON_THROWN_BOLAS_HAWKMOTH",
            "WEAPON_THROWN_BOLAS_IRONSPIKED",
            "WEAPON_THROWN_BOLAS_INTERTWINED",
            "WEAPON_THROWN_DYNAMITE",
            "WEAPON_THROWN_MOLOTOV",
            "WEAPON_THROWN_POISONBOTTLE",
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
            //"WEAPON_SHOTGUN_SEMIAUTO_HOSEA"
        };
        public static List<string> BowHashes { get; } = new List<string>()
        {
            "WEAPON_BOW",
            "WEAPON_BOW_IMPROVED"
        };

        public static List<string> AmmoHashes { get; } = new List<string>()
        {
            "AMMO_22",
            //"AMMO_22_TRANQUILIZER",
            "AMMO_ARROW",
            //"AMMO_ARROW_CONFUSION",
            //"AMMO_ARROW_DISORIENT",
            //"AMMO_ARROW_DRAIN",
            //"AMMO_ARROW_DYNAMITE",
            "AMMO_ARROW_FIRE",
            //"AMMO_ARROW_IMPROVED",
            "AMMO_ARROW_POISON",
            //"AMMO_ARROW_SMALL_GAME",
            //"AMMO_ARROW_TRACKING",
            //"AMMO_ARROW_TRAIL",
            //"AMMO_ARROW_WOUND",
            "AMMO_BOLAS",
            "AMMO_CANNON",
            "AMMO_DYNAMITE",
            //"AMMO_DYNAMITE_VOLATILE",
            "AMMO_HATCHET",
            //"AMMO_HATCHET_ANCIENT",
            //"AMMO_HATCHET_CLEAVER",
            //"AMMO_HATCHET_DOUBLE_BIT",
            //"AMMO_HATCHET_DOUBLE_BIT_RUSTED",
            //"AMMO_HATCHET_HEWING",
            //"AMMO_HATCHET_HUNTER",
            //"AMMO_HATCHET_HUNTER_RUSTED",
            //"AMMO_HATCHET_VIKING",
            "AMMO_LASSO",
            //"AMMO_LASSO_REINFORCED",
            "AMMO_MOLOTOV",
           // "AMMO_MOLOTOV_VOLATILE",
            "AMMO_MOONSHINEJUG",
            //"AMMO_MOONSHINEJUG_MP",
            "AMMO_PISTOL",
            //"AMMO_PISTOL_EXPRESS",
            //"AMMO_PISTOL_EXPRESS_EXPLOSIVE",
            //"AMMO_PISTOL_HIGH_VELOCITY",
            //"AMMO_PISTOL_SPLIT_POINT",
            "AMMO_POISONBOTTLE",
            "AMMO_REPEATER",
            //"AMMO_REPEATER_EXPRESS",
            //"AMMO_REPEATER_EXPRESS_EXPLOSIVE",
            //"AMMO_REPEATER_HIGH_VELOCITY",
            //"AMMO_REPEATER_SPLIT_POINT",
            "AMMO_REVOLVER",
            //"AMMO_REVOLVER_EXPRESS",
            //"AMMO_REVOLVER_EXPRESS_EXPLOSIVE",
            //"AMMO_REVOLVER_HIGH_VELOCITY",
            //"AMMO_REVOLVER_SPLIT_POINT",
            "AMMO_RIFLE",
            //"AMMO_RIFLE_ELEPHANT",
            //"AMMO_RIFLE_EXPRESS",
            //"AMMO_RIFLE_EXPRESS_EXPLOSIVE",
            //"AMMO_RIFLE_HIGH_VELOCITY",
            //"AMMO_RIFLE_SPLIT_POINT",
            "AMMO_SHOTGUN",
            //"AMMO_SHOTGUN_BUCKSHOT_INCENDIARY",
            //"AMMO_SHOTGUN_SLUG",
            //"AMMO_SHOTGUN_SLUG_EXPLOSIVE",
            "AMMO_THROWING_KNIVES",
            //"AMMO_THROWING_KNIVES_CONFUSE",
            //"AMMO_THROWING_KNIVES_DISORIENT",
            //"AMMO_THROWING_KNIVES_DRAIN",
            //"AMMO_THROWING_KNIVES_IMPROVED",
            //"AMMO_THROWING_KNIVES_JAVIER",
            //"AMMO_THROWING_KNIVES_POISON",
            //"AMMO_THROWING_KNIVES_TRAIL",
            //"AMMO_THROWING_KNIVES_WOUND",
            "AMMO_THROWN_ITEM",
            "AMMO_TOMAHAWK",
            //"AMMO_TOMAHAWK_ANCIENT",
            //"AMMO_TOMAHAWK_HOMING",
            //"AMMO_TOMAHAWK_IMPROVED",
            "AMMO_TURRET"
        };

        public static List<WeaponComponent> Variants = new List<WeaponComponent>()
        {
            new WeaponComponent("Bounty Hunter Variant", -1814368974),
            new WeaponComponent("Collector Variant", -840678979),
        };

        public static List<WeaponComponent> LongarmBarrelColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -1043593372),
            new WeaponComponent("Blued Steel", -267000841),
            new WeaponComponent("Brass", 1457729936),
            new WeaponComponent("Browned Steel", 1133578988),
            new WeaponComponent("Copper", -1132266290),
            new WeaponComponent("Gold", 681399557),
            new WeaponComponent("Nickel", 471677957),
            new WeaponComponent("Silver", 983988503),
        };

        public static List<WeaponComponent> SidearmBarrelColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -427290118),
            new WeaponComponent("Blued Steel", 405927245),
            new WeaponComponent("Brass", 1776195757),
            new WeaponComponent("Browned Steel", -1677230850),
            new WeaponComponent("Copper", 1420029480),
            new WeaponComponent("Gold", 2013836545),
            new WeaponComponent("Nickel", 165402785),
            new WeaponComponent("Silver", -960769434),
        };

        public static List<WeaponComponent> LongarmBarrelDecals = new List<WeaponComponent>()
        {
            new WeaponComponent("Art Nouveau", -1719565838),
            new WeaponComponent("Baroque", 443679705),
            new WeaponComponent("Ornamental", -1103410331),
            new WeaponComponent("Victorian", 1042400629)
        };

        public static List<WeaponComponent> SidearmBarrelDecals = new List<WeaponComponent>()
        {
            new WeaponComponent("Art Nouveau", -1368707528),
            new WeaponComponent("Baroque", -1114256243),
            new WeaponComponent("Ornamental", 1029393446),
        };

        public static List<WeaponComponent> LongarmBarrelDecalColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", 1298235955),
            new WeaponComponent("Blued Steel", 1585980544),
            new WeaponComponent("Brass", 508830741),
            new WeaponComponent("Browned Steel", -1024168617),
            new WeaponComponent("Copper", 1875494695),
            new WeaponComponent("Gold", 336465801),
            new WeaponComponent("Iron", -98051139),
            new WeaponComponent("Nickel", 2026592518),
            new WeaponComponent("Silver", 2105762458),
        };

        public static List<WeaponComponent> SidearmBarrelDecalColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", 546636762),
            new WeaponComponent("Blued Steel", 1382016879),
            new WeaponComponent("Brass", -2025729742),
            new WeaponComponent("Browned Steel", 901918260),
            new WeaponComponent("Copper", 1917396801),
            new WeaponComponent("Gold", -1653277288),
            new WeaponComponent("Iron", -857285521),
            new WeaponComponent("Nickel", 1089127557),
            new WeaponComponent("Silver", 1797200109),
        };

        public static List<WeaponComponent> LongarmFrameColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -1070377672),
            new WeaponComponent("Blued Steel", 715270684),
            new WeaponComponent("Brass", 1675631767),
            new WeaponComponent("Browned Steel", -789973339),
            new WeaponComponent("Copper", 1193534243),
            new WeaponComponent("Gold", 1906948138),
            new WeaponComponent("Nickel", 486837985),
            new WeaponComponent("Silver", 955336382),
        };

        public static List<WeaponComponent> SidearmFrameColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -1309913887),
            new WeaponComponent("Blued Steel", 1408995585),
            new WeaponComponent("Brass", 1712534832),
            new WeaponComponent("Browned Steel", 2022201882),
            new WeaponComponent("Copper", -1552961560),
            new WeaponComponent("Gold", -19142973),
            new WeaponComponent("Nickel", 461873182),
            new WeaponComponent("Silver", -323992980),
        };

        public static List<WeaponComponent> LongarmFrameDecals = new List<WeaponComponent>()
        {
            new WeaponComponent("Art Nouveau", 1338763465),
            new WeaponComponent("Baroque", -501379718),
            new WeaponComponent("Ornamental", 744497650),
            new WeaponComponent("Victorian", 1042400629),
        };

        public static List<WeaponComponent> SidearmFrameDecals = new List<WeaponComponent>()
        {
            new WeaponComponent("Art Nouveau", 709494243),
            new WeaponComponent("Baroque", -1371140647),
            new WeaponComponent("Ornamental", 362765454),
            new WeaponComponent("Victorian", 596834421),
        };

        public static List<WeaponComponent> LongarmFrameDecalColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -778293609),
            new WeaponComponent("Blued Steel", -1822969329),
            new WeaponComponent("Brass", 717185184),
            new WeaponComponent("Browned Steel", -1075475670),
            new WeaponComponent("Copper", -68517129),
            new WeaponComponent("Gold", 477479949),
            new WeaponComponent("Iron", -1422269997),
            new WeaponComponent("Nickel", -451553910),
            new WeaponComponent("Silver", 440123289),
        };

        public static List<WeaponComponent> SidearmFrameDecalColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -1547124156),
            new WeaponComponent("Blued Steel", -1171394802),
            new WeaponComponent("Brass", -1725944581),
            new WeaponComponent("Browned Steel", 1402380769),
            new WeaponComponent("Copper", -1345594798),
            new WeaponComponent("Gold", -2074737817),
            new WeaponComponent("Iron", 484521079),
            new WeaponComponent("Nickel", -2005660773),
            new WeaponComponent("Silver", -1240406308),
        };

        public static List<WeaponComponent> CarbineSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 1258698427),
            new WeaponComponent("Improved Sights", -1098615404),
        };

        public static List<WeaponComponent> EvansSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 431616357),
        };

        public static List<WeaponComponent> LitchfieldSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 1052491799),
            new WeaponComponent("Improved Sights", -1165022028),
        };

        public static List<WeaponComponent> LancasterSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", -1519167968),
        };

        public static List<WeaponComponent> BoltActionSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 1311478812),
        };

        public static List<WeaponComponent> SpringfieldSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 893095238),
            new WeaponComponent("Improved Sights", -1747228523),
        };

        public static List<WeaponComponent> CarcanoSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", -1301050378),
            new WeaponComponent("Improved Sights", -298805818),
        };

        public static List<WeaponComponent> RollingBlockSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", -555346213),
        };

        public static List<WeaponComponent> DoubleBarrelShotgunSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", -2125472551),
            new WeaponComponent("Improved Sights", 1465866430),
        };

        public static List<WeaponComponent> RepeatingShotgunSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", -379338353),
        };

        public static List<WeaponComponent> PumpShotgunSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", -660073031),
            new WeaponComponent("Improved Sights", -1381877519),
        };

        public static List<WeaponComponent> SemiAutoShotgunSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 760525697),
            new WeaponComponent("Improved Sights", 161409167),
        };

        public static List<WeaponComponent> SawedOffShotgunSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", -755483347),
        };

        public static List<WeaponComponent> M1899Sights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 85701152),
            new WeaponComponent("Improved Sights", 1846607767),
        };

        public static List<WeaponComponent> MauserSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 444690491),
        };

        public static List<WeaponComponent> SemiAutoSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 1234620219),
        };

        public static List<WeaponComponent> VolcanicSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", -1364839165),
            new WeaponComponent("Improved Sights", -1620844433),
            new WeaponComponent("Collector Sights", -858357099),
        };

        public static List<WeaponComponent> CattlemanSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", 173773832),
            new WeaponComponent("Improved Sights", 453873243),
        };

        public static List<WeaponComponent> DoubleActionSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 129601018),
        };

        public static List<WeaponComponent> LematSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Stock Sights", -235237690),
            new WeaponComponent("Improved Sights", 490457035),
        };

        public static List<WeaponComponent> SchofieldSights = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Sights", 449305157),
            new WeaponComponent("Bounty Hunter Sights", -1391305890),
        };

        public static List<WeaponComponent> Scopes = new List<WeaponComponent>()
        {
            new WeaponComponent("Short Scope", -404520310),
            new WeaponComponent("Medium Scope", -1844750633),
            new WeaponComponent("Long Scope", -1545766277),
        };

        public static List<WeaponComponent> ScopeSightColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -181540583),
            new WeaponComponent("Blued Steel", -1992289985),
            new WeaponComponent("Brass", -794550266),
            new WeaponComponent("Browned Steel", -411415118),
            new WeaponComponent("Copper", 1042152184),
            new WeaponComponent("Gold", -1021999895),
            new WeaponComponent("Iron", -1332551708),
            new WeaponComponent("Nickel", -1712803184),
            new WeaponComponent("Silver", 1147799440),
        };

        public static List<WeaponComponent> CarbineWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -2006479051),
            new WeaponComponent("Leather", -819094336)
        };

        public static List<WeaponComponent> EvansWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 1213912331),
            new WeaponComponent("Leather", -1013433120)
        };

        public static List<WeaponComponent> LancasterWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Collector Wrap", 1799288707),
            new WeaponComponent("Cloth", -974982909),
            new WeaponComponent("Leather", 1673079985)
        };

        public static List<WeaponComponent> LitchfieldWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 557684246),
            new WeaponComponent("Leather", 327383714)
        };

        public static List<WeaponComponent> BoltActionWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -556684967),
            new WeaponComponent("Leather", 1419411400),
        };

        public static List<WeaponComponent> SpringfieldWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -722454818),
            new WeaponComponent("Leather", -960882062),
        };

        public static List<WeaponComponent> VarmintWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 1800607228),
            new WeaponComponent("Leather", 1485926521),
        };

        public static List<WeaponComponent> CarcanoWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 1958362959),
            new WeaponComponent("Leather", -1649438395),
        };

        public static List<WeaponComponent> RollingBlockWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 627382224),
            new WeaponComponent("Leather", 1373106357),
        };

        public static List<WeaponComponent> DoubleBarrelShotgunWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -470655201),
            new WeaponComponent("Leather", -240387438),
        };

        public static List<WeaponComponent> RepeatingShotgunWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -856257300),
            new WeaponComponent("Leather", -1356443316),
        };

        public static List<WeaponComponent> PumpShotgunWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", 930176412),
            new WeaponComponent("Leather", 1509466794),
        };

        public static List<WeaponComponent> SemiAutoShotgunWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -1332582451),
            new WeaponComponent("Leather", 1680068337),
        };

        public static List<WeaponComponent> SawedOffShotgunWraps = new List<WeaponComponent>()
        {
            new WeaponComponent("Cloth", -221706764),
            new WeaponComponent("Leather", 635989042),
        };

        public static List<WeaponComponent> WrapColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Black", -1954915781),
            new WeaponComponent("Brick Red", -1952154020),
            new WeaponComponent("Chestnut", 1834893776),
            new WeaponComponent("Chocolate", -1550668232),
            new WeaponComponent("Olive", 1595450693),
            new WeaponComponent("Pebble", 1313932214),
            new WeaponComponent("Tobacco", -1311421763),
            new WeaponComponent("Walnut", 253101337),
            new WeaponComponent("White", 1170373926),
        };

        public static List<WeaponComponent> CarbineStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -1277277764),
            new WeaponComponent("Wide Grain", -1774830908),
        };

        public static List<WeaponComponent> EvansStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 2104230701),
            new WeaponComponent("Wide Grain", 590829769),
        };

        public static List<WeaponComponent> LitchfieldStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -473026721),
            new WeaponComponent("Wide Grain", 286341385),
        };

        public static List<WeaponComponent> LancasterStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 2104230701)
        };

        public static List<WeaponComponent> BoltActionStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Bounty Hunter Grain", 1043980328),
            new WeaponComponent("Straight Grain", -1776314974),
            new WeaponComponent("Wide Grain", -6205275),
        };

        public static List<WeaponComponent> SpringfieldStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 1161381174),
            new WeaponComponent("Wide Grain", -407737365),
        };

        public static List<WeaponComponent> CarcanoStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 2099965087),
            new WeaponComponent("Wide Grain", 445539346),
        };

        public static List<WeaponComponent> RollingBlockStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -821378099),
            new WeaponComponent("Wide Grain", -1186786648),
        };

        public static List<WeaponComponent> DoubleBarrelShotgunStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -808741305),
            new WeaponComponent("Wide Grain", -282809632),
        };

        public static List<WeaponComponent> RepeatingShotgunStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 313236058),
            new WeaponComponent("Wide Grain", -454348329),
        };

        public static List<WeaponComponent> PumpShotgunStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -364352498),
            new WeaponComponent("Trader Grain", -1850951144),
            new WeaponComponent("Wide Grain", -1341331027),
        };

        public static List<WeaponComponent> SemiAutoShotgunStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", 1316725623),
            new WeaponComponent("Wide Grain", -827932793),
        };

        public static List<WeaponComponent> SawedOffShotgunStocks = new List<WeaponComponent>()
        {
            new WeaponComponent("Straight Grain", -1049082913),
            new WeaponComponent("Wide Grain", -976603528),
        };

        public static List<WeaponComponent> LeverColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", 1908517984),
            new WeaponComponent("Blued Steel", 2116797748),
            new WeaponComponent("Brass", -986393783),
            new WeaponComponent("Copper", 221635402),
            new WeaponComponent("Gold", -1217972306),
            new WeaponComponent("Iron", 716152381),
            new WeaponComponent("Nickel", -1948295013),
            new WeaponComponent("Silver", -1425465614),
        };

        public static List<WeaponComponent> GripCarvings = new List<WeaponComponent>()
        {
            new WeaponComponent("Bear", -1540008178),
            new WeaponComponent("Buck Scene", -725174224),
            new WeaponComponent("Eagle Scene", -331258075),
            new WeaponComponent("Flying Eagle", 2110982730),
            new WeaponComponent("Ram", -90831918),
            new WeaponComponent("Wolf Scene", -617954056),
        };

        public static List<WeaponComponent> Rifling = new List<WeaponComponent>()
        {
            new WeaponComponent("Improved Rifling", 488786388),
        };

        public static List<WeaponComponent> LongarmHammerColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", 288090717),
            new WeaponComponent("Blued Steel", -1194015),
            new WeaponComponent("Brass", -1226263084),
            new WeaponComponent("Browned Steel", -1532817079),
            new WeaponComponent("Copper", 1876666299),
            new WeaponComponent("Gold", -897983242),
            new WeaponComponent("Nickel", -271964262),
            new WeaponComponent("Silver", 1569194772),
        };

        public static List<WeaponComponent> SidearmHammerColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Blackened Steel", -1007265484),
            new WeaponComponent("Blued Steel", -306828109),
            new WeaponComponent("Brass", 836580604),
            new WeaponComponent("Browned Steel", -736462468),
            new WeaponComponent("Copper", -755930407),
            new WeaponComponent("Gold", 1063571467),
            new WeaponComponent("Nickel", -679804867),
            new WeaponComponent("Silver", -14332015),
        };

        public static List<WeaponComponent> TriggerColours = new List<WeaponComponent>()
        {
            new WeaponComponent("Copper", 1430624941),
        };

        public static List<WeaponComponent> M1899Grips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", -1476624562),
            new WeaponComponent("Ironwood Grip", 484200990),
            new WeaponComponent("Pearl Grip", -1696458698),
        };

        public static List<WeaponComponent> MauserGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", 332352979),
            new WeaponComponent("Ironwood Grip", 702783910),
            new WeaponComponent("Pearl Grip", -1860555870),
        };

        public static List<WeaponComponent> SemiAutoGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", -478219023),
            new WeaponComponent("Ironwood Grip", 1129152595),
            new WeaponComponent("Pearl Grip", -67987027),
        };

        public static List<WeaponComponent> VolcanicGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Collector Grip", -49512990),
            new WeaponComponent("Ebony Grip", -153641544),
            new WeaponComponent("Ironwood Grip", 1857585472),
            new WeaponComponent("Pearl Grip", 1693826660),
        };

        public static List<WeaponComponent> CattlemanGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", 1645085645),
            new WeaponComponent("Hero Pearl Grip", -1634668636),
            new WeaponComponent("Ironwood Grip", 1502655719),
            new WeaponComponent("Pearl Grip", 42338655),
        };

        public static List<WeaponComponent> DoubleActionGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", 213673305),
            new WeaponComponent("Gunslinger Ebony Grip", 239026921),
            new WeaponComponent("Ironwood Grip", 1491810126),
            new WeaponComponent("Pearl Grip", -2055673461),
        };

        public static List<WeaponComponent> LematGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Ebony Grip", -783684318),
            new WeaponComponent("Ironwood Grip", 1954995285),
            new WeaponComponent("Pearl Grip", 392234002),
        };

        public static List<WeaponComponent> SchofieldGrips = new List<WeaponComponent>()
        {
            new WeaponComponent("Bounty Hunter Grip", 57366885),
            new WeaponComponent("Ebony Grip", -716143865),
            new WeaponComponent("Pearl Grip", -1368261825),
        };
    }

    public class WeaponComponent
    {
        public string Label { get; }
        public int Hash { get; }

        public WeaponComponent(string label, int hash)
        {
            Label = label;
            Hash = hash;
        }
    }
}
