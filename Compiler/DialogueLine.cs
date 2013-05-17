using System;
using System.Text.RegularExpressions;
using Compiler.At;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Compiler {
	public class DialogueLine {
		public DialogueFile File { get; private set; }

		public int LineNumber { get; private set; }

		public string LineType { get; private set; }

		public string QuotedContent { get; private set; }

		public string Content { get; private set; }

		public LineOptions Options { get; private set; }

		public DialogueLine(string type, LineOptions opts) {
			LineType = type;
			Content = "";
			QuotedContent = "";
			Options = opts ?? new LineOptions();
			LineNumber = 0;
			File = DialogueFile.NullFile;
		}

		public DialogueLine(string type, string contents, LineOptions opts) {
			LineType = type;
			Content = contents;
			QuotedContent = string.Format("\"{0}\"", contents);
			Options = opts ?? new LineOptions();
			LineNumber = 0;
			File = DialogueFile.NullFile;
		}

		public DialogueLine(string line, DialogueFile file, int num) {
			Regex re = new Regex("([^:]+):(\"([^\"]*)\")?({.*})?");
			var match = re.Match(line);

			LineType = match.Groups[1].Value;
			QuotedContent = match.Groups[2].Value;
			Content = match.Groups[3].Value;
			var opt = match.Groups[4].Value;
			Options = JsonConvert.DeserializeObject<LineOptions>(opt) ?? new LineOptions();

			File = file;
			LineNumber = num;
		}

		public bool InterpretAtSigns() {
			bool keep = true;

			AtStatement stmt = AtStatement.GetStatement(LineType, this);
			keep &= stmt.Run(File);

			return keep;
		}

		public override string ToString() {
			return string.Format("{0}:{1}{2}", LineType, QuotedContent, Options);
		}
	}
}

