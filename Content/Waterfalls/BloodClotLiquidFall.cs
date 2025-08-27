using ModLiquidLib.ModLoader;

namespace ModLiquidExampleMod.Content.Waterfalls
{
	public class BloodClotLiquidFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return false;
		}

		//Here we make our waterfall twice as slow as lava, honey and shimmer waterfalls
		//A basic example of manually animating a nromal waterfall
		public override void AnimateWaterfall(ref int frame, ref int frameBackground, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 12)
			{
				frameCounter = 0;
				frame++;
				if (frame > 15)
				{
					frame = 0;
				}
			}
		}
	}
}
