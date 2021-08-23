using CitizenFX.Core;
using System.Drawing;
using static CitizenFX.Core.Native.API;

namespace NativeUI
{
	public class Marker
	{
		public MarkerType MarkerType { get; set; }
		public float Distance { get; set; }
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
		/// Creates a Marker in a world position
		/// </summary>
		/// <param name="type">The type of marker</param>
		/// <param name="position">Position in world coords of the marker</param>
		/// <param name="distance">Drawing distance, if you're more distant than this value the marker won't be drawn </param>
		/// <param name="color">Color of the marker</param>
		/// <param name="bobUpDown">The marker will bounce up and down</param>
		/// <param name="rotate">The marker will rotate on its Z axiz</param>
		/// <param name="faceCamera">The marker will face camera</param>
		public Marker(MarkerType type, Vector3 position, float distance, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
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
		public Marker(MarkerType type, Vector3 position, Vector3 scale, float distance, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
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
			World.DrawMarker(MarkerType, Position, Direction, Rotation, Scale, Color, BobUpDown, FaceCamera, Rotate);
			var dist = Vector3.Distance(Position, MenuPool.PlayerPed.Position);
			IsInMarker = dist <= (Scale / 2).X || dist <= (Scale / 2).Y || dist <= (Scale / 2).Z;
		}
	}
}
