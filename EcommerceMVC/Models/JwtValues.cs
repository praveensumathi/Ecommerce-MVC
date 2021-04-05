using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceMVC.Models
{
    public class JwtValues
    {
        public const string Issuer = "EcommerceMVC";
        public const string Audience = "ApiUsers";
        public const string Key = "1234567890123456";
    }
}
