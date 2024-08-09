use libloading::{Library, Symbol};
use plugin_interface::{PluginCreateFn, WallpaperPlugin};
use std::collections::HashMap;
use std::fs;
use std::path::PathBuf;

pub struct PluginManager {
    plugins: HashMap<String, *mut dyn WallpaperPlugin>,
}

impl PluginManager {
    pub fn new() -> Self {
        PluginManager {
            plugins: HashMap::new(),
        }
    }

    pub fn load_plugins(&mut self, plugins_dir: &str) -> Result<(), Box<dyn std::error::Error>> {
        for entry in fs::read_dir(plugins_dir)? {
            let entry = entry?;
            let path = entry.path();
            println!("Trying to load plugin from path: {:?}", path);
            if path.extension().and_then(|s| s.to_str()) == Some("dll") {
                println!("Loading plugin from path: {:?}", path);
                self.load_plugin(path)?;
                println!("Plugin loaded successfully.");
            }
        }
        Ok(())
    }

    fn load_plugin(&mut self, path: PathBuf) -> Result<(), Box<dyn std::error::Error>> {
        println!("Loading plugin from path: {:?}", path);
        let lib = match unsafe { Library::new(&path) } {
            Ok(l) => l,
            Err(e) => {
                eprintln!("Failed to load library from path: {:?}, Error: {}", path, e);
                return Err(Box::new(e));
            }
        };
        println!("Library loaded successfully.");

        unsafe {
            let func: Symbol<PluginCreateFn> = lib.get(b"create_plugin")?;
            println!("Symbol 'create_plugin' retrieved successfully.");
            let plugin = func();
            if plugin.is_null() {
                eprintln!("Plugin creation function returned a null pointer.");
                return Err("Plugin creation function returned null".into());
            }
            println!("Plugin created successfully.");

            self.plugins.insert("plugin_name".to_string(), plugin);
        }
        Ok(())
    }

    pub fn get_plugin(&self, name: &str) -> Option<&dyn WallpaperPlugin> {
        self.plugins.get(name).map(|&ptr| unsafe { &*ptr })
    }
}

impl Drop for PluginManager {
    fn drop(&mut self) {
        for &plugin_ptr in self.plugins.values() {
            unsafe {
                if !plugin_ptr.is_null() {
                    Box::from_raw(plugin_ptr);
                }
            }
        }
    }
}
