package wallpaper

import (
	"errors"
	"fmt"
)

type WallpaperChanger interface {
	SetWallpaper(path string) error
}

var (
	currentChanger   WallpaperChanger
	ErrUnsupportedOS = errors.New("unsupported operating system")
)

func SetWallpaper(path string) error {
	if currentChanger == nil {
		return ErrUnsupportedOS
	}
	return currentChanger.SetWallpaper(path)
}

func RegisterChanger(changer WallpaperChanger) {
	 fmt.Printf("Registering changer: %T\n", changer)
	
	currentChanger = changer
}
