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
    }
}