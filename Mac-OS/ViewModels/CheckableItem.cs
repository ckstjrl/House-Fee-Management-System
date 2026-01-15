using CommunityToolkit.Mvvm.ComponentModel;

namespace ManagementHouseFee_Avalonia.ViewModels
{
    public partial class CheckableItem : ObservableObject
    {
        [ObservableProperty]
        private string _name; // 항목 이름 (예: 주차비)

        [ObservableProperty]
        private bool _isChecked; // 체크박스 체크 여부

        public CheckableItem(string name)
        {
            Name = name;
            IsChecked = false;
        }
    }
}