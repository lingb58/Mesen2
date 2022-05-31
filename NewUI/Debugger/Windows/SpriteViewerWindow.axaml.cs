using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using Mesen.Debugger.Controls;
using Mesen.Debugger.ViewModels;
using Mesen.Interop;
using System.ComponentModel;
using Avalonia.Input;
using Avalonia.Interactivity;
using Mesen.Debugger.Utilities;

namespace Mesen.Debugger.Windows
{
	public class SpriteViewerWindow : Window, INotificationHandler
	{
		private SpriteViewerViewModel _model;

		[Obsolete("For designer only")]
		public SpriteViewerWindow() : this(CpuType.Snes) { }

		public SpriteViewerWindow(CpuType cpuType)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif

			PictureViewer picViewer = this.FindControl<ScrollPictureViewer>("picViewer").InnerViewer;
			Grid spriteGrid = this.FindControl<Grid>("spriteGrid");
			var listView = this.FindControl<DataBoxControl.DataBox>("ListView");
			_model = new SpriteViewerViewModel(cpuType, picViewer, spriteGrid, listView, this);
			DataContext = _model;
		
			_model.Config.LoadWindowSettings(this);

			if(Design.IsDesignMode) {
				return;
			}

			MouseViewerModelEvents.InitEvents(_model, this, picViewer);
			picViewer.PositionClicked += PicViewer_PositionClicked;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		protected override void OnOpened(EventArgs e)
		{
			if(Design.IsDesignMode) {
				return;
			}

			_model.RefreshData();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			_model.Config.SaveWindowSettings(this);
		}

		private void PicViewer_PositionClicked(object? sender, PositionClickedEventArgs e)
		{
			PixelPoint p = e.Position;
			SpritePreviewModel? sprite = _model.GetMatchingSprite(p);
			_model.SelectedSprite = sprite;
			_model.UpdateSelection(sprite);
			e.Handled = true;
		}

		private void OnSettingsClick(object sender, RoutedEventArgs e)
		{
			_model.Config.ShowSettingsPanel = !_model.Config.ShowSettingsPanel;
		}

		public void ProcessNotification(NotificationEventArgs e)
		{
			if(e.NotificationType == ConsoleNotificationType.CodeBreak) {
				_model.ListView.ForceRefresh();
			}

			ToolRefreshHelper.ProcessNotification(this, e, _model.RefreshTiming, _model, _model.RefreshData);
		}
	}
}
