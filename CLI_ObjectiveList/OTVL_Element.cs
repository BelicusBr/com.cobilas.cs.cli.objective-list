using System;
using System.Xml;
using System.Text;

namespace Cobilas.CLI.ObjectiveList {
    internal class OTVL_Element : IDisposable {
        public bool status;
        public string title;
        public ElementPath path;
        public string description;

        public ElementPath Parent => ElementPath.GetParent(path);

        public void Dispose() {
            path = default;
            status = default;
            title = description = null;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("/=====[{0}]=====\r\n", title);
            builder.AppendFormat("Path[{0}]--Status[{1}]\r\n", path, status ? "Checked" : "Unchecked");
            if (!string.IsNullOrEmpty(description))
                builder.AppendFormat("Description:\r\n{0}\r\n", description);
            builder.Append("/===============\r\n");
            return base.ToString();
        }

        public static explicit operator ElementTag(OTVL_Element E)
            => new ElementTag("element",
                    new ElementAttribute("path", E.path.ToString()),
                    new ElementAttribute("status", E.status),
                    new ElementTag("title", E.title),
                    new ElementTag("descriptio", E.description)
                );
    }
}
