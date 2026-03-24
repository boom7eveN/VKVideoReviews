using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Services.AppAuth;

public class JwtTokenService(JwtAuthSettings settings) : IJwtTokenService
{
    public int GetAccessTokenLifetimeSeconds() => settings.AccessTokenLifeTimeMinutes * 60;

    public string CreateAccessToken(UserEntity user)
    {
        var role = user.IsAdmin ? "Admin" : "User";
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("role", role),
            new("vk_user_id", user.VkUserId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(settings.AccessTokenLifeTimeMinutes);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}