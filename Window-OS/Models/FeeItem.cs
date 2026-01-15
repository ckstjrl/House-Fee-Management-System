using System.Runtime.Serialization;

namespace ManagementHouseFee.Models
{
    /// <summary>
    /// 관리비 항목 하나를 나타내는 클래스입니다.
    /// [DataContract]를 사용하여 이 클래스가 직렬화 대상임을 명시합니다.
    /// </summary>
    
    [DataContract] // JSON으로 저장될 객체임을 명시
    public class FeeItem
    {
        /// <summary>
        /// 관리비 항목의 이름 (예: "일반관리비", "전기료", "수도료")
        /// [DataMember]가 붙어 있어야 이 데이터가 파일에 저장되거나 전송됩니다.
        /// </summary>
        
        [DataMember] // JSON의 필드로 저장
        public string Name { get; set; } // 항목명

        /// <summary>
        /// 해당 항목의 금액 (예: 15000.0)
        /// 마찬가지로 [DataMember]를 통해 직렬화 대상에 포함시킵니다.
        /// </summary>
        [DataMember]
        public double Amount { get; set; } // 금액
    }
}