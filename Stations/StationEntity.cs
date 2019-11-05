using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using itec_mobile_api_final.Entities;

namespace itec_mobile_api_final.Stations
{
    public class StationEntity : Entity
    {
        public string Name { get; set; }
        public int TotalSockets { get; set; }
        public int OccupiedSockets { get; set; }
        public PointF Location { get; set; }
    }
}