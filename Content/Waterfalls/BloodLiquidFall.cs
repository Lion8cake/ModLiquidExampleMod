using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Waterfalls
{
	public class BloodLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}

		//By setting the frame to 0 constantly, we freeze the frame on the first frame.
		//Just by overriding this hook/method causes the same effect as no animations normally play when this hook/method is detected to exist
		public override void AnimateWaterfall(ref int frame, ref int frameBackground, ref int frameCounter)
		{
			frame = 0;
		}
	}
}
