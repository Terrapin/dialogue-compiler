using System;
using CommandLine;

namespace Compiler {
	class MainClass {
		public static void Main(string[] args) {
			var opts = Parser.Default.ParseArguments<DialogueCompiler.Options>(args);

			if (opts.Value.InputFile == null) {
				Console.Error.WriteLine(opts.Value.GetUsage());
				return;
			}

			DialogueCompiler compiler = new DialogueCompiler(opts.Value);
			compiler.Run();
		}
	}
}
