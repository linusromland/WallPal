package main

import (
	trafikverketTypes "WallPal/plugins/trafikverket/types"
	"bytes"
	"encoding/json"
	"encoding/xml"
	"fmt"
	"image"
	"io/ioutil"
	"net/http"
)

type TrafikverketPlugin struct {
}

func (p *TrafikverketPlugin) Init() error {
    fmt.Println("Trafikverket plugin initialized")
    return nil
}

func (p *TrafikverketPlugin) GetName() string {
	return "trafikverket"
}

func (p *TrafikverketPlugin) Ready() bool {
    // Temp ready check
    return true
}

func (p *TrafikverketPlugin) Fetch() (image.Image, error) {
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
	err := SendRequest(p, request, &response)
	if err != nil {
		return nil, fmt.Errorf("failed to send request: %w", err)
	}

	if len(response.ResponseBody.Result) == 0 {
		return nil, fmt.Errorf("no results found")
	}

	cameraResponse := response.ResponseBody.Result[0].Camera[0]
	photoUrl := cameraResponse.PhotoUrl
	if cameraResponse.HasFullSizePhoto {
		photoUrl += "?type=fullsize"
	}
	fmt.Println("Photo URL found:", photoUrl)

	resp, err := http.Get(photoUrl)
	if err != nil {
		return nil, fmt.Errorf("failed to download image: %v", err)
	}
	defer resp.Body.Close()

	img, _, err := image.Decode(resp.Body)
	if err != nil {
		return nil, fmt.Errorf("failed to decode image: %v", err)
	}

	return img, nil
}

func SendRequest[T any](p *TrafikverketPlugin, requestBody interface{}, responseBody *T) error {
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


var PluginInstance TrafikverketPlugin
func main() {}
