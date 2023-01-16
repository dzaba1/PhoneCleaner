# PhoneCleaner

Because my wife's phone is always clogged due to photos and WhatsApp I wanted to automate cleaning.

## Usage

From `--help`:

```
Description:
  Mostly removes and copies files from a phone.

Usage:
  PhoneCleaner [options]

Options:
  --working-dir <working-dir>          The main directory where all files will be copied from the device.
  --device-name <device-name>          The friendly name of the device.
  --config-filepath <config-filepath>  The path to the confiuguration file. [default: config.xml]
  --test-only                          If set then any write operations like copy or delete won't be executed. Only log
                                       traces. [default: False]
  --version                            Show version information
  -?, -h, --help                       Show help and usage information
```

### Config file

