namespace CountriesMVVM.Services.Sensors
{
    public record Vector3Reading(double X, double Y, double Z);

    public interface IMotionSensorService
    {
        bool IsAccelerometerAvailable { get; }
        bool IsGyroscopeAvailable { get; }
        void Start(Action<Vector3Reading> onAccelerometer, Action<Vector3Reading> onGyroscope);
        void Stop();
    }
}
