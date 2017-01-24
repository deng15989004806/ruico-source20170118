using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.Common;
using Ruico.Dto.Weixin;

namespace Ruico.Application.WeixinModule
{
    public interface IAppMenuService
    {
        AppMenuDTO Add(AppMenuDTO item);

        void Update(AppMenuDTO item);

        void Remove(Guid id);

        AppMenuDTO FindBy(Guid id);

        IPagedList<AppMenuDTO> FindBy(int? appId, string name, int pageNumber, int pageSize);

        IList<NameValueDTO> GetApps();

        void DownloadMenus(int appId);

        void UploadMenus(int appId);
    }
}
