using System;

namespace Compiler.At {
	public class AtLoad : AtStatement {
		private DialogueFile File;

		public AtLoad(DialogueLine param) {
			File = DialogueFile.Open(param.Content);
		}

		public override bool Run(DialogueFile file) {
			DialogueCompiler.Instance.ImportFile(File);
			return false;
		}
	}
}

