using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria.ID;

namespace ModLiquidExampleMod.Content.Items
{
	public class BloodClotBucket : ExampleBucketBase
	{
		public BloodClotBucket()
		{
			BucketLiquidType = LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.HoneyBucket;
			LiquidID_TLmod.Sets.CreateLiquidBucketItem[LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>()] = Type;
		}
	}
}
