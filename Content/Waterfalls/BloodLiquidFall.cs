using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Waterfalls
{
	public class BloodLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}
	}
}
