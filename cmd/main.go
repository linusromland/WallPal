package main

import (
	plugins "WallPal/pkg/plugins"
	"WallPal/pkg/util"
	"WallPal/pkg/wallpaper"
	"log"
)

func main() {

	pm := plugins.NewPluginManager()

	pluginDirectory, error := util.GetPluginsPath();
	if error != nil {
		log.Fatalf("Failed to get plugins path: %v", error)
		return;
	}

	error = pm.LoadPlugins(pluginDirectory)

	integration, exists := pm.GetPlugin("trafikverket")
	if !exists {
		log.Fatalf("Failed to get plugin: %v", error)
		return;
	}

	downloadPath, error := util.GetImagePath()
	if error != nil {
		log.Fatalf("failed to get image path: %w", error)
		return;

	}

	image, error := integration.Fetch()
	if error != nil {
		log.Fatalf("Failed to fetch image: %v", error)
		return;
	}

	error = util.DownloadImage(image, downloadPath)
	if error != nil {
		log.Fatalf("Failed to download image: %v", error)
		return;
	}

	error = wallpaper.SetWallpaper(downloadPath)
	if error != nil {
		log.Fatalf("Failed to set wallpaper: %v", error)
	}
}
