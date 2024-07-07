package integrations

import (
	"WallPal/pkg/integrations/trafikverket"
	"WallPal/pkg/integrations/types"
	"fmt"
)

func NewIntegration(name string) (types.Integration, error) {
	switch name {
	case "trafikverket":
		return trafikverket.NewTrafikverketIntegration(), nil
	default:
		return nil, fmt.Errorf("unknown integration: %s", name)
	}
}
