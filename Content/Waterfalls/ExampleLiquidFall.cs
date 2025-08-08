using ModLiquidLib.ModLoader;
using Terraria;

namespace ModLiquidExampleMod.Content.Waterfalls
{
	//An example of the ModLiquidFall class (although pretty empty here, a proper example will be made soon)
	public class ExampleLiquidFall : ModLiquidFall
	{
		//Removes the waterfall sound that waterfalls normally make.
		//useful for when the waterfall is not ment to make waterfall sounds
		public override bool PlayWaterfallSounds()
		{
			return false;
		}

		//Usually waterfalls draw as a slight opacity
		//Lava, Honey and shimmer all draw at a slight higher opacity than water
		//We can modify how strong the alpha is.
		//0 (un-see-able), 1 (fully opaque)
		public override float? Alpha(int x, int y, float Alpha, int maxSteps, int s, Tile tileCache)
		{
			float num = 1f; //the strength we usually want
			if (s > maxSteps - 10)
			{
				num *= (float)(maxSteps - s) / 10f; //modifies the strength based on how faded the waterfall is based on length
			}
			return num;
		}

		//We add light to our waterfall as the liquid tied to this fall also shines a bright white light
		public override void AddLight(int i, int j)
		{
			Lighting.AddLight(i, j, 1f, 1f, 1f);
		}
	}
}
