using System;
using System.Collections.Generic;
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
        private readonly IWorker currencyWork;

        public TransactionController(ApplicationContext context)
        {
            db = context;
            transactionWork = new TransactionWorker(db);
            userWork = new UserWorker(db);
            itemWork=new ItemWorker(db);
            currencyWork = new CurrencyWorker(db);
        }

        ///<summary>Get an income or  incomes </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns a transaction</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="404"> if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
       
        
        [ProducesResponseType(typeof(JsonStructureExample<List<Transaction>>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),404)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [HttpGet]
        public async Task Get([FromHeader] string id,[FromHeader] DateTime? fromDate ,DateTime? toDate,[FromHeader] string type)
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

                    await Response.WriteAsync(JsonResponseFactory.CreateJson( 
                         
                        Response.StatusCode==200 ?new
                        {
                            transaction.Id,
                            transaction.UserId,
                            transaction.Money,
                            transaction.Date,
                            Type=transaction.Type.ToString(),
                            transaction.Description,
                            transaction.ItemId,
                            WorkType=transaction.WorkType.ToString(),
                            transaction.CurrencyId
                        }:null));
                }
                else if (id == null)
                {
                    Response.ContentType = "application/json";

                    Response.StatusCode = 200;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( 
                        transactionWork.GetEntities().Cast<Transaction>().Where(u =>
                                (((fromDate==null?true:u.Date>=fromDate)&&(toDate==null?true:u.Date<=toDate))&&type!=null?type.Trim().Equals("Outgo",StringComparison.OrdinalIgnoreCase)?u.ItemId!=null&&u.WorkType==null:type.Trim().Equals("Income",StringComparison.OrdinalIgnoreCase)?u.WorkType!=null&&u.ItemId==null:true:true&&(u.User.UserName ==
                                    User.Identity.Name ||(u.User.Family?.Users.Exists(usr=>usr.UserName==User.Identity.Name) ?? false)))).Select(t=>new
                            {
                                t.Id,
                                t.UserId,
                                t.Money,
                                t.Date,
                                Type=t.Type.ToString(),
                                t.Description,
                                t.ItemId,
                                WorkType=t.WorkType.ToString(),
                                t.CurrencyId
                            })
                            .ToList<object>()));
                }
            
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }
        }

        ///<summary>Add a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="200">Returns a transaction that was aded </response>
        ///<response code="400">"Bad request"</response>
        
        [ProducesResponseType(typeof(JsonStructureExample<Transaction>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),404)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [HttpPost]
        public async Task Post([FromBody] Transaction transaction)
        {
            try
            {
                

                if (transaction.ItemId != null)
                {
                    ((ItemWorker)itemWork).ManageItemData(itemWork.GetEntity(transaction.ItemId),transaction.Money,true);
                }

                if ((transaction.ItemId != null && transaction.WorkType != null) ||
                    (transaction.ItemId == null && transaction.WorkType == null))
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null,
                        new List<object> {"WorkType | ItemId"},
                        new List<string>()
                        {
                            "The transaction can be only outgo or income.\n If outgo the ItemId field must be set but itemId=null , else Item id must be set and WorkType=null"
                        }));
                    return;
                }
                transaction.User = userWork.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                transaction.CurrencyId=(transaction.CurrencyId==0||transaction.CurrencyId==null)?transaction.User.CurrencyId:transaction
                    .CurrencyId;
                if (transaction.User.CurrencyId != transaction.CurrencyId)
                {
                    var userCurrency = userWork.GetEntities().Cast<User>().First(u => u.UserName == User.Identity.Name).Currency;
                    var currentCurrency = (Currency)currencyWork.GetEntity(transaction.CurrencyId);
                    transaction.Money = transaction.Money * currentCurrency.rate / userCurrency.rate;
                    transaction.CurrencyId = userCurrency.Id;
                }
                transaction.User.CashSum += transaction.Money;
                
                transactionWork.AddEntity(transaction);
                
                Response.ContentType = "application/json";

                Response.StatusCode = 200;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(
                    transactionWork.GetEntities().Cast<Transaction>().LastOrDefault()));


            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }
        }

        ///<summary>Delete a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200"></response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [ProducesResponseType(typeof(JsonStructureExample<object>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),403)]

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
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
                    return;
                }
                Response.ContentType = "application/json";

                Response.StatusCode = 403;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
                return;
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
            }
        }

        ///<summary>Edit a transaction</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns transaction that was edited</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if transaction with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [ProducesResponseType(typeof(JsonStructureExample<Transaction>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),403)]



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
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( transaction));
                    return;
                }
                Response.ContentType = "application/json";

                Response.StatusCode = 403;
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null)); ;
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
            }
        }
    }
}