using Microsoft.Xna.Framework;
using ModLiquidLib.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Tiles
{
	//An example of a tile ignoring water movement for both normal updates and when creating a world
	public class ExampleGrate : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			LiquidID_TLmod.Sets.IgnoresWater.Add(Type); 
			LiquidID_TLmod.Sets.IgnoresWaterDuringWorldgen.Add(Type);
			DustType = DustID.Iron;
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(109, 88, 108));
		}
	}
}
