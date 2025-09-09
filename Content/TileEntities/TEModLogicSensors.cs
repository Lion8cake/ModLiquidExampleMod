using ModLiquidExampleMod.Content.Liquids;
using ModLiquidExampleMod.Content.Tiles;
using ModLiquidLib.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModLiquidExampleMod.Content.TileEntities
{
	//Here is the Tile Entity for sensors, ported from TELogicSensor to have modded sensors that detects modded liquids
	public class TEModLogicSensors : ModTileEntity
	{
		//This enum contains the types of all the avaliable sensors
		public enum LogicCheckType
		{
			None, //The default, never ment to be used, but a fallback to prevent issues with defaulting to a used type
			Example, //The type for ExampleLiquid
			Blood, //The type for ExampleCustomMergeLiquid
			BloodClot, //The type for ExampleCustomMergeLiquid2
			Cloud //The type for ExampleUpsideDownLiquid
		}

		private static List<Tuple<Point16, bool>> tripPoints = new List<Tuple<Point16, bool>>();

		private static List<int> markedIDsForRemoval = new List<int>();

		private static bool inUpdateLoop;

		public LogicCheckType logicCheck = LogicCheckType.None;

		public bool On = false;

		public int CountedData;

		public override void NetPlaceEntityAttempt(int x, int y)
		{
			NetPlaceEntity(x, y);
		}

		public void NetPlaceEntity(int x, int y)
		{
			int iD = Place(x, y);
			((TEModLogicSensors)ByID[iD]).FigureCheckState();
			NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, iD, x, y);
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			return ValidTile(x, y);
		}

		public override void PreGlobalUpdate()
		{
			inUpdateLoop = true;
			markedIDsForRemoval.Clear();
		}

		public override void Update()
		{
			bool state = GetState(Position.X, Position.Y, logicCheck, this);
			switch (logicCheck)
			{
				case LogicCheckType.Example:
				case LogicCheckType.Blood:
				case LogicCheckType.BloodClot:
				case LogicCheckType.Cloud:
					if (On != state)
					{
						ChangeState(state, TripWire: true);
					}
					break;
			}
		}

		public override void PostGlobalUpdate()
		{
			inUpdateLoop = false;
			foreach (Tuple<Point16, bool> tripPoint in tripPoints)
			{
				SoundEngine.PlaySound(SoundID.Mech, tripPoint.Item1.ToVector2() * 16f);
				Wiring.TripWire(tripPoint.Item1.X, tripPoint.Item1.Y, 1, 1);
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, tripPoint.Item1.X, tripPoint.Item1.Y);
				}
			}
			tripPoints.Clear();
			foreach (int item in markedIDsForRemoval)
			{
				if (ByID.TryGetValue(item, out var value) && value.type == Type)
				{
					lock (EntityCreationLock)
					{
						ByID.Remove(item);
						ByPosition.Remove(value.Position);
					}
				}
			}
			markedIDsForRemoval.Clear();
		}

		public void ChangeState(bool onState, bool TripWire)
		{
			if (onState == On || SanityCheck(Position.X, Position.Y))
			{
				Main.tile[Position.X, Position.Y].TileFrameX = (short)(onState ? 18 : 0);
				On = onState;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendTileSquare(-1, Position.X, Position.Y);
				}
			}
		}

		public static bool ValidTile(int x, int y)
		{
			if (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != ModContent.TileType<ExampleLiquidSensor>() || Main.tile[x, y].TileFrameY % 18 != 0 || Main.tile[x, y].TileFrameX % 18 != 0)
			{
				return false;
			}
			return true;
		}

		public static LogicCheckType FigureCheckType(int x, int y, out bool on)
		{
			on = false;
			if (!WorldGen.InWorld(x, y))
			{
				return LogicCheckType.None;
			}
			Tile tile = Main.tile[x, y];
			LogicCheckType result = LogicCheckType.None;
			switch (tile.TileFrameY / 18)
			{
				case 0:
					result = LogicCheckType.Example;
					break;
				case 1:
					result = LogicCheckType.Blood;
					break;
				case 2:
					result = LogicCheckType.BloodClot;
					break;
				case 3:
					result = LogicCheckType.Cloud;
					break;
			}
			on = GetState(x, y, result);
			return result;
		}

		public static bool GetState(int x, int y, LogicCheckType type, TEModLogicSensors instance = null)
		{
			switch (type)
			{
				case LogicCheckType.Example:
				case LogicCheckType.Blood:
				case LogicCheckType.BloodClot:
				case LogicCheckType.Cloud:
					{
						if (instance == null)
						{
							return false;
						}
						Tile tile = Main.tile[x, y];
						bool switched = false;
						if (tile.LiquidType == LiquidLoader.LiquidType<ExampleLiquid>() && type == LogicCheckType.Example)
						{
							switched = true;
						}
						if (tile.LiquidType == LiquidLoader.LiquidType<ExampleCustomMergeLiquid>() && type == LogicCheckType.Blood)
						{
							switched = true;
						}
						if (tile.LiquidType == LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>() && type == LogicCheckType.BloodClot)
						{
							switched = true;
						}
						if (tile.LiquidType == LiquidLoader.LiquidType<ExampleUpsideDownLiquid>() && type == LogicCheckType.Cloud)
						{
							switched = true;
						}
						if (tile.LiquidAmount == 0)
						{
							switched = false;
						}
						if (!switched && instance.On)
						{
							if (instance.CountedData == 0)
							{
								instance.CountedData = 15;
							}
							else if (instance.CountedData > 0)
							{
								instance.CountedData--;
							}
							switched = instance.CountedData > 0;
						}
						return switched;
					}
				default:
					return false;
			}
		}

		public void FigureCheckState()
		{
			logicCheck = FigureCheckType(Position.X, Position.Y, out On);
			GetFrame(Position.X, Position.Y, logicCheck, On);
		}

		public static void GetFrame(int x, int y, LogicCheckType type, bool on)
		{
			Tile tile = Main.tile[x, y];
			tile.TileFrameX = (short)(on ? 18 : 0);
			switch (type)
			{
				case LogicCheckType.Example:
					tile.TileFrameY = 0;
					break;
				case LogicCheckType.Blood:
					tile.TileFrameY = 18;
					break;
				case LogicCheckType.BloodClot:
					tile.TileFrameY = 36;
					break;
				case LogicCheckType.Cloud:
					tile.TileFrameY = 54;
					break;
				default:
					tile.TileFrameY = 0;
					break;
			}
		}

		public bool SanityCheck(int x, int y)
		{
			if (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != ModContent.TileType<ExampleLiquidSensor>())
			{
				Kill(x, y);
				return false;
			}
			return true;
		}

		public override int Hook_AfterPlacement(int x, int y, int type, int style, int direction, int alternate)
		{
			LogicCheckType logicCheckType = FigureCheckType(x, y, out bool on);
			GetFrame(x, y, logicCheckType, on);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, x, y, (int)Type);
				return -1;
			}
			int placeID = Place(x, y);
			((TEModLogicSensors)ByID[placeID]).FigureCheckState();
			return placeID;
		}

		public override void OnKill()
		{
			int x = Position.X;
			int y = Position.Y;
			if (!ByPosition.TryGetValue(new Point16(x, y), out var value) || value.type != Type)
			{
				return;
			}
			bool switched = false;
			if (((TEModLogicSensors)value).logicCheck == LogicCheckType.Example && ((TEModLogicSensors)value).On)
			{
				switched = true;
			}
			else if (((TEModLogicSensors)value).logicCheck == LogicCheckType.Blood && ((TEModLogicSensors)value).On)
			{
				switched = true;
			}
			else if (((TEModLogicSensors)value).logicCheck == LogicCheckType.BloodClot && ((TEModLogicSensors)value).On)
			{
				switched = true;
			}
			else if (((TEModLogicSensors)value).logicCheck == LogicCheckType.Cloud && ((TEModLogicSensors)value).On)
			{
				switched = true;
			}
			if (switched)
			{
				Wiring.HitSwitch(value.Position.X, value.Position.Y);
				NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, value.Position.X, value.Position.Y);
			}
			if (inUpdateLoop)
			{
				markedIDsForRemoval.Add(value.ID);
				return;
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((byte)logicCheck);
			writer.Write(On);
		}

		public override void NetReceive(BinaryReader reader)
		{
			logicCheck = (LogicCheckType)reader.ReadByte();
			On = reader.ReadBoolean();
		}

		public override void LoadData(TagCompound tag)
		{
			logicCheck = (LogicCheckType)tag.GetByte(nameof(logicCheck));
			On = tag.GetBool(nameof(On));
		}

		public override void SaveData(TagCompound tag)
		{
			tag[nameof(logicCheck)] = (byte)logicCheck;
			tag[nameof(On)] = On;
		}

		public override string ToString()
		{
			return Position.X + "x  " + Position.Y + "y " + logicCheck;
		}
	}
}
