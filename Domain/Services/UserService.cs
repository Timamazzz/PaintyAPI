using AutoMapper;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class UserService : ICrudService<User>
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(UserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<User?> CreateAsync(User user)
    {
        var entity = _mapper.Map<DataAccess.Models.User>(user);
        var createdUser = await _userRepository.CreateAsync(entity);
        return _mapper.Map<User>(createdUser);
    }

    public async Task<List<User>?> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<User>>(users);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<User>(user);
    }
    
    public async Task<User?> RegisterAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = await CreateAsync(user);
        return newUser;
    }
    
    public async Task<User?> AuthenticateUserAsync(User user)
    {
        var entity =  await _userRepository.GetByUserNameAsync(user.UserName!);
        
        if (BCrypt.Net.BCrypt.Verify(user.Password, entity!.Password))
        {
            return _mapper.Map<User>(await _userRepository.UpdateAsync(entity));
        }
        else
        {
            return null;
        }
    }
    public async Task UpdateAsync(User user)
    {
        var entity = _mapper.Map<DataAccess.Models.User>(user);
        await _userRepository.UpdateAsync(entity);
    }
    
    public async Task<User?> AddFriendAsync(int senderId, int recipientId)
    {
        var sender = await _userRepository.GetByIdAsync(senderId);
        var recipient = await _userRepository.GetByIdAsync(recipientId);
        
        var senderEntity = _mapper.Map<DataAccess.Models.User>(sender);
        var recipientEntity = _mapper.Map<DataAccess.Models.User>(recipient);
        
        senderEntity.AddFriend(recipientEntity);
        await _userRepository.Save();
        
        return  _mapper.Map<User>(await _userRepository.UpdateAsync(senderEntity));
    }
    

    public async Task DeleteAsync(int id)
    {
        await _userRepository.DeleteByIdAsync(id);
    }
}