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
            new TeleportLocation("Colter", -1361.96f, 2392.64f, 306.61f, 332.11f),
            new TeleportLocation("Emerald Ranch", 1518.24f, 429.25f, 90.68f, 153.84f),
            new TeleportLocation("Rhodes", 1240.31f, -1289.46f, 76.91f, 301.61f),
            new TeleportLocation("Saint Denis", 2715.28f, -1431.62f, 46.08f, 24.32f),
            new TeleportLocation("Valentine", -183.61f, 648.32f, 113.57f, 68.20f),
            new TeleportLocation("Van Horn", 2901.32f, 636.08f, 56.28f, 305.40f)
        };
    }
}
