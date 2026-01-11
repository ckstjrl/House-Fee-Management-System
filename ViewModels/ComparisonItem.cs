using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media; // 색상(Brush) 사용

namespace ManagementHouseFee.ViewModels
{
    public class ComparisonItem : ObservableObject
    {
        public string Name { get; set; } // 항목명 (전기세)
        public double CurrentAmount { get; set; } // 이번달 금액

        // 화면에 보여줄 메시지 (예: 지난달 대비 3,000원 증가)
        public string PrevMonthMessage { get; set; }
        public Brush PrevMonthColor { get; set; } // 빨강(증가) or 파랑(감소)

        public string PrevYearMessage { get; set; }
        public Brush PrevYearColor { get; set; }
    }
}