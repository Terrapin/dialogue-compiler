using System;
using System.IO;

namespace Compiler {
	public class DialogFile {
		private TextReader File;

		public int LineNumber { get; private set; }

		public string FileName { get; private set; }

		public DialogFile(string path) {
			LineNumber = 0;
			File = new StreamReader(path);
			FileName = Path.GetFileNameWithoutExtension(path);
		}

		public string ReadLine() {
			LineNumber += 1;
			return File.ReadLine();
		}
	}
}

