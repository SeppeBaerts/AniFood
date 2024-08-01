using AniFoodNew.Models.Classes;
using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.DataB
{
    public class ServerSendConnector : BaseServerConnector
    {

        public async Task<ServerAnswer<UserInfo>> RegisterUser(RegisterModel model)
        {
            JObject registerObj = new()
            {
                ["email"] = model.Email,
                ["password"] = model.Password,
                ["nickname"] = model.NickName,
                ["firstName"] = model.FirstName,
                ["lastName"] = model.LastName,
            };

            var answer = new ServerAnswer<UserInfo>();
            try
            {
                if (!await RegisterUserAsync(registerObj))
                {
                    answer.AddError("could not register user");
                    return answer;
                }
                if (!await SetBearerToken(model.Email, model.Password))
                {
                    answer.AddError("could not get the bearer token.");
                    return answer;
                }
                if (!string.IsNullOrWhiteSpace(model.FamilyCode))
                {
                    if (!await ConnectUserFamilyAsync(model.FamilyCode))
                    {
                        answer.AddError("could not connect user to family");
                        return answer;
                    }
                }
                else
                {
                    FamilyRegisterModel mod = new()
                    {
                        Name = model.FamilyName!
                    };
                    if (!await CreateFamilyAsync(mod))
                    {
                        answer.AddError("could not create a new family");
                        return answer;
                    }
                }
            }
            catch (Exception ex)
            {
                answer.Errors.Add($"Something went wrong: {ex.Message}");
            }
            return answer;

        }
        /// <summary>
        /// Registers a user in the database.
        /// </summary>
        /// <param name="registerObj">Needs: "email", "password", "nickname", "firstName", "lastName"</param>
        /// <returns>The bearer token of the user</returns>
        private async Task<bool> RegisterUserAsync(JObject registerObj)
        {
            string registerObjString = JsonConvert.SerializeObject(registerObj);
            //verzenden naar de api
            var response = await _client.PostAsync(BaseApiLink + "api/Authentication/register", new StringContent(registerObjString, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> AddUserToFamilyCode(string famcode)
        {
            return await ConnectUserFamilyAsync(famcode);
        }
        private async Task<bool> ConnectUserFamilyAsync(string famcode)
        {
            var response = await _client.GetAsync($"{BaseApiLink}api/Family/AddUser/{famcode}");
            return response.IsSuccessStatusCode;
        }
        private async Task<bool> CreateFamilyAsync(FamilyRegisterModel model)
        {
            string register = JsonConvert.SerializeObject(model);
            StringContent content = new(register, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(BaseApiLink + "api/Family/Create", content);
            return response.IsSuccessStatusCode;
        }
        public async Task<Guid> CreateFamilyAndGetGuidAsync(FamilyRegisterModel model)
        {
            string register = JsonConvert.SerializeObject(model);
            StringContent content = new(register, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(BaseApiLink + "api/Family/Create", content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                if (Guid.TryParse(responseData.Trim('\"'), out Guid id))
                    return id;
                throw new Exception("Could not parse the responseData.");
            }
            throw new Exception("Something went wrong while trying to create the family.");
        }
        /// <summary>
        /// THIS WILL DELETE A FAMILY!
        /// </summary>
        /// <param name="famId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFamilyAsync(Guid famId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{BaseApiLink}api/Family/Remove/{famId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> SetBearerToken(string email, string password)
        {
            LoginModel login = new()
            {
                Email = email,
                Password = password
            };
            string jsonLoginData = JsonConvert.SerializeObject(login);
            StringContent content = new(jsonLoginData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{BaseApiLink}api/Authentication/token", content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseData);
                var resp = (json["token"] ?? throw new Exception("Token cannot be null")).ToString();
                Preferences.Set(LoginTokenLocation, resp);
                var auth = new AuthenticationHeaderValue("Bearer", resp);
                BaseViewModel.ServerGetter.ChangeAuthorisation(auth);
                _client.DefaultRequestHeaders.Authorization = auth;
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<ServerAnswer<Animal>> RegisterAnimalAsync(AnimalRegisterModel model)
        {
            try
            {
                if (File.Exists(model.ImageUri))
                {
                    string answer = await UploadPhotoAsync(model.ImageUri, "Animal/UploadImage") ?? "unknown.png";
                    model.ImageUri = answer;
                }
            }
            catch
            {
                await Toast.Make("something went wrong while trying to upload the photo.").Show();
            }
            string register = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(register, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(BaseApiLink + "api/Animal/Add", content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                responseData = responseData.Trim('\"');
                FullFamily animFam = BaseViewModel.MainUser.Families.Where(f => f.FamilyId == model.MainFamilyId).First();
                if (Guid.TryParse(responseData, out Guid id))
                {
                    Animal anim = new()
                    {
                        FoodAmountPerDay = model.FoodAmountPerDay,
                        FoodTimesPerDay = model.FoodTimesPerDay,
                        ImageUri = model.ImageUri ?? "unknown.png",
                        Name = model.Name,
                        Id = id,
                        MainFamily = animFam,
                        TimesFed = 0,
                        Birthday = model.Birthday ?? DateTime.Today,
                        Breed = model.Breed,
                        Notes = model.Notes,

                    };
                    animFam.Animals.Add(anim);
                    return new ServerAnswer<Animal>() { ServerResponse = anim };
                }
                else
                    return new ServerAnswer<Animal>() { Errors = { "Something went wrong while trying to add the animal guid to the family." } };
            }
            return new ServerAnswer<Animal>() { Errors = { "Something went wrong while trying to register the animal." } };
        }
        public async Task<string?> UploadPhotoAsync(string pathToPhoto, string pathToUploadZone)
        {
            if (pathToPhoto == null|| !File.Exists(pathToPhoto))
                return "unknown.png";
            var content = new MultipartFormDataContent
            {
                { new StreamContent(File.OpenRead(pathToPhoto)), "file", $"file{new FileInfo(pathToPhoto).Extension}"}
            };
            var response = await _client.PostAsync(BaseApiLink + $"api/{pathToUploadZone}", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }
        public async Task<ServerAnswer<Food>> RegisterFoodAsync(FoodRegisterModel model, List<Animal> animalIds)
        {
            try
            {
                string answerUri = await UploadPhotoAsync(model.ImageUri, "Food/UploadImage") ?? "unknown.png";
                model.ImageUri = answerUri;
            }
            catch
            {
                await Toast.Make("something went wrong while trying to upload the photo.").Show();
            }
            var answer = await RegisterFoodAsync(model);
            if (answer.IsSuccess)
            {
                foreach (Animal ani in animalIds)
                {
                    try
                    {
                        if (!await AddFoodToAnimal(ani.Id, answer.ServerResponse.Id))
                            answer.Errors.Add($"Something went wrong while trying to add the food to animal with id {ani.Id}");
                        else
                            answer.ServerResponse.Animals.Add(ani);
                    }
                    catch
                    {
                        answer.Errors.Add($"Something went wrong while trying to add the food to animal with id {ani.Id}");
                    }
                }
            }
            return answer;
        }
        public async Task<ServerAnswer<Food>> RegisterFoodAsync(FoodRegisterModel model)
        {
            string register = JsonConvert.SerializeObject(model);
            StringContent content = new(register, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(BaseApiLink + "api/Food/Create", content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                responseData = responseData.Trim('\"');
                FullFamily foodFam = BaseViewModel.MainUser.Families.Where(f => f.FamilyId == model.ParentFamily).First();
                if (Guid.TryParse(responseData, out Guid id))
                {
                    Food food = new()
                    {
                        Id = id,
                        Name = model.Name,
                        ImageUri = model.ImageUri ?? "unknown.png",
                        Capacity = model.Capacity,
                        CurrentCapacity = model.CurrentCapacity ?? model.Capacity,
                        ParentFamily = foodFam,
                        Animals = []
                    };
                    foodFam.Foods.Add(food);
                    return new ServerAnswer<Food>() { ServerResponse = food };
                }
                else
                    return new ServerAnswer<Food>() { Errors = { "Something went wrong while trying to add the food guid to the family." } };
            }
            return new ServerAnswer<Food>() { Errors = { "Something went wrong while trying to register the food." } };
        }

        public async Task<bool> UpdateFoodBagsAsync(Guid foodId, int amount)
        {
            var response = await _client.GetAsync($"{BaseApiLink}api/Food/UpdateFoodBags/{foodId}/{amount}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFoodAsync(Guid foodId)
        {
            var response = await _client.DeleteAsync($"{BaseApiLink}api/Food/DeleteFood/{foodId}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteUserFromFamily(Guid familyId, Guid userId)
        {
            var response = await _client.DeleteAsync($"{BaseApiLink}api/Family/RemoveUser/{familyId}/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddFoodToAnimal(Guid animalId, Guid foodId)
        {
            var response = await _client.GetAsync($"{BaseApiLink}api/Food/AddToAnimal/{animalId}/{foodId}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> LeaveFamily(Guid familyId)
        {
            var response = await _client.DeleteAsync($"{BaseApiLink}api/Family/CloseFamily/{familyId}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAnimal(Guid animalId)
        {
            var response = await _client.DeleteAsync($"{BaseApiLink}api/Animal/RemoveAnimal/{animalId}");
            return response.IsSuccessStatusCode;
        }
        public async Task<ServerAnswer<bool>> FeedAnimals(List<Guid> animals)
        {
            ServerAnswer<bool> answer = new();
            bool hasFedAll = true;
            foreach (Guid id in animals)
            {
                try
                {
                    if (!await FeedAnimal(id))
                    {
                        answer.Errors.Add($"Something went wrong while trying to feed animal with id {id}");
                        hasFedAll = false;
                    }
                }
                catch
                {
                    answer.Errors.Add($"Something went wrong while trying to feed animal with id {id}");
                    hasFedAll = false;
                }
            }
            answer.ServerResponse = hasFedAll;
            return answer;
        }
        public async Task<bool> FeedAnimal(Guid animalId)
        {
            var response = await _client.GetAsync($"{BaseApiLink}api/Animal/FeedAnimal/{animalId}");
            var content = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ChangeAnimal(TempAnimal animal)
        {
            string registerObjString = JsonConvert.SerializeObject(animal);
            var response = await _client.PutAsync(BaseApiLink + "api/Animal/ChangeAnimal", new StringContent(registerObjString, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

    }
}

