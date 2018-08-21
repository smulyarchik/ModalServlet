# ModalHandler

### Console application designed to handle modal dialogs via [WinApi](https://msdn.microsoft.com/en-us/library/aa288468(v=vs.71).aspx) or [UIAutomation](https://msdn.microsoft.com/en-us/library/ms747327(v=vs.110).aspx).
##
#### Usage:

##### Help: 
```
ModalHandler.Console.exe
```

##### Upload:
```
ModalHandler.Console.exe upload -o[--owner] {title} -p[--path] {path} -t[--timeout] {timeout}
```
where
* `upload` - command to handle a file upload dialog. (Required)
* `-o` or `--owner` - name of a browser that owns the modal dialog. Fully qualified title containing a browser type is expected. (Required)
* `-p` or `--path`- absolute path to a file to upload. (Required)
* `-t` or `--timeout`- maximum amount of time in seconds allotted for modal window detection. (Optional. Default: 10)

Example:
```
ModalHandler.Console.exe upload -o "Title - Google Chrome" -p "t:\est\path\to.file" -t 30
```

##### Cleanup:
```
ModalHandler.Console.exe cleanup -t[--timeout] {timeout}
```
where
* `cleanup` - command to close all open top level modal dialogs. (Required)
* `-t` or `--timeout`- maximum amount of time in seconds allotted for modal window detection. (Optional. Default: 10)

Example:
```
ModalHandler.Console.exe cleanup -t 5
```

##### Auth:
```
ModalHandler.Console.exe auth -u[--username] {username} -p[--password] {password} -t[--timeout] {timeout}
```
where:
* `auth` - command to handle IE11 'Windows Security' dialog. (Required)
* `-u` or `--username` - Username for authentication. (Required)
* `-p` or `--password` - Password for authentication. (Required)
* `-t` or `--timeout`- maximum amount of time in seconds allotted for modal window detection. (Optional. Default: 10)

Example:
```
ModalHandler.Console.exe auth -u user -p pass -t 15
```

### Related links:

  * **[PInvoke Wiki](http://www.pinvoke.net/)** List of all commands with examples.