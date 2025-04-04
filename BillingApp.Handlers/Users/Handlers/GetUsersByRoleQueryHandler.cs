using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingApp.Common.Constants;
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Users.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BillingApp.Handlers.Users.Handlers
{
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, List<UserDTO>>
    {
        private readonly BillingDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetUsersByRoleQueryHandler(
            BillingDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            var requestingUser = await _userManager.FindByIdAsync(request.RequestingUserId.ToString());
            if (requestingUser == null)
                throw new UnauthorizedAccessException("User not found");

           
            if (await _userManager.IsInRoleAsync(requestingUser, UserRoles.SuperAdmin))
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(request.Role))
                {
                    var role = await _roleManager.FindByNameAsync(request.Role);
                    if (role != null)
                    {
                        var userIdsInRole = await _context.UserRoles
                            .Where(ur => ur.RoleId == role.Id)
                            .Select(ur => ur.UserId)
                            .ToListAsync(cancellationToken);

                        query = query.Where(u => userIdsInRole.Contains(u.Id));
                    }
                }

                var result = await query
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                await PopulateUserRolesAndAdminInfo(result);
                return result;
            }

            
            if (await _userManager.IsInRoleAsync(requestingUser, UserRoles.Admin))
            {
                var agentIds = await _context.UserClaims
                    .Where(uc => uc.ClaimType == "ManagedByAdmin" && uc.ClaimValue == requestingUser.Id)
                    .Select(uc => uc.UserId)
                    .ToListAsync(cancellationToken);

                var agentsQuery = _context.Users
                    .Where(u => agentIds.Contains(u.Id));

                
                var adminRole = await _roleManager.FindByNameAsync(UserRoles.Admin);
                var adminsQuery = _context.UserRoles
                    .Where(ur => ur.RoleId == adminRole.Id && ur.UserId != requestingUser.Id)
                    .Join(_context.Users,
                        ur => ur.UserId,
                        u => u.Id,
                        (ur, u) => u);

                var combinedResults = await agentsQuery
                    .Union(adminsQuery)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                await PopulateUserRolesAndAdminInfo(combinedResults);
                return combinedResults;
            }

            
            var selfResult = await _context.Users
                .Where(u => u.Id == requestingUser.Id)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            await PopulateUserRolesAndAdminInfo(selfResult);
            return selfResult;
        }

        private async Task PopulateUserRolesAndAdminInfo(List<UserDTO> users)
        {
            foreach (var userDto in users)
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);
                if (user != null)
                {
                    
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Role = roles.FirstOrDefault();

                    
                    if (userDto.Role == UserRoles.Agent)
                    {
                        var adminClaim = (await _userManager.GetClaimsAsync(user))
                            .FirstOrDefault(c => c.Type == "ManagedByAdmin");
                        userDto.AdminId = adminClaim?.Value;
                    }
                }
            }
        }
    }
}