#!/bin/bash

# Define project directories
PLUGINS=("SamplePlugin" "TrafficCameraProxy")
PLUGIN_DIR="Plugins"
MAINAPP_NAME="WallPal"
OUTPUT_DIR="release"

if [ -d "$OUTPUT_DIR" ]; then
    rm -rf $OUTPUT_DIR/*
else
    mkdir $OUTPUT_DIR
fi

# Build the main app
cd $MAINAPP_NAME
dotnet publish -c Release -o ../$OUTPUT_DIR/app
cd ..

# Build the plugins
for PLUGIN in "${PLUGINS[@]}"
do
    cd $PLUGIN_DIR
    cd $PLUGIN
    dotnet build -c Release
    if [ $? -ne 0 ]; then
        echo "Build failed."
        exit 1
    fi

    # create the plugins directory if it doesn't exist
    mkdir -p ../../$OUTPUT_DIR/plugins

    # Move the dll to the release folder
    cp bin/Release/net8.0/$PLUGIN.dll ../../$OUTPUT_DIR/plugins/$PLUGIN.dll


    cd ../..
done

# Build the NSIS installer

"C:\Program Files (x86)\NSIS\makensis.exe" "windows_setup.nsi"
if [ $? -ne 0 ]; then
    echo "NSIS installer creation failed."
    exit 1
fi

mv wallpal_installer.exe $OUTPUT_DIR