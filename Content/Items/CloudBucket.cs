using ModLiquidExampleMod.Content.Liquids;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Items
{
	public class CloudBucket : ExampleBucketBase
	{
		public CloudBucket()
		{
			BucketLiquidType = ModContent.LiquidType<ExampleUpsideDownLiquid>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WaterBucket;
			LiquidID.Sets.CreateLiquidBucketItem[ModContent.LiquidType<ExampleUpsideDownLiquid>()] = Type;
		}
	}
}
