using System.Windows.Controls;
using ManagementHouseFee.ViewModels; // 뷰모델 참조

namespace ManagementHouseFee.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            // 뷰모델 연결 (DataContext 설정)

            this.DataContext = new DashboardViewModel();
        }
    }
}