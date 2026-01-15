using CommunityToolkit.Mvvm.ComponentModel;
using ManagementHouseFee.Models;
using ManagementHouseFee.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace ManagementHouseFee.ViewModels
{
    public partial class ItemComparisonViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private List<FeeRecord> _allRecords;

        // 날짜 선택
        public List<int> Years { get; } = new List<int>();
        public List<int> Months { get; } = Enumerable.Range(1, 12).ToList();

        [ObservableProperty] private int _selectedYear;
        [ObservableProperty] private int _selectedMonth;

        // 화면에 뿌려줄 분석 리스트
        [ObservableProperty]
        private ObservableCollection<ComparisonItem> _comparisonList;

        public ItemComparisonViewModel()
        {
            _dataService = new DataService();
            _allRecords = _dataService.Load();

            // 날짜 초기화 (현재 날짜)
            var now = DateTime.Now;
            for (int i = now.Year - 5; i <= now.Year + 1; i++) Years.Add(i);

            _selectedYear = now.Year;
            _selectedMonth = now.Month;

            AnalyzeData(); // 처음에 한 번 실행
        }

        // 연도나 월이 바뀌면 자동으로 분석 다시 실행
        partial void OnSelectedYearChanged(int value) => AnalyzeData();
        partial void OnSelectedMonthChanged(int value) => AnalyzeData();

        private void AnalyzeData()
        {
            ComparisonList = new ObservableCollection<ComparisonItem>();

            // 1. 선택한 달의 데이터 찾기
            var currentRecord = _allRecords.FirstOrDefault(r => r.Year == SelectedYear && r.Month == SelectedMonth);

            // 데이터가 없으면 아무것도 안 함
            if (currentRecord == null || currentRecord.Items == null) return;

            // 2. 비교 대상 찾기 (지난달, 작년 동월)
            var prevMonthDate = new DateTime(SelectedYear, SelectedMonth, 1).AddMonths(-1);
            var prevMonthRecord = _allRecords.FirstOrDefault(r => r.Year == prevMonthDate.Year && r.Month == prevMonthDate.Month);

            var prevYearRecord = _allRecords.FirstOrDefault(r => r.Year == (SelectedYear - 1) && r.Month == SelectedMonth);

            // 3. 각 항목별로 루프 돌면서 계산
            foreach (var item in currentRecord.Items)
            {
                var compItem = new ComparisonItem
                {
                    Name = item.Name,
                    CurrentAmount = item.Amount
                };

                // 지난달 비교 로직
                double prevMonthAmount = prevMonthRecord?.Items.FirstOrDefault(i => i.Name == item.Name)?.Amount ?? 0;
                compItem.PrevMonthMessage = GetComparisonString(item.Amount, prevMonthAmount, "지난달");
                compItem.PrevMonthColor = GetColor(item.Amount, prevMonthAmount);

                // 작년 동월 비교 로직
                double prevYearAmount = prevYearRecord?.Items.FirstOrDefault(i => i.Name == item.Name)?.Amount ?? 0;
                compItem.PrevYearMessage = GetComparisonString(item.Amount, prevYearAmount, "작년");
                compItem.PrevYearColor = GetColor(item.Amount, prevYearAmount);

                ComparisonList.Add(compItem);
            }
        }

        // "000원 (00%) 증가" 문자열 만들어주는 함수
        private string GetComparisonString(double current, double prev, string targetName)
        {
            if (prev == 0) return $"{targetName} 데이터 없음";

            double diff = current - prev;
            double percent = (diff / prev); // 0.1 = 10%

            string sign = diff > 0 ? "증가 🔺" : "감소 🔻";
            string diffText = Math.Abs(diff).ToString("N0");

            if (diff == 0) return $"{targetName}과 동일";

            return $"{targetName} 대비 {diffText}원 ({percent:P1}) {sign}";
        }

        // 색상 결정 (증가는 빨강, 감소는 파랑)
        private Brush GetColor(double current, double prev)
        {
            if (prev == 0 || current == prev) return Brushes.Gray;
            return current > prev ? Brushes.Red : Brushes.Blue;
        }
    }
}