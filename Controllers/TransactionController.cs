using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.json_structure;
using CCostsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCostsProject.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/transactions")]
    public class TransactionController : Controller
    {
        private ApplicationContext db;
        private readonly IWorker transactionWork;
        private readonly IWorker userWork;
        private readonly IWorker itemWork;

        public TransactionController(ApplicationContext context)
        {
            db = context;
            transactionWork = new TransactionWorker(db);
            userWork = new UserWorker(db);
            itemWork=new ItemWorker(db);
        }

        ///<summary>Get an income or  incomes </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns a transaction</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="404"> if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpGet]
        public async Task Get([FromHeader] string id)
        {
            
            int IntegerId;
            try
            {
            if (int.TryParse(id, out IntegerId))
            {
                
                    var currentUser = userWork.GetEntities().Cast<User>()
                        .FirstOrDefault(u => u.UserName == User.Identity.Name);
                    Transaction transaction = (Transaction) transactionWork.GetEntity(IntegerId); 
                    
                        Response.ContentType = "application/json";

                        Response.StatusCode =transaction!=null?transaction.User.Family!=null?(transaction.User.UserName==User.Identity.Name||
                                             transaction.User.Family.Users.Contains(currentUser))?200:404:transaction.User.UserName==User.Identity.Name?200:404:404;

                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", Response.StatusCode==200?"Ok":"Not found", 
                            Response.StatusCode==200?"Success":"Error", 
                            Response.StatusCode==200 ?transaction:null));
                        
                    


               
            }
            else if (id == null)
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 200;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success",
                    transactionWork.GetEntities().Cast<Transaction>().Where(u =>
                            (u.User.UserName ==
                             User.Identity.Name ||(u.User.Family?.Users.Exists(usr=>usr.UserName==User.Identity.Name) ?? false)))
                        .Cast<ITable>().ToList()));

            }
            
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));

            }
           
        }

        ///<summary>Add a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="200">Returns a transaction that was aded </response>
        ///<response code="403">if request data was incorrect</response>
        ///<response code="400">"Bad request"</response>
        [HttpPost]
        public async Task Post([FromBody] Transaction transaction)
        {
            try
            {


                if (transaction.ItemId != null)
                {
                    ((ItemWorker)itemWork).ManageItemData(itemWork.GetEntity(transaction.ItemId),transaction.Money,true);
                }
                transaction.User = userWork.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                transaction.User.CashSum += transaction.Money;
                transactionWork.AddEntity(transaction);
                
                Response.ContentType = "application/json";

                Response.StatusCode = 200;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success",
                    transactionWork.GetEntities().Cast<Transaction>().LastOrDefault()));


            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

        ///<summary>Delete a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200"></response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpDelete]
        public async Task Delete([FromHeader] int id)
        {
            try
            {
                Transaction transaction = (Transaction) transactionWork.GetEntity(id);
                if (transaction != null && transaction.User.UserName == User.Identity.Name)
                {
                    ((ItemWorker)itemWork).ManageItemData(itemWork.GetEntity(transaction.ItemId),transaction.Money,false);

                    transactionWork.DeleteEntity(transaction);
                    Response.ContentType = "application/json";

                    Response.StatusCode = 200;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                    return;
                }
                Response.ContentType = "application/json";

                Response.StatusCode = 403;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbidden", "Error", null));
                return;
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

        ///<summary>Edit a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns transaction that was edited</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpPut]
        public async Task Put([FromBody] Transaction transaction)
        {

            try
            {
                Transaction trans = (Transaction) transactionWork.GetEntity(transaction.Id);
                if (trans != null && trans.User.UserName == User.Identity.Name)
                {
                    transactionWork.EditEntity(trans);
                    Response.ContentType = "application/json";

                    Response.StatusCode = 200;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", transaction));
                    return;
                }
                Response.ContentType = "application/json";

                Response.StatusCode = 403;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbidden", "Error", null)); ;
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
    }
}