/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;

namespace DeviceManagement4
{
    public class InvalidDeviceReadingException : Exception
    {
        public string DeviceId { get; }
        public int InvalidValue { get; }

        public InvalidDeviceReadingException(string deviceId, int invalidValue, string message)
            : base(message)
        {
            DeviceId = deviceId;
            InvalidValue = invalidValue;
        }
    }
}