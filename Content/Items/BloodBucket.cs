using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.ModLoader;
using Terraria.ID;

namespace ModLiquidExampleMod.Content.Items
{
	public class BloodBucket : ExampleBucketBase
	{
		public BloodBucket()
		{
			BucketLiquidType = LiquidLoader.LiquidType<ExampleCustomMergeLiquid>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults(); //base is called so we have the original contents (shimmer, ID sets, etc) added as well
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WaterBucket;
		}
	}
}
