use plugin_interface::WallpaperPlugin;
use std::fs::File;
use std::io::Read;

struct SamplePlugin;

impl WallpaperPlugin for SamplePlugin {
    fn get_image(&self) -> Result<Vec<u8>, Box<dyn std::error::Error>> {
        Ok(vec![0, 1, 2, 3, 4]) // Dummy data
    }
}

#[no_mangle]
pub extern "C" fn create_plugin() -> *mut dyn WallpaperPlugin {
    println!("Creating plugin...");

    println!("Image is: {:?}", SamplePlugin.get_image(&SamplePlugin));

    let plugin = Box::new(SamplePlugin);

    println!("Plugin created with pointer: {:?}", &plugin as *const _);

    let plugin_ptr = Box::into_raw(plugin);

    println!("plugin created with pointer: {:?}", plugin_ptr);

    plugin_ptr
}

// use plugin_interface::WallpaperPlugin;

// struct SamplePlugin;

// impl WallpaperPlugin for SamplePlugin {
//     fn get_image(&self) -> Result<Vec<u8>, Box<dyn std::error::Error>> {
//         Ok(Vec::new()) // Return empty image data
//     }
// }

// // #[no_mangle]
// // pub extern "C" fn create_plugin() -> *mut dyn WallpaperPlugin {
// //     Box::into_raw(Box::new(SamplePlugin)) as *mut dyn WallpaperPlugin
// // }

// #[no_mangle]
// pub extern "C" fn create_plugin() -> Box<dyn WallpaperPlugin> {
//     Box::new(SamplePlugin)
// }
