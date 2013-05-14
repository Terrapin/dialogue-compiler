using System;
using System.IO;
using System.Collections.Generic;

namespace Compiler {
	public class DialogueFile {
		private TextReader File;
		private int LineNumber;
		private List<DialogueLine> contents;

		public IEnumerable<DialogueLine> Lines {
			get {
				if (contents == null) {
					Parse();
				}

				return contents;
			}
		}

		public string FileName { get; private set; }

		public DialogueFile(string path) {
			LineNumber = 0;
			FileName = Path.GetFileNameWithoutExtension(path);
			File = new StreamReader(Path.Combine(DialogueCompiler.Instance.BasePath, path));
		}

		public void AddLines(IEnumerable<DialogueLine> lines) {
			contents.AddRange(lines);
		}

		public void Parse() {
			contents = new List<DialogueLine>();

			var line = File.ReadLine();
			while (line != null) {
				LineNumber += 1;

				line = line.Trim();
				if (line.Length > 0) {
					var dlgLine = new DialogueLine(line, this, LineNumber); 
					if (dlgLine.InterpretAtSigns()) {
						contents.Add(dlgLine);
					}
				}

				line = File.ReadLine();
			}
		}
	}
}

