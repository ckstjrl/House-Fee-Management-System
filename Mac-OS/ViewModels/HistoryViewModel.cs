using CommunityToolkit.Mvvm.ComponentModel;
using ManagementHouseFee_Avalonia.Models;
using ManagementHouseFee_Avalonia.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace ManagementHouseFee_Avalonia.ViewModels
{
    public partial class HistoryViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private ObservableCollection<FeeRecord> _allRecords = new();

        [ObservableProperty]
        private FeeRecord? _selectedRecord; // 리스트에서 선택된 데이터

        public HistoryViewModel()
        {
            _dataService = new DataService();
            LoadData();
        }

        private void LoadData()
        {
            // 최신순(연도 내림차순, 월 내림차순)으로 정렬하여 로드
            var data = _dataService.Load()
                                   .OrderByDescending(r => r.Year)
                                   .ThenByDescending(r => r.Month)
                                   .ToList();
            AllRecords = new ObservableCollection<FeeRecord>(data);
        }
    }
}