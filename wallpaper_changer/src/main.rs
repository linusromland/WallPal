// mod wallpaper;

// mod plugin_manager;
// use plugin_manager::PluginManager;

// fn main() {
//     let mut plugin_manager = PluginManager::new();

//     // TODO: Dynamic path (path from some util thingy that gets it from the home/documents dir)
//     if let Err(e) = plugin_manager
//         .load_plugins("C:\\Users\\hello\\Documents\\Git\\WallPal\\wallpaper_changer\\src\\plugins")
//     {
//         eprintln!("Failed to load plugins: {}", e);
//         return;
//     }

//     println!("Loaded plugins!");

//     if let Some(plugin) = plugin_manager.get_plugin("plugin_sample") {
//         match plugin.get_image() {
//             Ok(_image) => println!("Successfully retrieved image from plugin."),
//             Err(e) => eprintln!("Failed to get image from plugin: {}", e),
//         }
//     } else {
//         eprintln!("Plugin not found.");
//     }
// }

use libloading::{Library, Symbol};
use plugin_interface::{PluginCreateFn, WallpaperPlugin};
use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    // Replace with the path to your compiled plugin
    let path = "C:\\Users\\hello\\Documents\\Git\\WallPal\\plugins\\plugin_sample\\target\\debug\\plugin_sample.dll"; // On Windows: "target/release/simple_plugin.dll"

    // Load the library
    let lib = unsafe { Library::new(path)? };

    // Load the 'create_plugin' function
    let create_plugin: Symbol<PluginCreateFn> = unsafe { lib.get(b"create_plugin")? };

    // Create the plugin
    let plugin_ptr = create_plugin();
    if plugin_ptr.is_null() {
        return Err("Failed to create plugin: returned null pointer".into());
    }

    // Safely use the plugin
    let plugin: &dyn WallpaperPlugin = unsafe { &*plugin_ptr };
    let image = plugin.get_image()?;
    println!("Successfully retrieved image with {} bytes", image.len());

    Ok(())
}

