package trafikverket

import (
	"WallPal/pkg/integrations/types"
	"fmt"
)

type TrafikverketIntegration struct {
}

func NewTrafikverketIntegration() types.Integration {
	return &TrafikverketIntegration{}
}

func (t *TrafikverketIntegration) Fetch() (string, error) {
	fmt.Println("Fetching data from Trafikverket")
	return "C:\\Users\\hello\\AppData\\Roaming\\.minecraft\\screenshots\\2024-06-17_18.00.36.png", nil
}
