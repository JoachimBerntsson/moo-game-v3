using MooGameV3.Domain.Game;

namespace MooGameV3.Presentation.Console;
internal static class BullsCowsText
{
	public static string ToMarkers(this BullsCows bc, int codeLength)
		=> new string('B', Math.Clamp(bc.Bulls, 0, codeLength))
		   + ","
		   + new string('C', Math.Clamp(bc.Cows, 0, codeLength));
}