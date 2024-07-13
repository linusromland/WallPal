package main

import (
	"WallPal/pkg/integrations"
	"WallPal/pkg/wallpaper"
	"WallPal/ui"
	"log"
)

func main() {
	// Initialize the UI
	ui.InitUI()
 
	// Just temporary code to test the integration
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
