mod wallpaper;

use std::process;

fn main() {
    if let Err(e) = wallpaper::set_wallpaper("C:\\Users\\hello\\Pictures\\Screenshots\\Screenshot 2024-05-25 103801.png") {
        eprintln!("Error setting wallpaper: {}", e);
        process::exit(1);
    }

    println!("Wallpaper set successfully!");
}