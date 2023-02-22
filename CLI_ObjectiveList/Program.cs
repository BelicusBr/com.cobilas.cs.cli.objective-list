using System;
using System.Text;
using Cobilas.Collections;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList {
    class Program {

        public const string version = "1.8.4";
        internal static string BaseDirectory => Environment.CurrentDirectory;
        internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

        static void Main(string[] args) {

            if (ArrayManipulation.EmpytArray(args)) {
                Console.WriteLine("Enter some argument.");
                return;
            }

            ErrorMensager error = new ErrorMensager();
            CLIArgCollection collection = new CLIArgCollection();
            CLICommand root = CLIBase.Create();

            if (CLICommand.Cateter(new StringArrayToIEnumerator(args), root, collection, error, out int funcID)) {
                if (funcID == 0)
                    Console.WriteLine("Command '{0}' not found.", JoinArgs(args));
                else if (!FuncHub.Invok(funcID, error, collection))
                    PrintError(error);
            } else PrintError(error);

            collection.Clear();
            root.Dispose();
            GC.Collect();
        }

        static string JoinArgs(string[] args) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < args.Length; I++)
                builder.AppendFormat("{0} ", args[I]);
            return builder.ToString().TrimEnd();
        }

        static void PrintError(ErrorMensager msm) {
            foreach (var item in msm)
                PrintError(item);
        }

        static void PrintError(string msm) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msm);
            Console.ResetColor();
        }
    }
}
