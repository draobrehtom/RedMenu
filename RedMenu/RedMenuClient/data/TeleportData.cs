using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedMenuClient.data
{
    class TeleportLocation
    {
        private string name;
        private float x, y, z, h;
        public TeleportLocation(string name, float x, float y, float z, float h)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
            this.h = h;
        }

        public string Name { get { return name; } }
        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Z { get { return z; } }
        public float H { get { return h; } }
    }
    class TeleportData
    {
        public static List<TeleportLocation> TeleportLocations { get; } = new List<TeleportLocation>()
        {
            new TeleportLocation("Annesburg", 2929.09f, 1291.44f, 44.66f, 70.79f),
            new TeleportLocation("Armadillo", -3742.67f, -2607.75f, -13.24f, 272.03f),
            new TeleportLocation("Blackwater", -731.29f, -1243.02f, 44.73f, 90.96f),
            new TeleportLocation("Colter", -1361.96f, 2392.64f, 306.61f, 332.11f),
            new TeleportLocation("Emerald Ranch", 1518.24f, 429.25f, 90.68f, 153.84f),
            new TeleportLocation("Manzanita Post", -1948.67f, -1621.65f, 116.08f, 76.63f),
            new TeleportLocation("McFarland Ranch", -2499.87f, -2446.45f, 60.15f, 283.89f),
            new TeleportLocation("Rhodes", 1240.31f, -1289.46f, 76.91f, 301.61f),
            new TeleportLocation("Saint Denis", 2715.28f, -1431.62f, 46.08f, 24.32f),
            new TeleportLocation("Sisika Penitentiary", 2715.28f, -1431.62f, 46.08f, 24.32f),
            new TeleportLocation("Strawberry", -1743.25f, -412.08f, 155.45f, 32.19f),
            new TeleportLocation("Thieves Landing", -1393.38f, -2233.62f, 43.35f, 147.97f),
            new TeleportLocation("Tumbleweed", -5432.90f, -2947.03f, 0.64f, 80.80f),
            new TeleportLocation("Valentine", -183.61f, 648.32f, 113.57f, 68.20f),
            new TeleportLocation("Van Horn", 2901.32f, 636.08f, 56.28f, 305.40f),
            new TeleportLocation("Wapiti", 489.37f, 2211.77f, 247.06f, 58.47f)
        };
    }
}
