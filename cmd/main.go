package main

import (
	"WallPal/pkg/integrations"
	"WallPal/pkg/wallpaper"
	"log"
)

func main() {
	integration, err := integrations.NewIntegration("trafikverket")
	if err != nil {
		log.Fatalf("Failed to create integration: %v", err)
	}

	path, err := integration.Fetch()

	err = wallpaper.SetWallpaper(path)
	if err != nil {
		log.Fatalf("Failed to set wallpaper: %v", err)
	}
}
