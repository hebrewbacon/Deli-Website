using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItalianDeli.Models;
using System.Transactions;

namespace ItalianDeli.Controllers
{
    public class DbReloadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //TODO: Add a reload button in the view which when clicked calls ReloadItemsAndCategories() function
            ReloadItemsAndCategories();

            return View();
        }

        private void ReloadItemsAndCategories()
        {
            ClearItemsAndCategories();
            CreateCalzones();
            CreateDesserts();
            CreatePastas();
            CreateSalads();
            CreateSandwiches();
            CreateSides();
            CreateStrombolis();
        }

        private void ClearItemsAndCategories()
        {
            using (var transaction = new TransactionScope())
            {
                //Remove all Items from database
                if (db.Items.Count() > 0)
                {
                    db.Items.RemoveRange(db.Items);
                    db.SaveChanges();
                }

                //Remove all Categories from database
                if (db.Catagories.Count() > 0)
                {
                    db.Catagories.RemoveRange(db.Catagories);
                    db.SaveChanges();
                }

                //Run the transaction
                transaction.Complete();
            }
        }

        private void CreateCalzones()
        {
            Catagorie calzones = new Catagorie()
            {
                Name = "Calzones"
            };

            if(db.Catagories.Any(c => c.Name == calzones.Name))
            {
                //Don't create calzones category if it already exists
                return;
            }

            Item hamCalzone = new Item()
            {
                Name = "Ham Calzone",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Calzones/ham_calzone.jpg")
            };

            Item pepperoniCalzone = new Item()
            {
                Name = "Pepperoni Calzone",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Calzones/pepperoni_calzone.jpg")
            };

            Item spinachCalzone = new Item()
            {
                Name = "Spinach Calzone",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Calzones/spinach_calzone.jpg")
            };

            calzones.Items = new List<Item> { hamCalzone, pepperoniCalzone, spinachCalzone };
            db.Catagories.Add(calzones);
            db.SaveChanges();
        }

        private void CreateDesserts()
        {
            Catagorie desserts = new Catagorie()
            {
                Name = "Desserts"
            };

            if (db.Catagories.Any(c => c.Name == desserts.Name))
            {
                //Don't create desserts category if it already exists
                return;
            }

            Item brownies = new Item()
            {
                Name = "Brownies",
                Price = 1.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/brownies.jpg")
            };

            Item cannoli = new Item()
            {
                Name = "Cannoli",
                Price = 4.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/cannoli.jpg")
            };

            Item carrotCake = new Item()
            {
                Name = "Carrot Cake",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/carrot_cake.jpg")
            };

            Item cheeseCake = new Item()
            {
                Name = "Cheesecake",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/cheesecake.jpg")
            };

            Item chocolateCake = new Item()
            {
                Name = "Chocolate Cake",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/chocolate_cake.jpg")
            };

            Item cookies = new Item()
            {
                Name = "Cookies",
                Price = 2.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/cookies.jpg")
            };

            Item ricePudding = new Item()
            {
                Name = "Rice Pudding",
                Price = 2.99m,
                ItemPictureUrl = Url.Content("~/Images/Desserts/rice_pudding.jpg")
            };

            desserts.Items = new List<Item> { brownies, cannoli, carrotCake, cheeseCake, chocolateCake, cookies, ricePudding };
            db.Catagories.Add(desserts);
            db.SaveChanges();
        }

        private void CreatePastas()
        {
            Catagorie pastas = new Catagorie()
            {
                Name = "Pastas"
            };

            if (db.Catagories.Any(c => c.Name == pastas.Name))
            {
                //Don't create pastas category if it already exists
                return;
            }

            Item chickenAlfredo = new Item()
            {
                Name = "Chicken Alfredo",
                Price = 10.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/chicken_alfredo.jpg")
            };

            Item eggplantParmesan = new Item()
            {
                Name = "Eggplant Parmesan",
                Price = 10.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/eggplant_parmesan.jpg")
            };

            Item lasagna = new Item()
            {
                Name = "Lasagna",
                Price = 8.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/lasagna.jpg")
            };

            Item manicotti = new Item()
            {
                Name = "Manicotti",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/manicotti.jpg")
            };

            Item ravioli = new Item()
            {
                Name = "Ravioli",
                Price = 8.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/ravioli.jpg")
            };

            Item spaghetti = new Item()
            {
                Name = "Spaghetti",
                Price = 7.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/spaghetti.jpg")
            };

            Item stuffedShells = new Item()
            {
                Name = "Stuffed Shells",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Pastas/stuffed_shells.jpg")
            };

            pastas.Items = new List<Item> { chickenAlfredo, eggplantParmesan, lasagna, manicotti, ravioli, spaghetti, stuffedShells };
            db.Catagories.Add(pastas);
            db.SaveChanges();
        }

        private void CreateSalads()
        {
            Catagorie salads = new Catagorie()
            {
                Name = "Salads"
            };

            if (db.Catagories.Any(c => c.Name == salads.Name))
            {
                //Don't create salads category if it already exists
                return;
            }

            Item caesarSalad = new Item()
            {
                Name = "Caesar Salad",
                Price = 5.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/caesar_salad.jpg")
            };

            Item chefSalad = new Item()
            {
                Name = "Chef Salad",
                Price = 7.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/chef_salad.jpg")
            };

            Item gardenSalad = new Item()
            {
                Name = "Garden Salad",
                Price = 5.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/garden_salad.jpg")
            };

            Item greekSalad = new Item()
            {
                Name = "Greek Salad",
                Price = 7.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/greek_salad.jpg")
            };

            Item houseSalad = new Item()
            {
                Name = "House Salad",
                Price = 5.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/house_salad.jpg")
            };

            Item shrimpSalad = new Item()
            {
                Name = "Shrimp Salad",
                Price = 9.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/shrimp_salad.jpg")
            };

            Item tunaSalad = new Item()
            {
                Name = "Tuna Salad",
                Price = 7.99m,
                ItemPictureUrl = Url.Content("~/Images/Salads/tuna_salad.jpg")
            };

            salads.Items = new List<Item> { caesarSalad, chefSalad, gardenSalad, greekSalad, houseSalad, shrimpSalad, tunaSalad };
            db.Catagories.Add(salads);
            db.SaveChanges();
        }

        private void CreateSandwiches()
        {
            Catagorie sandwiches = new Catagorie()
            {
                Name = "Sandwiches"
            };

            if (db.Catagories.Any(c => c.Name == sandwiches.Name))
            {
                //Don't create sandwiches category if it already exists
                return;
            }

            Item chickenSandwich = new Item()
            {
                Name = "Chicken Sandwich",
                Price = 4.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/chicken_sandwich.jpg")
            };

            Item fishSandwich = new Item()
            {
                Name = "Fish Sandwich",
                Price = 5.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/fish_sandwich.jpg")
            };

            Item grilledCheese = new Item()
            {
                Name = "Grilled Cheese",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/grilled_cheese.jpg")
            };

            Item hamSandwich = new Item()
            {
                Name = "Ham Sandwich",
                Price = 4.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/ham_sandwich.jpg")
            };

            Item roastBeefSandwich = new Item()
            {
                Name = "Roast Beef Sandwich",
                Price = 6.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/roastbeef_sandwich.jpg")
            };

            Item tunaSandwich = new Item()
            {
                Name = "Tuna Sandwich",
                Price = 4.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/tuna_sandwich.jpg")
            };

            Item turkeySandwich = new Item()
            {
                Name = "Turkey Sandwich",
                Price = 4.99m,
                ItemPictureUrl = Url.Content("~/Images/Sandwiches/turkey_Sandwich.jpg")
            };

            sandwiches.Items = new List<Item> { chickenSandwich, fishSandwich, grilledCheese, hamSandwich, roastBeefSandwich, tunaSandwich, turkeySandwich };
            db.Catagories.Add(sandwiches);
            db.SaveChanges();
        }

        private void CreateSides()
        {
            Catagorie sides = new Catagorie()
            {
                Name = "Sides"
            };

            if (db.Catagories.Any(c => c.Name == sides.Name))
            {
                //Don't create sides category if it already exists
                return;
            }

            Item buffaloWings = new Item()
            {
                Name = "Buffalo Wings",
                Price = 6.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/buffalo_wings.jpg")
            };

            Item chickenNuggets = new Item()
            {
                Name = "Chicken Nuggets",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/chicken_nuggets.jpg")
            };

            Item fries = new Item()
            {
                Name = "Fries",
                Price = 2.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/fries.jpg")
            };

            Item garlicBread = new Item()
            {
                Name = "Garlic Bread",
                Price = 2.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/garlic_bread.jpg")
            };

            Item jalapenoPoppers = new Item()
            {
                Name = "Jalapeno Poppers",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/jalapeno_poppers.jpg")
            };

            Item mozzarellaSticks = new Item()
            {
                Name = "Mozzarella Sticks",
                Price = 3.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/mozzarella_sticks.jpg")
            };

            Item onionRings = new Item()
            {
                Name = "Onion Rings",
                Price = 2.99m,
                ItemPictureUrl = Url.Content("~/Images/Sides/onion_rings.jpg")
            };

            sides.Items = new List<Item> { buffaloWings, chickenNuggets, fries, garlicBread, jalapenoPoppers, mozzarellaSticks, onionRings };
            db.Catagories.Add(sides);
            db.SaveChanges();
        }

        private void CreateStrombolis()
        {
            Catagorie strombolis = new Catagorie()
            {
                Name = "Strombolis"
            };

            if (db.Catagories.Any(c => c.Name == strombolis.Name))
            {
                //Don't create strombolis category if it already exists
                return;
            }

            Item cheeseSteakStromboli = new Item()
            {
                Name = "CheeseSteak Stromboli",
                Price = 11.99m,
                ItemPictureUrl = Url.Content("~/Images/Strombolis/cheesesteak_stromboli.jpg")
            };

            Item chickenStromboli = new Item()
            {
                Name = "Chicken Stromboli",
                Price = 10.99m,
                ItemPictureUrl = Url.Content("~/Images/Strombolis/chicken_stromboli.jpg")
            };

            Item greekStromboli = new Item()
            {
                Name = "Greek Stromboli",
                Price = 12.99m,
                ItemPictureUrl = Url.Content("~/Images/Strombolis/greek_stromboli.jpg")
            };

            Item italianStromboli = new Item()
            {
                Name = "Italian Stromboli",
                Price = 11.99m,
                ItemPictureUrl = Url.Content("~/Images/Strombolis/italian_stromboli.jpg")
            };

            strombolis.Items = new List<Item> { cheeseSteakStromboli, chickenStromboli, greekStromboli, italianStromboli };
            db.Catagories.Add(strombolis);
            db.SaveChanges();
        }
    }
}