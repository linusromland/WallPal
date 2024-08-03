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
	exec.Command("/bin/bash", "-c", "/Users/linusromland/Documents/GitHub/WallPal/pkg/wallpaper/darwin/setter.applescript"+" "+path).Run()
	return nil
}