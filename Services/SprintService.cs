using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace challenge_3_net.Services
{
    public class SprintService : BaseService, ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SprintService(
            ISprintRepository sprintRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _sprintRepository = sprintRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<SprintResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _sprintRepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<SprintResponseDto>>(items);

            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdSprint, "v1.0/Sprints", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/Sprints", GetBaseUrl());
        }

        public async Task<SprintResponseDto?> ObterPorIdAsync(int id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id);
            if (sprint == null)
                return null;

            var response = _mapper.Map<SprintResponseDto>(sprint);
            response.Links = CreateHateoasLinks(id, "v1.0/Sprints", GetBaseUrl());
            return response;
        }

        public async Task<List<SprintResponseDto>> ObterPorUsuarioAsync(int usuarioId)
        {
            var sprints = await _sprintRepository.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<List<SprintResponseDto>>(sprints);
            
            foreach (var item in response)
            {
                item.Links = CreateHateoasLinks(item.IdSprint, "v1.0/Sprints", GetBaseUrl());
            }

            return response;
        }

        public async Task<SprintResponseDto> CriarAsync(CriarSprintDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("Usuário não autenticado");

            var sprint = _mapper.Map<Sprint>(dto);
            sprint.IdUsuario = userId.Value;

            // Verificar se já existe sprint com mesmo nome para o usuário
            var existing = await _sprintRepository.GetByUsuarioAndNomeAsync(sprint.IdUsuario, sprint.NomeSprint);
            if (existing != null)
                throw new InvalidOperationException("Já existe uma sprint com este nome para este usuário");

            var sprintCriado = await _sprintRepository.AddAsync(sprint);
            var response = _mapper.Map<SprintResponseDto>(sprintCriado);
            response.Links = CreateHateoasLinks(sprintCriado.IdSprint, "v1.0/Sprints", GetBaseUrl());
            return response;
        }

        public async Task<SprintResponseDto?> AtualizarAsync(int id, AtualizarSprintDto dto)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id);
            if (sprint == null)
                return null;

            _mapper.Map(dto, sprint);
            var sprintAtualizado = await _sprintRepository.UpdateAsync(sprint);
            var response = _mapper.Map<SprintResponseDto>(sprintAtualizado);
            response.Links = CreateHateoasLinks(id, "v1.0/Sprints", GetBaseUrl());
            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _sprintRepository.RemoveByIdAsync(id);
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

