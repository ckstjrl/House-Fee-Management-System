using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ManagementHouseFee.Models;
using ManagementHouseFee.Views;
using ManagementHouseFee.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls; // UI 컨트롤 참조 (UserControl)

namespace ManagementHouseFee.ViewModels
{
    // ObservableObject: 데이터가 바뀌면 화면에 알려주는 기능 (MVVM 필수)
    public partial class MainViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        // 전체 관리비 데이터 리스트
        [ObservableProperty]
        private ObservableCollection<FeeRecord> _records;

        // 현재 보여줄 화면 (메인, 추가, 분석 등)
        [ObservableProperty]
        private object _currentView;

        public MainViewModel()
        {
            _dataService = new DataService();
            Records = new ObservableCollection<FeeRecord>(_dataService.Load());
            
            // 프로그램 켜자마자 대시보드 화면을 보여줌
            CurrentView = new DashboardView();
        }

        // 메뉴 이동 명령 (RelayCommand)
        [RelayCommand]
        public void Navigate(string destination)
        {
            switch (destination)
            {
                case "Main":
                    CurrentView = new DashboardView(); // 뷰모델은 뷰 내부에서 생성됨 (UserControl.DataContext)
                    break;
                case "Add":
                    var addVm = new AddRecordViewModel();
                    CurrentView = new AddRecordView { DataContext = addVm };
                    break;

                case "History":
                    CurrentView = new HistoryView { DataContext = new HistoryViewModel() };
                    break;

                case "Yearly":
                    CurrentView = new YearlyChartView();
                    break;
                case "Monthly":
                    CurrentView = new MonthlyChartView();
                    break;
                case "Item":
                    CurrentView = new ItemComparisonView();
                    break;
            }
        }
    }
}