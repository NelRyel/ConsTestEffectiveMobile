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
while (w == true)
{
    using (_DbContext db = new _DbContext())
    {
        var dist = db.CityDistricts.ToList();
        foreach (var item in dist)
        {
            Console.WriteLine("ID: " + item.Id +" "+ "Name: " + item.Name);
        }
    }
    Console.WriteLine( "enter id ");
    int id = Convert.ToInt32(Console.ReadLine());
    GetOrdersByDistrict(id);

    Console.WriteLine( "y - еще. другое - всё" );
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
            foreach (CityDistrict comp in some)
            {
                Console.WriteLine($"\n район: {comp.Name}");
                foreach (Order order in comp.Orders.OrderByDescending(s => s.DateTimeOredDelivery))
                {
                    Console.WriteLine("ID: " + order.Id + " " + "Вес заказа: " + order.Weight + " " + "Дата заказа: " + order.DateTimeOredDelivery);
                }
            }
        }
    }

