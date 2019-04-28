# KeePassPipe

The CLI tool KeePassPipe.exe searches entries by title in an open KeePass database.

### Syntax: KeePassPipe.exe [ -t | -u |  -p  ] Title

The username (-u) and password (-p ) of the first entry with matching title will be printed to stdout. The search is case sensitive. First matching entry will be retuned. In case the title is not found, an empty strings will be printed. 

Errors messages will be printed to stderr. 


## Latest Release
https://github.com/limapap/KeePassPipe/releases/latest

## Plugin Installation 
https://keepass.info/help/v2/plugins.html

## Usage


## Examples

![grafik](https://user-images.githubusercontent.com/49816044/56849564-7d016e00-68f6-11e9-96ac-5931549384c7.png)

![grafik](https://user-images.githubusercontent.com/49816044/56849671-ae2e6e00-68f7-11e9-869f-c624dd06c98d.png)

### In a Batch File:

```batch
:: Get username and password from KeePass and use them as parameters
:: for some app.

set PTITLE=Sample Entry #2

for /F "tokens=*" %%l in ('KeePassPipe.exe -U "%PTITLE%"') do set "PUSERNAME=%%~l"
for /F "tokens=*" %%l in ('KeePassPipe.exe -P "%PTITLE%"') do set "PPASSWORD=%%~l"

echo SomeApp.exe "%PUSERNAME%" "%PPASSWORD%" 

```
