using Newtonsoft.Json;

using Microsoft.Extensions.Logging;
namespace IntegrationTest;

/// <summary>
/// VehicleBase / VehicleSmallCar / VehicleLargeCar 的 TPH 繼承查詢:
/// 全表查詢、<c>OfType&lt;T&gt;</c> 子型別過濾、JSON 序列化反映多型結果。
/// </summary>
[TestClass]
public class TestVehicle : BaseTest
{
    [TestMethod]
    public async Task Query()
    {
        {
            var r = await _dbContext.VehicleBase.ToListAsync();
            _logger.LogInformation("{Message}", JsonConvert.SerializeObject(r, Formatting.Indented));
        }
        {
            var r = await _dbContext.VehicleBase.OfType<VehicleSmallCar>().ToListAsync();
            _logger.LogInformation("{Message}", JsonConvert.SerializeObject(r, Formatting.Indented));

        }
        {
            var r = await _dbContext.VehicleBase.OfType<VehicleLargeCar>().ToListAsync();
            _logger.LogInformation("{Message}", JsonConvert.SerializeObject(r, Formatting.Indented));
        }
        {
            var r = await _dbContext.VehicleSmallCar.ToListAsync();
            _logger.LogInformation("{Message}", JsonConvert.SerializeObject(r, Formatting.Indented));

        }
        {
            var r = await _dbContext.VehicleLargeCar.ToListAsync();
            _logger.LogInformation("{Message}", JsonConvert.SerializeObject(r, Formatting.Indented));
        }
    }

    [TestMethod]
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
