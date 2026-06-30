using System.Windows.Input;
using CountriesMVVM.Commands;
using CountriesMVVM.Services.Sensors;

namespace CountriesMVVM.ViewModels
{
    [QueryProperty(nameof(NombrePais), "nombrePais")]
    [QueryProperty(nameof(CapitalPais), "capitalPais")]
    [QueryProperty(nameof(MonedaPais), "monedaPais")]
    public class CountryDetailViewModel : BaseViewModel
    {
        private const double ShakeThreshold = 2.7;
        private readonly ILocationService locationService;
        private readonly ICameraService cameraService;
        private readonly IMotionSensorService motionSensorService;
        private readonly IVibrationService vibrationService;
        private DateTime ultimaVibracion = DateTime.MinValue;

        private string nombrePais = string.Empty;
        public string NombrePais { get => nombrePais; set => SetProperty(ref nombrePais, Uri.UnescapeDataString(value ?? string.Empty)); }

        private string capitalPais = string.Empty;
        public string CapitalPais { get => capitalPais; set => SetProperty(ref capitalPais, Uri.UnescapeDataString(value ?? string.Empty)); }

        private string monedaPais = string.Empty;
        public string MonedaPais { get => monedaPais; set => SetProperty(ref monedaPais, Uri.UnescapeDataString(value ?? string.Empty)); }

        private string ubicacionGps = "Sin datos de ubicación";
        public string UbicacionGps
        {
            get => ubicacionGps;
            set => SetProperty(ref ubicacionGps, value);
        }

        private string datosAcelerometro = "Acelerómetro: sin lecturas";
        public string DatosAcelerometro
        {
            get => datosAcelerometro;
            set => SetProperty(ref datosAcelerometro, value);
        }

        private string datosGiroscopio = "Giroscopio: sin lecturas";
        public string DatosGiroscopio
        {
            get => datosGiroscopio;
            set => SetProperty(ref datosGiroscopio, value);
        }

        private string rutaFoto = "Sin foto capturada";
        public string RutaFoto
        {
            get => rutaFoto;
            set => SetProperty(ref rutaFoto, value);
        }

        private string mensajeSensores = string.Empty;
        public string MensajeSensores
        {
            get => mensajeSensores;
            set => SetProperty(ref mensajeSensores, value);
        }

        public ICommand ObtenerUbicacionCommand { get; }
        public ICommand CapturarFotoCommand { get; }

        public CountryDetailViewModel(
            ILocationService locationService,
            ICameraService cameraService,
            IMotionSensorService motionSensorService,
            IVibrationService vibrationService)
        {
            this.locationService = locationService;
            this.cameraService = cameraService;
            this.motionSensorService = motionSensorService;
            this.vibrationService = vibrationService;

            ObtenerUbicacionCommand = new RelayCommand(ObtenerUbicacionAsync);
            CapturarFotoCommand = new RelayCommand(CapturarFotoAsync);
        }

        public void IniciarSensores()
        {
            motionSensorService.Start(OnAccelerometerReading, OnGyroscopeReading);
            MensajeSensores = motionSensorService.IsAccelerometerAvailable || motionSensorService.IsGyroscopeAvailable
                ? "Sensores de movimiento activos. Agitá el dispositivo para vibrar."
                : "Los sensores de movimiento no están disponibles en este dispositivo.";
        }

        public void DetenerSensores()
        {
            motionSensorService.Stop();
        }

        private async Task ObtenerUbicacionAsync()
        {
            try
            {
                MensajeSensores = "Solicitando permiso de ubicación...";
                var ubicacion = await locationService.GetCurrentLocationAsync();

                if (ubicacion is null)
                {
                    UbicacionGps = "No se pudo obtener la ubicación.";
                    return;
                }

                UbicacionGps = $"Lat: {ubicacion.Latitude:F4}, Lon: {ubicacion.Longitude:F4}";
                MensajeSensores = "Ubicación GPS obtenida correctamente.";
                vibrationService.Vibrate(TimeSpan.FromMilliseconds(120));
            }
            catch (Exception ex)
            {
                UbicacionGps = "Ubicación no disponible.";
                MensajeSensores = ex.Message;
            }
        }

        private async Task CapturarFotoAsync()
        {
            try
            {
                MensajeSensores = "Solicitando permiso de cámara...";
                var nombreArchivo = $"pais_{NombrePais.Replace(' ', '_')}_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                var ruta = await cameraService.CapturePhotoAsync(nombreArchivo);

                if (string.IsNullOrWhiteSpace(ruta))
                {
                    MensajeSensores = "Captura de foto cancelada.";
                    return;
                }

                RutaFoto = ruta;
                MensajeSensores = "Foto capturada correctamente.";
                vibrationService.Vibrate(TimeSpan.FromMilliseconds(180));
            }
            catch (Exception ex)
            {
                RutaFoto = "Sin foto capturada";
                MensajeSensores = ex.Message;
            }
        }

        private void OnAccelerometerReading(Vector3Reading lectura)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DatosAcelerometro = $"Acelerómetro → X:{lectura.X:F2} Y:{lectura.Y:F2} Z:{lectura.Z:F2}";

                var magnitud = Math.Sqrt(lectura.X * lectura.X + lectura.Y * lectura.Y + lectura.Z * lectura.Z);
                if (magnitud < ShakeThreshold)
                    return;

                if ((DateTime.UtcNow - ultimaVibracion).TotalMilliseconds < 900)
                    return;

                ultimaVibracion = DateTime.UtcNow;
                vibrationService.Vibrate(TimeSpan.FromMilliseconds(250));
                MensajeSensores = "¡Movimiento detectado! Vibración activada.";
            });
        }

        private void OnGyroscopeReading(Vector3Reading lectura)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DatosGiroscopio = $"Giroscopio → X:{lectura.X:F2} Y:{lectura.Y:F2} Z:{lectura.Z:F2}";
            });
        }
    }
}
