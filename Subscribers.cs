/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;
using System.Collections.Generic;

namespace DeviceManagement4
{
    public class ConsoleNotificationService
    {
        public void HandleAlert(object? sender, DeviceAlertEventArgs e)
        {
            Console.WriteLine($"[THÔNG BÁO - {e.Severity}] Thiết bị {e.DeviceId}: {e.Message} (Lúc: {e.OccurredAt:HH:mm:ss})");
        }
    }

    public class DeviceAuditLogger
    {
        private readonly List<string> entries = new();
        public IReadOnlyList<string> Entries => entries.AsReadOnly();

        public void HandleStatusChanged(object? sender, DeviceStatusChangedEventArgs e)
        {
            string log = $"{e.ChangedAt:HH:mm:ss}: Thiết bị {e.DeviceId} chuyển trạng thái từ {e.OldStatus} sang {e.NewStatus}";
            entries.Add(log);
            Console.WriteLine($"[AUDIT LOG] {log}");
        }
    }

    public class MaintenanceService
    {
        private readonly List<string> requests = new();
        public IReadOnlyList<string> Requests => requests.AsReadOnly();

        public void HandleCriticalAlert(object? sender, DeviceAlertEventArgs e)
        {
            if (e.Severity != AlertSeverity.Critical)
            {
                return;
            }

            string req = $"Yêu cầu bảo trì khẩn cấp cho thiết bị {e.DeviceId}: {e.Message}";
            requests.Add(req);
            Console.WriteLine($"[BẢO TRÌ CRITICAL] {req}");
        }
    }

    public delegate bool AlertFilter(DeviceAlertEventArgs alert);

    public class FilteredAlertSubscriber
    {
        private readonly AlertFilter filter;
        private readonly Action<DeviceAlertEventArgs> action;

        public FilteredAlertSubscriber(AlertFilter filter, Action<DeviceAlertEventArgs> action)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void HandleAlert(object? sender, DeviceAlertEventArgs e)
        {
            if (filter(e))
            {
                action(e);
            }
        }
    }
}