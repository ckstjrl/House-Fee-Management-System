using ManagementHouseFee_Avalonia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization; // 이 네임스페이스가 필요할 수 있음

namespace ManagementHouseFee_Avalonia.Models
{
    public class FeeRecord
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public List<FeeItem> Items { get; set; } = new List<FeeItem>();

        // 저장하지 않을 속성 (계산용)에는 [JsonIgnore]를 붙여줍니다.
        [JsonIgnore]
        public string FullDateDisplay => $"{Year}년 {Month}월";

        [JsonIgnore]
        public string DetailTitleDisplay => $"{Year}년 {Month}월 세부 내역";

        [JsonIgnore]
        public string ListDisplay => $"{Year}년 {Month}월 {TotalAmount:N0}원";

        [JsonIgnore]
        public double TotalAmount => Items?.Sum(i => i.Amount) ?? 0;
    }
}