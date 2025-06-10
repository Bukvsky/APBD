using Kolokwium1b.DTOs;
using Kolokwium1b.Models;
using Kolokwium1b.Repositories;
using System;

namespace Kolokwium1b.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IServiceRepository _serviceRepository;

        public VisitService(
            IVisitRepository visitRepository,
            IClientRepository clientRepository,
            IMechanicRepository mechanicRepository,
            IServiceRepository serviceRepository)
        {
            _visitRepository = visitRepository;
            _clientRepository = clientRepository;
            _mechanicRepository = mechanicRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<VisitDetailsDto?> GetVisitDetailsAsync(int visitId)
        {
            var visit = await _visitRepository.GetVisitByIdAsync(visitId);
            if (visit == null)
                return null;

            return new VisitDetailsDto
            {
                Date = visit.Date,
                Client = new ClientDto
                {
                    FirstName = visit.Client.FirstName,
                    LastName = visit.Client.LastName,
                    DateOfBirth = visit.Client.DateOfBirth
                },
                Mechanic = new MechanicDto
                {
                    MechanicId = visit.Mechanic.MechanicId,
                    LicenceNumber = visit.Mechanic.LicenceNumber
                },
                VisitServices = visit.VisitServices.Select(vs => new VisitServiceDto
                {
                    Name = vs.Service.Name,
                    ServiceFee = vs.ServiceFee
                }).ToList()
            };
        }

        public async Task<bool> CreateVisitAsync(CreateVisitDto createVisitDto)
        {
            // Check if visit already exists
            if (await _visitRepository.VisitExistsAsync(createVisitDto.VisitId))
                return false;

            // Check if client exists
            if (!await _clientRepository.ClientExistsAsync(createVisitDto.ClientId))
                return false;

            // Check if mechanic exists
            var mechanic = await _mechanicRepository.GetMechanicByLicenceNumberAsync(createVisitDto.MechanicLicenceNumber);
            if (mechanic == null)
                return false;

            // Check if all services exist
            var serviceIds = new List<int>();
            foreach (var serviceDto in createVisitDto.Services)
            {
                var service = await _serviceRepository.GetServiceByNameAsync(serviceDto.ServiceName);
                if (service == null)
                    return false;
                serviceIds.Add(service.ServiceId);
            }

            // Create visit
            var visit = new Visit
            {
                VisitId = createVisitDto.VisitId,
                ClientId = createVisitDto.ClientId,
                MechanicId = mechanic.MechanicId,
                Date = DateTime.Now,
                VisitServices = createVisitDto.Services.Select((serviceDto, index) => new Models.VisitService
                {
                    VisitId = createVisitDto.VisitId,
                    ServiceId = serviceIds[index],
                    ServiceFee = serviceDto.ServiceFee
                }).ToList()
            };

            await _visitRepository.CreateVisitAsync(visit);
            return true;
        }
    }
}