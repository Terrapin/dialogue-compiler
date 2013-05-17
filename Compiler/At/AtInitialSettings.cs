using System;

namespace Compiler.At {
	public class AtInitialSettings : AtStatement {
		private DialogueLine settings;

		public AtInitialSettings(DialogueLine settings) {
			this.settings = settings;
		}

		public override bool Run(DialogueFile file) {
			var duplicates = DialogueCompiler.Instance.SetInitialSettings(settings.Options);
			if (duplicates.Count > 0) {
				DialogueCompiler.Instance.Error(settings, "Conflicting initial_settings : {0}", String.Join(", ", duplicates));
			}

			return false;
		}
	}
}

