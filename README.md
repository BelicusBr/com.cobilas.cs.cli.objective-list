# Cobilas.CLI.ObjectiveList

A command-line tool for managing task lists stored in `.tskl` files (XML format).

---

## Commands

### `--version` / `-v`
Displays the current version of the application.

```
[AppName] --version
[AppName] -v
```

---

### `help` / `-h` / `-?`
Displays the list of available commands.

```
[AppName] help
[AppName] -h
[AppName] -?
```

---

### `init` / `-i`
Initializes a new `.tskl` file where tasks will be stored.

```
[AppName] init [path:file/folder]
[AppName] -i [path:file/folder]
```

| Option | Description |
|---|---|
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

---

### `--rename` / `-r`
Renames an existing `.tskl` file.

```
[AppName] --rename [arg1:file name] [arg2:new file name] [path:file/folder]
[AppName] -r [arg1:file name] [arg2:new file name] [path:file/folder]
```

| Option | Description |
|---|---|
| `arg1:file name` *(required)* | The name of the target file. |
| `arg2:new file name` *(required)* | The new name for the target file. |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

---

### `--clear` / `-c`
Clears all tasks from the task list.

```
[AppName] --clear [path:file/folder]
[AppName] -c [path:file/folder]
```

| Option | Description |
|---|---|
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

---

### `--show` / `-s`

#### Show a specific task (`--item` / `--i`)
Displays a specific task from the task list tree.

```
[AppName] --show --item --path [path] [--otk|--stk] [path:file/folder]
[AppName] -s --i -p [path] [--otk|--stk] [path:file/folder]
```

| Option | Description |
|---|---|
| `--path` / `-p` *(required)* | Path of the task in the list tree. Example: `1.2.5` |
| `--otk` *(optional)* | Displays the specified task **without** its subtasks. |
| `--stk` *(optional)* | Displays the specified task **along with** its subtasks. |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

#### Show the full task list (`--list` / `-l`)
Displays all tasks in the task list, with optional filtering by status.

```
[AppName] --show --list [--status [true|false|all]] [path:file/folder]
[AppName] -s -l [--status [true|false|all]] [path:file/folder]
```

| Option | Description |
|---|---|
| `--status` / `--s` *(optional)* | Filters tasks by status. Accepted values: `true`, `false`, `all`. |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

---

### `--element` / `-e`

#### Add a task (`add`)
Adds a new task to the task list.

```
[AppName] --element add --title [title] [--path [path]] [--description [desc]] [path:file/folder]
[AppName] -e add -t [title] [-p [path]] [-d [desc]] [path:file/folder]
```

| Option | Description |
|---|---|
| `--title` / `-t` *(required)* | The task title. |
| `--path` / `-p` *(optional)* | Position in the task list tree where the task will be added. Example: `1.2.5` |
| `--description` / `-d` *(optional)* | The task description. |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

#### Remove a task (`remove`)
Removes a task from the task list.

```
[AppName] --element remove --path [path] [path:file/folder]
[AppName] -e remove -p [path] [path:file/folder]
```

| Option | Description |
|---|---|
| `--path` / `-p` *(required)* | Path of the task to remove in the list tree. Example: `1.2.5` |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

---

### `set`

#### Replace task data (`--replace` / `-rp`)
Updates the title, description, or status of a specific task.

```
[AppName] set --replace --path [path] [--title [title]] [--description [desc]] [--status [true|false]] [path:file/folder]
[AppName] set -rp -p [path] [-t [title]] [-d [desc]] [--s [true|false]] [path:file/folder]
```

| Option | Description |
|---|---|
| `--path` / `-p` *(required)* | Path of the task in the list tree. Example: `1.2.5` |
| `--title` / `-t` *(optional)* | New title for the task. |
| `--description` / `-d` *(optional)* | New description for the task. |
| `--status` / `--s` *(optional)* | New status for the task. Accepted values: `true`, `false`. |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |

#### Move a task (`--move` / `-m`)
Moves a task from one position to another in the task list tree.

```
[AppName] set --move --path [origin] --moveto [destination] [path:file/folder]
[AppName] set -m -p [origin] -mt [destination] [path:file/folder]
```

| Option | Description |
|---|---|
| `--path` / `-p` *(required)* | Current path of the task. Example: `1.0.1` |
| `--moveto` / `-mt` *(required)* | Destination path for the task. Example: `1.0.2` |
| `path:file/folder` *(optional)* | Relative or full path of the destination directory or file. |
