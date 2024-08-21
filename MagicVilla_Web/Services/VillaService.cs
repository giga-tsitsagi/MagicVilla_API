using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using System;
using static MagicVilla_Utility.SD;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _ClientFactory;
        private string villaUrl;
        private ApiType ApiType;

        public VillaService(IHttpClientFactory Clientfactory, IConfiguration configuration) : base(Clientfactory)
        {
            _ClientFactory = Clientfactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }

        public Task<T> CreateSync<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/villaApi"
            });
        }
        public Task<T> DeleteSync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaApi/" + id
            });
        }

        public Task<T> GetAllSync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaApi"
            });

            throw new NotImplementedException();
        }

        public Task<T> GetSync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaApi/" + id
            });
        }

        public Task<T> UpdateSync<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/villaApi/" + dto.Id
            });
        }
    }
}
