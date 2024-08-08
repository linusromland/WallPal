
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
    use cocoa::foundation::{NSString, NSURL};
    use objc::runtime::{Object, BOOL};
    use objc::{class, msg_send, sel, sel_impl};

    unsafe {
        let workspace: *mut Object = msg_send![class!(NSWorkspace), sharedWorkspace];
        let url = NSURL::fileURLWithPath_(
            nil,
            NSString::alloc(nil).init_str(path),
        );
        let result: BOOL = msg_send![workspace, setDesktopImageURL: url forScreen: nil options: nil];

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