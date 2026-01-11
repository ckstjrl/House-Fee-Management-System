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
using System.Windows.Media;

namespace ManagementHouseFee.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private List<FeeRecord> _allRecords;

        // 차트 데이터
        [ObservableProperty] private SeriesCollection _chartSeries;
        [ObservableProperty] private string[] _chartLabels; // X축 라벨 (예: "25.01", "25.02")
        [ObservableProperty] private Func<double, string> _yAxisFormatter; // Y축 숫자 포맷

        // 요약 카드 데이터
        [ObservableProperty] private string _lastMonthTotal; // 저번달 관리비
        [ObservableProperty] private string _lastYearAverage; // 작년 월평균

        // 차트 타입 토글 (True: 꺾은선, False: 막대)
        [ObservableProperty]
        private bool _isLineChart = true;

        public DashboardViewModel()
        {
            _dataService = new DataService();
            _allRecords = _dataService.Load();

            // Y축 포맷 (10000 -> 10,000)
            YAxisFormatter = value => value.ToString("N0");

            LoadSummaryData();
            LoadChartData();
        }

        // 차트 타입 변경 시 자동으로 갱신
        partial void OnIsLineChartChanged(bool value)
        {
            LoadChartData();
        }

        private void LoadSummaryData()
        {
            var today = DateTime.Today;

            // 1. 저번달 관리비 찾기
            var lastMonthDate = today.AddMonths(-1);
            var lastMonthRecord = _allRecords.FirstOrDefault(r => r.Year == lastMonthDate.Year && r.Month == lastMonthDate.Month);

            if (lastMonthRecord != null)
                LastMonthTotal = $"{lastMonthRecord.TotalAmount:N0}원";
            else
                LastMonthTotal = "데이터 없음";

            // 2. 작년 월평균 찾기
            int lastYear = today.Year - 1;
            var lastYearRecords = _allRecords.Where(r => r.Year == lastYear).ToList();

            if (lastYearRecords.Any())
            {
                double avg = lastYearRecords.Average(r => r.TotalAmount);
                LastYearAverage = $"{avg:N0}원";
            }
            else
            {
                LastYearAverage = "데이터 없음";
            }
        }

        private void LoadChartData()
        {
            // 최근 6개월 데이터 추출
            var today = DateTime.Today;
            var values = new ChartValues<double>();
            var labels = new List<string>();

            // 5달 전부터 이번 달까지 (총 6개) 반복
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = today.AddMonths(-i);
                var record = _allRecords.FirstOrDefault(r => r.Year == targetDate.Year && r.Month == targetDate.Month);

                double amount = record?.TotalAmount ?? 0; // 데이터 없으면 0원

                values.Add(amount);
                labels.Add($"{targetDate.Year % 100}.{targetDate.Month:D2}"); // 예: 26.01
            }

            ChartLabels = labels.ToArray();

            // 그래프 타입에 따라 시리즈 생성
            if (IsLineChart)
            {
                ChartSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "관리비",
                        Values = values,
                        PointGeometrySize = 15,
                        LineSmoothness = 0, // 직선으로 꺾임
                        DataLabels = true
                    }
                };
            }
            else
            {
                ChartSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "관리비",
                        Values = values,
                        DataLabels = true,
                        MaxColumnWidth = 50
                    }
                };
            }
        }
    }
}