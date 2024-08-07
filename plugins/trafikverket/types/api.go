package types

import (
	"encoding/xml"
	"time"
)

type Request struct {
	XMLName xml.Name `xml:"REQUEST"`
	Login   Login    `xml:"LOGIN"`
	Query   Query    `xml:"QUERY"`
}

type Login struct {
	AuthenticationKey string `xml:"authenticationkey,attr"`
}

type Query struct {
	ObjectType    string `xml:"objecttype,attr"`
	SchemaVersion string `xml:"schemaversion,attr"`
	Limit         int    `xml:"limit,attr"`
	Filter        Filter `xml:"FILTER"`
}

type Filter struct {
	Eq Eq `xml:"EQ"`
}

type Eq struct {
	Name  string `xml:"name,attr"`
	Value string `xml:"value,attr"`
}

type Response struct {
	ResponseBody ResponseBody `json:"RESPONSE"`
}

type ResponseBody struct {
	Result []Result `json:"RESULT"`
}

type Result struct {
	Camera []Camera `json:"Camera,omitempty"`
	Error  *Error   `json:"ERROR,omitempty"`
}

type Camera struct {
	Active           bool      `json:"Active"`
	ContentType      string    `json:"ContentType"`
	CountyNo         []int     `json:"CountyNo"`
	Deleted          bool      `json:"Deleted"`
	Geometry         Geometry  `json:"Geometry"`
	HasFullSizePhoto bool      `json:"HasFullSizePhoto"`
	HasSketchImage   bool      `json:"HasSketchImage"`
	IconId           string    `json:"IconId"`
	Id               string    `json:"Id"`
	ModifiedTime     time.Time `json:"ModifiedTime"`
	Name             string    `json:"Name"`
	Type             string    `json:"Type"`
	PhotoTime        string    `json:"PhotoTime"` // Keeping as string due to timezone offset
	PhotoUrl         string    `json:"PhotoUrl"`
	Status           string    `json:"Status"`
}

type Geometry struct {
	SWEREF99TM string `json:"SWEREF99TM"`
	WGS84      string `json:"WGS84"`
}

type Error struct {
	Source  string `json:"SOURCE"`
	Message string `json:"MESSAGE"`
}
