namespace CountriesMVVM.Services.Sensors
{
    public class MauiLocationService : ILocationService
    {
        private readonly IPermissionService permissionService;

        public MauiLocationService(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        public async Task<Location?> GetCurrentLocationAsync()
        {
            if (!await permissionService.EnsureGrantedAsync<Permissions.LocationWhenInUse>())
                throw new InvalidOperationException("Permiso de ubicación denegado.");

            return await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });
        }
    }
}
