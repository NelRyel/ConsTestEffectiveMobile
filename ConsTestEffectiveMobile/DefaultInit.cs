using ConsTestEffectiveMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsTestEffectiveMobile
{
    public class DefaultInit
    {
        public void StartInitCityDist()
        {
            using (_DbContext db = new _DbContext())
            {
                for (int i = 0; i < 10; i++)
                {
                    CityDistrict district = new CityDistrict();
                    district.Name = "Район " + i.ToString();
                    db.CityDistricts.Add(district);
                }
                db.SaveChanges();
            }
        }


        public double MyRand(int MaxSize)
        {
            Random rand = new Random();
            double randValDouble = rand.NextDouble();
            float valuefloat = rand.Next(0, MaxSize);
            float temp = valuefloat + ((float)randValDouble);
            return temp;
            
        }
        public int MyRandDay(int Month)
        {
            Random rand = new Random();
            if (Month == 2)
            {
                return rand.Next(1,24);
            }
            if(Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
            {
                return rand.Next(1,31);
            }
            else
            {
                return rand.Next(1, 30);
            }
        }

        public void StartOrderInit()
        {
            Random rand = new Random();
            using (_DbContext db = new _DbContext())
            {
                int CountCity = db.CityDistricts.Count();

                for (int i = 0; i < 100; i++)
                {
                    
                    int IDD = rand.Next(1, CountCity);
                    
                    Order order = new Order();
                    order.Weight = MyRand(50);
                   
                    int month = rand.Next(1, 12);
                    int day = MyRandDay(month);
                    int hour = rand.Next(0, 24);
                    int minute = rand.Next(0, 59);
                    int second = rand.Next(0, 59);
                    DateTime dateTime = new DateTime(2024, month, day,hour, minute, second);
                    order.DateTimeOredDelivery = dateTime;
                    CityDistrict district = db.CityDistricts.Find(IDD);
                    order.CityDistrict = district;
                    db.Orders.Add(order);
                }
                db.SaveChanges();
            }
        }
    }
}
