﻿using Rabbot.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel.Syndication;
using System.Text;

namespace Rabbot
{
    public static class Extensions
    {
        public static int CountWords(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
                return 0;

            return @this.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
        }

        public static string ToFormattedString(this int @this)
        {
            return @this.ToString("N0", new System.Globalization.CultureInfo("de-DE"));
        }

        public static string ToFormattedString(this DateTime @this)
        {
            return @this.ToString("dd.MM.yyyy HH:mm");
        }

        public static DateTime ToCET(this DateTime @this)
        {
            TimeZoneInfo europeTimeZone;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                europeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            else
                europeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");

            return TimeZoneInfo.ConvertTimeFromUtc(@this, europeTimeZone);
        }

        public static int GetPercentFrom(this int @this, double percent)
        {
            double percentValue = percent / 100;
            double value = Convert.ToDouble(@this);
            return (int)(value * (1 + percentValue)) - @this;
        }

        public static YouTubeVideo GetFirstVideo(this SyndicationFeed @this)
        {
            var firstItem = @this.Items.FirstOrDefault();
            if (firstItem == null)
                return null;

            return new YouTubeVideo { Title = firstItem.Title.Text, UploadDate = firstItem.PublishDate, Id = firstItem.Id.Substring(9), ChannelName = firstItem.Authors.First().Name };
        }
    }
}
