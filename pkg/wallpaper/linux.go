//UNTESTED CODE

package wallpaper

import (
	"os/exec"
)

type LinuxWallpaperChanger struct{}

func init() {
	RegisterChanger(&LinuxWallpaperChanger{})
}

func (l *LinuxWallpaperChanger) SetWallpaper(path string) error {
	cmd := exec.Command("feh", "--bg-scale", path)
	return cmd.Run()
}
