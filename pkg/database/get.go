package database

import (
	"database/sql"
	"fmt"
)

func Get(dbPath, key string) (string, error) {
	db, err := getDB(dbPath)
	if err != nil {
		return "", fmt.Errorf("failed to get database: %v", err)
	}

	var value string
	err = db.QueryRow(`SELECT value FROM kv_store WHERE key = ?`, key).Scan(&value)

	if err != nil {
		if err == sql.ErrNoRows {
			return "", nil
		}
		return "", err
	}
	return value, nil
}