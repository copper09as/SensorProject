using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectSystem.Data
{
    [System.Serializable]
    public class THLData
    {
        public int THDataId { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Light { get; set; }
        public DateTime DTime { get; set; }
        public override string ToString()
        {
            return $"THLData [ID={THDataId}, Temperature={Temperature:F2}°C, Humidity={Humidity:F2}%, Light={Light:F2} lx, Time={DTime:yyyy-MM-dd HH:mm:ss}]";
        }
    }
}
