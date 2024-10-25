// See https://aka.ms/new-console-template for more information
using ConsTestEffectiveMobile;
using ConsTestEffectiveMobile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System.Collections.Generic;
using System.IO;
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

//DefaultInit я тоже в большей степени захардкодил. Оно заполняет от 2024-10-22 с 10 часов. Что-бы было проще тестировать. Района так же там созднание районов. 
//поидее, если подтянуть с гитхаба, БД заполненая тоже подтянется. 

// это для первого заполнения БД 
DefaultInit defaultInit = new DefaultInit();
//defaultInit.StartInitCityDist();
//defaultInit.StartOrderInit();
// это для первого заполнения БД 

MyGeters myGeters = new MyGeters();
string path = "_deliveryOrder.txt";
bool w = true;
int cityCount = 0;
int FromHour = 0;
int ToHour = 0;
string s = "";
string fh = "";
string th = "";
bool isNumFh=false;
bool isNumTh=false;
bool isNumeric=false;

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
    int id = 0;
    do
    {
        try
        {
            //собираем данные для фильтрации
            Console.WriteLine("enter id ");
            s = Console.ReadLine();
            Console.WriteLine("enter from hour ");
            fh = Console.ReadLine();
            Console.WriteLine("enter to hour ");
            th = Console.ReadLine();
            isNumFh = int.TryParse(fh, out FromHour);
            isNumTh = int.TryParse(th, out ToHour);
            isNumeric = int.TryParse(s, out id);
        }
        catch(Exception e) 
        { 
            Console.WriteLine("wrong enter "+e.Message);
            logger.LogInformation("wrong enter "+e.Message);

        }
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
        logger.LogInformation("entered datas. Id: " + s+" | FromHour "+fh+ " | ToHour "+th+" | ");
    }
    while (x == true);

    //GetOrdersByDistrict(id);
    myGeters.GetOrdersByDistrict(id, FromHour, ToHour);
    myGeters.SaveToFile(id, FromHour, ToHour, path);

    //SaveToFile(id);

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

