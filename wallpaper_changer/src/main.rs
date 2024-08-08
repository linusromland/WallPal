mod wallpaper;

use std::process;

fn main() {
    if let Err(e) = wallpaper::set_wallpaper("/Users/linusromland/Downloads/wallpaper.webp") {
        eprintln!("Error setting wallpaper: {}", e);
        process::exit(1);
    }

    println!("Wallpaper set successfully!");
}