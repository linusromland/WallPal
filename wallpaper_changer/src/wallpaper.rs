#[cfg(target_os = "windows")]
pub fn set_wallpaper(path: &str) -> Result<(), Box<dyn std::error::Error>> {
    use std::ptr::null_mut;
    use winapi::um::winuser::{SystemParametersInfoW, SPI_SETDESKWALLPAPER};
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;

    let wide: Vec<u16> = OsStr::new(path).encode_wide().chain(Some(0)).collect();

    unsafe {
        if SystemParametersInfoW(SPI_SETDESKWALLPAPER, 0, wide.as_ptr() as *mut _, 0) == 0 {
            return Err("Failed to set wallpaper on Windows".into());
        }
    }
    Ok(())
}

#[cfg(target_os = "macos")]
pub fn set_wallpaper(path: &str) -> Result<(), Box<dyn std::error::Error>> {
    use cocoa::base::{nil, YES};
    use cocoa::foundation::{NSURL, NSString};
    use objc::runtime::{Class, Object, BOOL};
    use objc::{msg_send, sel, sel_impl};
    use std::path::Path;
    use std::ffi::CString;

    // Check if the file exists before trying to set it as wallpaper
    if !Path::new(path).exists() {
        return Err("File does not exist".into());
    }

    let nsurl = unsafe {
        let nsstring = NSString::alloc(nil).init_str(path);
        NSURL::fileURLWithPath_(nil, nsstring)
    };

    unsafe {
        let workspace: *mut Object = msg_send![Class::get("NSWorkspace").unwrap(), sharedWorkspace];
        let screen: *mut Object = nil; // nil for all screens
        let options: *mut Object = nil; // nil for default options

        let result: BOOL = msg_send![workspace, setDesktopImageURL: nsurl forScreen: screen options: options];

        if result == YES {
            Ok(())
        } else {
            Err("Failed to set wallpaper on macOS".into())
        }
    }
}

#[cfg(not(any(target_os = "windows", target_os = "macos")))]
pub fn set_wallpaper(_path: &str) -> Result<(), Box<dyn std::error::Error>> {
    Err("Unsupported platform".into())
}