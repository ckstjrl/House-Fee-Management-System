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
            Labels = years.Select(y => y.ToString()).ToArray();

            // 2. 모든 항목 이름 찾기 (전기세, 수도세 등 중복 제거)
            // (데이터에 한 번이라도 등장한 적 있는 항목 이름들을 다 모읍니다)
            var allItemNames = _allRecords.SelectMany(r => r.Items)
                                          .Select(i => i.Name)
                                          .Distinct()
                                          .ToList();

            // 3. 차트 시리즈 생성 (항목별로 Series를 만듦)
            var seriesCollection = new SeriesCollection();

            foreach (var itemName in allItemNames)
            {
                var values = new ChartValues<double>();

                foreach (var year in years)
                {
                    // 해당 연도의 모든 기록 가져오기
                    var recordsInYear = _allRecords.Where(r => r.Year == year).ToList();

                    if (recordsInYear.Any())
                    {
                        // 해당 연도, 해당 항목의 총합 계산
                        double sum = recordsInYear.SelectMany(r => r.Items)
                                                  .Where(i => i.Name == itemName)
                                                  .Sum(i => i.Amount);

                        // 월 평균 계산 (총합 / 기록된 개월 수)
                        double avg = sum / recordsInYear.Count;
                        values.Add(avg);
                    }
                    else
                    {
                        values.Add(0);
                    }
                }

                // 스택(Stacked) 막대 그래프 추가
                seriesCollection.Add(new StackedColumnSeries
                {
                    Title = itemName, // 범례 이름 (전기세 등)
                    Values = values,
                    DataLabels = false, // 막대 위에 숫자 표시X

                    LabelPoint = point => $"{point.Y:NO} ({point.Participation:P1})"
                });
            }

            SeriesCollection = seriesCollection;
        }
    }
}