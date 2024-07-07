package types

type Integration interface {
	Fetch() (string, error)
}
