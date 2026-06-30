namespace CountriesMVVM.Services.Sensors
{
    public class MauiPermissionService : IPermissionService
    {
        public async Task<PermissionStatus> RequestAsync<TPermission>()
            where TPermission : Permissions.BasePermission, new()
        {
            var status = await Permissions.CheckStatusAsync<TPermission>();
            if (status == PermissionStatus.Granted)
                return status;

            return await Permissions.RequestAsync<TPermission>();
        }

        public async Task<bool> EnsureGrantedAsync<TPermission>()
            where TPermission : Permissions.BasePermission, new()
        {
            var status = await RequestAsync<TPermission>();
            return status == PermissionStatus.Granted;
        }
    }
}
