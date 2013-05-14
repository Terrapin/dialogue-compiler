using System;
using System.Text.RegularExpressions;
using At;

namespace Compiler {
	public class DialogueLine {
		public DialogueFile File { get; private set; }

		public int LineNumber { get; private set; }

		public string LineType { get; private set; }

		public string QuotedContent { get; private set; }

		public string Content { get; private set; }

		public string Options { get; private set; }

		public DialogueLine(string line, DialogueFile file, int num) {
			Regex re = new Regex("([^:]+):(\"([^\"]*)\")?({.*})?");
			var match = re.Match(line);

			LineType = match.Groups[1].Value;
			QuotedContent = match.Groups[2].Value;
			Content = match.Groups[3].Value;
			Options = match.Groups[4].Value;
			File = file;
			LineNumber = num;
		}

		public bool InterpretAtSigns() {
			bool keep = true;
			if (LineType.StartsWith("@")) {
				AtStatement stmt = AtStatement.GetStatement(LineType.Substring(1), this);
				keep &= stmt.Run(File);
			}

			return keep;
		}

		public override string ToString() {
			return string.Format("{0}:{1}{2}", LineType, QuotedContent, Options);
		}
	}
}

