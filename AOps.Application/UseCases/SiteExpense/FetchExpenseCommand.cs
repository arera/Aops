using AOps.Application.DTOs.Expense;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.SiteExpense
{
    public class FetchExpenseCommand():IRequest<List<FetchExpenseDto>>;
   
}
