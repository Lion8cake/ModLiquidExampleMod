using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Items
{
	public class ExampleGrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Grate;
			ItemID.Sets.ShimmerTransformToItem[ItemID.Grate] = Type;
		}

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<Tiles.ExampleGrate>();
			Item.width = 16;
			Item.height = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
		}
	}
}
