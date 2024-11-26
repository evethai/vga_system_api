using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class CorsConstant
    {
        public const string PolicyName = "MyDefaultPolicy";
        public static readonly string[] AllowedOrigins = new[]
        {
            "http://localhost:3000",
            "http://localhost:3030",
            "https://zalo.me",
            "https://h5.zdn.vn",
            "https://vga-beta.vercel.app",
            "https://vga-nh8pp1j38-nhat679s-projects.vercel.app/",
            "https://vga-payment-qt992zzqy-nhat679s-projects.vercel.app/"
        };
    }
}
