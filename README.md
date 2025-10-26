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

| Fan N. | Pin | Label |
|--------|-----|-------|
| 1      | 2   | CPU F |
| 2      | 4   | SYS F |
| 3      | 8   | Fan1  |
| 4      | 10  | Fan-2 |
| 5      | 12  | Fan 3 |

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
- Putting a 1K resistor across the PWN pin and GND should prevent the fans from going at full speed when it shouldn't

---

## Use case

- If you want to be able to control dell's fans, you can by hijacking the PWM/(in most cases)Blue wire to the Arduino

---

## 💬 Feedback & Support

If you need help setting up the Arduino plugin, have questions, or want to share feedback:

- Ask questions or start a discussion here:  
  👉 [GitHub Discussions](https://github.com/Yanat000/FanControl.ArduinoFanController/discussions)

- Report confirmed bugs or request features here:  
  👉 [GitHub Issues](https://github.com/Yanat000/FanControl.ArduinoFanController/issues)
