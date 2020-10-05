﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace property_market_backend.Data.Repo
{
    interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void AddCity(City city);
        void DeleteCity(int CityId);
        Task<bool> SaveAsync();
    }
}
