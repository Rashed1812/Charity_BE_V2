using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.NavItemDto;

namespace BLL.Service
{
    public class NavItemService : INavItemService
    {
        private readonly INavItemRepository _navItemRepository;

        public NavItemService(INavItemRepository navItemRepository)
        {
            _navItemRepository = navItemRepository;
        }

        public async Task<List<NavItems>> GetAllNavItemsAsync()
        {
            return await _navItemRepository.GetAllNavItemsAsync();
        }

        public async Task<NavItemDto?> GetNavItemByIdAsync(int id)
        {
            return await _navItemRepository.GetNavItemByIdAsync(id);
        }

        public async Task AddNavItemAsync(NavItems navItem)
        {
            await _navItemRepository.AddNavItemAsync(navItem);
        }

        public async Task<bool> UpdateNavItemAsync(int id, NavItems navItem)
        {
            var existingNavItem = await _navItemRepository.FindNavItemEntityAsync(id);
            if (existingNavItem == null)
                return false;

            existingNavItem.label = navItem.label;
            existingNavItem.href = navItem.href;

            await _navItemRepository.UpdateNavItemAsync(existingNavItem);
            return true;
        }

        public async Task<bool> DeleteNavItemAsync(int id)
        {
            var existingNavItem = await _navItemRepository.FindNavItemEntityAsync(id);
            if (existingNavItem == null)
                return false;

            await _navItemRepository.DeleteNavItemAsync(id);
            return true;
        }

        public async Task<List<Pages>> GetPagesAsync(int navItemId)
        {
            return await _navItemRepository.GetPagesByNavItemIdAsync(navItemId);
        }

        public async Task<bool> AddPageAsync(int navItemId, PageDto page)
        {
            var navItem = await _navItemRepository.FindNavItemEntityAsync(navItemId);
            if (navItem == null)
                return false;

            var newPage = new Pages
            {
                subTilte = page.subTilte,
                subLink = page.subLink,
                NavItemsId = navItemId
            };

            await _navItemRepository.AddPageAsync(newPage);
            return true;
        }

        public async Task<bool> UpdatePageAsync(int pageId, PageDto page)
        {
            var existingPage = await _navItemRepository.FindPageEntityAsync(pageId);
            if (existingPage == null)
                return false;

            existingPage.subTilte = page.subTilte;
            existingPage.subLink = page.subLink;

            await _navItemRepository.UpdatePageAsync(existingPage);
            return true;
        }

        public async Task<bool> DeletePageAsync(int pageId)
        {
            var existingPage = await _navItemRepository.FindPageEntityAsync(pageId);
            if (existingPage == null)
                return false;

            await _navItemRepository.DeletePageAsync(pageId);
            return true;
        }
    }
}

