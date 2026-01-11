using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;
using ManagementHouseFee.Models;
using ManagementHouseFee.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ManagementHouseFee.ViewModels
{
    public partial class MonthlyChartViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private List<FeeRecord> _allRecords;

        public List<int> Years { get; } = new List<int>();

        [ObservableProperty]
        private int _selectedYear; // 선택된 연도

        [ObservableProperty] private SeriesCollection _seriesCollection;
        [ObservableProperty] private string[] _labels;
        [ObservableProperty] private Func<double, string> _formatter;

        public MonthlyChartViewModel()
        {
            _dataService = new DataService();
            _allRecords = _dataService.Load();
            Formatter = value => value.ToString("N0");

            // X축 라벨은 1월~12월 고정
            Labels = Enumerable.Range(1, 12).Select(m => $"{m}월").ToArray();

            // 연도 목록 초기화
            var now = DateTime.Now;
            for (int i = now.Year - 5; i <= now.Year + 1; i++) Years.Add(i);
            _selectedYear = now.Year; // 기본값: 올해

            LoadChartData();
        }

        // 연도 바꾸면 차트 다시 그리기
        partial void OnSelectedYearChanged(int value) => LoadChartData();

        private void LoadChartData()
        {
            // 1. 선택된 연도의 데이터만 필터링
            var targetRecords = _allRecords.Where(r => r.Year == SelectedYear).ToList();

            // 2. 모든 항목 이름 찾기
            var allItemNames = _allRecords.SelectMany(r => r.Items)
                                          .Select(i => i.Name)
                                          .Distinct()
                                          .ToList();

            var seriesCollection = new SeriesCollection();

            // 3. 항목별로 루프 (전기세, 수도세...)
            foreach (var itemName in allItemNames)
            {
                var values = new ChartValues<double>();

                // 1월부터 12월까지 루프
                for (int month = 1; month <= 12; month++)
                {
                    var record = targetRecords.FirstOrDefault(r => r.Month == month);
                    if (record != null)
                    {
                        var item = record.Items.FirstOrDefault(i => i.Name == itemName);
                        values.Add(item?.Amount ?? 0);
                    }
                    else
                    {
                        values.Add(0); // 데이터 없는 달은 0
                    }
                }

                seriesCollection.Add(new StackedColumnSeries
                {
                    Title = itemName,
                    Values = values,
                    DataLabels = false // 막대그래프에 숫자 표시 X
                });
            }

            SeriesCollection = seriesCollection;
        }
    }
}