using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using property_market_backend.Dtos;
using property_market_backend.Interfaces;
using property_market_backend.Models;

namespace property_market_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET api/city
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await uow.CityRepository.GetCitiesAsync();

            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            // var citiesDto = from c in cities
            //                select new CityDto()
            //               {
            //                   Id = c.Id,
            //                 Name = c.Name
            //               };

            return Ok(citiesDto);
        }

        // Post api/city/post --Post the data in JSON Format
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            var city = mapper.Map<City>(cityDto);
            city.LastUpdateBy = 1;
            city.LastUpdateOn = DateTime.Now;

            // var city = new City
            // {
            //    Name = cityDto.Name,
            //    LastUpdateBy = 1,
            //    LastUpdateOn = DateTime.Now
            // };

            uow.CityRepository.AddCity(city);
            await uow.SaveAsync();
            return StatusCode(201);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            uow.CityRepository.DeleteCity(id);
            await uow.SaveAsync();
            return Ok(id);
        }
        // Post api/city/add?cityname=Miami
        // Post api/city/add/Los Angeles
        // [HttpPost("add")]
        // [HttpPost("add/{cityname}")]
        // public async Task<IActionResult> AddCity(string cityName)
        // {
        //     City city = new City();
        //     city.Name = cityName;
        //     await dc.Cities.AddAsync(city);
        //     await dc.SaveChangesAsync();
        //     return Ok(city);
        // }
    }
}
