using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class VnpayHelper
    {
        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var b in hashValue)
                {
                    hash.Append(b.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        public static bool ValidateSignature(IQueryCollection vnpayParams, string hashSecret)
        {
            // 1. Lấy chữ ký gốc
            string vnp_SecureHash = vnpayParams["vnp_SecureHash"];

            // 2. Tạo "dữ liệu thô" từ các tham số (trừ vnp_SecureHash)
            var sortedParams = new SortedList<string, string>(StringComparer.Ordinal);
            foreach (var (key, value) in vnpayParams)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") && key != "vnp_SecureHash")
                {
                    sortedParams.Add(key, value.ToString());
                }
            }

            // 3. Ghép chuỗi (key=value&key=value...)
            var rawDataBuilder = new StringBuilder();
            foreach (var (key, value) in sortedParams)
            {
                rawDataBuilder.Append(key + "=" + value + "&");
            }

            // Xóa dấu & cuối cùng
            string rawData = rawDataBuilder.ToString().Remove(rawDataBuilder.Length - 1, 1);

            // 4. Hash dữ liệu thô với "chìa khóa bí mật"
            string myNewSignature = HmacSHA512(hashSecret, rawData);

            // 5. So sánh
            return myNewSignature.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
