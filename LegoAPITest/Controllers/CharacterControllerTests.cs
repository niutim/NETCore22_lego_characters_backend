using LegoAPI.Controllers;
using LegoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LegoAPITest.Controllers
{
    [TestClass]
    public class CharacterControllerTests
    {
        private readonly CharacterController _characterController;

        public CharacterControllerTests()
        {
            DbContextOptionsBuilder<CharacterContext> optionsBuilder = new DbContextOptionsBuilder<CharacterContext>();
            _characterController = new CharacterController(new CharacterContext(optionsBuilder.Options, true));

        }

        [TestMethod]
        public void CharacterItemsList_should_not_be_null() {
            var task = TaskGetCharactersAsync();
            task.Wait();
            Assert.IsNotNull(task.Result, "La valeur retournée est nulle.");
        }

        [TestMethod]
        public void CharacterItemsList_should()
        {
            var task = TaskGetCharactersAsync();
            task.Wait();
            Assert.IsTrue(task.Result is List<CharacterItem>, "Le résultat n'est pas une liste de personnage.");
        }

        [TestMethod]
        public void CharacterItemsList_at_least_one_should()
        {
            var task = TaskGetCharactersAsync();
            task.Wait();
            Assert.IsTrue(task.Result.Count>0, "La liste doit contenir au moins un élément.");
        }

        [TestMethod]
        public void CharacterItemsList_add_one_should()
        {
            var taskGet = TaskGetCharactersAsync();
            taskGet.Wait();

            int count = taskGet.Result.Count;

            TaskPostCharacterAsync(new CharacterItem() { Name = "Lloyd Garmaddon", BasedOn = "Ninjago" });

            taskGet = TaskGetCharactersAsync();
            taskGet.Wait();

            Assert.IsTrue(taskGet.Result.Count > count, "La liste devrait contenir un élément de plus.");

            CharacterItem newItem = taskGet.Result.Find(elt => elt.Name == "Lloyd Garmaddon");
            Assert.IsNotNull(newItem, "La liste ne contient pas l'élément ajouté.");

        }

        [TestMethod]
        public void CharacterItemsList_add_one_and_delete_should()
        {
            var taskGet = TaskGetCharactersAsync();
            taskGet.Wait();

            int count = taskGet.Result.Count;

            TaskPostCharacterAsync(new CharacterItem() { Name = "Sensei Wu", BasedOn = "Ninjago" });

            taskGet = TaskGetCharactersAsync();
            taskGet.Wait();

            Assert.IsTrue(taskGet.Result.Count > count, "La liste devrait contenir un élément de plus.");

            CharacterItem newItem = taskGet.Result.Find(elt => elt.Name == "Sensei Wu");
            Assert.IsNotNull(newItem, "La liste ne contient pas l'élément ajouté.");

            TaskDeleteCharacterAsync(newItem.Id);

            taskGet = TaskGetCharactersAsync();
            taskGet.Wait();

            Assert.IsTrue(taskGet.Result.Count == count, "La liste devrait contenir le même nombre d'éléments qu'au départ.");

            CharacterItem noItem = taskGet.Result.Find(elt => elt.Name == "Sensei Wu");
            Assert.IsNull(noItem, "La liste ne devrait plus contenir l'élément.");

        }

        private async Task<List<CharacterItem>> TaskGetCharactersAsync() {
            var result = await _characterController.GetCharacterItems();
            if(result is ActionResult<IEnumerable<CharacterItem>>)
            {
                if (result is ActionResult<IEnumerable<CharacterItem>> res) return res.Value as List<CharacterItem>;
            }
            return null;
        }

        private async void TaskPostCharacterAsync(CharacterItem character)
        {
            var response = await _characterController.PostCharacterItem(character);
            if (response is ActionResult<CharacterItem>)
            {
                ActionResult<CharacterItem> result = response as ActionResult<CharacterItem>;
                if (result.Result is CreatedAtActionResult)
                {
                    CreatedAtActionResult actionResult = result.Result as CreatedAtActionResult;
                    if (actionResult != null)
                    {
                        if (actionResult.StatusCode != 201)
                        {
                            Debug.WriteLine("TaskPostCharacterAsync : La valeur StatusCode n'est pas celle attendue.");
                        }
                    } else Debug.WriteLine("TaskPostCharacterAsync : actionResult est incorrect.");
                }
            } else Debug.WriteLine("TaskPostCharacterAsync : La response n'est pas celle attendue.");
        }

        private async void TaskDeleteCharacterAsync(long id)
        {
            var response = await _characterController.DeleteCharacterItem(id);
            if (response is ActionResult)
            {
                ActionResult result = response as ActionResult;
                if (result is NoContentResult)
                {
                    NoContentResult noContentResult = result as NoContentResult;
                    if (noContentResult != null)
                    {
                        if (noContentResult.StatusCode != 204)
                        {
                            Debug.WriteLine("TaskDeleteCharacterAsync : La valeur StatusCode n'est pas celle attendue.");
                        }
                    }
                    else Debug.WriteLine("TaskDeleteCharacterAsync : noContentResult est incorrect.");
                }
            }
        }
    }
}
