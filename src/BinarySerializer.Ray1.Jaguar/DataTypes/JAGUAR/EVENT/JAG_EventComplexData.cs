﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Ray1.Jaguar
{
	/// <summary>
	/// Event graphics block for some special events
	/// </summary>
	public class JAG_EventComplexData : BinarySerializable
	{
		public ushort Pre_Verbe { get; set; } // Read from MultiSprite
		public ushort Pre_SpritesCount { get; set; }

		public Pointer[] UnkPointers { get; set; }
		public byte[] UnkBytes { get; set; }
		public Pointer SpritesPointer { get; set; }
		public JAG_EventComplexDataTransition[] Transitions { get; set; }
		public JAG_EventComplexDataState[] States { get; set; }

		// Parsed
		public Sprite[] Sprites { get; set; }

		/// <summary>
		/// Handles the data serialization
		/// </summary>
		/// <param name="s">The serializer object</param>
		public override void SerializeImpl(SerializerObject s)
		{
			var settings = s.GetRequiredSettings<Ray1Settings>();

			if (settings.EngineVersion == Ray1EngineVersion.Jaguar_Proto && Pre_Verbe != 29)
				UnkPointers = s.SerializePointerArray(UnkPointers, 64, allowInvalid: true, name: nameof(UnkPointers));

			if (Pre_Verbe != 29)
				UnkBytes = s.SerializeArray<byte>(UnkBytes, 0x10, name: nameof(UnkBytes));

			SpritesPointer = s.SerializePointer(SpritesPointer, name: nameof(SpritesPointer));

			if (Pre_Verbe != 29) 
			{
				Transitions = s.SerializeObjectArray<JAG_EventComplexDataTransition>(Transitions, settings.EngineVersion == Ray1EngineVersion.Jaguar_Proto ? 5 : 7, onPreSerialize: g => {
					g.Pre_Verbe = Pre_Verbe;
					g.Pre_NumLayers = Pre_SpritesCount;
				}, name: nameof(Transitions));
			}

			// Serialize from first state index
			{
                // TODO: This isn't always accurate
				var temp = new List<JAG_EventComplexDataState>();

				var index = 0;
				while (true) {
					// Always check for pointer
					{
						Pointer CheckPtr0 = null;
						bool success = true;
						s.DoAt(s.CurrentPointer, () => {
							try {
								CheckPtr0 = s.SerializePointer(CheckPtr0, name: nameof(CheckPtr0));
							} catch (Exception) {
								success = false;
							}
						});
						if (!success
						|| (CheckPtr0 != null && CheckPtr0.File != Offset.File)) {
							break;
						} else if(CheckPtr0 != null) {
							// Can't check animation header, the frame pointer doesn't always point to the start of the actual animation
							/*byte[] CheckBytes = null;
							s.DoAt(CheckPtr0 - 4, () => {
								CheckBytes = s.SerializeArray<byte>(CheckBytes, 4, name: nameof(CheckBytes));
								if (CheckBytes[1] != 0 || CheckBytes[3] != 0
								|| CheckBytes[0] == 0 || CheckBytes[2] == 0) {
									// Padding should be padding, other values should be filled in
									success = false;
								}
							});*/
							if (!success) break;
						}
					}
					var i = s.SerializeObject<JAG_EventComplexDataState>(default, onPreSerialize: state => state.LayersPerFrame = Pre_SpritesCount, name: $"{nameof(States)}[{index}]");

					temp.Add(i);

					index++;
				}

				States = temp.ToArray();
			}

			if (settings.EngineVersion != Ray1EngineVersion.Jaguar_Proto)
			{
				s.DoAt(SpritesPointer, () => 
				{
					if (States != null && States.Length > 0)
					{
						int maxImageIndex = States
							.Where(x => x?.Layers != null)
							.SelectMany(x => x.Layers)
							.Max(x => /*UShort_12 == 5 ? BitHelpers.ExtractBits(x.ImageIndex, 7, 0) :*/ x.SpriteIndex);
						Sprites = s.SerializeObjectArray<Sprite>(Sprites, maxImageIndex + 1, name: nameof(Sprites));
					}
					else
					{
						var temp = new List<Sprite>();

						var index = 0;
						while (true)
						{
							var i = s.SerializeObject<Sprite>(default, name: $"{nameof(Sprites)}[{index}]");

							if (temp.Any() && i.Index != 0xFF && i.ImageBufferOffset < temp.Last().ImageBufferOffset)
								break;

							temp.Add(i);

							index++;
						}

						Sprites = temp.ToArray();
					}
				});
			}
		}
	}
}