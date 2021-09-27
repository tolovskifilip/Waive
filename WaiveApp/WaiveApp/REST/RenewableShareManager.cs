using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaiveApp.Models;

namespace WaiveApp.REST
{
    public class RenewableShareManager
	{
			IRestService restService;

			public RenewableShareManager(IRestService service)
			{
				restService = service;
			}

			public Task<List<RenewableShareData>> GetRenewableShareDataAsync(string region)
			{
				return restService.RefreshDataAsync(region);
			}

			//public Task SaveTaskAsync(RenewableShareItem item, bool isNewItem = false)
			//{
			//	return restService.SaveRenewableShareAsync(item, isNewItem);
			//}
			//
			//public Task DeleteTaskAsync(RenewableShareItem item)
			//{
			//	return restService.DeleteRenewableShareAsync(item.ID);
			//}
	}
}


