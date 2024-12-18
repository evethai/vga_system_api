﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class MessagesConstants
    {
        static MessagesConstants()
        {
        }

        public static class MajorOrOccupationChoice
        {
            public const string MajorChoice = "Dựa trên kết quả bài kiểm tra của bạn, chúng tôi đã tổng hợp một số ngành học phù hợp với bạn. Hãy khám phá và lựa chọn những ngành mà bạn quan tâm nhất!";
            public const string NoMajorsFound = "Không tìm thấy ngành học phù hợp cho nhóm cá nhân này.";
            public const string NoStudentTestFound = "Không tìm thấy bài kiểm tra cho sinh viên này.";
            public const string FilterUniversity = "Dựa trên kết quả lựa chọn của bạn, chúng tôi đã tổng hợp một số trường đại học phù hợp với bạn ngành học mà bạn cảm thấy thích nhất. Hãy khám phá và lựa chọn những trường mà bạn quan tâm nhất!";
            public const string FilterOccupation = "Dựa trên kết quả đánh giá tính cách và ngành học của bạn, chúng tôi đã chọn lọc những nghề nghiệp phù hợp nhất. Hãy khám phá và tìm kiếm cơ hội nghề nghiệp mà bạn đam mê ngay hôm nay!";
        }
    }

}
