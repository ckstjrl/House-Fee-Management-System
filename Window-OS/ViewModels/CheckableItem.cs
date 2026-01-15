using CommunityToolkit.Mvvm.ComponentModel;
using System.Xml.Linq;

namespace ManagementHouseFee.ViewModels
{
    // 리스트박스에 들어갈 항목 하나하나를 담당하는 친구입니다.
    public partial class CheckableItem : ObservableObject
    {
        [ObservableProperty]
        private string _name; // 항목 이름 (예: 주차비)

        [ObservableProperty]
        private bool _isChecked; // 체크박스 체크 여부

        public CheckableItem(string name)
        {
            Name = name;
            IsChecked = false; // 처음엔 체크 해제 상태
        }
    }
}