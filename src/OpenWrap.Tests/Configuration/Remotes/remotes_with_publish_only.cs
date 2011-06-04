using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenWrap;
using OpenWrap.Configuration;
using OpenWrap.Services;
using OpenWrap.Testing;
using Tests;
using Tests.Commands.contexts;

namespace Tests.Configuration.Remotes
{
    public class remotes_with_publish_only : command
    {
        public remotes_with_publish_only()
        {
            given_remote_config("sauron", fetchToken: null, publishTokens: "[memory]somewhere");
        }

        [Test]
        public void persisted_has_no_fetch()
        {
            ConfiguredRemotes["sauron"].FetchRepository.ShouldBeNull();
        }
    }
}