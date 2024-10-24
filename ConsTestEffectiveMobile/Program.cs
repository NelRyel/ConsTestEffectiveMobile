// See https://aka.ms/new-console-template for more information
using ConsTestEffectiveMobile;
using ConsTestEffectiveMobile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using InterpolatedLoggingDemo;


//https://drive.google.com/file/d/1hwrCwvhyNJUtDHreyqeSM_P3yQQ_XcYC/view
Console.WriteLine("Hello, World!");
//MyLog.AddLogging();

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .WriteTo.File("_deliveryLog.txt")
   .CreateLogger();

// Регистрируем Serilog при создании Microsoft.Extensions.Logging.LoggerFactory
using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddSerilog(dispose: true));
// Создаем экземпляр ILogger при помощи фабрики
var logger = loggerFactory.CreateLogger<Program>();


// это для первого заполнения БД 
DefaultInit defaultInit = new DefaultInit();
//defaultInit.StartInitCityDist();
//defaultInit.StartOrderInit();
// это для первого заполнения БД 


bool w = true;
int cityCount;
int FromHour;
int ToHour;
while (w == true)
{
    using (_DbContext db = new _DbContext())
    {
       
        //выводим районы
        var dist = db.CityDistricts.ToList();
        cityCount = db.CityDistricts.Count();
        foreach (var item in dist)
        {
            Console.WriteLine("ID: " + item.Id + " " + "Name: " + item.Name);
        }
        logger.LogInformation("get cityDist from db: " + dist.ToString());
    }
    bool x = true;
    int id;
    do
    {
            //собираем данные для фильтрации
            Console.WriteLine("enter id ");
            string s = Console.ReadLine();
            Console.WriteLine("enter from hour ");
            string fh = Console.ReadLine();
            Console.WriteLine("enter to hour ");
            string th = Console.ReadLine();
            bool isNumFh = int.TryParse(fh, out FromHour);
            bool isNumTh = int.TryParse(th, out ToHour);
            bool isNumeric = int.TryParse(s, out id);
        
        //огрооооомный иф, хм, наверное можно было запихнуть это в отдельный метод бул который это всё собирает и проверяет...ну да ладно
        if (isNumeric == true && id <= cityCount && isNumFh == true && isNumTh == true&& FromHour <= ToHour&&FromHour>=0&&FromHour<= 24&&ToHour >= 0 && ToHour <= 24)
        {
            x = false;
            //Console.WriteLine("да");
        }
        else
        {
            Console.WriteLine("не то");
        }
        logger.LogInformation("enter datas. Id: " + s+" | FromHour "+fh+ " | ToHour "+th+" | ");
    }
    while (x == true);

    GetOrdersByDistrict(id);

    Console.WriteLine("y - еще. другое - всё");
    char c = Convert.ToChar(Console.ReadLine());
    if (c == 'y')
    {
        w = true;
    }
    else {
        logger.LogInformation("____END_____");

        w = false; 
    }
}


void GetOrdersByDistrict(int id)
    {
        using (_DbContext db = new _DbContext())
        {
        logger.LogInformation("started GetOrdersByDistrict");
        var some = db.CityDistricts.Include(o => o.Orders).Where(a => a.Id == id).ToList();
            foreach (CityDistrict dstrckt in some)
            {
                Console.WriteLine($"\n район: {dstrckt.Name}");

            var sss = dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a => a.DateTimeOredDelivery.Hour > FromHour && a.DateTimeOredDelivery.Hour < ToHour && a.DateTimeOredDelivery.Year == 2024 && a.DateTimeOredDelivery.Month == 10).LastOrDefault();
            
            //Console.WriteLine("@@@@@@@@@@@@@@@@@ " + sss.Id + "@@@@@@@@@@@@@@");
            //ну тут я в итоге захардкодил определенный день, чтоб не заполнять лишний раз базу и не плодить ридЛайны. дата 2024-10-22
//            foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a=>a.DateTimeOredDelivery.Hour>FromHour&&a.DateTimeOredDelivery.Hour<ToHour&&a.DateTimeOredDelivery.Year==2024&&a.DateTimeOredDelivery.Month==10))
            foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery).Where(a=>a.DateTimeOredDelivery.Hour>=sss.DateTimeOredDelivery.Hour&& a.DateTimeOredDelivery.Hour <= sss.DateTimeOredDelivery.Hour && a.DateTimeOredDelivery.Minute>=sss.DateTimeOredDelivery.Minute+30&&a.DateTimeOredDelivery.Year==2024&&a.DateTimeOredDelivery.Month==10))

                {

                    Console.WriteLine("ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                logger.LogInformation("getting info: ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + $"{order.DateTimeOredDelivery:u}");
                }
           
        }
        }
    }

