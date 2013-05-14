using System;
using System.Text.RegularExpressions;

namespace Compiler {
	public class DialogLine {
		public string LineType { get; private set; }

		public string Content { get; private set; }

		public string Options { get; private set; }

		public DialogLine(string line) {
			Regex re = new Regex("([^:]+):(\"[^\"]*\")?({.*})?");
			var match = re.Match(line);

			LineType = match.Groups[1].Value;
			Content = match.Groups[2].Value;
			Options = match.Groups[3].Value;
		}

		public override string ToString() {
			return string.Format("{0}:{1}{2}", LineType, Content, Options);
		}
	}
}

