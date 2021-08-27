﻿using CitizenFX.Core;
using System;
using System.Drawing;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NativeUI
{
	public class Marker
	{
		internal float _height = 0;
		private float distance;

		public MarkerType MarkerType { get; set; }
		/// <summary>
		/// Hardcoded to be not more than 250units away.
		/// </summary>
		public float Distance { get => distance; set => distance = value > 250f ? 250f : value; }
		public Vector3 Position { get; set; }

		// this is optional and default to 0
		public Vector3 Direction { get; set; } = Vector3.Zero;
		// this is optional and default to 0
		public Vector3 Rotation { get; set; } = Vector3.Zero;

		public Vector3 Scale { get; set; } = new Vector3(1.5f);
		public Color Color { get; set; }
		public bool BobUpDown { get; set; }
		public bool Rotate { get; set; }
		public bool FaceCamera { get; set; }
		public bool IsInMarker { get; private set; }
		public bool IsInRange { get => MenuPool.PlayerPed.IsInRangeOf(Position, Distance); }
		/// <summary>
		/// It doesn't work on water and under the map!
		/// </summary>
		public bool PlaceOnGround { get; set; }

		/// <summary>
		/// Creates a Marker in a world position
		/// </summary>
		/// <param name="type">The type of marker</param>
		/// <param name="position">Position in world coords of the marker</param>
		/// <param name="distance">Drawing distance, if you're more distant than this value the marker won't be drawn </param>
		/// <param name="color">Color of the marker</param>
		/// <param name="bobUpDown">The marker will bounce up and down</param>
		/// <param name="rotate">The marker will rotate on its Z axiz</param>
		/// <param name="faceCamera">The marker will face camera</param>
		public Marker(MarkerType type, Vector3 position, float distance, Color color, bool placeOnGround = false, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Distance = distance;
			Color = color;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = faceCamera;
			if (Rotate && FaceCamera)
				Rotate = false;
		}

		/// <summary>
		/// Creates a Marker in a world position
		/// </summary>
		/// <param name="type">The type of marker</param>
		/// <param name="position">Position in world coords of the marker</param>
		/// <param name="scale">Dimensions of the marker</param>
		/// <param name="distance">Drawing distance, if you're more distant than this value the marker won't be drawn </param>
		/// <param name="color">Color of the marker</param>
		/// <param name="bobUpDown">The marker will bounce up and down</param>
		/// <param name="rotate">The marker will rotate on its Z axiz</param>
		/// <param name="faceCamera">The marker will face camera</param>
		public Marker(MarkerType type, Vector3 position, Vector3 scale, float distance, Color color, bool placeOnGround = false, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Scale = scale;
			Distance = distance;
			Color = color;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = faceCamera;
			if (Rotate && FaceCamera)
				Rotate = false;
		}

		public void Draw()
		{
			// [Position.Z != _height] means that we make the check only if we change position
			// but if we change position and the Z is still the same then we don't need to check again
			// We draw it with _height + 0.1 to ensure marker drawing (like horizontal circles)
			if (IsInRange && PlaceOnGround && Position.Z != _height)
			{
				if (GetGroundZFor_3dCoord(Position.X, Position.Y, Position.Z + 10f, ref _height, false) && _height != 0)
					Position = new Vector3(Position.X, Position.Y, _height + 0.1f);
			}
			World.DrawMarker(MarkerType, Position, Direction, Rotation, Scale, Color, BobUpDown, FaceCamera, Rotate);
			float dist = Vector3.Distance(Position, MenuPool.PlayerPed.Position);

			IsInMarker = dist <= (Scale / 2).X || dist <= (Scale / 2).Y || dist <= (Scale / 2).Z;
		}
	}
}