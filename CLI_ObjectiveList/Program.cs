using System;
using System.Text;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList {
    class Program {
        static void Main(string[] args) {
            ErrorMensager error = new ErrorMensager();
            CLIArgCollection collection = new CLIArgCollection();
            CLICommand root = CLIBase.Create();

            if (CLICommand.Cateter(new StringArrayToIEnumerator(args), root, collection, error, out int funcID)) {
                if (funcID == 0)
                    Console.WriteLine("CMD Unk [{0}]", JoinArgs(args));
                else
                    _ = FuncHub.Invok(funcID, error, collection);
            } else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var item in error)
                    Console.WriteLine(item);
                Console.ResetColor();
            }

            collection.Clear();
            root.Dispose();
        }

        static string JoinArgs(string[] args) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < args.Length; I++)
                builder.AppendFormat("{0} ", args[I]);
            return builder.ToString().TrimEnd();
        }
    }
}
