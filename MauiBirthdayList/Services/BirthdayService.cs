using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiBirthdayList.Services;
public class BirthdayService
{
	private readonly HttpClient _client;
	private const string BaseUrl = "https://birthdaysrest.azurewebsites.net/api/persons";

	public BirthdayService()
	{
		_client = new HttpClient();
	}

	public async Task<List<Person>> GetAllAsync(string userId)
	{
		var all = await _client.GetFromJsonAsync<List<Person>>(BaseUrl) ?? [];
		return all.Where(p => p.UserId == userId).ToList();
	}


	public async Task<Person> CreateAsync(Person person)
	{
		var response = await _client.PostAsJsonAsync(BaseUrl, person);
		return await response.Content.ReadFromJsonAsync<Person>();
	}

	public async Task UpdateAsync(Person person) =>
		await _client.PutAsJsonAsync($"{BaseUrl}/{person.Id}", person);

	public async Task DeleteAsync(int id) =>
		await _client.DeleteAsync($"{BaseUrl}/{id}");
}
