// See https://aka.ms/new-console-template for more information
using ConsTestEffectiveMobile;
using ConsTestEffectiveMobile.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

//https://drive.google.com/file/d/1hwrCwvhyNJUtDHreyqeSM_P3yQQ_XcYC/view
Console.WriteLine("Hello, World!");

DefaultInit defaultInit = new DefaultInit();
//defaultInit.StartInitCityDist();
//defaultInit.StartOrderInit();
bool w = true;
int cityCount;
while (w == true)
{
    using (_DbContext db = new _DbContext())
    {
        var dist = db.CityDistricts.ToList();
        cityCount = db.CityDistricts.Count();
        foreach (var item in dist)
        {
            Console.WriteLine("ID: " + item.Id +" "+ "Name: " + item.Name);
        }
    }
    bool x = true;
    int id;
    do {
        Console.WriteLine("enter id ");
        string s = Console.ReadLine();
        bool isNumeric = int.TryParse(s, out id);
        if (isNumeric == true&&id<=cityCount)
        {
            x = false;
            //Console.WriteLine("да");
        }
        else
        {
            Console.WriteLine("не то");
        }
    }
    while (x==true);

    GetOrdersByDistrict(id);

    Console.WriteLine("y - еще. другое - всё");
    char c = Convert.ToChar(Console.ReadLine());
    if (c == 'y')
    {
        w = true;
    }
    else { w = false; }
}


    void GetOrdersByDistrict(int id)
    {
        using (_DbContext db = new _DbContext())
        {
            var some = db.CityDistricts.Include(o => o.Orders).Where(a => a.Id == id).ToList();
            foreach (CityDistrict dstrckt in some)
            {
                Console.WriteLine($"\n район: {dstrckt.Name}");
                foreach (Order order in dstrckt.Orders.OrderByDescending(s => s.DateTimeOredDelivery))
                {
                    Console.WriteLine("ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + order.DateTimeOredDelivery);
                }
            }
        }
    }

