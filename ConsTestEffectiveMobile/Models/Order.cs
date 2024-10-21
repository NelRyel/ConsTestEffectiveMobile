using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ConsTestEffectiveMobile.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Weight { get; set; }

        public DateTime DateTimeOredDelivery { get; set; }

        public int CityDistrictId { get; set; }
        public CityDistrict? CityDistrict { get; set; }
    }
}
