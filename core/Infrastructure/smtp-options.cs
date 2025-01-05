﻿namespace UsersMicroservice.core.Infrastructure
{
    public class SmtpOptions
    {
        public string? Server { get; set; }
        public int? Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseSSL { get; set; }
    }
}
