﻿using System;
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
            public const string UpdateGold = Notification + "cập nhật số lượng xu.";
            public const string RequestPointOut = Notification + "yêu cầu đổi điểm thưởng.";
        }
        public static class Messages
        {
            public const string NewBooking = "Có một học sinh đặt lịch tư vấn vào lúc ";
            public const string UpdateGold = "Có biến động số xu trong tài khoản";
            public const string RequestPointOut = "Có yêu cầu đổi điểm thưởng từ ";
        }
    }
}
