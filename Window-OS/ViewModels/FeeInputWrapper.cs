using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Xml.Linq;

namespace ManagementHouseFee.ViewModels
{
    // 입력 화면에서 항목 하나(예: 전기세)를 담당하는 뷰모델
    public partial class FeeInputWrapper : ObservableObject
    {
        [ObservableProperty]
        private string _name; // 항목 이름

        [ObservableProperty]
        private double _amount; // 사용자가 입력한 금액

        // 금액이 바뀔 때마다 실행될 행동(차트 갱신)을 저장하는 변수
        private readonly Action _onAmountChanged;

        public FeeInputWrapper(string name, double amount, Action onAmountChanged)
        {
            Name = name;
            Amount = amount;
            _onAmountChanged = onAmountChanged;
        }

        // Amount가 바뀔 때마다 호출됨 (CommunityToolkit 기능)
        partial void OnAmountChanged(double value)
        {
            _onAmountChanged?.Invoke(); // 부모 뷰모델에게 "값 바뀌었으니 차트 다시 그려!"라고 알림
        }
    }
}