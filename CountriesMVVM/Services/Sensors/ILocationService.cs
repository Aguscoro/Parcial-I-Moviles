namespace CountriesMVVM.Services.Sensors
{
    public interface ILocationService
    {
        Task<Location?> GetCurrentLocationAsync();
    }
}
