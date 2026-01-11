using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts; // 차트 코어
using LiveCharts.Wpf; // WPF 차트
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

        // 연도/월 선택용
        public List<int> Years { get; } = new List<int>();
        public List<int> Months { get; } = Enumerable.Range(1, 12).ToList();

        [ObservableProperty] private int _selectedYear;
        [ObservableProperty] private int _selectedMonth;

        // 좌측: 입력 리스트 (전기세, 수도세...)
        [ObservableProperty]
        private ObservableCollection<FeeInputWrapper> _inputItems;

        // 우측: 파이 차트 데이터
        [ObservableProperty]
        private SeriesCollection _pieSeries;

        public AddRecordViewModel()
        {
            _dataService = new DataService();

            // 날짜 초기화 (현재 날짜)
            var now = DateTime.Now;
            for (int i = now.Year - 5; i <= now.Year + 1; i++) Years.Add(i);
            SelectedYear = now.Year;
            SelectedMonth = now.Month;

            // [수정 포인트 1] 
            // 생성자 안에서 직접 리스트를 만드는 코드는 지우고, 
            // 바로 RefreshItems()를 호출하여 초기화합니다.
            // (DataService.LoadItems()가 파일이 없으면 알아서 기본값을 줍니다)
            RefreshItems();

        } 

        // [수정 포인트 2] 메서드는 생성자 밖으로 나와야 합니다.
        private void RefreshItems()
        {
            // 1. 파일에서 항목 이름들 불러오기
            var loadedNames = _dataService.LoadItems();

            InputItems = new ObservableCollection<FeeInputWrapper>();
            foreach (var name in loadedNames)
            {
                InputItems.Add(new FeeInputWrapper(name, 0, UpdateChart));
            }

            UpdateChart(); // 차트 초기화
        }

        // 차트 갱신 로직
        private void UpdateChart()
        {
            var series = new SeriesCollection();

            if (InputItems != null) // 혹시 null일 경우 대비
            {
                foreach (var item in InputItems)
                {
                    if (item.Amount > 0) // 금액이 0보다 큰 것만 차트에 표시
                    {
                        series.Add(new PieSeries
                        {
                            Title = item.Name,
                            Values = new ChartValues<double> { item.Amount },
                            DataLabels = true,
                            LabelPoint = chartPoint => $"{chartPoint.Y:N0}원 ({chartPoint.Participation:P1})" // 예: 10,000원 (25.0%)
                        });
                    }
                }
            }
            PieSeries = series;
        }

        // 저장 버튼 클릭 시
        [RelayCommand]
        public void Save()
        {
            // 1. 현재 입력된 데이터를 FeeRecord 모델로 변환
            var newRecord = new FeeRecord
            {
                Year = SelectedYear,
                Month = SelectedMonth,
                Items = InputItems.Select(i => new FeeItem { Name = i.Name, Amount = i.Amount }).ToList()
            };

            // 2. 기존 데이터 불러오기
            var allRecords = _dataService.Load();

            // 3. 중복 데이터 확인 (같은 연/월이 있으면 삭제하고 덮어쓰기)
            var existing = allRecords.FirstOrDefault(r => r.Year == newRecord.Year && r.Month == newRecord.Month);
            if (existing != null)
            {
                allRecords.Remove(existing);
            }
            allRecords.Add(newRecord);

            // 4. 저장
            _dataService.Save(allRecords);

            MessageBox.Show($"{SelectedYear}년 {SelectedMonth}월 관리비가 저장되었습니다!");
        }

        [RelayCommand]
        public void ManageItems()
        {
            // 1. 팝업창 생성 및 표시
            var window = new ItemManagerWindow();
            window.ShowDialog();

            // 2. 창이 닫히면 여기로 넘어옴 -> 리스트 새로고침
            RefreshItems();
        }
    }
}