/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;
using System.Collections.Generic;

namespace DeviceManagement4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH KIỂM THỬ DELEGATE & EVENT (DEVICEMANAGEMENT4) ===\n");

            var projector = new ProjectorRemote("PJ01", "Máy chiếu Sony", 85);
            var ac = new AirConditionerRemote("AC01", "Điều hòa Daikin", "A203", true);
            var mic = new WirelessMicrophone("MIC01", "Micro Shure", 40, 50);

            var deviceList = new List<TeachingDevice> { projector, ac, mic };

            var notificationService = new ConsoleNotificationService();
            var auditLogger = new DeviceAuditLogger();
            var maintenanceService = new MaintenanceService();

            using (var coordinator = new DeviceMonitoringCoordinator(deviceList, notificationService, auditLogger, maintenanceService))
            {
                coordinator.Subscribe();

                Console.WriteLine("\n--- KỊCH BẢN 1: Thay đổi trạng thái thiết bị ---");
                projector.ChangeStatus(DeviceStatus.Borrowed);
                projector.ChangeStatus(DeviceStatus.Available);

                Console.WriteLine("\n--- KỊCH BẢN 2: Cập nhật pin máy chiếu xuống mức cảnh báo và nghiêm trọng ---");
                projector.UpdateBatteryLevel(15);
                projector.UpdateBatteryLevel(0);

                Console.WriteLine("\n--- KỊCH BẢN 3: Điều hòa chuyển sang không phản hồi ---");
                ac.UpdateResponseState(false);

                Console.WriteLine("\n--- KỊCH BẢN 4: Sử dụng Delegate tùy chỉnh (FilteredAlertSubscriber) ---");
                var criticalFilter = new FilteredAlertSubscriber(
                    alert => alert.Severity == AlertSeverity.Critical,
                    alert => Console.WriteLine($"==> [FILTERED CALLBACK] Bắt được lỗi NẶNG trên thiết bị [{alert.DeviceId}]: {alert.Message}")
                );

                mic.AlertRaised += criticalFilter.HandleAlert;

                mic.UpdateBatteryLevel(25);
                mic.UpdateBatteryLevel(0);

                Console.WriteLine("\n--- KỊCH BẢN 5: Kiểm tra ngoại lệ InvalidDeviceReadingException ---");
                RunTest("Nhập giá trị pin không hợp lệ (150%) cho micro", () => {
                    mic.UpdateBatteryLevel(150);
                });

                Console.WriteLine($"\nTổng số bản ghi trong Audit Log: {auditLogger.Entries.Count}");
                Console.WriteLine($"Tổng số yêu cầu bảo trì: {maintenanceService.Requests.Count}");
            }

            Console.WriteLine("\n=== HOÀN TẤT KIỂM THỬ DELEGATE & EVENT ===");
        }

        private static void RunTest(string testName, Action testAction)
        {
            Console.Write($"[TEST] {testName} ... ");
            try
            {
                testAction();
                Console.WriteLine("THẤT BẠI (Không bắt được ngoại lệ).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"THÀNH CÔNG (Bắt được ngoại lệ {ex.GetType().Name}): {ex.Message}");
            }
        }
    }
}