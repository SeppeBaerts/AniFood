using AniFoodNew.Models.Classes;
using CommunityToolkit.Maui.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AniFoodNew.DataB
{
    public class ServerGetConnector : BaseServerConnector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A ServerAnswer type bool, when there is no a connection failure, you will get a false and an error </returns>
        public async Task<ServerAnswer<bool>> HasAuthorisation()
        {
            ServerAnswer<bool> answer = new();
            var response = await _client.GetAsync(BaseApiLink + "testauth");
            if (response.IsSuccessStatusCode)
                answer.ServerResponse = true;
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                answer.AddError("notFound");
            else
                answer.ServerResponse = false;
            return answer;
        }
        /// <summary>
        /// Fills in MainUser: FullUser(including the list of Families).
        /// </summary>
        /// <returns>Task with the fullUser profile.</returns>
        public async Task<ServerAnswer<FullUser>> FillUserAsync(IProgress<double> progress)
        {
            ServerAnswer<FullUser> answer = new();
            var response = await _client.GetAsync(BaseApiLink + "api/User/GetUser");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseData);
                var families = obj["familyIds"]!.ToObject<List<Guid>>();
                List<FullFamily> fullFamilies = [];
                int totalFamilies = families.Count;
                foreach (Guid famId in families)
                {
                    var fam = await GetFamilyAsync(famId);
                    fullFamilies.Add(fam.ServerResponse);
                    progress.Report((double)fullFamilies.Count / totalFamilies);
                }
                FullUser user = new()
                {
                    UserId = Guid.Parse(obj["id"].ToString()),
                    UserEmail = obj["email"].ToString(),
                    Families = fullFamilies,
                    NickName = obj["nickName"].ToString(),
                    FirstName = obj["firstName"]?.ToString(),
                    LastName = obj["lastName"]?.ToString(),
                };
                BaseViewModel.MainUser = user;
                answer.ServerResponse = user;
                return answer;
            }
            else
            {
                answer.AddError("Something went wrong trying to get the user.");
                return answer;
            }
        }
        /// <summary>
        /// Moet maar 1x opgeroepen worden.
        /// </summary>
        /// <returns></returns>
        public async Task<ServerAnswer<UserInfo>> GetUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return new ServerAnswer<UserInfo>() { Errors = { "Please enter a valid username and password." } };

            LoginModel login = new()
            {
                Email = email,
                Password = password
            };
            ServerAnswer<UserInfo> answer = new();
            try
            {
                string jsonLoginData = JsonConvert.SerializeObject(login);
                StringContent content = new(jsonLoginData, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{BaseApiLink}api/Authentication/token", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseData);
                    var resp = await GetUserBearerAsync((json["token"] ?? throw new Exception("Token cannot be null")).ToString());
                    answer.ServerResponse = resp.ServerResponse;
                }
                else
                {
                    answer.AddError("Please enter a valid username and password.");
                }
            }
            catch (Exception ex)
            {
                answer.AddError(ex.Message);
            }
            if (answer.IsSuccess)
            {
                FullUser user = new();
                user.LoadFromUserInfo(answer.ServerResponse);
                try
                {
                    foreach (Guid familyId in answer.ServerResponse.FamilyIds)
                    {
                        ServerAnswer<FullFamily> fam = await GetFamilyAsync(familyId);
                        if (fam.IsSuccess)
                            user.Families.Add(fam.ServerResponse);
                    }
                    BaseViewModel.MainUser = user;
                }
                catch (Exception ex)
                {
                    answer.AddError(ex.Message);
                }
            }
            return answer;
        }
        public async Task<string> GetFamilyCodeAsync(Guid familyId)
        {
            try
            {
                var response = await _client.GetAsync(BaseApiLink + "api/Family/GetCode/" + familyId);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                    return "No code Found.";
            }
            catch
            {
                return "No code Found";
            }
        }
        public async Task<ServerAnswer<List<UserInfoModel>>> GetMinUserInfoFromFamilyAsync(Guid famId)
        {
            ServerAnswer<List<UserInfoModel>> answer = new();
            try
            {
                var response = await _client.GetAsync($"{BaseApiLink}api/User/GetFamUsers/{famId}");
                if (response.IsSuccessStatusCode)
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    List<UserInfoModel> users = JsonConvert.DeserializeObject<List<UserInfoModel>>(resp) ?? throw new Exception("Something went wrong with decoding users.");
                    answer.ServerResponse = users;
                }
                else
                {
                    answer.AddError("something went wrong while trying to get the family");
                }
            }
            catch (Exception ex)
            {
                answer.AddError(ex.Message);
            }
            return answer;
        }
        private async Task<ServerAnswer<UserInfo>> GetUserBearerAsync(string token)
        {
            Preferences.Set(LoginTokenLocation, token);
            ServerAnswer<UserInfo> answer = new();
            try
            {
                AuthenticationHeaderValue value = new AuthenticationHeaderValue("Bearer", token);
                _client.DefaultRequestHeaders.Authorization = value;
                BaseViewModel.ServerSender.ChangeAuthorisation(value);
                var response = await _client.GetAsync(BaseApiLink + "api/User/GetUser");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(responseData);
                    UserInfo user = new()
                    {
                        UserId = Guid.Parse(obj["id"]!.ToString()),
                        UserEmail = obj["email"]!.ToString(),
                        FamilyIds = obj["familyIds"]!.ToObject<List<Guid>>()!,
                        NickName = obj["nickName"]!.ToString(),
                        FirstName = obj["firstName"]?.ToString(),
                        LastName = obj["lastName"]?.ToString(),
                    };
                    answer.ServerResponse = user;
                    return answer;
                }
                else
                {
                    answer.Errors.Add("something went wrong while trying to get the user");
                }
            }
            catch (Exception ex)
            {
                answer.Errors.Add(ex.Message);
            }
            return answer;
        }
        private async Task<ServerAnswer<FullFamily>> GetFamilyAsync(Guid familyId)
        {
            BaseViewModel.LoadFavorites();
            ServerAnswer<FullFamily> answer = new();
            try
            {
                var response = await _client.GetAsync($"{BaseApiLink}api/Family/GetGuidFamily/{familyId}");
                if (response.IsSuccessStatusCode)
                {
                    List<Animal> anims = new();
                    List<Food> foods = new();
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(responseData);
                    FullFamily family = new()
                    {
                        FamilyId = Guid.Parse(obj["familyId"]!.ToString()),
                        FamilyName = obj["name"]!.ToString(),
                        UserIds = obj["userIds"]!.ToObject<List<Guid>>()!,
                        FamilyHeadId = Guid.Parse(obj["familyHeadId"]!.ToString()),
                        Animals = anims,
                        Foods = foods,
                        ImageUri = obj["imageUri"]!.ToString(),
                    };

                    try
                    {
                        family.FamilyCode = await GetFamilyCodeAsync(familyId);
                        ServerAnswer<List<UserInfoModel>> famUsers = await GetMinUserInfoFromFamilyAsync(familyId);
                        var animAnswer = await GetAllAnimalsFromFamily(familyId);
                        var foodAnswer = await GetAllFoodsFromFamily(familyId);
                        if (animAnswer.IsSuccess && foodAnswer.IsSuccess && famUsers.IsSuccess)
                        {
                            family.SmallUsers = famUsers.ServerResponse;

                            foods.AddRange(foodAnswer.ServerResponse.Select(food => new Food
                            {
                                Id = food.Id,
                                Name = food.Name,
                                ImageUri = food.ImageUri == "unknown.png" ? food.ImageUri : BaseApiLink + "api/Food/GetImage/" + food.ImageUri,
                                Capacity = food.Capacity,
                                CurrentCapacity = food.CurrentCapacity,
                                ParentFamily = family,
                                Animals = [],
                                Type = food.Type,
                                NewBags = food.NewBags
                            }));
                            foreach (TempAnimal ani in animAnswer.ServerResponse)
                            {
                                Animal a = new Animal
                                {
                                    Food = foods.FirstOrDefault(f => f.Id == ani.FoodId),
                                    FoodAmountPerDay = ani.FoodAmountPerDay,
                                    FoodTimesPerDay = ani.FoodTimesPerDay,
                                    Id = ani.Id,
                                    ImageUri = ani.ImageUri == "unknown.png" ? ani.ImageUri : BaseApiLink + "api/Animal/GetImage/" + ani.ImageUri,
                                    MainFamily = family,
                                    Name = ani.Name,
                                    TimesFed = ani.TimesFed,
                                    LastTimeFed = ani.LastTimeFed,
                                    Breed = ani.Breed,
                                    Birthday = ani.Birthday,
                                    Notes = ani.Notes,
                                    IsFavorite = BaseViewModel.Favorites.Contains(ani.Id)
                                };
                                a.Food?.Animals.Add(a);
                                anims.Add(a);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        answer.AddError(ex.Message);
                        return answer;
                    }
                    answer.ServerResponse = family;
                }
                else
                {
                    answer.AddError("something went wrong while trying to get the family");
                }
            }
            catch (Exception ex)
            {
                answer.AddError(ex.Message);
            }
            return answer;
        }
        private async Task<ServerAnswer<List<TempAnimal>>> GetAllAnimalsFromFamily(Guid familyId)
        {
            ServerAnswer<List<TempAnimal>> answer = new();
            try
            {
                var response = await _client.GetAsync($"{BaseApiLink}api/Animal/GetAnimals/{familyId}");
                if (response.IsSuccessStatusCode)
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    List<TempAnimal> animals = JsonConvert.DeserializeObject<List<TempAnimal>>(resp) ?? throw new Exception("Something went wrong with decoding animals.");

                    answer.ServerResponse = animals;
                }
                else
                {
                    answer.Errors.Add("something went wrong while trying to get the family");
                }
            }
            catch (Exception ex)
            {
                answer.Errors.Add(ex.Message);
            }
            return answer;
        }
        private async Task<ServerAnswer<List<TempFood>>> GetAllFoodsFromFamily(Guid familyId)
        {
            ServerAnswer<List<TempFood>> answer = new();
            try
            {
                var response = await _client.GetAsync($"{BaseApiLink}api/Food/FamilyFood/{familyId}");
                if (response.IsSuccessStatusCode)
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    List<TempFood> foods = JsonConvert.DeserializeObject<List<TempFood>>(resp) ?? throw new Exception("Something went wrong with decoding food.");
                    answer.ServerResponse = foods;
                }
                else
                {
                    answer.Errors.Add("something went wrong while trying to get the Food");
                }
            }
            catch (Exception ex)
            {
                answer.Errors.Add(ex.Message);
            }
            return answer;
        }

    }
}
