using ManagementHouseFee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ManagementHouseFee.Services
{
    /// <summary>
    /// 관리비 데이터 및 사용자 정의 항목 리스트를 JSON 파일로 관리하는 서비스 클래스입니다.
    /// </summary>
    public class DataService
    {
        private readonly string _filePath;      // 관리비 내역 JSON 경로
        private readonly string _itemsFilePath; // 항목 리스트 JSON 경로

        public DataService()
        {
            // 실행 파일(.exe)이 있는 폴더 경로를 가져옵니다.
            string folder = AppDomain.CurrentDomain.BaseDirectory;

            // 각 데이터를 별도의 JSON 파일로 관리합니다.
            _filePath = Path.Combine(folder, "fee_data.json");
            _itemsFilePath = Path.Combine(folder, "items.json");
        }

        #region [관리비 내역 저장/로드]

        /// <summary>
        /// 월별 관리비 기록 리스트를 JSON 파일로 저장합니다.
        /// </summary>
        public void Save(List<FeeRecord> records)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<FeeRecord>));
            // FileMode.Create: 파일이 있으면 덮어쓰고, 없으면 새로 만듭니다.
            using (var stream = new FileStream(_filePath, FileMode.Create))
            {
                serializer.WriteObject(stream, records);
            }
        }

        /// <summary>
        /// JSON 파일로부터 저장된 관리비 내역을 불러옵니다.
        /// </summary>
        public List<FeeRecord> Load()
        {
            if (!File.Exists(_filePath)) return new List<FeeRecord>();

            var serializer = new DataContractJsonSerializer(typeof(List<FeeRecord>));
            // FileMode.Open: 기존 파일을 엽니다.
            using (var stream = new FileStream(_filePath, FileMode.Open))
            {
                var result = serializer.ReadObject(stream) as List<FeeRecord>;
                return result ?? new List<FeeRecord>();
            }
        }

        #endregion

        #region [항목 리스트 저장/로드]

        /// <summary>
        /// 사용자가 설정한 항목 이름 리스트(콤보박스용 등)를 저장합니다.
        /// </summary>
        public void SaveItems(List<string> items)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<string>));
            using (var stream = new FileStream(_itemsFilePath, FileMode.Create))
            {
                serializer.WriteObject(stream, items);
            }
        }

        /// <summary>
        /// 저장된 항목 리스트를 불러옵니다. 파일이 없으면 초기 기본값을 반환합니다.
        /// </summary>
        public List<string> LoadItems()
        {
            if (!File.Exists(_itemsFilePath))
            {
                // 파일이 없으면 기본 제공 카테고리 반환
                return new List<string> { "전기세", "수도세", "가스세", "관리비", "인터넷" };
            }

            var serializer = new DataContractJsonSerializer(typeof(List<string>));
            using (var stream = new FileStream(_itemsFilePath, FileMode.Open))
            {
                var result = serializer.ReadObject(stream) as List<string>;
                return result ?? new List<string>();
            }
        }

        #endregion
    }
}