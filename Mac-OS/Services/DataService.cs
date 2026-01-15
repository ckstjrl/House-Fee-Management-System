using ManagementHouseFee_Avalonia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ManagementHouseFee_Avalonia.Services
{
    public class DataService
    {
        private readonly string _folderPath;
        private readonly string _filePath;
        private readonly string _itemsFilePath;

        // JSON 저장 옵션 (한글 깨짐 방지 + 들여쓰기)
        private readonly JsonSerializerOptions _jsonOptions;

        public DataService()
        {
            // 1. 경로 설정 (OS 공용 데이터 폴더 사용)
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _folderPath = Path.Combine(baseFolder, "ManagementHouseFee_Avalonia");

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            _filePath = Path.Combine(_folderPath, "fee_data.json");
            _itemsFilePath = Path.Combine(_folderPath, "items.json");

            // 2. JSON 옵션 설정
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // 보기 좋게 줄바꿈
                // 한글을 유니코드(\uXXXX)로 변환하지 않고 그대로 저장
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

        // [데이터 저장]
        public void Save(List<FeeRecord> records)
        {
            string jsonString = JsonSerializer.Serialize(records, _jsonOptions);
            File.WriteAllText(_filePath, jsonString);
        }

        // [데이터 로드]
        public List<FeeRecord> Load()
        {
            if (!File.Exists(_filePath)) return new List<FeeRecord>();

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var result = JsonSerializer.Deserialize<List<FeeRecord>>(jsonString, _jsonOptions);
                return result ?? new List<FeeRecord>();
            }
            catch
            {
                // 파일이 깨졌거나 읽을 수 없을 때 빈 리스트 반환
                return new List<FeeRecord>();
            }
        }

        // [항목 저장]
        public void SaveItems(List<string> items)
        {
            string jsonString = JsonSerializer.Serialize(items, _jsonOptions);
            File.WriteAllText(_itemsFilePath, jsonString);
        }

        // [항목 로드]
        public List<string> LoadItems()
        {
            if (!File.Exists(_itemsFilePath))
            {
                return new List<string> { "전기세", "수도세", "가스세", "관리비", "인터넷" };
            }

            try
            {
                string jsonString = File.ReadAllText(_itemsFilePath);
                var result = JsonSerializer.Deserialize<List<string>>(jsonString, _jsonOptions);
                return result ?? new List<string>();
            }
            catch
            {
                return new List<string> { "전기세", "수도세", "가스세", "관리비", "인터넷" };
            }
        }
    }
}