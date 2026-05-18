using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace MauiBirthdayList.Services;


public static class FirebaseAuthService
{
	private static readonly FirebaseAuthClient _client;

	static FirebaseAuthService()
	{
		var config = new FirebaseAuthConfig
		{
			ApiKey = "AIzaSyA4yCLW2u-_JXlqhOxVBCdYX-HFEysZ1CE",
			AuthDomain = "compose-valde.firebaseapp.com",
			Providers = new FirebaseAuthProvider[]
			{
				new EmailProvider()
			}
		};

		_client = new FirebaseAuthClient(config);
	}

	public static async Task<UserCredential> LoginAsync(string email, string password)
	{
		return await _client.SignInWithEmailAndPasswordAsync(email, password);
	}

	public static async Task<UserCredential> RegisterAsync(string email, string password)
	{
		return await _client.CreateUserWithEmailAndPasswordAsync(email, password);
	}

	public static void Logout()
	{
		_client.SignOut();
	}

	public static bool IsLoggedIn => _client.User != null;

	public static string? CurrentUserEmail => _client.User?.Info?.Email;
}
