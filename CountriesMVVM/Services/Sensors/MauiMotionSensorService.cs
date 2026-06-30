namespace CountriesMVVM.Services.Sensors
{
    public class MauiMotionSensorService : IMotionSensorService
    {
        private Action<Vector3Reading>? accelerometerHandler;
        private Action<Vector3Reading>? gyroscopeHandler;

        public bool IsAccelerometerAvailable => Accelerometer.Default.IsSupported;
        public bool IsGyroscopeAvailable => Gyroscope.Default.IsSupported;

        public void Start(Action<Vector3Reading> onAccelerometer, Action<Vector3Reading> onGyroscope)
        {
            accelerometerHandler = onAccelerometer;
            gyroscopeHandler = onGyroscope;

            if (IsAccelerometerAvailable)
            {
                Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }

            if (IsGyroscopeAvailable)
            {
                Gyroscope.Default.ReadingChanged += OnGyroscopeReadingChanged;
                Gyroscope.Default.Start(SensorSpeed.UI);
            }
        }

        public void Stop()
        {
            if (IsAccelerometerAvailable)
            {
                Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
                Accelerometer.Default.Stop();
            }

            if (IsGyroscopeAvailable)
            {
                Gyroscope.Default.ReadingChanged -= OnGyroscopeReadingChanged;
                Gyroscope.Default.Stop();
            }

            accelerometerHandler = null;
            gyroscopeHandler = null;
        }

        private void OnAccelerometerReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            accelerometerHandler?.Invoke(new Vector3Reading(data.Acceleration.X, data.Acceleration.Y, data.Acceleration.Z));
        }

        private void OnGyroscopeReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading;
            gyroscopeHandler?.Invoke(new Vector3Reading(data.AngularVelocity.X, data.AngularVelocity.Y, data.AngularVelocity.Z));
        }
    }
}
