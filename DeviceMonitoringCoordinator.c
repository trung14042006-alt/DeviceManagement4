/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;
using System.Collections.Generic;

namespace DeviceManagement4
{
    public class DeviceMonitoringCoordinator : IDisposable
    {
        private readonly IEnumerable<TeachingDevice> devices;
        private readonly ConsoleNotificationService notificationService;
        private readonly DeviceAuditLogger auditLogger;
        private readonly MaintenanceService maintenanceService;
        private bool isSubscribed = false;

        public DeviceMonitoringCoordinator(
            IEnumerable<TeachingDevice> devices,
            ConsoleNotificationService notificationService,
            DeviceAuditLogger auditLogger,
            MaintenanceService maintenanceService)
        {
            this.devices = devices ? ? throw new ArgumentNullException(nameof(devices));
            this.notificationService = notificationService ? ? throw new ArgumentNullException(nameof(notificationService));
            this.auditLogger = auditLogger ? ? throw new ArgumentNullException(nameof(auditLogger));
            this.maintenanceService = maintenanceService ? ? throw new ArgumentNullException(nameof(maintenanceService));
        }

        public void Subscribe()
        {
            if (isSubscribed) return;

            foreach(var device in devices)
            {
                device.StatusChanged -= auditLogger.HandleStatusChanged;
                device.StatusChanged += auditLogger.HandleStatusChanged;

                device.AlertRaised -= notificationService.HandleAlert;
                device.AlertRaised += notificationService.HandleAlert;

                device.AlertRaised -= maintenanceService.HandleCriticalAlert;
                device.AlertRaised += maintenanceService.HandleCriticalAlert;
            }

            isSubscribed = true;
            Console.WriteLine("[COORDINATOR] Đã đăng ký thành công các sự kiện thiết bị.");
        }

        public void Unsubscribe()
        {
            if (!isSubscribed) return;

            foreach(var device in devices)
            {
                device.StatusChanged -= auditLogger.HandleStatusChanged;
                device.AlertRaised -= notificationService.HandleAlert;
                device.AlertRaised -= maintenanceService.HandleCriticalAlert;
            }

            isSubscribed = false;
            Console.WriteLine("[COORDINATOR] Đã hủy đăng ký các sự kiện thiết bị.");
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.SuppressFinalize(this);
        }
    }
}