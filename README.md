# KeePassPipe

The CLI tool KeePassPipe.exe searches entries by title in an opened KeePass database.

```Syntax: KeePassPipe.exe [-t|-u|-p] Title ```

The username (-u) and password (-p ) of the first entry with matching title will be printed to stdout. The search is case sensitive. First matching entry will be returned. Results will be enclosed by double quote characters. Error messages will be printed to stderr. 

## Plugin Installation 

1.  [Download](https://github.com/limapap/KeePassPipe/releases/latest "Lastest Release") the plugin and unpack the ZIP file to a new folder.
2.  In KeePass, click 'Tools' → 'Plugins' → button 'Open Folder'; KeePass now opens a folder called 'Plugins'. Move the file 'KeePassPipePlugin.dll' from the new folder into the 'Plugins' folder.
3.  Restart KeePass in order to load the new plugin.

To uninstall the plugin, delete the plugin file 'KeePassPipePlugin.dll' and restart KeePass.

## Usage

![grafik](https://user-images.githubusercontent.com/49816044/56849564-7d016e00-68f6-11e9-96ac-5931549384c7.png)

![grafik](https://user-images.githubusercontent.com/49816044/56859290-0eb9bb80-6989-11e9-87f0-73906f719be1.png)

### In a Batch File:

```batch
:: Get username and password from KeePass and use them as parameters
:: for some app.

set KPTITLE=Sample Entry #2

for /F "tokens=*" %%l in ('KeePassPipe.exe -U "%KPTITLE%"') do set "KPUSERNAME=%%~l"
for /F "tokens=*" %%l in ('KeePassPipe.exe -P "%KPTITLE%"') do set "KPPASSWORD=%%~l"

echo SomeApp.exe "%KPUSERNAME%" "%KPPASSWORD%" 

```
## Security

Adding software to a computer is always having the potential to increase security risks. If someone only needs to enter credentials a few times a day, I would not see a good reason to bother with automating logins and to deal with this plugin. However if your daily routine includes many logins it might be worse considering this plugin. 

Important: Please note that starting a program with a password as parameter might expose the password in the taskmanager. This is not related directly to using this plugin, but to its intented use in e.x. batch files. In general it's not recommendable to pass credentials as arguments on computers which allow multiple sessions. 

Querying the keepass database with this plugin is only possible, if keepass is running and the KeePass database is opened after a successful authentification. Unauthorized remote access over the KeePassPipe plugin is prevented by allowing access to the plugin pipe for the user only, who is running KeePass and the plugin:
```c#
...
AddAccessRule(
   new PipeAccessRule(WindowsIdentity.GetCurrent().Name, PipeAccessRights.FullControl, 
   AccessControlType.Allow));
...   
```
Hence running Keepass and the plugin as user "tester-pc\tester" and trying to access the plugin pipe as user "tester-pc\Alien" will not be successful:

![grafik](https://user-images.githubusercontent.com/49816044/56861455-171df080-69a1-11e9-9eea-f539a09a2de1.png)

In case a computer is infected it's only a matter of effort to gain access to the users data and keepass database. Despite the work which was put in to hardnening KeePass, spezialized spyware attacks might succeed. There are already more common and easier approaches to do this, than using the interface offered by this plugin. Using this plugin on a single user computer, should not increase the security risc significantly. 

Please see the [KeePass Security](https://keepass.info/help/base/security.html) page and [KeePass 2.x Plugins](https://keepass.info/help/v2/plugins.html) for more information.

