namespace CountriesMVVM.Services.Sensors
{
    public interface IPermissionService
    {
        Task<PermissionStatus> RequestAsync<TPermission>()
            where TPermission : Permissions.BasePermission, new();

        Task<bool> EnsureGrantedAsync<TPermission>()
            where TPermission : Permissions.BasePermission, new();
    }
}
