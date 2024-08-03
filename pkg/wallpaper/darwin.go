//go:build darwin

package wallpaper

import (
	"os/exec"
)

type MacOSWallpaperChanger struct{}

func init() {
	RegisterChanger(&MacOSWallpaperChanger{})
}

func (m *MacOSWallpaperChanger) SetWallpaper(path string) error {
	cmd := exec.Command("osascript", "-e", `tell application "Finder" to set desktop picture to POSIX file "`+path+`"`)
	return cmd.Run()
}
