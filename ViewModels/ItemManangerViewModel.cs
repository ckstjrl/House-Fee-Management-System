using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ManagementHouseFee.Services;
using System.Collections.Generic; // List용
using System.Collections.ObjectModel;
using System.Linq; 
using System.Windows;

namespace ManagementHouseFee.ViewModels
{
    public partial class ItemManagerViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        // string -> CheckableItem 으로 변경
        [ObservableProperty]
        private ObservableCollection<CheckableItem> _items;

        [ObservableProperty]
        private string _newItemName;
 

        public ItemManagerViewModel()
        {
            _dataService = new DataService();
            LoadData();
        }

        private void LoadData()
        {
            var loadedStrings = _dataService.LoadItems();
            Items = new ObservableCollection<CheckableItem>();

            // 불러온 문자열들을 CheckableItem으로 포장해서 넣기
            foreach (var name in loadedStrings)
            {
                Items.Add(new CheckableItem(name));
            }
        }

        [RelayCommand]
        public void Add()
        {
            if (string.IsNullOrWhiteSpace(NewItemName)) return;

            // 중복 체크
            if (Items.Any(i => i.Name == NewItemName))
            {
                MessageBox.Show("이미 존재하는 항목입니다.");
                return;
            }

            // 새 항목 추가 (체크박스 객체로 만들어서)
            Items.Add(new CheckableItem(NewItemName));
            NewItemName = ""; // 입력창 비우기
            Save();
        }

        // 체크된 항목들을 일괄 삭제
        [RelayCommand]
        public void Delete()
        {
            // 체크된 항목들만 골라냅니다.
            var toDelete = Items.Where(i => i.IsChecked).ToList();

            if (toDelete.Count == 0)
            {
                MessageBox.Show("삭제할 항목을 체크해주세요.");
                return;
            }

            // 확인 메시지
            var result = MessageBox.Show($"{toDelete.Count}개의 항목을 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            // 리스트에서 제거
            foreach (var item in toDelete)
            {
                Items.Remove(item);
            }

            Save();
        }

        private void Save()
        {
            // 저장할 때는 다시 문자열(string) 리스트로 변환해서 저장해야 합니다.
            var stringList = Items.Select(i => i.Name).ToList();
            _dataService.SaveItems(stringList);
        }
    }
}