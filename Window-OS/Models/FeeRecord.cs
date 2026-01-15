using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ManagementHouseFee.Models
{
    [DataContract]
    public class FeeRecord
    {
        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public List<FeeItem> Items { get; set; } = new List<FeeItem>();

        // 화면 표시용 속성 (바인딩용)
        public string FullDateDisplay => $"{Year}년 {Month}월";
        public string DetailTitleDisplay => $"{Year}년 {Month}월 세부 내역";

        // [추가] 리스트에 "2026년 1월 2,000원" 형식으로 표시하기 위한 속성
        public string ListDisplay => $"{Year}년 {Month}월 {TotalAmount:N0}원";

        // 총액은 저장하지 않고, 불러올 때마다 항목들의 합으로 자동 계산
        public double TotalAmount => Items?.Sum(i => i.Amount) ?? 0;
    }
}