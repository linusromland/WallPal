#!/bin/bash

# Check if a project path and filename are provided as arguments
if [ -z "$1" ]; then
    echo "Usage: $0 <plugin-name>"
    exit 1
fi

PROJECT_PATH=$1

pwd=$(pwd)

# if in the scripts dir, move back once
if [[ $pwd == *"/scripts" ]]; then
    cd ..
fi

cd Plugins
cd "$PROJECT_PATH"

dotnet build
if [ $? -ne 0 ]; then
    echo "Build failed."
    cd $pwd
    exit 1
fi

# Get the Documents folder
PLUGINS_DIR=~/Documents/WallPal/Plugins

# Move the plugin to the WallPal plugin directory
mkdir -p $PLUGINS_DIR
cp -r bin/Debug/net8.0/$PROJECT_PATH.dll $PLUGINS_DIR/$PROJECT_PATH.dll

cd $pwd