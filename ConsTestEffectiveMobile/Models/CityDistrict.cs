using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsTestEffectiveMobile.Models
{
    public class CityDistrict
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order>? Orders { get; set; } = new();
    }
}
