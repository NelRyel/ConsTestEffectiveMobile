using ConsTestEffectiveMobile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsTestEffectiveMobile
{
    public class MyGeters
    {
        //выводит список заказов по ИД района и времени С и ПО
       public void GetOrdersByDistrict(int id,int FromHour, int ToHour)
        {
                Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.File("_deliveryLog.txt")
               .CreateLogger();

                // Регистрируем Serilog при создании Microsoft.Extensions.Logging.LoggerFactory
                using var loggerFactory = LoggerFactory.Create(
                    builder => builder.AddSerilog(dispose: true));
                // Создаем экземпляр ILogger при помощи фабрики
                var logger = loggerFactory.CreateLogger<Program>();

            using (_DbContext db = new _DbContext())
            {
                logger.LogInformation("started GetOrdersByDistrict");
                var some = db.CityDistricts.Include(o => o.Orders).Where(a => a.Id == id).ToList();
                foreach (CityDistrict dstrckt in some)
                {
                    Console.WriteLine($"\n район: {dstrckt.Name}");

                    var sss = dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a => a.DateTimeOredDelivery.Hour >= FromHour && a.DateTimeOredDelivery.Hour <= ToHour && a.DateTimeOredDelivery.Year == 2024 && a.DateTimeOredDelivery.Month == 10).LastOrDefault();

                    //Console.WriteLine("@@@@@@@@@@@@@@@@@ " + sss.Id + "@@@@@@@@@@@@@@");
                    //ну тут я в итоге захардкодил определенный день, чтоб не заполнять лишний раз базу и не плодить ридЛайны. дата 2024-10-22
                    foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a => a.DateTimeOredDelivery.Hour > FromHour && a.DateTimeOredDelivery.Hour < ToHour && a.DateTimeOredDelivery.Year == 2024 && a.DateTimeOredDelivery.Month == 10))
                    //foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a=>a.DateTimeOredDelivery.Hour>=sss.DateTimeOredDelivery.Hour && a.DateTimeOredDelivery.Hour <= sss.DateTimeOredDelivery.Hour&&a.DateTimeOredDelivery.Minute<=sss.DateTimeOredDelivery.Minute+30 && a.DateTimeOredDelivery.Year==2024&&a.DateTimeOredDelivery.Month==10))

                    {

                        Console.WriteLine("ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                        logger.LogInformation("getting info: ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                    }

                }
            }
        }

        //сохраняет в файл заказы по ИД района от заданного часа и по +30минут
       public void SaveToFile(int id, int FromHour, int ToHour, string path)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("_deliveryLog.txt")
            .CreateLogger();

            // Регистрируем Serilog при создании Microsoft.Extensions.Logging.LoggerFactory
           using var loggerFactory = LoggerFactory.Create(
           builder => builder.AddSerilog(dispose: true));
          // Создаем экземпляр ILogger при помощи фабрики
            var logger = loggerFactory.CreateLogger<Program>();



            string text = "";
            using (_DbContext db = new _DbContext())
            {
                logger.LogInformation("started SaveToFile");
                var some = db.CityDistricts.Include(o => o.Orders).Where(a => a.Id == id).ToList();
                foreach (CityDistrict dstrckt in some)
                {
                    Console.WriteLine($"\n район: {dstrckt.Name}");
                    
                    var sss = dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a => a.DateTimeOredDelivery.Hour >= FromHour && a.DateTimeOredDelivery.Hour <= ToHour && a.DateTimeOredDelivery.Year == 2024 && a.DateTimeOredDelivery.Month == 10).LastOrDefault();

                    //ну тут я в итоге захардкодил определенный день, чтоб не заполнять лишний раз базу и не плодить ридЛайны. дата 2024-10-22
                     foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a => a.DateTimeOredDelivery.Hour >= sss.DateTimeOredDelivery.Hour && 
                     a.DateTimeOredDelivery.Hour <= sss.DateTimeOredDelivery.Hour && a.DateTimeOredDelivery.Minute <= sss.DateTimeOredDelivery.Minute + 30 && 
                     a.DateTimeOredDelivery.Year == 2024 && a.DateTimeOredDelivery.Month == 10))
                    {

                        //Console.WriteLine("ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                        text += "ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}" + Environment.NewLine;

                        logger.LogInformation("saving info: ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                    }
                    File.WriteAllText(path, text);
                    Console.WriteLine("File saved...");

                }
            }
        }


    }
}
