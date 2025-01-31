﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace NativeUI
{
	public class UIMenuPercentagePanel : UIMenuPanel
	{

		private UIResRectangle ActiveBar;
		private UIResRectangle BackgroundBar;
		private UIMenuGridAudio Audio;
		private UIResText Min;
		private UIResText Max;
		private UIResText Title;
		private readonly PointF safezoneOffset = ScreenTools.SafezoneBounds;
		private bool Pressed;
		public float Percentage
		{
			get
			{
				float progress = (float)Math.Round(API.GetDisabledControlNormal(0, 239) * Resolution.Width) - ActiveBar.Position.X;
				return (float)Math.Round(((progress >= 0 && progress <= 413) ? progress : ((progress < 0) ? 0 : 413)) / Background.Size.Width, 2);
			}
			set
			{
				float percent = (value < 0.0f) ? 0.0f : (value > 1f) ? 1.0f : value;
				ActiveBar.Size = new SizeF(BackgroundBar.Size.Width * percent, ActiveBar.Size.Height);
			}
		}


		public UIMenuPercentagePanel(string title, string MinText, string MaxText)
		{
			Enabled = true;
			Background = new Sprite("commonmenu", "gradient_bgd", new Point(0, 0), new Size(431, 275));
			ActiveBar = new UIResRectangle(new Point(0, 0), new Size(413, 10), Color.FromArgb(245, 245, 245));
			BackgroundBar = new UIResRectangle(new Point(0, 0), new Size(413, 10), Color.FromArgb(80, 80, 80));
			Min = new UIResText(MinText != "" || MinText != null ? MinText : "0%", new Point(0, 0), .35f, Color.FromArgb(255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, Alignment.Center);
			Max = new UIResText(MaxText != "" || MaxText != null ? MaxText : "100%", new Point(0, 0), .35f, Color.FromArgb(255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, Alignment.Center);
			Title = new UIResText(title != "" || title != null ? title : "Opacity", new Point(0, 0), .35f, Color.FromArgb(255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, Alignment.Center);
			Audio = new UIMenuGridAudio("CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
		}

		internal override void Position(float y)
		{
			float ParentOffsetX = ParentItem.Offset.X;
			int ParentOffsetWidth = ParentItem.Parent.WidthOffset;
			Background.Position = new PointF(ParentOffsetX, 35f + y);
			ActiveBar.Position = new PointF(ParentOffsetX + (ParentOffsetWidth / 2) + 9, 50 + 35f + y);
			BackgroundBar.Position = ActiveBar.Position;
			Min.Position = new PointF(ParentOffsetX + (ParentOffsetWidth / 2) + 25, 15 + 35f + y);
			Max.Position = new PointF(ParentOffsetX + (ParentOffsetWidth / 2) + 398, 15 + 35f + y);
			Title.Position = new PointF(ParentOffsetX + (ParentOffsetWidth / 2) + 215.5f, 15 + 35f + y);
		}

		public void UpdateParent(float Percentage)
		{
			ParentItem.Parent.ListChange(ParentItem, ParentItem.Index);
			ParentItem.ListChangedTrigger(ParentItem.Index);
		}

		private async void Functions()
		{
			if (ScreenTools.IsMouseInBounds(new PointF(BackgroundBar.Position.X + safezoneOffset.X, BackgroundBar.Position.Y - 4f + safezoneOffset.Y), new SizeF(BackgroundBar.Size.Width, BackgroundBar.Size.Height + 8f)))
			{
				if (API.IsDisabledControlPressed(0, 24))
				{
					if (!Pressed)
					{
						Pressed = true;
						Audio.Id = API.GetSoundId();
						API.PlaySoundFrontend(Audio.Id, Audio.Slider, Audio.Library, true);
					}
					await BaseScript.Delay(0);
					float Progress = API.GetDisabledControlNormal(0, 239) * Resolution.Width;
					Progress -= ActiveBar.Position.X + safezoneOffset.X;
					ActiveBar.Size = new SizeF(Progress >= 0 && Progress <= 413 ? Progress : (Progress < 0 ? 0 : 413), ActiveBar.Size.Height);
					UpdateParent((float)Math.Round(Progress >= 0 && Progress <= 413 ? Progress : (Progress < 0 ? 0 : 413) / BackgroundBar.Size.Width, 2));
				}
				else
				{
					API.StopSound(Audio.Id);
					API.ReleaseSoundId(Audio.Id);
					Pressed = false;

				}
			}
			else
			{
				API.StopSound(Audio.Id);
				API.ReleaseSoundId(Audio.Id);
				Pressed = false;
			}
		}

		internal async override Task Draw()
		{
			if (Enabled)
			{
				Background.Size = new Size(431 + ParentItem.Parent.WidthOffset, 76);
				Background.Draw();
				BackgroundBar.Draw();
				ActiveBar.Draw();
				Min.Draw();
				Max.Draw();
				Title.Draw();
				Functions();
			}
			await Task.FromResult(0);
		}
	}
}
