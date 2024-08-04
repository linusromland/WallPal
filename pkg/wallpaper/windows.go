//go:build windows

package wallpaper

import (
	"fmt"
	"os/exec"
	"syscall"

	"golang.org/x/sys/windows/registry"
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
			fmt.Println("Failed to close registry key:", err)
		}
	}(key)

	err = key.SetStringValue("Wallpaper", path)
	if err != nil {
		return err
	}

	fmt.Println("Wallpaper set to:", path)


	cmd := exec.Command("RUNDLL32.EXE", "user32.dll,UpdatePerUserSystemParameters")
	cmd.SysProcAttr = &syscall.SysProcAttr{HideWindow: true}
	return cmd.Run()
}
