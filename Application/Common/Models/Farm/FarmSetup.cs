using System.Collections.Generic;
using System.Linq;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Models.Farm
{
    public class FarmSetup
    {

        public List<Account> Accounts => Bots.Concat(Mules).ToList();
        
        public IReadOnlyList<Account> Bots => _bots.AsReadOnly();

        private readonly List<Account> _bots;

        public IReadOnlyList<Account> Mules => _mules.AsReadOnly();

        private readonly List<Account> _mules;

        public FarmSetup(List<Account> bots, List<Account> mules)
        {
            _bots = bots;
            _mules = mules;
        }

        public FarmSetup() : this(new List<Account>(), new List<Account>())
        {
        }

        public bool IsEmpty()
        {
            return Bots.Count == 0;
        }

        public FarmSetup RemoveAccounts(IEnumerable<Account> accounts)
        {
            var accountIds = accounts.Select(a => a.ID).ToList();
            
            var bots = new List<Account>(_bots.Where(a => !accountIds.Contains(a.ID)));
            var mules = new List<Account>(_mules.Where(a => !accountIds.Contains(a.ID)));
            return new FarmSetup(bots, mules);
        }

        public static readonly FarmSetup EmptyFarm = new FarmSetup();
    }
}
