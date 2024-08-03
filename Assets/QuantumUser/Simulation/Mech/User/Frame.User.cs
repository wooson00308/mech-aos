using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum
{
    unsafe partial class Frame
    {
        private byte _activeUsers;
        public byte ActiveUsers => _activeUsers;

        public void UserJoined()
        {
            _activeUsers++;
        }

        public void UserLeft()
        {
            _activeUsers--;
        }

        partial void SerializeUser(FrameSerializer serializer)
        {
            serializer.Stream.Serialize(ref _activeUsers);
        }

        partial void CopyFromUser(Frame frame)
        {
            _activeUsers = frame._activeUsers;
        }
    }
}