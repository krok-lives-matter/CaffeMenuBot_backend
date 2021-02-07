﻿using System;
using CaffeMenuBot.AppHost.Authentication;
using CaffeMenuBot.Data;

namespace CaffeMenuBot.IntegrationTests
{
    internal static class Utilities
    {
        public static void InitializeDatabaseForTests(AuthorizationDbContext context, IServiceProvider services)
        {
            const string salt = "YWRtaW5AY2FmZmVtZW51Ym90LmNvbQ==";
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                /*Role = "admin",
                Username = "my_username",
                Salt = salt,*/
                PasswordHash = EncryptionProvider.Encrypt("my_password", EncryptionProvider.ReadSaltFromBase64(salt))
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}