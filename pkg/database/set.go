package database

import "fmt"

func Set(dbPath, key, value string) error {
	db, err := getDB(dbPath)
	if err != nil {
		return fmt.Errorf("failed to get database: %v", err)
	}

	_, err = db.Exec(`INSERT INTO kv_store (key, value) 
		VALUES (?, ?) 
		ON CONFLICT(key) 
		DO UPDATE SET value=excluded.value`, key, value)

	return err
}