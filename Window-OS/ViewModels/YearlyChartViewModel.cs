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
    public partial class YearlyChartViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private List<FeeRecord> _allRecords;

        // 차트 데이터
        [ObservableProperty] private SeriesCollection _seriesCollection;
        [ObservableProperty] private string[] _labels; // X축 (2024, 2025...)
        [ObservableProperty] private Func<double, string> _formatter; // Y축 숫자 포맷

        public YearlyChartViewModel()
        {
            _dataService = new DataService();
            _allRecords = _dataService.Load();
            Formatter = value => value.ToString("N0"); // 10,000 형태

            LoadChartData();
        }

        private void LoadChartData()
        {
            // 1. 데이터가 존재하는 모든 연도 찾기 (오름차순)
            var years = _allRecords.Select(r => r.Year).Distinct().OrderBy(y => y).ToList();

            // 2. X축 라벨 생성 (연도 + 연 평균 총액)
            var labelsWithTotals = new List<string>();
            foreach (var year in years)
            {
                var recordsInYear = _allRecords.Where(r => r.Year == year).ToList();

                // 해당 연도의 월 평균 총액 계산
                double avgTotal = recordsInYear.Any() ? recordsInYear.Average(r => r.TotalAmount) : 0;

                // \n을 사용하여 연도 밑에 금액이 나오도록 구성
                labelsWithTotals.Add($"{year}년\n({avgTotal:N0}원)");
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

                foreach (var year in years)
                {
                    var recordsInYear = _allRecords.Where(r => r.Year == year).ToList();
                    if (recordsInYear.Any())
                    {
                        double sum = recordsInYear.SelectMany(r => r.Items)
                                                  .Where(i => i.Name == itemName)
                                                  .Sum(i => i.Amount);
                        double avg = sum / recordsInYear.Count;
                        values.Add(avg);
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