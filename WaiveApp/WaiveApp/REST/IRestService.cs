using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaiveApp.Models;

namespace WaiveApp.REST
{
    public interface IRestService
    {
		Task<List<RenewableShareData>> RefreshDataAsync(string region);

		//Task SaveRenewableShareItemAsync(RenewableShareItem item, bool isNewItem);

		//Task DeleteRenewableShareItemAsync(string id);
		
	}
}
