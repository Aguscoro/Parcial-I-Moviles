namespace CountriesMVVM.Services.Sensors
{
    public interface ICameraService
    {
        Task<string?> CapturePhotoAsync(string fileName);
    }
}
