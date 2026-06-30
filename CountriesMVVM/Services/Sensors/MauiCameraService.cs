namespace CountriesMVVM.Services.Sensors
{
    public class MauiCameraService : ICameraService
    {
        private readonly IPermissionService permissionService;

        public MauiCameraService(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        public async Task<string?> CapturePhotoAsync(string fileName)
        {
            if (!MediaPicker.Default.IsCaptureSupported)
                throw new InvalidOperationException("La cámara no está disponible en este dispositivo.");

            if (!await permissionService.EnsureGrantedAsync<Permissions.Camera>())
                throw new InvalidOperationException("Permiso de cámara denegado.");

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null)
                return null;

            var destino = Path.Combine(FileSystem.CacheDirectory, fileName);
            await using var origen = await photo.OpenReadAsync();
            await using var destinoStream = File.Create(destino);
            await origen.CopyToAsync(destinoStream);

            return destino;
        }
    }
}
