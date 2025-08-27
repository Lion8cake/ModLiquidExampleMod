using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria.ID;

namespace ModLiquidExampleMod.Content.Items
{
	public class CloudBucket : ExampleBucketBase
	{
		public CloudBucket()
		{
			BucketLiquidType = LiquidLoader.LiquidType<ExampleUpsideDownLiquid>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WaterBucket;
			LiquidID_TLmod.Sets.CreateLiquidBucketItem[LiquidLoader.LiquidType<ExampleUpsideDownLiquid>()] = Type;
		}
	}
}
