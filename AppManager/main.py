import subprocess
import os
import platform
import sys
from pystray import Icon as icon, Menu as menu, MenuItem as item
from PIL import Image
import portalocker
import winreg

IS_DEV = False

WALLPAL_DIRECTORY_PROD = "C:\\Program Files (x86)\\WallPal\\"
WALLPAL_DIRECTORY_DEV = "./"
WALLPAL_DIRECTORY = WALLPAL_DIRECTORY_PROD if IS_DEV == False else WALLPAL_DIRECTORY_DEV

WALLPAL_SERVICE_EXE_FILE = "WallPal_Service.exe"
WALLPAL_SERVICE_EXE = WALLPAL_DIRECTORY + WALLPAL_SERVICE_EXE_FILE


def start_service():
    image = Image.open(WALLPAL_DIRECTORY + "icon.png")

    print("Starting WallPal Service...")
    if platform.system() == "Windows":
        creation_flags = subprocess.CREATE_NO_WINDOW
        service_pid = subprocess.Popen([WALLPAL_SERVICE_EXE], creationflags=creation_flags)
    else:
        service_pid = subprocess.Popen([WALLPAL_SERVICE_EXE])
    print("WallPal Service started successfully.")

    def handle_open_config():
        print("Opening WallPal Config...")
        if platform.system() == "Windows":
            config_path = os.path.join(os.path.expanduser("~"), "Documents", "WallPal", "config.json")
            os.startfile(config_path)
        else:
            config_path = os.path.join(os.path.expanduser("~"), ".wallpal", "config.json")

            if platform.system() == "Darwin":
                subprocess.call(('open', config_path))
            elif platform.system() == "Linux":
                subprocess.call(('xdg-open', config_path))

    state = check_autostart()

    def handle_autoboot(icon, item):
        nonlocal state
        state = not item.checked
        set_autostart(state)
        icon.update_menu()

    def handle_exit(icon):
        print("Exiting WallPal Service...")
        service_pid.kill()
        icon.stop()

    icon('WallPal Service', image, menu=menu(
        item('Open Config', handle_open_config),
        item('Autostart on boot', handle_autoboot, checked=lambda item: state),
        item('Exit', handle_exit))).run()

def single_instance_check():
    lock_file_path = os.path.join(
        os.path.expanduser("~"), ".WallPalService.lock")
    lock_file = open(lock_file_path, "w")

    try:
        portalocker.lock(lock_file, portalocker.LOCK_EX | portalocker.LOCK_NB)
    except portalocker.LockException:
        print("Another instance is already running. Exiting.")
        sys.exit(1)

    return lock_file

def set_autostart(enabled):
    key = r"Software\Microsoft\Windows\CurrentVersion\Run"
    try:
        registry_key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, key, 0, winreg.KEY_SET_VALUE)
        if enabled:

            winreg.SetValueEx(registry_key, "WallPalService", 0, winreg.REG_SZ, "C:\\Program Files (x86)\\WallPal\\WallPal.exe")
        else:
            try:
                winreg.DeleteValue(registry_key, "WallPalService")
            except FileNotFoundError:
                pass
    except Exception as e:
        print(f"Failed to set autostart: {e}")
    finally:
        print("Autostart set to", enabled)
        winreg.CloseKey(registry_key)

def check_autostart():
    key = r"Software\Microsoft\Windows\CurrentVersion\Run"
    try:
        registry_key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, key, 0, winreg.KEY_READ)
        try:
            value, _ = winreg.QueryValueEx(registry_key, "WallPalService")
            return True
        except FileNotFoundError:
            return False
    except Exception as e:
        print(f"Failed to check autostart: {e}")
        return False
    finally:
        winreg.CloseKey(registry_key)

def main():
    lock_file = single_instance_check()

    start_service()

    lock_file.close()
    os.remove(lock_file.name)

if __name__ == "__main__":
    main()
