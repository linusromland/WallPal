echo "Building plugin_sample for Windows"
cargo build
echo "Plugin built. Copying to wallpaper_changer"
cp target/debug/plugin_sample.dll ../../wallpaper_changer/src/plugins/plugin_sample.dll
echo "Plugin copied. Done"