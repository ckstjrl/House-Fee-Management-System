using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagementHouseFee.Views
{
    public partial class AddRecordView : UserControl
    {
        public AddRecordView()
        {
            InitializeComponent();
        }

        // 포커스를 받았을 때 (Tab키로 이동했을 때 등) 전체 선택
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox?.SelectAll();
        }

        // 마우스로 클릭했을 때 처리
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;

            // 이미 포커스가 있는 상태가 아니라면 (처음 클릭했다면)
            if (textBox != null && !textBox.IsKeyboardFocusWithin)
            {
                e.Handled = true; // 1. 기본 클릭 동작(커서가 글자 사이에 박히는 것)을 막음
                textBox.Focus();  // 2. 강제로 포커스를 줌 -> 위쪽 'TextBox_GotFocus'가 실행됨
            }
        }
    }
}