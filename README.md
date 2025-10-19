# FanControl.ArduinoFanController

**Arduino Fan Controller for FanControl**  

This plugin allows FanControl to manage fans connected to an Arduino via PWM. It supports 1–8 fans (more may work, but untested) and automatically adapts to the number of fans defined in your Arduino sketch. Fans default to **30% speed** when no control input is received.

---

## ⚠️ Use at Your Own Risk
This plugin is highly experimental and has not been fully tested.  
Tested versions: **FanControl V244**.  

---

## Installation

1. **Get the plugin files**
   - Either copy from `bin/` or build from `src/` using:  
     ```bash
     dotnet build -f net48
     ```
     (.NET Framework 4.8 required)

2. **Copy the plugin**
   - Place the plugin files into your FanControl **Plugins** folder.

3. **Set Arduino COM port**
   - Create `arduino_port.txt` in the **Plugins folder**.
   - Add your Arduino COM port, e.g., `COM4`.

---

## Arduino Setup

1. Open the Arduino sketch from `bin/`.  
2. Modify the **pins** at the top to match your setup. Each pin should have its label set in the line immediately below.  
3. Upload the sketch to the Arduino. Fans may spin at full speed briefly during upload.

**Default Pins Example:**

| Fan | Pin |
|-----|-----|
| 1   | 2   |
| 2   | 4   |
| 3   | 8   |
| 4   | 10  |
| 5   | 12  |

---

## Usage

1. Connect your Arduino to the computer.  
2. Open FanControl → Settings → **Install Plugin…** → select `FanControl.ArduinoInterface.dll`.  
3. Restart FanControl.  
4. Your configured fans should now appear in FanControl, ready for PWM control.

---

## Notes

- Supports 1–8 fans; unused pins are ignored automatically.  
- If the Arduino COM port changes, update `arduino_port.txt`.  
- Fans default to **30% speed** when idle.  

