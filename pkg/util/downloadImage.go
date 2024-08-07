package util

import (
	"fmt"
	"image"
	"image/gif"
	"image/jpeg"
	"image/png"
	"net/http"
	"os"
	"path/filepath"
	"strings"
)

func DownloadImage(photoURL string, downloadURL string) error {
	fileExtension := strings.ToLower(filepath.Ext(downloadURL))
	if fileExtension == "" {
		return fmt.Errorf("no file extension found in download URL")
	}

	resp, err := http.Get(photoURL)
	if err != nil {
		return fmt.Errorf("failed to download image: %v", err)
	}
	defer resp.Body.Close()

	img, _, err := image.Decode(resp.Body)
	if err != nil {
		return fmt.Errorf("failed to decode image: %v", err)
	}

	out, err := os.Create(downloadURL)
	if err != nil {
		return fmt.Errorf("failed to create file: %v", err)
	}
	defer out.Close()

	switch fileExtension {
	case ".jpeg", ".jpg":
		err = jpeg.Encode(out, img, nil)
	case ".png":
		err = png.Encode(out, img)
	case ".gif":
		err = gif.Encode(out, img, nil)
	default:
		return fmt.Errorf("unsupported file extension: %s", fileExtension)
	}

	if err != nil {
		return fmt.Errorf("failed to encode image: %v", err)
	}

	return nil
}
