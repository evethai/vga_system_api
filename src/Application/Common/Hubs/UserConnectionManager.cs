using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Hubs
{
    public class UserConnectionManager
    {
        private static readonly Dictionary<string, List<string>> _userConnections = new Dictionary<string, List<string>>();

        public void AddConnection(string accountId, string connectionId)
        {
            lock (_userConnections)
            {
                if (!_userConnections.ContainsKey(accountId))
                {
                    _userConnections[accountId] = new List<string>();
                }
                _userConnections[accountId].Add(connectionId);
            }
        }

        public void RemoveConnection(string accountId, string connectionId)
        {
            lock (_userConnections)
            {
                if (_userConnections.ContainsKey(accountId))
                {
                    _userConnections[accountId].Remove(connectionId);
                    if (_userConnections[accountId].Count == 0)
                    {
                        _userConnections.Remove(accountId);
                    }
                }
            }
        }

        public List<string> GetConnections(string accountId)
        {
            lock (_userConnections)
            {
                return _userConnections.ContainsKey(accountId) ? _userConnections[accountId] : new List<string>();
            }
        }
    }

}
