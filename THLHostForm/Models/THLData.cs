using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [System.Serializable]
    public class THLData
    {
        public int THDataId { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Light { get; set; }
        public DateTime DTime { get; set; }
    }
}
