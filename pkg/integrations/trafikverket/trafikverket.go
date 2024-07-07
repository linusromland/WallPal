package trafikverket

import (
	trafikverketTypes "WallPal/pkg/integrations/trafikverket/types"
	"WallPal/pkg/integrations/types"
	"bytes"
	"encoding/json"
	"encoding/xml"
	"fmt"
	"io/ioutil"
	"net/http"
)

type TrafikverketIntegration struct {
}

func NewTrafikverketIntegration() types.Integration {
	return &TrafikverketIntegration{}
}

func (t *TrafikverketIntegration) Fetch() (string, error) {
	fmt.Println("Fetching data from Trafikverket")
	request := trafikverketTypes.Request{
		Login: trafikverketTypes.Login{
			AuthenticationKey: "96c58a7582f14b9b8f232e8bcf1b96e1",
		},
		Query: trafikverketTypes.Query{
			ObjectType:    "Camera",
			SchemaVersion: "1",
			Limit:         10,
			Filter: trafikverketTypes.Filter{
				Eq: trafikverketTypes.Eq{
					Name:  "Name",
					Value: "Åbromotet Norra norrut",
				},
			},
		},
	}

	var response trafikverketTypes.Response
	err := SendRequest(request, &response)
	if err != nil {
		return "", fmt.Errorf("failed to send request: %w", err)
	}

	if len(response.ResponseBody.Result) == 0 {
		return "", fmt.Errorf("no results found")
	}

	cameraResponse := response.ResponseBody.Result[0].Camera[0]
	photoUrl := cameraResponse.PhotoUrl
	if cameraResponse.HasFullSizePhoto {
		photoUrl += "?type=fullsize"
	}
	fmt.Println("Photo URL found:", photoUrl)

}

func SendRequest[T any](requestBody interface{}, responseBody *T) error {
	xmlData, err := xml.Marshal(requestBody)
	if err != nil {
		return fmt.Errorf("failed to marshal XML: %w", err)
	}

	req, err := http.NewRequest("POST", "https://api.trafikinfo.trafikverket.se/v2/data.json", bytes.NewBuffer(xmlData))
	if err != nil {
		return fmt.Errorf("failed to create request: %w", err)
	}
	req.Header.Set("Content-Type", "application/xml")

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return fmt.Errorf("failed to send request: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return fmt.Errorf("failed to read response body: %w", err)
	}

	err = json.Unmarshal(body, responseBody)
	if err != nil {
		return fmt.Errorf("failed to unmarshal JSON: %w", err)
	}

	return nil
}
