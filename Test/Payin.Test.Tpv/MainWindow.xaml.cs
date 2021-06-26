using PayIn.TestTpv;
using System.Windows;

namespace Payin.Test.Tpv
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public PayinService Service;

		public MainWindow()
		{
			Service = new PayinService(new SessionData());

			InitializeComponent();
		}

		private async void Autenticar_Click(object sender, RoutedEventArgs e)
		{
			await Service.LoginAsync();
		}

		private async void Connect_Click(object sender, RoutedEventArgs e)
		{
			await Service.ConnectSignalR();
		}
	}
}
