package ui

import (
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/container"
	"fyne.io/fyne/v2/widget"
)

func InitUI() {
	a := app.New()
	w := a.NewWindow("WallPal")

	btn := widget.NewButton("Change Wallpaper", func() {
		// Logic to change wallpaper
	})

	w.SetContent(container.NewVBox(
		btn,
	))

	w.ShowAndRun()
}
