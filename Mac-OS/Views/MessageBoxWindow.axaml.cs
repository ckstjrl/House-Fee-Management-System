using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace ManagementHouseFee_Avalonia.Views
{
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow()
        {
            InitializeComponent();
        }

        public MessageBoxWindow(string message) : this()
        {
            var textBlock = this.FindControl<TextBlock>("MessageText");
            if (textBlock != null) textBlock.Text = message;
        }

        private void Button_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        // 정적 헬퍼 메서드: 어디서든 MessageBoxWindow.Show(this, "내용") 처럼 부를 수 있게 함
        public static async Task Show(Window owner, string message)
        {
            var dialog = new MessageBoxWindow(message);
            if (owner != null)
            {
                await dialog.ShowDialog(owner);
            }
            else
            {
                dialog.Show();
            }
        }
    }
}