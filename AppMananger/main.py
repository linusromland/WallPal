import subprocess
import os
import platform
import sys
from pystray import Icon as icon, Menu as menu, MenuItem as item
from PIL import Image
import portalocker

WALLPAL_SERVICE_EXE = "C:\\Program Files (x86)\\WallPal\\WallPal.exe"

def start_service():
    image = Image.open("icon.png")

    print("Starting WallPal Service...")
    api_pid = subprocess.Popen([WALLPAL_SERVICE_EXE])
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

    state = False
    def handle_autoboot(icon, item):
        nonlocal state
        state = not item.checked

    def handle_exit(icon):
        print("Exiting WallPal Service...")
        api_pid.kill()
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

def main():
    lock_file = single_instance_check()

    start_service()

    lock_file.close()
    os.remove(lock_file.name)


if __name__ == "__main__":
    main()