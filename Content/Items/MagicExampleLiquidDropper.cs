using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using ModLiquidExampleMod.Content.Tiles;
using Terraria.ID;
using ModLiquidLib.ModLoader;
using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.Utils;

namespace ModLiquidExampleMod.Content.Items
{
	public class MagicExampleLiquidDropper : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<ExampleDrip>();
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 0, 40);
		}

		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(Type);
			recipe.AddIngredient(ItemID.EmptyDropper);
			recipe.AddLiquid<ExampleLiquid>();
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}
