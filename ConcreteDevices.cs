/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;

namespace DeviceManagement4
{
    public class ProjectorRemote : TeachingDevice
    {
        public int BatteryLevel { get; private set; }

        public ProjectorRemote(string id, string name, int initialBattery) : base(id, name)
        {
            ValidateBattery(initialBattery);
            BatteryLevel = initialBattery;
        }

        public void UpdateBatteryLevel(int newLevel)
        {
            ValidateBattery(newLevel);
            if (newLevel == BatteryLevel) return;

            BatteryLevel = newLevel;

            if (BatteryLevel == 0)
            {
                RaiseAlert("Máy chiếu hết pin hoàn toàn (0%).", AlertSeverity.Critical);
            }
            else if (BatteryLevel < 20)
            {
                RaiseAlert($"Mức pin máy chiếu thấp ({BatteryLevel}%).", AlertSeverity.Warning);
            }
        }

        private void ValidateBattery(int value)
        {
            if (value < 0 || value > 100)
                throw new InvalidDeviceReadingException(Id, value, $"Mức pin máy chiếu '{value}' không hợp lệ (phải từ 0 đến 100).");
        }
    }

    public class AirConditionerRemote : TeachingDevice
    {
        public string AssignedRoom { get; }
        public bool IsResponsive { get; private set; }

        public AirConditionerRemote(string id, string name, string assignedRoom, bool isResponsive) : base(id, name)
        {
            AssignedRoom = assignedRoom;
            IsResponsive = isResponsive;
        }

        public void UpdateResponseState(bool isResponsive)
        {
            if (isResponsive == IsResponsive) return;

            bool oldResponsive = IsResponsive;
            IsResponsive = isResponsive;

            if (oldResponsive && !IsResponsive)
            {
                RaiseAlert($"Điều hòa phòng {AssignedRoom} chuyển sang trạng thái không phản hồi.", AlertSeverity.Critical);
            }
        }
    }

    public class WirelessMicrophone : TeachingDevice
    {
        public int BatteryLevel { get; private set; }
        public int SignalQuality { get; private set; }

        public WirelessMicrophone(string id, string name, int battery, int signal) : base(id, name)
        {
            ValidateReading(battery, signal);
            BatteryLevel = battery;
            SignalQuality = signal;
        }

        public void UpdateBatteryLevel(int newBattery)
        {
            ValidateReading(newBattery, SignalQuality);
            if (newBattery == BatteryLevel) return;

            BatteryLevel = newBattery;

            if (BatteryLevel == 0)
            {
                RaiseAlert("Micro hết pin hoàn toàn (0%).", AlertSeverity.Critical);
            }
            else if (BatteryLevel < 30)
            {
                RaiseAlert($"Mức pin micro thấp ({BatteryLevel}%).", AlertSeverity.Warning);
            }
        }

        public void UpdateSignalQuality(int newSignal)
        {
            ValidateReading(BatteryLevel, newSignal);
            if (newSignal == SignalQuality) return;

            SignalQuality = newSignal;

            if (SignalQuality == 0)
            {
                RaiseAlert("Chất lượng tín hiệu micro mất hoàn toàn (0%).", AlertSeverity.Critical);
            }
            else if (SignalQuality < 40)
            {
                RaiseAlert($"Chất lượng tín hiệu micro kém ({SignalQuality}%).", AlertSeverity.Warning);
            }
        }

        private void ValidateReading(int battery, int signal)
        {
            if (battery < 0 || battery > 100)
                throw new InvalidDeviceReadingException(Id, battery, $"Pin micro '{battery}' không hợp lệ.");
            if (signal < 0 || signal > 100)
                throw new InvalidDeviceReadingException(Id, signal, $"Tín hiệu micro '{signal}' không hợp lệ.");
        }
    }
}