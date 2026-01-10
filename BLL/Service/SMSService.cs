using BLL.ServiceAbstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class SMSService : ISMSService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SMSService> _logger;
        private readonly List<string> _smsTemplates;

        public SMSService(IConfiguration configuration, ILogger<SMSService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            // Initialize SMS Templates (نماذج رسائل SMS المقترحة)
            _smsTemplates = new List<string>
            {
                "طلب الاستشارة قيد التنفيذ، وسيتم التواصل معكم خلال الفترة القريبة القادمة.",
                "تم استلام طلب الاستشارة الخاص بكم، وسيتم التواصل معكم في أقرب وقت ممكن. شكرًا لثقتكم بنا.",
                "نود إفادتكم بأنه جارٍ العمل على طلبكم، وسيتم التواصل معكم قريبًا بإذن الله.",
                "نفيدكم باستلام طلب الاستشارة، وسيتم التواصل معكم خلال أقرب وقت ممكن.",
                "تم استلام طلبكم، وسيتم التواصل معكم خلال 24 ساعة عمل بإذن الله."
            };
        }

        public async Task<bool> SendSMSAsync(string phoneNumber, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new ArgumentException("رقم الهاتف مطلوب", nameof(phoneNumber));

                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException("نص الرسالة مطلوب", nameof(message));

                // Format phone number
                var formattedPhone = FormatPhoneNumber(phoneNumber);

                // Get SMS provider from configuration
                var provider = _configuration["SMS:Provider"] ?? "Mock"; // Default to Mock for testing

                _logger.LogInformation($"Attempting to send SMS to {formattedPhone} using provider: {provider}");

                switch (provider.ToLower())
                {
                    case "twilio":
                        return await SendViaTwilioAsync(formattedPhone, message);
                    
                    case "mobily":
                        return await SendViaMobilyAsync(formattedPhone, message);
                    
                    case "gateway":
                        return await SendViaGatewayAsync(formattedPhone, message);
                    
                    case "mock":
                    default:
                        // Mock implementation for development/testing
                        _logger.LogInformation($"Mock SMS sent to {formattedPhone}: {message}");
                        await Task.CompletedTask;
                        return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send SMS to {phoneNumber}");
                throw;
            }
        }

        private async Task<bool> SendViaTwilioAsync(string phoneNumber, string message)
        {
            try
            {
                var accountSid = _configuration["SMS:Twilio:AccountSid"];
                var authToken = _configuration["SMS:Twilio:AuthToken"];
                var fromNumber = _configuration["SMS:Twilio:FromNumber"];

                if (string.IsNullOrWhiteSpace(accountSid) || string.IsNullOrWhiteSpace(authToken))
                {
                    _logger.LogWarning("Twilio credentials not configured. Using mock sending.");
                    return await Task.FromResult(true);
                }

                // Uncomment when Twilio NuGet package is installed
                /*
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
                {
                    From = new PhoneNumber(fromNumber),
                    Body = message
                };
                
                var messageResponse = await MessageResource.CreateAsync(messageOptions);
                return messageResponse.ErrorCode == null;
                */

                _logger.LogInformation($"Twilio SMS would be sent to {phoneNumber} (Twilio package not installed)");
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS via Twilio");
                throw;
            }
        }

        private async Task<bool> SendViaMobilyAsync(string phoneNumber, string message)
        {
            try
            {
                // Mobily SMS Gateway implementation
                var apiKey = _configuration["SMS:Mobily:ApiKey"];
                var sender = _configuration["SMS:Mobily:Sender"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    _logger.LogWarning("Mobily credentials not configured. Using mock sending.");
                    return await Task.FromResult(true);
                }

                // TODO: Implement Mobily SMS Gateway API call
                // Example HTTP request to Mobily API
                /*
                using var httpClient = new HttpClient();
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("apiKey", apiKey),
                    new KeyValuePair<string, string>("sender", sender),
                    new KeyValuePair<string, string>("mobile", phoneNumber),
                    new KeyValuePair<string, string>("message", message)
                });

                var response = await httpClient.PostAsync("https://api.mobily.ws/sendSMS.php", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
                */

                _logger.LogInformation($"Mobily SMS would be sent to {phoneNumber} (Not implemented yet)");
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS via Mobily");
                throw;
            }
        }

        private async Task<bool> SendViaGatewayAsync(string phoneNumber, string message)
        {
            try
            {
                // Generic SMS Gateway implementation
                var apiUrl = _configuration["SMS:Gateway:ApiUrl"];
                var apiKey = _configuration["SMS:Gateway:ApiKey"];
                var sender = _configuration["SMS:Gateway:Sender"];

                if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiKey))
                {
                    _logger.LogWarning("Gateway credentials not configured. Using mock sending.");
                    return await Task.FromResult(true);
                }

                // TODO: Implement Generic SMS Gateway API call
                /*
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                
                var payload = new
                {
                    to = phoneNumber,
                    from = sender,
                    message = message
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);
                
                return response.IsSuccessStatusCode;
                */

                _logger.LogInformation($"Gateway SMS would be sent to {phoneNumber} (Not implemented yet)");
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS via Gateway");
                throw;
            }
        }

        public List<string> GetSMSTemplates()
        {
            return _smsTemplates;
        }

        public string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            // Remove all non-digit characters
            var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

            // Handle Saudi phone numbers
            if (digitsOnly.StartsWith("966"))
            {
                // Already has country code
                return $"+{digitsOnly}";
            }
            else if (digitsOnly.StartsWith("05"))
            {
                // Local format (05xxxxxxxx)
                return $"+966{digitsOnly.Substring(1)}"; // Remove leading 0 and add country code
            }
            else if (digitsOnly.StartsWith("5"))
            {
                // Without leading 0 (5xxxxxxxx)
                return $"+966{digitsOnly}";
            }
            else if (digitsOnly.Length == 9)
            {
                // 9 digits without country code or leading 0
                return $"+966{digitsOnly}";
            }

            // If it already starts with +, return as is
            if (phoneNumber.StartsWith("+"))
                return phoneNumber;

            // Default: assume it's a valid international format or add + if missing
            return phoneNumber.StartsWith("+") ? phoneNumber : $"+{digitsOnly}";
        }
    }
}

