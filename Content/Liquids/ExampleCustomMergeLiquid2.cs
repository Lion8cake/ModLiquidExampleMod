using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLiquidExampleMod.Content.Dusts;
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils.Structs;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Liquids
{
	//The second merge example for a modded liquid
	//here instead we prevent any merging all together, and the way to do this is super simple
	public class ExampleCustomMergeLiquid2 : ModLiquid
	{
		public override void SetStaticDefaults()
		{
			VisualViscosity = 200;
			LiquidFallLength = 4;
			DefaultOpacity = 0.75f;
			SlopeOpacity = 0.75f;
			SplashDustType = ModContent.DustType<BloodClotSplash>();
			SplashSound = SoundID.SplashWeak;
			AddMapEntry(new Color(100, 0, 0));
		}

		//We prevent any merging just by returning false, no sounds are played, no tiles or liquids are created
		//As this liquid is the result of 2 liquids merging, we want this liquid to not merge at all
		//This is so that this liquid isn't immediately turned into a tile when touching another liquid that was used to create this liquid
		public override bool PreLiquidMerge(int liquidX, int liquidY, int tileX, int tileY, int otherLiquid)
		{
			return false;
		}

		public override int ChooseWaterfallStyle(int i, int j)
		{
			return ModContent.GetInstance<BloodClotLiquidFall>().Slot;
		}

		public override void RetroDrawEffects(int i, int j, SpriteBatch spriteBatch, ref RetroLiquidDrawInfo drawData, float liquidAmountModified, int liquidGFXQuality)
		{
			drawData.liquidAlphaMultiplier *= 1.5f;
			if (drawData.liquidAlphaMultiplier > 1f)
			{
				drawData.liquidAlphaMultiplier = 1f;
			}
		}
	}
}
