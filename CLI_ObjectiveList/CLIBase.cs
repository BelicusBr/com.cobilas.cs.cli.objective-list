using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList {
    internal struct CLIBase {
        /*root --version/-v
         *root --help/-h
         *root --rename/-r {arg:folder path} {arg:file name} {arg:new file name}
         *root init/-i {arg:file path}
         *root --show/-s --item/--i --path/-p {opc:arg} {arg:file path}
         *root --show/-s --list/-l --status/--s {opc:arg[true|false]} {arg:file path}
         *root --clear/-c {arg:file path}
         *root --element/-e add --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
         *root --element/-e remove --path/-p {opc:arg} {arg:file path}
         *root set --replacetile/-rt --path/-p {opc:arg} --title/-t {opc:arg} {arg:file path}
         *root set --replacedesc/-rd --path/-p {opc:arg} --description/-d {opc:arg} {arg:file path}
         *root set --replacestatus/-rs --path/-p {opc:arg} --status/--s {opc:arg[true|false]} {arg:file path}
         *root set --move/-m --path/-p {opc:arg} --moveto/-mt {opc:arg} {arg:file path}
         *root init/-i --help/-h
         *root --rename/-r --help/-h
         *root --show/-s --help/-h
         *root --clear/-c --help/-h
         *root --element/-e --help/-h
         *root set --help/-h
         */
        public static CLICommand Create()
            => new CLICommand("root",
                    new CLICommand(1, "init/-i",
                            new CLICommand(14, "--help/-h"),
                            CLICMDArg.Default
                        ),
                    new CLICommand("--show/-s",
                            new CLICommand(15, "--help/-h"),
                            new CLICommand(2, "--item/--i",
                                    new CLIOption(1, "--path/-p", false),
                                    CLICMDArg.Default
                                ),
                            new CLICommand(9, "--list/-l",
                                    new CLIOption(1, "--status/--s"),
                                    CLICMDArg.Default
                                )
                        ),
                    new CLICommand(3, "--help/-h"),
                    new CLICommand(4, "--version/-v"),
                    new CLICommand("--element/-e",
                            new CLICommand(16, "--help/-h"),
                            new CLICommand(5, "add",
                                    new CLIOption(1, "--path/-p"),
                                    new CLIOption(1, "--title/-t", false),
                                    new CLIOption(1, "--description/-d"),
                                    CLICMDArg.Default
                                ),
                            new CLICommand(7, "remove",
                                    new CLIOption(1, "--path/-p", false),
                                    CLICMDArg.Default
                                )
                        ),
                    new CLICommand(6, "--clear/-c",
                            new CLICommand(17, "--help/-h"),
                            CLICMDArg.Default
                        ),
                    new CLICommand(8, "--rename/-r",
                            new CLICommand(18, "--help/-h"),
                            CLICMDArg.Default,
                            CLICMDArg.Default,
                            CLICMDArg.Default
                        ),
                    new CLICommand("set",
                            new CLICommand(19, "--help/-h"),
                            new CLICommand(10, "--replacetile/-rt",
                                    new CLIOption(1, "--path/-p", false),
                                    new CLIOption(1, "--title/-t", false),
                                    CLICMDArg.Default
                                ),
                            new CLICommand(11, "--replacedesc/-rd",
                                    new CLIOption(1, "--path/-p", false),
                                    new CLIOption(1, "--description/-d", false),
                                    CLICMDArg.Default
                                ),
                            new CLICommand(12, "--replacestatus/-rs",
                                    new CLIOption(1, "--path/-p", false),
                                    new CLIOption(1, "--status/--s", false),
                                    CLICMDArg.Default
                                ),
                            new CLICommand(13, "--move/-m",
                                    new CLIOption(1, "--path/-p", false),
                                    new CLIOption(1, "--moveto/-mt", false),
                                    CLICMDArg.Default
                                )
                        )
                );
    }
}
