using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace challenge_3_net.Services
{
    public class AlertaIAService : BaseService, IAlertaIAService
    {
        private readonly IAlertaIARepository _alertaIARepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AlertaIAService(
            IAlertaIARepository alertaIARepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _alertaIARepository = alertaIARepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<AlertaIAResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _alertaIARepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<AlertaIAResponseDto>>(items);

            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdAlerta, "v1.0/AlertasIA", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/AlertasIA", GetBaseUrl());
        }

        public async Task<AlertaIAResponseDto?> ObterPorIdAsync(int id)
        {
            var alerta = await _alertaIARepository.GetByIdAsync(id);
            if (alerta == null)
                return null;

            var response = _mapper.Map<AlertaIAResponseDto>(alerta);
            response.Links = CreateHateoasLinks(id, "v1.0/AlertasIA", GetBaseUrl());
            return response;
        }

        public async Task<List<AlertaIAResponseDto>> ObterPorUsuarioAsync(int usuarioId)
        {
            var alertas = await _alertaIARepository.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<List<AlertaIAResponseDto>>(alertas);
            
            foreach (var item in response)
            {
                item.Links = CreateHateoasLinks(item.IdAlerta, "v1.0/AlertasIA", GetBaseUrl());
            }

            return response;
        }

        public async Task<AlertaIAResponseDto> CriarAsync(CriarAlertaIADto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("Usuário não autenticado");

            var alerta = _mapper.Map<AlertaIA>(dto);
            alerta.IdUsuario = userId.Value;
            alerta.DataAlerta = DateTime.UtcNow;

            var alertaCriado = await _alertaIARepository.AddAsync(alerta);
            var response = _mapper.Map<AlertaIAResponseDto>(alertaCriado);
            response.Links = CreateHateoasLinks(alertaCriado.IdAlerta, "v1.0/AlertasIA", GetBaseUrl());
            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _alertaIARepository.RemoveByIdAsync(id);
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            if (request == null)
                return "https://localhost:7000";
            return $"{request.Scheme}://{request.Host}";
        }
    }
}

