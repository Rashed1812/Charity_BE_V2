using BLL.ServiceAbstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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

                    case "4jawaly":
                        return await SendVia4jawalyAsync(formattedPhone, message);

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

        private async Task<bool> SendVia4jawalyAsync(string phoneNumber, string message)
        {
            try
            {
                var apiKey = _configuration["SMS:4jawaly:ApiKey"];
                var apiSecret = _configuration["SMS:4jawaly:ApiSecret"];
                var sender = _configuration["SMS:4jawaly:Sender"];
                var numberIso = "SA";

                var formattedNumber = FormatPhoneNumber(phoneNumber);

                var auth = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}")
                );

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", auth);

                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new
                {
                    messages = new[]
                    {
                new
                {
                    text = message,
                    numbers = new[] { formattedNumber },
                    sender = sender,
                    number_iso = numberIso
                }
            }
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(
                    "https://api-sms.4jawaly.com/api/v1/account/area/sms/send",
                    content
                );

                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"4jawaly Response: {response.StatusCode} - {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "4jawaly SMS Error");
                return false;
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

            var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

            if (digitsOnly.StartsWith("966"))
                return digitsOnly;

            if (digitsOnly.StartsWith("05"))
                return "966" + digitsOnly.Substring(1);

            if (digitsOnly.StartsWith("5"))
                return "966" + digitsOnly;

            if (digitsOnly.Length == 9)
                return "966" + digitsOnly;

            return digitsOnly;
        }
    }
}