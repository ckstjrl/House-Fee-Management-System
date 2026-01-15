using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using ManagementHouseFee.Models;
using ManagementHouseFee.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ManagementHouseFee.Views;

namespace ManagementHouseFee.ViewModels
{
    public partial class AddRecordViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        public List<int> Years { get; } = new List<int>();
        public List<int> Months { get; } = Enumerable.Range(1, 12).ToList();

        [ObservableProperty] private int _selectedYear;
        [ObservableProperty] private int _selectedMonth;

        [ObservableProperty] private ObservableCollection<FeeInputWrapper> _inputItems;
        [ObservableProperty] private SeriesCollection _pieSeries;

        // 실시간 총액을 화면에 바인딩하기 위한 속성
        [ObservableProperty] private string _totalAmountText;

        public AddRecordViewModel()
        {
            _dataService = new DataService();

            var now = DateTime.Now;
            for (int i = now.Year - 5; i <= now.Year + 1; i++) Years.Add(i);
            SelectedYear = now.Year;
            SelectedMonth = now.Month;

            RefreshItems();
        }

        private void RefreshItems()
        {
            var loadedNames = _dataService.LoadItems();

            InputItems = new ObservableCollection<FeeInputWrapper>();
            foreach (var name in loadedNames)
            {
                InputItems.Add(new FeeInputWrapper(name, 0, UpdateChart));
            }

            UpdateChart();
        }

        private void UpdateChart()
        {
            var series = new SeriesCollection();
            double total = 0; // 합계 계산용 변수

            if (InputItems != null)
            {
                foreach (var item in InputItems)
                {
                    total += item.Amount; // 실시간 금액 합산

                    if (item.Amount > 0)
                    {
                        series.Add(new PieSeries
                        {
                            Title = item.Name,
                            Values = new ChartValues<double> { item.Amount },
                            DataLabels = true,
                            LabelPoint = chartPoint => $"{chartPoint.Y:N0}원 ({chartPoint.Participation:P1})"
                        });
                    }
                }
            }
            PieSeries = series;

            // 계산된 합계를 문자열로 변환하여 속성에 저장
            TotalAmountText = $"총액 : {total:N0}원";
        }

        [RelayCommand]
        public void Save()
        {
            var newRecord = new FeeRecord
            {
                Year = SelectedYear,
                Month = SelectedMonth,
                Items = InputItems.Select(i => new FeeItem { Name = i.Name, Amount = i.Amount }).ToList()
            };

            var allRecords = _dataService.Load();
            var existing = allRecords.FirstOrDefault(r => r.Year == newRecord.Year && r.Month == newRecord.Month);
            if (existing != null) allRecords.Remove(existing);
            allRecords.Add(newRecord);

            _dataService.Save(allRecords);
            MessageBox.Show($"{SelectedYear}년 {SelectedMonth}월 관리비가 저장되었습니다!");
        }

        [RelayCommand]
        public void ManageItems()
        {
            var window = new ItemManagerWindow();
            window.ShowDialog();
            RefreshItems();
        }

        // 1. 연도가 변경될 때 호출
        partial void OnSelectedYearChanged(int value) => LoadRecordForSelectedDate();

        // 2. 월이 변경될 때 호출
        partial void OnSelectedMonthChanged(int value) => LoadRecordForSelectedDate();

        // 3. 날짜에 맞는 데이터를 로드하거나 초기화하는 핵심 로직
        private void LoadRecordForSelectedDate()
        {
            // 전체 데이터를 다시 로드
            var allRecords = _dataService.Load();

            // 현재 선택된 연/월에 해당하는 기록 찾기
            var existingRecord = allRecords.FirstOrDefault(r => r.Year == SelectedYear && r.Month == SelectedMonth);

            if (existingRecord != null)
            {
                // [케이스 1] 저장된 내역이 있는 경우: 저장된 금액으로 채우기
                var loadedItems = new ObservableCollection<FeeInputWrapper>();

                // 기준이 되는 항목 리스트를 가져와서 저장된 값이 있는지 매칭
                var itemNames = _dataService.LoadItems();
                foreach (var name in itemNames)
                {
                    var savedItem = existingRecord.Items.FirstOrDefault(i => i.Name == name);
                    double amount = savedItem?.Amount ?? 0; // 저장된 게 없으면 0
                    loadedItems.Add(new FeeInputWrapper(name, amount, UpdateChart));
                }
                InputItems = loadedItems;
            }
            else
            {
                // [케이스 2] 저장된 내역이 없는 경우: 모두 0으로 초기화
                RefreshItems();
            }

            // 차트와 총액 텍스트 갱신
            UpdateChart();
        }
    }
}