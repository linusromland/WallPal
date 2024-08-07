package integrations

import (
	types "WallPal/pkg/plugins/types"
	"io/ioutil"
	"log"
	"path/filepath"
	"plugin"
)

type PluginManager struct {
    plugins map[string]types.Plugin
}

func NewPluginManager() *PluginManager {
    return &PluginManager{
        plugins: make(map[string]types.Plugin),
    }
}

func (pm *PluginManager) LoadPlugins(dir string) error {
    files, err := ioutil.ReadDir(dir)
    if err != nil {
        log.Fatalf("could not read plugin directory: %v", err)
        return err
    }

    if len(files) == 0 {
        log.Fatalf("no plugins found in directory: %s", dir)
        return nil
    }

    for _, file := range files {
        if filepath.Ext(file.Name()) == ".so" {
            path := filepath.Join(dir, file.Name())
            p, err := plugin.Open(path)
            if err != nil {
                log.Fatalf("Error loading plugin %s: %v", file.Name(), err)
                continue
            }

            symbol, err := p.Lookup("PluginInstance")
            if err != nil {
                log.Fatalf("Error finding PluginInstance in %s: %v", file.Name(), err)
                continue
            }

            plug, ok := symbol.(types.Plugin)
            if !ok {
                log.Fatalf("Plugin %s does not implement the required interface", file.Name())
                continue
            }

            err = plug.Init()
            if err != nil {
                log.Fatalf("Failed to initialize plugin %s: %v", file.Name(), err)
                continue
            }

            if plug.Ready() {
                pm.plugins[plug.GetName()] = plug
                log.Fatalf("Loaded plugin: %s", plug.GetName())
            } else {
                log.Fatalf("Plugin %s is not ready", plug.GetName())
            }
        }
    }
    return nil
}


func (pm *PluginManager) GetPlugin(name string) (types.Plugin, bool) {
    plug, exists := pm.plugins[name]
    return plug, exists
}