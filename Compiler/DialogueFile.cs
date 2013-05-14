using System;
using System.IO;
using System.Collections.Generic;

namespace Compiler {
	public class DialogueFile {
		private TextReader file;
		private int lineNumber;
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
			lineNumber = 0;
			FileName = Path.GetFileNameWithoutExtension(path);

			var f = path = Path.Combine(DialogueCompiler.Instance.BasePath, path);
			if (!File.Exists(f)) {
				f = path + ".txt";
				if (!File.Exists(f)) {
					f = path + ".dlg";
					if (!File.Exists(f)) {
						f = path + ".ch";
					}
				}
			}

			file = new StreamReader(f);
		}

		public void AddLines(IEnumerable<DialogueLine> lines) {
			contents.AddRange(lines);
		}

		public void Parse() {
			contents = new List<DialogueLine>();

			var line = file.ReadLine();
			while (line != null) {
				lineNumber += 1;

				line = line.Trim();
				if (line.Length > 0) {
					var dlgLine = new DialogueLine(line, this, lineNumber); 
					if (dlgLine.InterpretAtSigns()) {
						contents.Add(dlgLine);
					}
				}

				line = file.ReadLine();
			}
		}
	}
}

