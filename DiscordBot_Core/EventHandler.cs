﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot_Core.API;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DiscordBot_Core.Database;
using DiscordBot_Core.Systems;
using DiscordBot_Core.API.Models;
using DiscordBot_Core.ImageGenerator;
using System.IO;
using ImageFormat = Discord.ImageFormat;

namespace DiscordBot_Core
{
#pragma warning disable CS1998
    class EventHandler
    {
        DiscordSocketClient _client;
        CommandService _service;
        private IServiceProvider services;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            CancellationTokenSource _cancelationTokenSource = new CancellationTokenSource();
            //new Task(async () => await CheckOnlineUsers(), _cancelationTokenSource.Token, TaskCreationOptions.LongRunning).Start();
            new Task(async () => await CheckBannedUsers(), _cancelationTokenSource.Token, TaskCreationOptions.LongRunning).Start();
            new Task(async () => await CheckWarnings(), _cancelationTokenSource.Token, TaskCreationOptions.LongRunning).Start();
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            _client.MessageReceived += MessageReceived;
            _client.MessageDeleted += MessageDeleted;
            _client.JoinedGuild += JoinedGuild;
        }

        private async Task JoinedGuild(SocketGuild guild)
        {
            if (guild.Roles.Where(p => p.Name == "Muted").Count() == 0)
            {
                var mutedPermission = new GuildPermissions(false, false, false, false, false, false, false, false, true, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                await guild.CreateRoleAsync("Muted", mutedPermission, Color.Red);
            }
            var permission = new OverwritePermissions(PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Inherit, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Inherit, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny);
            foreach (var textChannel in guild.TextChannels)
            {
                var muted = guild.Roles.Where(p => p.Name == "Muted").FirstOrDefault();
                await textChannel.AddPermissionOverwriteAsync(muted, permission, null);
            }
            foreach (var voiceChannel in guild.VoiceChannels)
            {
                var muted = guild.Roles.Where(p => p.Name == "Muted").FirstOrDefault();
                await voiceChannel.AddPermissionOverwriteAsync(muted, permission, null);
            }
            using (swaightContext db = new swaightContext())
            {
                var Guild = db.Guild.Where(p => p.ServerId == (long)guild.Id).FirstOrDefault();
                if (Guild == null)
                {
                    await db.Guild.AddAsync(new Guild { ServerId = (long)guild.Id });
                    await db.SaveChangesAsync();
                }
            }
        }

        private async Task CheckWarnings()
        {
            while (true)
            {
                using (swaightContext db = new swaightContext())
                {
                    if (!db.Warning.Any())
                        continue;

                    var warnings = db.Warning.ToList();
                    foreach (var warn in warnings)
                    {
                        if (warn.ActiveUntil < DateTime.Now)
                            db.Warning.Remove(warn);
                        if (warn.Counter >= 3)
                        {
                            var dcGuild = _client.Guilds.Where(p => p.Id == (ulong)warn.ServerId).FirstOrDefault();
                            var dcUser = dcGuild.Users.Where(p => p.Id == (ulong)warn.UserId).FirstOrDefault();
                            var mutedRole = dcGuild.Roles.Where(p => p.Name == "Muted").FirstOrDefault();


                            var roles = dcGuild.CurrentUser.Roles;
                            var rolesTarget = dcGuild.Users.Where(p => p.Id == (ulong)warn.UserId).FirstOrDefault().Roles;
                            int position = 0;
                            int targetPosition = 0;
                            foreach (var item in roles)
                            {
                                if (item.Position > position)
                                    position = item.Position;
                            }
                            foreach (var item in rolesTarget)
                            {
                                if (item.Position > targetPosition)
                                    targetPosition = item.Position;
                            }
                            if (!(mutedRole.Position < position) && dcGuild.CurrentUser.GuildPermissions.ManageRoles)
                            {
                                Console.WriteLine("Mindestens eine meiner Rollen muss in der Reihenfolge über der Muted Rolle stehen!");
                                db.Warning.Remove(warn);
                                await db.SaveChangesAsync();
                                continue;
                            }
                            if (targetPosition > position)
                            {
                                Console.WriteLine("Es fehlen die Berechtigungen um den User zu muten!");
                                db.Warning.Remove(warn);
                                await db.SaveChangesAsync();
                                continue;
                            }
                            DateTime date = DateTime.Now;
                            DateTime banUntil;
                            banUntil = date.AddHours(1);
                            if (db.Muteduser.Where(p => p.ServerId == (long)dcGuild.Id && p.UserId == (long)dcUser.Id).Count() == 0)
                            {
                                var mutedUser = dcGuild.Users.Where(p => p.Id == dcUser.Id).FirstOrDefault();
                                string userRoles = "";
                                foreach (var role in mutedUser.Roles)
                                {
                                    if (!role.IsEveryone && !role.IsManaged)
                                        userRoles += role.Name + "|";
                                }
                                userRoles = userRoles.TrimEnd('|');
                                await db.Muteduser.AddAsync(new Muteduser { ServerId = (long)dcGuild.Id, UserId = (long)dcUser.Id, Duration = banUntil, Roles = userRoles });
                            }
                            else
                            {
                                var ban = db.Muteduser.Where(p => p.ServerId == (long)dcGuild.Id && p.UserId == (long)dcUser.Id).FirstOrDefault();
                                ban.Duration = banUntil;
                            }

                            var guild = db.Guild.Where(p => p.ServerId == (long)dcGuild.Id).FirstOrDefault();
                            var embedPrivate = new EmbedBuilder();
                            embedPrivate.WithDescription($"Du wurdest wegen zu vielen Warnungen auf **{dcGuild.Name}** für **1 Stunde** gemuted.");
                            embedPrivate.AddField("Gemuted bis", banUntil.ToShortDateString() + " " + banUntil.ToShortTimeString());
                            embedPrivate.WithFooter($"Bei einem ungerechtfertigten Mute kontaktiere bitte einen Admin vom {dcGuild.Name} Server.");
                            embedPrivate.WithColor(new Color(255, 0, 0));
                            await dcUser.SendMessageAsync(null, false, embedPrivate.Build());
                            if (guild.LogchannelId != null && guild.Log == 1)
                            {
                                var logchannel = dcGuild.TextChannels.Where(p => p.Id == (ulong)guild.LogchannelId).FirstOrDefault();
                                var embed = new EmbedBuilder();
                                embed.WithDescription($"{dcUser.Mention} wurde aufgrund von 3 Warnings für 1 Stunde gemuted.");
                                embed.WithColor(new Color(255, 0, 0));
                                await logchannel.SendMessageAsync("", false, embed.Build());
                            }
                            db.Warning.Remove(warn);
                        }
                    }
                    await db.SaveChangesAsync();
                }
                await Task.Delay(1000);
            }
        }

        private async Task CheckBannedUsers()
        {
            while (true)
            {
                using (swaightContext db = new swaightContext())
                {
                    if (!(db.Muteduser.Count() == 0) && _client.Guilds.Count() > 0)
                    {
                        var mutes = db.Muteduser.ToList();
                        foreach (var ban in mutes)
                        {
                            var guild = _client.Guilds.Where(p => p.Id == (ulong)ban.ServerId).FirstOrDefault();
                            if (guild == null)
                            {
                                db.Muteduser.Remove(ban);
                                await db.SaveChangesAsync();
                                continue;
                            }
                            var mutedRole = guild.Roles.Where(p => p.Name == "Muted").FirstOrDefault();
                            if (guild.CurrentUser == null)
                                continue;
                            var roles = guild.CurrentUser.Roles;
                            int position = 0;
                            foreach (var item in roles)
                            {
                                if (item.Position > position)
                                    position = item.Position;
                            }
                            var user = guild.Users.Where(p => p.Id == (ulong)ban.UserId).FirstOrDefault();
                            if(user == null)
                            {
                                if (ban.Duration < DateTime.Now)
                                {
                                    db.Muteduser.Remove(ban);
                                    await db.SaveChangesAsync();
                                }
                                continue;
                            }
                            if (guild.CurrentUser.GuildPermissions.ManageRoles == true && position > mutedRole.Position)
                            {
                                if (ban.Duration < DateTime.Now)
                                {
                                    db.Muteduser.Remove(ban);
                                    try
                                    {
                                        await user.RemoveRoleAsync(mutedRole);
                                        var oldRoles = ban.Roles.Split('|');
                                        foreach (var oldRole in oldRoles)
                                        {
                                            var role = guild.Roles.Where(p => p.Name == oldRole).FirstOrDefault();
                                            if (role != null)
                                                await user.AddRoleAsync(role);
                                        }

                                        var Guild = db.Guild.Where(p => p.ServerId == (long)guild.Id).FirstOrDefault();
                                        if (Guild.LogchannelId != null && Guild.Log == 1)
                                        {
                                            var embed = new EmbedBuilder();
                                            embed.WithDescription($"{user.Mention} wurde entmuted.");
                                            embed.WithColor(new Color(0, 255, 0));
                                            var logchannel = guild.TextChannels.Where(p => p.Id == (ulong)Guild.LogchannelId).FirstOrDefault();
                                            await logchannel.SendMessageAsync("", false, embed.Build());
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                                else
                                {
                                    if (user.Roles.Where(p => p.Id == mutedRole.Id).Count() == 0)
                                    {
                                        try
                                        {
                                            var oldRoles = ban.Roles.Split('|');
                                            foreach (var oldRole in oldRoles)
                                            {
                                                if (!oldRole.Contains("everyone"))
                                                {
                                                    var role = guild.Roles.Where(p => p.Name == oldRole).FirstOrDefault();
                                                    if (role != null)
                                                        await user.RemoveRoleAsync(role);
                                                }
                                            }
                                            await user.AddRoleAsync(mutedRole);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                }
                await Task.Delay(1000);
            }
        }

        #region OnlineUser
        //private async Task CheckOnlineUsers()
        //{
        //    int onlineUsers = -1;
        //    int crashCounter = 0;
        //    DateTime time = DateTime.Now;
        //    while (true)
        //    {
        //        try
        //        {
        //            List<Server> server = new List<Server>();
        //            ApiRequest DB = new ApiRequest();
        //            server = await DB.GetServer();
        //            int onlinecount = 0;
        //            foreach (var item in server)
        //            {
        //                if (item.Player_online >= 0)
        //                    onlinecount += item.Player_online;
        //            }
        //            var percent = (Convert.ToDouble(onlinecount) / Convert.ToDouble(onlineUsers)) * 100;
        //            if ((percent < 80) && onlineUsers != -1 && onlinecount != 0)
        //            {
        //                var embed = new EmbedBuilder();
        //                embed.WithDescription($"***Server Liste:***");
        //                embed.WithColor(new Color(111, 116, 124));
        //                using (swaightContext db = new swaightContext())
        //                {
        //                    string crashedServer = "";
        //                    foreach (var item in server)
        //                    {
        //                        if (item.Player_online >= 0)
        //                        {
        //                            string status = "";
        //                            switch (item.State)
        //                            {
        //                                case 0:
        //                                    status = "Offline";
        //                                    crashedServer = item.Name;
        //                                    break;
        //                                case 1:
        //                                    status = "Slow";
        //                                    break;
        //                                case 2:
        //                                    status = "Online";
        //                                    break;
        //                                default:
        //                                    status = "Unknown";
        //                                    break;
        //                            }
        //                            embed.AddField(item.Name, "Status: **" + status + "** | User online: **" + item.Player_online.ToString() + "**", false);
        //                        }
        //                    }
        //                    crashCounter++;
        //                    TimeSpan span = DateTime.Now - time;
        //                    if (db.Guild.Count() > 0)
        //                    {
        //                        foreach (var item in db.Guild)
        //                        {
        //                            if (item.NotificationchannelId != null && item.Notify == 1 && !String.IsNullOrWhiteSpace(crashedServer))
        //                                await _client.Guilds.Where(p => p.Id == (ulong)item.ServerId).FirstOrDefault().TextChannels.Where(p => p.Id == (ulong)item.NotificationchannelId).FirstOrDefault().SendMessageAsync($"**{crashedServer}** ist gecrashed! Das ist der **{crashCounter}.** Crash in den letzten **{span.Days}D {span.Hours}H {span.Minutes}M!**", false, embed.Build());
        //                            else if (item.NotificationchannelId != null && item.Notify == 1)
        //                                await _client.Guilds.Where(p => p.Id == (ulong)item.ServerId).FirstOrDefault().TextChannels.Where(p => p.Id == (ulong)item.NotificationchannelId).FirstOrDefault().SendMessageAsync($"Die Spieleranzahl ist in den letzten **{span.Days}D {span.Hours}H {span.Minutes}M** schon **{crashCounter} mal** eingebrochen!", false, embed.Build());
        //                        }
        //                    }
        //                }
        //            }
        //            onlineUsers = onlinecount;
        //            if (onlinecount > 0)
        //                await _client.SetGameAsync($"{onlinecount} Players online!", null, ActivityType.Watching);
        //            else
        //                await _client.SetGameAsync($"Auth Server is down!", null, ActivityType.Watching);
        //            await Task.Delay(10000);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Fehler: " + e.Message + "\n" + e.StackTrace);
        //        }
        //    }
        //}
        #endregion

        private async Task MessageReceived(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return;
            using (swaightContext db = new swaightContext())
            {
                if (db.Badwords.Any(p => Helper.replaceCharacter(msg.Content).Contains(p.BadWord, StringComparison.OrdinalIgnoreCase)) && !(msg.Author as SocketGuildUser).GuildPermissions.ManageMessages)
                {
                    await msg.DeleteAsync();
                    SocketGuild dcGuild = ((SocketGuildChannel)msg.Channel).Guild;
                    if (!db.Warning.Where(p => p.UserId == (long)msg.Author.Id && p.ServerId == (long)dcGuild.Id).Any())
                    {
                        await db.Warning.AddAsync(new Warning { ServerId = (long)dcGuild.Id, UserId = (long)msg.Author.Id, ActiveUntil = DateTime.Now.AddHours(1), Counter = 1 });
                        await msg.Channel.SendMessageAsync($"**{msg.Author.Mention} du wurdest für schlechtes Benehmen verwarnt. Warnung 1/3**");
                    }
                    else
                    {
                        var warn = db.Warning.Where(p => p.UserId == (long)msg.Author.Id && p.ServerId == (long)dcGuild.Id).FirstOrDefault();
                        warn.Counter++;
                        await msg.Channel.SendMessageAsync($"**{msg.Author.Mention} du wurdest für schlechtes Benehmen verwarnt. Warnung {warn.Counter}/3**");
                    }
                }
                await db.SaveChangesAsync();
            }

            Userlevel User = new Userlevel(msg);
            await User.SendLevelUp();
            await User.SetRoles();
        }

        private async Task MessageDeleted(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {

        }

        private async Task UserLeft(SocketGuildUser user)
        {
            using (swaightContext db = new swaightContext())
            {
                if (db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).Count() == 0)
                    return;
                if (db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).FirstOrDefault().Notify == 0)
                    return;
                else
                {
                    var channelId = db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).FirstOrDefault().LogchannelId;
                    var embed = new EmbedBuilder();
                    embed.WithDescription($"{user.Mention} left the server!");
                    embed.WithColor(new Color(255, 0, 0));
                    embed.AddField("User ID", user.Id.ToString(), true);
                    embed.AddField("Username", user.Username + "#" + user.Discriminator, true);
                    embed.ThumbnailUrl = user.GetAvatarUrl(ImageFormat.Auto, 1024);
                    embed.AddField("Joined Server at", user.JoinedAt.Value.DateTime.ToShortDateString() + " " + user.JoinedAt.Value.DateTime.ToShortTimeString(), false);
                    await _client.GetGuild(user.Guild.Id).GetTextChannel((ulong)channelId).SendMessageAsync("", false, embed.Build());
                }
            }
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            using (swaightContext db = new swaightContext())
            {
                if (db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).Count() == 0)
                    return;
                if (db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).FirstOrDefault().Notify == 0)
                    return;

                var memberRole = _client.Guilds.Where(p => p.Id == user.Guild.Id).FirstOrDefault().Roles.Where(p => p.Name == "Mitglied").FirstOrDefault();
                if (memberRole != null)
                    await user.AddRoleAsync(memberRole);
                var channelId = db.Guild.Where(p => p.ServerId == (long)user.Guild.Id).FirstOrDefault().LogchannelId;
                var embed = new EmbedBuilder();
                embed.WithDescription($"{user.Mention} joined the server!");
                embed.WithColor(new Color(0, 255, 0));
                embed.AddField("User ID", user.Id.ToString(), true);
                embed.AddField("Username", user.Username + "#" + user.Discriminator, true);
                embed.ThumbnailUrl = user.GetAvatarUrl(ImageFormat.Auto, 1024);
                embed.AddField("Joined Discord at", user.CreatedAt.DateTime.ToShortDateString() + " " + user.CreatedAt.DateTime.ToShortTimeString(), false);
                await _client.GetGuild(user.Guild.Id).GetTextChannel((ulong)channelId).SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
