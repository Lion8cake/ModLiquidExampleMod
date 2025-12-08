using ModLiquidExampleMod.Content.Liquids;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Items
{
	public class BloodClotBucket : ExampleBucketBase
	{
		public BloodClotBucket()
		{
			BucketLiquidType = ModContent.LiquidType<ExampleCustomMergeLiquid2>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.HoneyBucket;
			LiquidID.Sets.CreateLiquidBucketItem[ModContent.LiquidType<ExampleCustomMergeLiquid2>()] = Type;
		}
	}
}
