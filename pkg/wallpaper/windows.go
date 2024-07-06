package wallpaper

import (
	"golang.org/x/sys/windows/registry"
	"os/exec"
	"syscall"
)

type WindowsWallpaperChanger struct{}

func init() {
	RegisterChanger(&WindowsWallpaperChanger{})
}

func (w *WindowsWallpaperChanger) SetWallpaper(path string) error {
	key, err := registry.OpenKey(registry.CURRENT_USER, `Control Panel\Desktop`, registry.SET_VALUE)
	if err != nil {
		return err
	}
	defer func(key registry.Key) {
		err := key.Close()
		if err != nil {

		}
	}(key)

	err = key.SetStringValue("Wallpaper", path)
	if err != nil {
		return err
	}

	cmd := exec.Command("RUNDLL32.EXE", "user32.dll,UpdatePerUserSystemParameters")
	cmd.SysProcAttr = &syscall.SysProcAttr{HideWindow: true}
	return cmd.Run()
}
