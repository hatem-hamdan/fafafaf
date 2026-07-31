using System.Security.Cryptography;

using System.Text;

using System.Text.Json;

using MyStore.DataAccess.DTOs;


namespace    MyStore.API      // تأكد أن Namespace صحيح حسب مشروعك

{

    public class AdTrackingService : IAdTrackingService

    {

        private readonly HttpClient _httpClient;



        public AdTrackingService(HttpClient httpClient)

        {

            _httpClient = httpClient;

        }



        public async Task TrackPurchaseAsync(string email, OrderCreateDto dto)

        {

            try

            {

                var url = "https://tr.snapchat.com/v3/f731ced3-551f-4477-96e1-646bbd4a38ea/events";

                

                var payload = new {

                    data = new[] {

                        new {

                            event_name = "PURCHASE",

                            event_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                            user_data = new { em = HashEmail(email) },

                            custom_data = new {

                                currency = "SAR",

                                value = dto.TotalPrice,

                                item_ids = dto.Items.Select(i => i.ProductId.ToString()).ToArray()

                            }

                        }

                    }

                };



                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var finalUrl = $"{url}?access_token=eyJhbGciOiJIUzI1NiIsImtpZCI6IkNhbnZhc1MyU0hNQUNQcm9kIiwidHlwIjoiSldUIn0.eyJhdWQiOiJjYW52YXMtY2FudmFzYXBpIiwiaXNzIjoiY2FudmFzLXMyc3Rva2VuIiwibmJmIjoxNzg1NDc3NzQwLCJzdWIiOiIyODE5NzkwZS05YTc1LTQ1OGUtYWMyOC03OTcwZDIzNGFjNjJ-UFJPRFVDVElPTn5hNmQ0MTViYi02ZjMwLTQyMzQtYWQyYi1hNzk1MTdiN2MwMmQifQ.w6FnKEB0VJLz4xuCBr59llVlvLeOuvb21av5vKJ0geI"; // ضع التوكين هنا



                await _httpClient.PostAsync(finalUrl, content);

            }

            catch { }

        }



        private string HashEmail(string email)

        {

            using var sha256 = SHA256.Create();

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(email.ToLower().Trim()));

            return Convert.ToHexString(bytes).ToLower();

        }

    }
    }
