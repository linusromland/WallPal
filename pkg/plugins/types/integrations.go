package types

import "image"



type Plugin interface {
	Init() error
	Ready() bool
	Fetch() (image.Image, error)
	GetName() string
}
