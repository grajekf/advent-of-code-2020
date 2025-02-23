using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
	public static void Main()
	{
		string[] instructions = new string[]{"jmp +236", "acc +43", "acc +28", "jmp +149", "acc +28", "acc +13", "acc +36", "acc +42", "jmp +439", "acc -14", "jmp +29", "jmp +154", "acc +16", "acc -13", "acc -16", "nop +317", "jmp +497", "acc +21", "jmp +386", "jmp +373", "acc +22", "jmp +311", "acc -16", "acc +27", "acc +21", "acc +43", "jmp +512", "jmp +218", "jmp +217", "acc +12", "acc +44", "nop +367", "nop +180", "jmp +134", "acc -2", "acc +42", "acc +13", "acc -11", "jmp +442", "nop +457", "jmp +151", "acc +15", "acc -4", "acc +0", "jmp +131", "acc +6", "acc -2", "acc +37", "jmp +112", "acc +32", "acc +6", "acc -15", "jmp +474", "jmp +515", "acc +12", "acc +11", "acc +4", "jmp +339", "acc -3", "acc +36", "jmp +220", "nop +91", "acc -12", "jmp +49", "acc -17", "jmp +204", "acc +40", "jmp +535", "acc +37", "acc +8", "nop +147", "nop +174", "jmp +306", "jmp +305", "acc +7", "acc +33", "jmp +305", "acc +22", "acc +17", "acc +24", "jmp +458", "jmp +1", "acc +36", "acc +34", "jmp +113", "acc -3", "nop +113", "nop -34", "jmp +506", "acc -19", "acc +21", "acc +35", "acc -1", "jmp +74", "acc +15", "acc +7", "jmp +79", "acc +29", "acc +42", "jmp +427", "acc +33", "jmp +29", "acc +6", "acc +13", "nop +477", "acc +26", "jmp +493", "acc +33", "acc +43", "acc +49", "acc +35", "jmp +409", "acc -7", "acc +35", "acc +40", "jmp +309", "acc -13", "acc -14", "acc +32", "jmp +322", "jmp +10", "jmp +44", "acc +20", "acc +25", "jmp +175", "acc +22", "acc +16", "acc +1", "acc +36", "jmp -65", "jmp +231", "acc +35", "jmp +155", "jmp +218", "acc -10", "acc -13", "acc +38", "jmp -92", "acc +15", "jmp +134", "acc -16", "acc +18", "jmp -30", "nop -41", "acc +48", "acc +49", "jmp -107", "acc +4", "acc +34", "acc +38", "acc -18", "jmp +247", "acc +45", "acc +23", "jmp +149", "nop +164", "acc +26", "jmp -24", "jmp +240", "jmp +77", "acc +30", "acc -13", "jmp -158", "nop +136", "jmp +33", "jmp +189", "jmp +143", "jmp +1", "acc +4", "acc +30", "jmp -106", "acc +16", "nop -52", "acc +37", "jmp +119", "acc -11", "acc -9", "acc +15", "acc +4", "jmp +301", "jmp +1", "acc -3", "jmp +188", "nop +86", "nop +125", "acc -10", "jmp -105", "acc +36", "acc +9", "acc +0", "jmp +317", "jmp +347", "acc +48", "nop +380", "acc -18", "acc +28", "jmp +398", "jmp -152", "jmp -86", "acc +22", "acc +11", "acc +39", "jmp -173", "jmp +343", "nop +194", "nop +98", "nop +382", "jmp +300", "acc +35", "nop +287", "acc -8", "jmp +302", "acc +19", "acc +45", "jmp +95", "acc +29", "jmp +274", "acc +18", "acc -13", "acc +23", "acc +7", "jmp +164", "acc +17", "acc +36", "acc -5", "jmp +153", "acc +21", "jmp +105", "jmp +1", "nop +267", "jmp +277", "jmp +88", "acc +2", "acc +18", "nop +182", "jmp +189", "acc +37", "acc +46", "jmp +258", "acc +22", "acc +15", "jmp +249", "acc +17", "jmp -162", "jmp +25", "acc -6", "nop +314", "jmp -30", "jmp +312", "acc +34", "nop -230", "acc -2", "jmp +158", "acc -4", "acc +37", "jmp +318", "acc +18", "acc +23", "acc -8", "jmp -248", "jmp +181", "acc +17", "acc +4", "jmp -189", "acc +27", "acc -13", "acc -4", "acc +8", "jmp +222", "jmp +310", "acc -5", "acc +35", "jmp +241", "jmp -130", "jmp +124", "acc -19", "jmp +331", "acc -8", "acc +45", "jmp +106", "acc +23", "acc +48", "jmp -107", "acc +7", "acc -19", "acc +3", "jmp +130", "jmp -104", "nop +5", "acc +29", "acc +8", "acc -6", "jmp +7", "acc +12", "jmp +102", "acc -4", "acc +46", "acc -17", "jmp -209", "acc +20", "jmp -271", "acc +48", "jmp +30", "nop +204", "acc -19", "acc +4", "acc +38", "jmp +17", "jmp +116", "acc -17", "acc +23", "jmp -75", "jmp -129", "jmp +152", "acc +36", "nop -193", "acc +26", "acc +38", "jmp +242", "jmp -197", "acc +32", "acc -5", "acc -19", "jmp -201", "jmp -304", "acc +9", "jmp +175", "acc +1", "jmp -15", "jmp +1", "nop -74", "jmp -38", "nop -165", "acc -19", "jmp -317", "acc -19", "acc -1", "jmp +17", "acc +0", "nop +151", "jmp +93", "acc +32", "acc +29", "acc +0", "jmp -340", "acc +39", "jmp -115", "acc +0", "acc +47", "nop -320", "jmp +244", "acc +29", "jmp +81", "jmp -84", "acc +2", "acc +16", "nop -345", "acc +23", "jmp +9", "acc +26", "jmp -67", "acc -11", "acc +38", "jmp +150", "acc +19", "acc -2", "jmp -244", "jmp +88", "acc -4", "jmp -157", "acc +22", "acc +33", "acc +41", "jmp -117", "acc +31", "acc +50", "acc +24", "jmp -265", "jmp +1", "jmp -352", "jmp -312", "acc +35", "acc +30", "jmp -90", "jmp +8", "acc +14", "acc +39", "jmp -112", "acc -11", "acc -3", "acc +22", "jmp -116", "acc +48", "jmp -194", "acc -5", "jmp -252", "jmp +66", "jmp -295", "jmp +196", "acc +25", "acc -11", "nop +112", "acc +33", "jmp +123", "acc -10", "acc +28", "nop -119", "acc +12", "jmp -166", "jmp -356", "acc +8", "acc +16", "jmp +161", "acc +25", "acc +3", "jmp -5", "acc +32", "acc +40", "jmp +181", "acc -11", "acc -5", "jmp +1", "acc +0", "jmp -265", "acc +5", "acc +24", "acc +15", "acc -17", "jmp -326", "nop +103", "acc -9", "acc +13", "jmp -379", "acc +38", "acc +16", "jmp -65", "jmp +1", "jmp +1", "jmp +1", "acc -1", "jmp -191", "acc +35", "acc -19", "acc -6", "jmp -52", "acc +15", "jmp -357", "nop -134", "acc -3", "nop +103", "jmp -123", "acc +43", "acc +0", "acc +47", "jmp -373", "acc +0", "acc +50", "acc +44", "acc +21", "jmp -114", "acc -19", "jmp -339", "acc +25", "jmp -410", "jmp -126", "acc -2", "acc -6", "acc +14", "jmp -207", "acc +35", "acc -7", "jmp +75", "acc +9", "acc +22", "jmp +114", "acc +18", "acc +36", "acc +0", "acc +40", "jmp -192", "acc +35", "acc +0", "acc +28", "acc +3", "jmp -346", "nop -131", "acc +46", "nop -467", "nop -179", "jmp -151", "jmp -120", "acc +30", "acc +22", "acc -7", "acc +18", "jmp -157", "acc +5", "jmp +76", "nop -315", "acc +25", "jmp -357", "acc +44", "jmp -12", "acc +0", "acc +19", "nop -485", "jmp -495", "nop -115", "acc +12", "jmp -8", "acc +31", "acc -7", "jmp -158", "acc +44", "acc +32", "jmp +87", "acc +1", "acc +37", "acc +44", "jmp -86", "acc +0", "acc +17", "acc -13", "jmp -434", "acc +37", "jmp -342", "acc +3", "jmp +1", "acc +29", "jmp -242", "acc +48", "jmp -442", "jmp -283", "acc -19", "acc +6", "acc +20", "acc +44", "jmp -533", "acc -15", "nop -356", "acc +18", "jmp -408", "acc -9", "acc +17", "acc +16", "jmp -385", "nop -130", "jmp +1", "acc +38", "acc +39", "jmp -324", "jmp -141", "acc +4", "acc +3", "acc -4", "jmp -114", "acc +2", "jmp +1", "acc +44", "jmp -360", "acc +43", "acc +36", "nop -177", "nop -288", "jmp -496", "acc +45", "acc +0", "jmp -322", "acc +13", "jmp -511", "acc -2", "acc +36", "jmp -460", "acc +28", "acc +28", "jmp -455", "acc -4", "acc +38", "jmp -145", "jmp -163", "jmp -331", "nop -227", "jmp -470", "acc +35", "nop -419", "acc +39", "acc +0", "jmp -435", "jmp +1", "jmp -69", "acc +20", "acc +46", "nop +2", "jmp -239", "acc -3", "acc +12", "acc +38", "jmp -259", "jmp -60", "jmp -67", "nop -542", "jmp -397", "acc +32", "jmp -57", "acc +30", "nop -393", "jmp -380", "acc +16", "acc -7", "acc +0", "acc +2", "jmp +1"};
		var parsedInstructions = instructions.Select(i =>
		{
			var terms = i.Split(" ");
			var operation = terms[0];
			var argument = int.Parse(terms[1]);
			return new Instruction()
			{Operation = operation, Argument = argument};
		}).ToArray();
		var instructionsRun = new HashSet<int>();
		int accumulator = 0;
		int currentInstruction = 0;
		while (currentInstruction < parsedInstructions.Length && !instructionsRun.Contains(currentInstruction))
		{
			var instruction = parsedInstructions[currentInstruction];
			(int nextInstruction, var newAccumulator) = instruction.Execute(currentInstruction, accumulator);
			instructionsRun.Add(currentInstruction);
			currentInstruction = nextInstruction;
			accumulator = newAccumulator;
		}

		Console.WriteLine("Acc:" + accumulator.ToString());
	}

	class Instruction
	{
		public string Operation
		{
			get;
			set;
		}

		public int Argument
		{
			get;
			set;
		}

		public (int nextInstruction, int accumulator) Execute(int currentInstruction, int currentAccumulator)
		{
			if (Operation == "nop")
				return (currentInstruction + 1, currentAccumulator);
			if (Operation == "acc")
				return (currentInstruction + 1, currentAccumulator + Argument);
			if (Operation == "jmp")
				return (currentInstruction + Argument, currentAccumulator);
			throw new ArgumentException("Unrecognized operation");
		}

		public (int nextInstruction, int accumulator) ExecuteFlipped(int currentInstruction, int currentAccumulator)
		{
			var oldOperation = Operation;
			if (oldOperation == "nop")
				Operation = "jmp";
			if (oldOperation == "jmp")
				Operation = "nop";
			var result = Execute(currentInstruction, currentAccumulator);
			Operation = oldOperation;
			return result;
		}
	}
}