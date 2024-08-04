package util

import (
	"fmt"
	"os"
	"path/filepath"
	"runtime"
	"strings"
	"time"
)

func getAppDir() (string, error) {
	var appDir string

	switch runtime.GOOS {
	case "windows":
		appDir = filepath.Join(os.Getenv("USERPROFILE"), "Documents", APP_NAME)
	case "darwin", "linux":
		homeDir, err := os.UserHomeDir()
		if err != nil {
			return "", err
		}
		appDir = filepath.Join(homeDir, fmt.Sprintf(".%s", strings.ToLower(APP_NAME)))
	default:
		return "", fmt.Errorf("unsupported platform")
	}

	err := createPathRecursiveIfNeeded(appDir)
	if err != nil {
		return "", err
	}

	return appDir, nil
}

func createPathRecursiveIfNeeded(path string) error {
	if _, err := os.Stat(path); os.IsNotExist(err) {
		err := os.MkdirAll(path, 0755)
		if err != nil {
			return err
		}

		return nil
	}

	return nil
}

func GetImagePath() (string, error) {
	appDir, err := getAppDir()
	if err != nil {
		return "", err
	}

	wallpaperDir := filepath.Join(appDir, WALLPAPER_DIRECTORY)

	err = createPathRecursiveIfNeeded(wallpaperDir)
	if err != nil {
		return "", err
	}

	var wallpaperFileName string;

	if KEEP_OLD_WALLPAPERS {
		wallpaperFileName = fmt.Sprintf("%s_%s%s", WALLPAPER_FILE_NAME, time.Now().Format("2006_01_02T15_04_0500"), WALLPAPER_FILE_EXT)
	} else {
		wallpaperFileName = fmt.Sprintf("%s%s", WALLPAPER_FILE_NAME, WALLPAPER_FILE_EXT)
	}

	return filepath.Join(wallpaperDir, wallpaperFileName), nil
}

func GetDatabasePath() (string, error) {
	appDir, err := getAppDir()
	if err != nil {
		return "", err
	}

	databaseDir := filepath.Join(appDir, DATABASE_DIRECTORY)

	err = createPathRecursiveIfNeeded(databaseDir)
	if err != nil {
		return "", err
	}

	return filepath.Join(databaseDir, DATABASE_FILE_NAME), nil
}
