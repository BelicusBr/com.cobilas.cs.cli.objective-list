# Cobilas.CLI.ObjectiveList
## Functions
### [AppName] [--version/-v]
Displays the current version of the application.

### [AppName] [--help/-h]
Displays the list of program commands.

### [AppName] [init/-i] [arg:file/folder path]
Initializes a file in .xml format where tasks will be stored.</br>

[arg:file/folder path]		Relative or full path of the destination directory or file.

### [AppName] [--rename/-r] [arg1:file name] [arg2:new file name] [arg3:file/folder path]
Renames the file where tasks are stored. \
[arg2:file name] The name of the target file. \
[arg3:new file name] The new name for the target file. \
[arg1:file/folder path] Relative or full path of the destination directory or file.

### [AppName] [--clear/-c] [arg:file/folder path]
Clears the task list.</br>

[arg:file/folder path] Relative or full path of the destination directory or file.

### [AppName] [--show/-s] [--item/--i] [arg:file/folder path]
[AppName] [--show/-s] [--item/--i] [--path/-p] [opc-arg:path] [arg:file/folder path] \
Displays task list items. \

[arg:file/folder path] Relative or full path of the destination directory or file.
#### mandatory options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [--show/-s] [--list/-l] [--status/--s] [arg:file/folder path]
Displays task list items. \

[arg:file/folder path] Relative or full path of the destination directory or file.
#### options
[--status/--s] [arg[true|false]] The status of the task.

### [AppName] [--element/-e] [add] [arg:file/folder path]
Changes the task list.</br>
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [--description/-d] [opc-arg] [arg:file/folder path] \
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [arg:file/folder path] \
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--description/-d] [opc-arg] [arg:file/folder path] \
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [arg:file/folder path] \

[arg:file/folder path]		Relative or full path of the destination directory or file.

#### mandatory options
[--title/-t] [opc-arg] The task title.

#### options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5] \
[--description/-d] [opc-arg] The task description.

### [AppName] [--element/-e] [remove] [arg:file/folder path]
Changes the task list. \
[AppName] [--element/-e] [remove] [--path/-p] [opc-arg:path] [arg:file/folder path] \

[arg:file/folder path]		Relative or full path of the destination directory or file.

#### mandatory options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--replacetile/-rt] [arg:file/folder path]
Change specific items in task list. \
[AppName] [set] [--replacetile/-rt] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [arg:file/folder path] \

[arg:file/folder path]		Relative or full path of the destination directory or file.

#### mandatory options
[--title/-t] [opc-arg] The task title. \
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--replace/-rp] [arg:file/folder path]
Change specific items in task list. \
[AppName] [set] [--replace/-rp] [--path/-p] [opc-arg:path] [--title/-t] [opc-arg] [--description/-d] [opc-arg] [--status/--s] [arg[true|false]] [arg:file/folder path] \

[arg:file/folder path]		Relative or full path of the destination directory or file.

#### mandatory options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

#### options
[--title/-t] [opc-arg] The task title. \

### [AppName] [set] [--move/-m] [arg:file/folder path]
Change specific items in task list. \
[AppName] [set] [--move/-m] [--path/-p] [opc-arg:1.0.1] [--moveto/-mt] [arg[1.0.2]] [arg:file/folder path] \

[arg:file/folder path]		Relative or full path of the destination directory or file.

#### mandatory options
[--moveto/-mt] [arg[true|false]] The path to move to.[example:1.0.1 to 1.0.2] \
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]