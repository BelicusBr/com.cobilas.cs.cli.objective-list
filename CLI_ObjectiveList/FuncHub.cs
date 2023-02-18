using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using System.Collections;

namespace Cobilas.CLI.ObjectiveList {
    internal struct FuncHub {
        private static Dictionary<int, Func<ErrorMensager, CLIArgCollection, bool>> funcs = new Dictionary<int, Func<ErrorMensager, CLIArgCollection, bool>>(new FuncHubObjects());

        public static bool Invok(int id, ErrorMensager error, CLIArgCollection clt)
            => funcs[id](error, clt);

        private struct FuncHubObjects : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
            IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
                yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(4, (e, c) => { Console.WriteLine("version-{0}", CLIBase.version); return true; });
            }

            IEnumerator IEnumerable.GetEnumerator()
                => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;
        }
    }
}
