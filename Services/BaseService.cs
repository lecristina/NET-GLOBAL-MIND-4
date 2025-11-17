using AutoMapper;
using challenge_3_net.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace challenge_3_net.Services
{
    /// <summary>
    /// Classe base para serviços com funcionalidades comuns
    /// </summary>
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor? _httpContextAccessor;

        protected BaseService(IMapper mapper, IHttpContextAccessor? httpContextAccessor = null)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtém o ID do usuário atual do token JWT
        /// </summary>
        /// <returns>ID do usuário ou null se não encontrado</returns>
        protected int? GetCurrentUserId()
        {
            if (_httpContextAccessor?.HttpContext?.User == null)
                return null;

            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return null;

            return userId;
        }

        /// <summary>
        /// Cria links HATEOAS para uma entidade
        /// </summary>
        /// <param name="id">ID da entidade</param>
        /// <param name="controllerName">Nome do controller</param>
        /// <param name="baseUrl">URL base da API</param>
        /// <returns>Lista de links</returns>
        protected List<LinkDto> CreateHateoasLinks(int id, string controllerName, string baseUrl)
        {
            var links = new List<LinkDto>
            {
                new($"{baseUrl}/api/{controllerName}/{id}", "self", "GET", "Obter por ID"),
                new($"{baseUrl}/api/{controllerName}/{id}", "update", "PUT", "Atualizar"),
                new($"{baseUrl}/api/{controllerName}/{id}", "delete", "DELETE", "Excluir"),
                new($"{baseUrl}/api/{controllerName}", "list", "GET", "Listar todos")
            };

            return links;
        }

        /// <summary>
        /// Cria links HATEOAS para paginação
        /// </summary>
        /// <param name="pageNumber">Número da página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalPages">Total de páginas</param>
        /// <param name="controllerName">Nome do controller</param>
        /// <param name="baseUrl">URL base da API</param>
        /// <returns>Lista de links de paginação</returns>
        protected List<LinkDto> CreatePaginationLinks(int pageNumber, int pageSize, int totalPages, string controllerName, string baseUrl)
        {
            var links = new List<LinkDto>
            {
                new($"{baseUrl}/api/{controllerName}?pageNumber={pageNumber}&pageSize={pageSize}", "self", "GET", "Página atual")
            };

            if (pageNumber > 1)
            {
                links.Add(new($"{baseUrl}/api/{controllerName}?pageNumber={pageNumber - 1}&pageSize={pageSize}", "previous", "GET", "Página anterior"));
            }

            if (pageNumber < totalPages)
            {
                links.Add(new($"{baseUrl}/api/{controllerName}?pageNumber={pageNumber + 1}&pageSize={pageSize}", "next", "GET", "Próxima página"));
            }

            links.Add(new($"{baseUrl}/api/{controllerName}?pageNumber=1&pageSize={pageSize}", "first", "GET", "Primeira página"));
            links.Add(new($"{baseUrl}/api/{controllerName}?pageNumber={totalPages}&pageSize={pageSize}", "last", "GET", "Última página"));

            return links;
        }

        /// <summary>
        /// Cria resultado paginado com links HATEOAS
        /// </summary>
        /// <typeparam name="T">Tipo dos itens</typeparam>
        /// <param name="items">Itens da página</param>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalItems">Total de itens</param>
        /// <param name="controllerName">Nome do controller</param>
        /// <param name="baseUrl">URL base da API</param>
        /// <returns>Resultado paginado</returns>
        protected PagedResultDto<T> CreatePagedResult<T>(IEnumerable<T> items, int pageNumber, int pageSize, long totalItems, string controllerName, string baseUrl)
        {
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            
            return new PagedResultDto<T>
            {
                Items = items.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                Links = CreatePaginationLinks(pageNumber, pageSize, totalPages, controllerName, baseUrl)
            };
        }
    }
}
