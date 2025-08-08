using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLiquidExampleMod.Content.Waterfalls;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.WaterfallManager;

namespace ModLiquidExampleMod.Content.Tiles
{
	//This tile shows how to use the ILiquidModTile interface to create a custom cloud tiles
	//This tile copies over all information that makes up a rain/snow cloud tile
	public class HoneyCloud : ModTile, ILiquidModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoSunLight[Type] = false;
			Main.tileSolid[Type] = true;
			TileID.Sets.MergesWithClouds[Type] = true;
			TileID.Sets.Clouds[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.NegatesFallDamage[Type] = true;
			DustType = DustID.RainCloud;
		}

		public override bool HasWalkDust()
		{
			return true;
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
		{
			dustType = DustID.RainCloud;
			color = new(100, 150, 130, 100);
		}

		//When adding ILiquidModTile, this method is generated alongside
		//This method is used to spawn a waterfall after waterfalls have been processed
		//Liquids use this to spawn their waterfalls next to slabs/slopes
		//Clouds use this to spawn their rain/snow effects (which are waterfalls)
		public WaterfallData? CreateWaterfall(int i, int j)
		{
			Tile below = Main.tile[i, j + 1];
			if (below.Slope == 0 && !WorldGen.SolidTile(below)) //as long as below us the slope is normal and not solid...
			{
				return new() //we spawn a new waterfall with the following data
				{
					type = ModContent.GetInstance<HoneyRain>().Slot, //Honey Rain shows a customly drawn waterfall, render similarly to both to snow and rain
					x = i,
					y = j + 1
				};
			}
			return null; //otherwise we return null. Null makes the tile not spawn any waterfall, defaulting to the normal beaviour of most tiles
		}

		//Due to interfaces requiring all methods to be included in a class, this hook is also included here, depsite not being used/needed to be used
		//So we do absolutely nothing!
		public void DrawTileInWater(int i, int j, SpriteBatch spriteBatch)
		{
		}
	}
}
