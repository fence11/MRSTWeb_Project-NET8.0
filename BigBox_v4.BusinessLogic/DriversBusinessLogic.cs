using BigBox_v4.Domain;
using BigBox_v4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.BusinessLogic
{
    public class DriversBusinessLogic : BusinessService<Drivers>, IDriversBusinessLogic
    {
        private readonly IDriversRepository _driversRepository;

        public DriversBusinessLogic(IDriversRepository repository) : base(repository)
        {
            _driversRepository = repository;
        }

        // validation logic
        public async Task<IEnumerable<Drivers>> GetDriversWithValidLicenseAsync()
        {
            var allDrivers = await _driversRepository.GetAllAsync();
            return allDrivers.Where(d => !string.IsNullOrEmpty(d.LicenseNumber) && d.LicenseNumber.Length >= 5);
        }

        public async Task<bool> IsLicenseNumberUniqueAsync(string licenseNumber)
        {
            if (string.IsNullOrEmpty(licenseNumber))
                throw new ArgumentException("License number cannot be empty", nameof(licenseNumber));

            var driver = await _driversRepository.GetDriverByLicenseNumberAsync(licenseNumber);
            return driver == null;
        }

        public override async Task<Drivers> CreateItemAsync(Drivers entity)
        {
            // validate driver license
            if (string.IsNullOrEmpty(entity.LicenseNumber) || entity.LicenseNumber.Length < 5)
                throw new InvalidOperationException("Driver license is not valid");

            // check if license is unique
            if (!await IsLicenseNumberUniqueAsync(entity.LicenseNumber))
                throw new InvalidOperationException("License number already exists");

            return await base.CreateItemAsync(entity);
        }

        public override async Task UpdateItemAsync(Drivers entity)
        {
            // validate driver license
            if (string.IsNullOrEmpty(entity.LicenseNumber) || entity.LicenseNumber.Length < 5)
                throw new InvalidOperationException("Driver license is not valid");

            // get existing driver to check if license number has changed
            var existingDriver = await _driversRepository.GetByIdAsync(entity.Id);

            // check if license is unique
            if (existingDriver != null && existingDriver.LicenseNumber != entity.LicenseNumber)
            {
                if (!await IsLicenseNumberUniqueAsync(entity.LicenseNumber))
                    throw new InvalidOperationException("License number already exists");
            }

            await base.UpdateItemAsync(entity);
        }
    }
}

