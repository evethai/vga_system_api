using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class NotificationConstant
    {
        static NotificationConstant()
        {
        }
        public const string Notification = "Thông báo ";
        public static class Title
        {
            public const string NewBooking = Notification + "có lịch tư vấn mới.";
            public const string Booked = Notification + "có lịch tư vấn đã được đặt.";
            public const string UpdateGold = Notification + "cập nhật thông tin điểm.";
            public const string Request = Notification + "yêu cầu rút tiền.";
            public const string Withdraw = Notification + "yêu cầu rút tiền đã xử lý thành công.";
            public const string Reject = Notification + "yêu cầu rút tiền đã bị từ chối.";
            public const string BookingConsulted = Notification + "đã hoàn thành buổi tư vấn.";
            public const string BookingCanceled = Notification + "buổi tư vấn đã được hủy.";
            public const string BookingReported = Notification + "báo cáo buổi tư vấn.";
            public const string BookingProcessConsult = Notification + "không chấp nhận báo cáo buổi tư vấn.";
            public const string BookingProcessCancel = Notification + "xử lý báo cáo buổi tư vấn thành công.";
        }
        public static class Messages
        {
            public const string NewBooking = "Có một học sinh đặt lịch tư vấn vào lúc ";
            public const string UpdateGold = "Có biến động số điểm trong tài khoản";
        }
    }
}
