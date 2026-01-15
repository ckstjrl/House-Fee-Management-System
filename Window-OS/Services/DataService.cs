using ManagementHouseFee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ManagementHouseFee.Services
{
    public class DataService
    {
        // 파일이 저장될 경로 (실행 파일과 같은 폴더의 fee_data.json)
        private readonly string _filePath; 

        public DataService()// 실행 파일 경로에 'fee_data.json'으로 저장 경로 설정
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(folder, "fee_data.json");
        }

        // 데이터 저장 (직렬화)
        public void Save(List<FeeRecord> records)
        {
            // List<FeeRecord> 타입을 처리하는 도구 생성
            var serializer = new DataContractJsonSerializer(typeof(List<FeeRecord>));

            // 파일을 생성 모드로 열고 쓰기
            using (var stream = new FileStream(_filePath, FileMode.Create))
            {
                serializer.WriteObject(stream, records);
            }
        }

        // 데이터 불러오기 (역직렬화)
        public List<FeeRecord> Load()
        {
            if (!File.Exists(_filePath)) return new List<FeeRecord>();

            var serializer = new DataContractJsonSerializer(typeof(List<FeeRecord>));

            using (var stream = new FileStream(_filePath, FileMode.Open))
            {
                // 읽어온 데이터를 List<FeeRecord>로 형변환
                var result = serializer.ReadObject(stream) as List<FeeRecord>;
                return result ?? new List<FeeRecord>();
            }
        }
        // 항목 정의 파일 경로
        private string _itemsFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "items.json");

        // 1. 항목 리스트 저장
        public void SaveItems(List<string> items)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<string>));
            using (var stream = new FileStream(_itemsFilePath, FileMode.Create))
            {
                serializer.WriteObject(stream, items);
            }
        }

        // 2. 항목 리스트 불러오기 (파일 없으면 기본값 반환)
        public List<string> LoadItems()
        {
            if (!File.Exists(_itemsFilePath))
            {
                // 파일이 없으면 기본 항목 리스트 반환
                return new List<string> { "전기세", "수도세", "가스세", "관리비", "인터넷" };
            }

            var serializer = new DataContractJsonSerializer(typeof(List<string>));
            using (var stream = new FileStream(_itemsFilePath, FileMode.Open))
            {
                var result = serializer.ReadObject(stream) as List<string>;
                return result ?? new List<string>();
            }
        }
    }
}