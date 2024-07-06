package main

import (
	"WallPal/pkg/wallpaper"
	"log"
)

func main() {
	err := wallpaper.SetWallpaper("C:\\Users\\hello\\AppData\\Roaming\\.minecraft\\screenshots\\2024-06-17_18.00.36.png")
	if err != nil {
		log.Fatalf("Failed to set wallpaper: %v", err)
	}
}
