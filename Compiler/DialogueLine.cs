using System;
using System.Text.RegularExpressions;

namespace Compiler {
	public class DialogueLine {
		public string FileName { get; private set; }

		public int LineNumber { get; private set; }

		public string LineType { get; private set; }

		public string Content { get; private set; }

		public string Options { get; private set; }

		public DialogueLine(string line, string file, int num) {
			Regex re = new Regex("([^:]+):(\"[^\"]*\")?({.*})?");
			var match = re.Match(line);

			LineType = match.Groups[1].Value;
			Content = match.Groups[2].Value;
			Options = match.Groups[3].Value;
			FileName = file;
			LineNumber = num;
		}

		public override string ToString() {
			return string.Format("{0}:{1}{2}", LineType, Content, Options);
		}
	}
}

