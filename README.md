# Cobilas.CLI.ObjectiveList
## Functions
### [AppName] [--version/-v]
Displays the current version of the application.

### [AppName] [--help/-h]
Displays the list of program commands.

### [AppName] [init/-i] [arg:file path]
Initializes a file in .xml format where tasks will be stored.</br>

[arg:file path]		Relative or full path of the target file.

### [AppName] [--rename/-r] [arg1:folder path] [arg2:file name] [arg3:new file name]
Renames the file where tasks are stored.</br>
[arg1:folder path] Relative or full directory path of the target file.</br>
[arg2:file name] The name of the target file.</br>
[arg3:new file name] The new name for the target file.

### [AppName] [--clear/-c] [arg:file path]
Clears the task list.</br>

[arg:file path]		Relative or full path of the target file.

### [AppName] [--show/-s] [--item/--i] [arg:file path]
[AppName] [--show/-s] [--item/--i] [--path/-p] [opc-arg:path] [arg:file path]</br>
Displays task list items.</br>

[arg:file path]		Relative or full path of the target file.
#### mandatory options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [--show/-s] [--list/-l] [--status/--s] [arg:file path]
Displays task list items.</br>

[arg:file path]		Relative or full path of the target file.
#### options
[--status/--s] [arg[true|false]] The status of the task.

### [AppName] [--element/-e] [add] [arg:file path]
Changes the task list.</br>
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [--description/-d] [opc-arg] [arg:file path]</br>
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [arg:file path]</br>
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [--description/-d] [opc-arg] [arg:file path]</br>
[AppName] [--element/-e] [add] [--title/-t] [opc-arg] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--title/-t] [opc-arg] The task title.

#### options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]</br>
[--description/-d] [opc-arg] The task description.

### [AppName] [--element/-e] [remove] [arg:file path]
Changes the task list.</br>
[AppName] [--element/-e] [remove] [--path/-p] [opc-arg:path] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--replacetile/-rt] [arg:file path]
Change specific items in task list.</br>
[AppName] [set] [--replacetile/-rt] [--title/-t] [opc-arg] [--path/-p] [opc-arg:path] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--title/-t] [opc-arg] The task title.</br>
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--replacedesc/-rd] [arg:file path]
Change specific items in task list.</br>
[AppName] [set] [--replacedesc/-rd] [--description/-d] [opc-arg] [--path/-p] [opc-arg:path] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--description/-d] [opc-arg] The task description.</br>
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--replacestatus/-rs] [arg:file path]
Change specific items in task list.</br>
[AppName] [set] [--replacestatus/-rs] [--status/--s] [arg[true|false]] [--path/-p] [opc-arg:path] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--status/--s] [arg[true|false]] The status of the task.</br>
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]

### [AppName] [set] [--move/-m] [arg:file path]
Change specific items in task list.</br>
[AppName] [set] [--move/-m] [--path/-p] [opc-arg:1.0.1] [--moveto/-mt] [arg[1.0.2]] [arg:file path]</br>

[arg:file path]		Relative or full path of the target file.

#### mandatory options
[--moveto/-mt] [arg[true|false]] The path to move to.[example:1.0.1 to 1.0.2]</br>
[--path/-p] [opc-arg:path] Represents the path in the task list tree.[example: 1.2.5]