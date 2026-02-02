using Microsoft.EntityFrameworkCore;
using Model.Definitions;
using Model.Entities;
using Newtonsoft.Json;

namespace Test;

[TestClass]
public class TestVehicle : BaseTest
{
    [TestMethod(DisplayName = "Query")]
    public async Task Query()
    {
        {
            var r = await _dbContext.VehicleBase.ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
        }
        {
            var r = await _dbContext.VehicleBase.OfType<VehicleSmallCar>().ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

        }
        {
            var r = await _dbContext.VehicleBase.OfType<VehicleLargeCar>().ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
        }
        {
            var r = await _dbContext.VehicleSmallCar.ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

        }
        {
            var r = await _dbContext.VehicleLargeCar.ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
        }
    }

    [TestMethod(DisplayName = "Create")]
    public async Task Create()
    {
        _dbContext.VehicleBase.Add(new VehicleSmallCar()
        {
            Name = "VehicleCarName",
            CarName = "VehicleSmallCarName",
            SmallCarName = "VehicleSmallCarName",
            CarType = VehicleSmallCarType.Sedan,
            Content = new(Type: VehicleSmallCarType.CompactCar, SmallCarContent: "CompactCar"),
        });
        _dbContext.VehicleBase.Add(new VehicleLargeCar()
        {
            Name = "VehicleCarName",
            CarName = "VehicleLargeCarName",
            LargeCarName = "VehicleLargeCarName",
            CarType = VehicleLargeCarType.Bus,
            Content = new(Type: VehicleLargeCarType.Coach, LargeCarContent: "Coach"),
        });
        await _dbContext.SaveChangesAsync();
        await Query();
    }
}
