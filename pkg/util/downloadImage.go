package util

import (
	"fmt"
	"image"
	"image/gif"
	"image/jpeg"
	"image/png"
	"os"
	"path/filepath"
	"strings"
)

func DownloadImage(image image.Image, downloadURL string) error {
	fileExtension := strings.ToLower(filepath.Ext(downloadURL))
	if fileExtension == "" {
		return fmt.Errorf("no file extension found in download URL")
	}
	out, err := os.Create(downloadURL)
	if err != nil {
		return fmt.Errorf("failed to create file: %v", err)
	}
	defer out.Close()

	switch fileExtension {
	case ".jpeg", ".jpg":
		err = jpeg.Encode(out, image, nil)
	case ".png":
		err = png.Encode(out, image)
	case ".gif":
		err = gif.Encode(out, image, nil)
	default:
		return fmt.Errorf("unsupported file extension: %s", fileExtension)
	}

	if err != nil {
		return fmt.Errorf("failed to encode image: %v", err)
	}

	return nil
}
