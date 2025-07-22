using Business.Interfaces.Implements.Security;
using Data.Interfaces.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Security.Me;
using Mapster;
using MapsterMapper;

public class MeService : IMeService
{
    private readonly IMeRepository _meRepository;
    private readonly IMapper _mapper;

    public MeService(IMeRepository meRepository, IMapper mapper)
    {
        _meRepository = meRepository;
        _mapper = mapper;
    }

    public Task<UserMeDto> GetCurrentUserInfoAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
