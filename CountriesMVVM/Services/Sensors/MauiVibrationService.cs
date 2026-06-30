namespace CountriesMVVM.Services.Sensors
{
    public class MauiVibrationService : IVibrationService
    {
        public void Vibrate(TimeSpan duration)
        {
            if (!Vibration.Default.IsSupported)
                return;

            Vibration.Default.Vibrate(duration);
        }
    }
}
