using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Liquids
{
	public class BloodLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}
	}
}
