pub trait WallpaperPlugin {
    fn get_image(&self) -> Result<Vec<u8>, Box<dyn std::error::Error>>;
}

pub type PluginCreateFn = fn() -> *mut dyn WallpaperPlugin;
