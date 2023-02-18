using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList {
    internal struct CLIBase {
        public const string version = "1.0.0";
        /*
            root --version/-v
            root --help/-h
            root init/-i {arg:file path}
            root --show/-s {arg:file path}
            root add task --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
            root add block --path/-p {opc:arg} --title/-t {opc:arg} {arg:file path}
         */
        public static CLICommand Create()
            => new CLICommand("root",
                    new CLICommand(4,
                            "--version/-v"
                        )
                );
    }
}
