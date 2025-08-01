using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Liquids
{
	public class BloodClotLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}
	}
}
