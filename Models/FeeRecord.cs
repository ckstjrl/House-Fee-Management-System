using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ManagementHouseFee.Models
{
    /// <summary>
    /// 특정 연도와 월의 전체 관리비 기록을 담는 클래스입니다.
    /// [DataContract] 선언을 통해 이 객체 전체가 저장 및 전송 가능함을 명시합니다.
    /// </summary>
    [DataContract]
    public class FeeRecord
    {
        /// <summary>
        /// 관리비가 청구된 연도 (예: 2023)
        /// [DataMember]가 있으므로 파일에 저장됩니다.
        /// </summary>
        [DataMember]
        public int Year { get; set; } // 해당 연도

        /// <summary>
        /// 관리비가 청구된 월 (예: 10)
        /// [DataMember]가 있으므로 파일에 저장됩니다.
        /// </summary>
        [DataMember]
        public int Month { get; set; } // 해당 월


        /// <summary>
        /// 세부 관리비 항목 리스트 (예: 전기료, 수도료 등의 목록)
        /// FeeItem 객체들을 리스트 형태로 저장합니다.
        /// </summary>
        [DataMember]
        public List<FeeItem> Items { get; set; } = new List<FeeItem>(); // 세부 항목 리스트

        /// <summary>
        /// 관리비 총액 (계산 속성)
        /// *중요*: [DataMember]가 없으므로 이 값은 파일에 따로 저장되지 않습니다.
        /// 대신 데이터를 불러오거나 사용할 때마다 Items 리스트의 합계를 즉석에서 계산합니다.
        /// </summary>
        public double TotalAmount => Items?.Sum(i => i.Amount) ?? 0;
        // 총액은 저장하지 않고, 불러올 때마다 항목들의 합으로 자동 계산 (읽기 전용)
    }
}