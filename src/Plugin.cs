using System;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Threading;
using FanControl.Plugins;

namespace FanControl.ArduinoInterface
{
    public class ArduinoPlugin : IPlugin, IDisposable
    {
        private SerialPort _serial;
        private ArduinoFanControlSensor[] _fans;
        private Timer _reconnectTimer;

        public string Name => "Arduino Auto Fan Controller";

        public void Initialize()
        {
            DetectAndConnectArduino();
            _reconnectTimer = new Timer(_ => CheckReconnect(), null, 5000, 5000);
        }

        public void Load(IPluginSensorsContainer container)
        {
            if (_fans == null) return;
            foreach (var fan in _fans)
                container.ControlSensors.Add(fan);
        }

        private void DetectAndConnectArduino()
        {
            string port = ReadArduinoPortFromFile();
            if (port == null)
            {
                _serial = null;
                return;
            }

            try
            {
                _serial?.Close();
                _serial = new SerialPort(port, 115200) { DtrEnable = true, NewLine = "\n" };
                _serial.Open();
                Thread.Sleep(2000); // allow Arduino to reset
                _serial.DiscardInBuffer();

                // Get number of fans
                int fanCount = 0;
                _serial.WriteLine("NUM_FANS");
                Thread.Sleep(200);
                if (int.TryParse(_serial.ReadExisting().Trim(), out int n))
                    fanCount = n;

                // Get fan names
                string[] fanNames = new string[fanCount];
                _serial.WriteLine("FAN_NAMES");
                Thread.Sleep(200);
                string[] lines = _serial.ReadExisting().Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fanCount && i < lines.Length; i++)
                    fanNames[i] = lines[i];

                // Create fan sensors dynamically
                _fans = new ArduinoFanControlSensor[fanCount];
                for (int i = 0; i < fanCount; i++)
                {
                    _fans[i] = new ArduinoFanControlSensor(
                        $"arduino_fan_{i + 1}",
                        fanNames[i],
                        i + 1,
                        () => _serial
                    );
                    _fans[i].Set(30); // default 30%
                }
            }
            catch
            {
                _serial = null;
            }
        }

        private string ReadArduinoPortFromFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins/arduino_port.txt");
            if (!File.Exists(filePath))
                return null;

            string port = File.ReadAllText(filePath).Trim();
            if (string.IsNullOrEmpty(port))
                return null;

            return port;
        }

        private void CheckReconnect()
        {
            if (_serial == null || !_serial.IsOpen)
                DetectAndConnectArduino();
        }

        public void Close()
        {
            _reconnectTimer?.Dispose();
            if (_serial?.IsOpen == true) _serial.Close();
        }

        public void Dispose() => Close();
    }

    public class ArduinoFanControlSensor : IPluginControlSensor
    {
        public string Id { get; }
        public string Name { get; }
        public float? Value { get; private set; }

        private readonly int _fanNumber;
        private readonly Func<SerialPort> _getSerial;

        public ArduinoFanControlSensor(string id, string name, int fanNumber, Func<SerialPort> getSerial)
        {
            Id = id;
            Name = name;
            _fanNumber = fanNumber;
            _getSerial = getSerial;
            Value = 30;
        }

        public void Reset() => Set(30);

        public void Update() { }

        public void Set(float val)
        {
            Value = val;
            var serial = _getSerial();
            if (serial == null || !serial.IsOpen) return;
            try
            {
                int pwm = (int)(val * 255 / 100f);
                serial.WriteLine($"SET_FAN:{_fanNumber}:{pwm}");
            }
            catch { }
        }
    }
}
