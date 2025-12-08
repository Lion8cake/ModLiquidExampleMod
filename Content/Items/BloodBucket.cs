using ModLiquidExampleMod.Content.Liquids;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Items
{
	public class BloodBucket : ExampleBucketBase
	{
		public BloodBucket()
		{
			BucketLiquidType = ModContent.LiquidType<ExampleCustomMergeLiquid>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults(); //base is called so we have the original contents (shimmer, ID sets, etc) added as well
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WaterBucket;
			LiquidID.Sets.CreateLiquidBucketItem[ModContent.LiquidType<ExampleCustomMergeLiquid>()] = Type;
		}
	}
}
