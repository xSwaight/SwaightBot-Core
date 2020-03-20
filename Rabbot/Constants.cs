﻿using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbot
{
    public static class Constants
    {
        #region Streak Constants
        public static readonly int MinimumWordCount = 200;
        #endregion

        #region Emotes
        public static readonly Emote Sword = Emote.Parse("<a:sword:593493621400010795>");
        public static readonly Emote Shield = Emote.Parse("<a:shield:593498755441885275>");

        public static readonly Emote Glitch = Emote.Parse("<:glitch:597053743623700490>");
        public static readonly Emote Diego = Emote.Parse("<:diego:597054124294668290>");
        public static readonly Emote Shyguy = Emote.Parse("<:shyguy:597053511951187968>");
        public static readonly Emote Goldenziege = Emote.Parse("<:goldengoat:597052540290465794>");

        public static readonly Emote Doggo = Emote.Parse("<:doggo:597065709339672576>");
        public static readonly Emote Slot = Emote.Parse("<a:slot:597872810760732672>");

        public static readonly Emoji Yes = new Emoji("✅");
        public static readonly Emoji No = new Emoji("❌");

        public static readonly Emoji Fire = new Emoji("🔥");

        public static readonly Emoji thumbsUp = new Emoji("👍");
        public static readonly Emoji thumbsDown = new Emoji("👎");
        #endregion
    }
}