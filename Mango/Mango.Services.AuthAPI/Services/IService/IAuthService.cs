﻿using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterationRequestDto registerationRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}