# SamplePlugin

The `SamplePlugin` is a simple plugin designed to demonstrate the basic structure of a plugin. It allows you to set a image specified in the configuration file as the background.

## Usage

Set the `source` to `SamplePlugin` in the configuration file to use this plugin. Then you can set the `imagePath` under the plugins configuration to the path of the image you want to use as the background.

```json
{
  "source": "SamplePlugin",
  "plugins": {
    "SamplePlugin": {
      "imagePath": "/path/to/image.jpg"
    }
  }
}
``` 