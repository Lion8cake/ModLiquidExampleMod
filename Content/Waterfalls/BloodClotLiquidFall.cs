using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Waterfalls
{
	public class BloodClotLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}
	}
}
