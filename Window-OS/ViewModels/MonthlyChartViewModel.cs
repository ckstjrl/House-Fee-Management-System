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
            // 1. 선택된 연도의 데이터 필터링
            var targetRecords = _allRecords.Where(r => r.Year == SelectedYear).ToList();

            // 2. X축 라벨 생성 (월 + 해당 월 총액)
            var labelsWithTotals = new List<string>();
            for (int m = 1; m <= 12; m++)
            {
                var record = targetRecords.FirstOrDefault(r => r.Month == m);
                double monthTotal = record?.TotalAmount ?? 0;

                // 해당 월 밑에 총액 표시
                labelsWithTotals.Add($"{m}월\n({monthTotal:N0}원)");
            }
            Labels = labelsWithTotals.ToArray();

            // 3. 모든 항목 이름 찾기
            var allItemNames = _allRecords.SelectMany(r => r.Items)
                                          .Select(i => i.Name)
                                          .Distinct()
                                          .ToList();

            var seriesCollection = new SeriesCollection();

            // 항목별 루프 및 시리즈 생성 (기존 로직 유지)
            foreach (var itemName in allItemNames)
            {
                var values = new ChartValues<double>();

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
                        values.Add(0);
                    }
                }

                seriesCollection.Add(new StackedColumnSeries
                {
                    Title = itemName,
                    Values = values,
                    DataLabels = false,
                });
            }

            SeriesCollection = seriesCollection;
        }
    }
}