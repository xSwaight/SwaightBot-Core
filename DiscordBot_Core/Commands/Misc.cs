﻿using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot_Core.Database;

namespace DiscordBot_Core.Commands
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        private readonly string version = "0.2";

        [Command("help")]
        public async Task Help()
        {
            var embed = new EmbedBuilder();
            embed.Description = "Commandlist:";
            embed.AddField("Legende:", "Pflicht Argumente: [argument] | Optionale Argumente: (argument)");
            embed.AddField("*Normal:* \n" + Config.bot.cmdPrefix + "player [S4 Username]", "Gibt die Stats eines S4 Spielers aus.");
            embed.AddField(Config.bot.cmdPrefix + "playercard [S4 Username]", "Erstellt eine Playercard Grafik.");
            embed.AddField(Config.bot.cmdPrefix + "s4dbcard [S4 Username]", "Erstellt eine Playercard Grafik im S4DB Style.");
            embed.AddField(Config.bot.cmdPrefix + "server", "Gibt die aktuelle Spielerzahl aus.");
            embed.AddField(Config.bot.cmdPrefix + "about", "Gibt Statistiken zum Bot aus.");
            embed.AddField(Config.bot.cmdPrefix + "ping", "Gibt die Verzögerung zum Bot aus.");
            embed.AddField(Config.bot.cmdPrefix + "level (User Mention)", "Ohne Argument gibt es das eigene Level aus, mit Argument das Level des Markierten Users.");
            if (Context.Guild.Roles.Where(p => p.Name == "S4 League").Count() > 0)
                embed.AddField(Config.bot.cmdPrefix + "s4", "Gibt dir die S4 League Rolle.");
            embed.AddField("\n*Administration:* \n" + Config.bot.cmdPrefix + "delete [anzahl]", "Löscht die angegebene Anzahl an Nachrichten im aktuellen Channel (Limit von 100 Nachrichten).");
            embed.AddField(Config.bot.cmdPrefix + "mute [User Mention] [duration]", "Muted den User für angegebene Zeit (Zeitindikatoren: s = sekunden, m = minuten, h = stunden, d = tage).");
            embed.AddField(Config.bot.cmdPrefix + "unmute [User Mention]", "Unmuted den User.");
            embed.AddField(Config.bot.cmdPrefix + "settings", "Zeigt die aktuellen Einstellungen an.");
            embed.AddField(Config.bot.cmdPrefix + "setLog", "Setzt den aktuellen Channel als Log Channel.");
            embed.AddField(Config.bot.cmdPrefix + "delLog", "Löscht den aktuell gesetzten Log Channel.");
            embed.AddField(Config.bot.cmdPrefix + "setNotification", "Setzt den aktuellen Channel als Notification Channel.");
            embed.AddField(Config.bot.cmdPrefix + "delNotification", "Löscht den aktuell gesetzten Log Channel.");
            embed.AddField(Config.bot.cmdPrefix + "notification", "Aktiviert oder deaktiviert die Notifications auf dem Server.");
            embed.WithFooter(new EmbedFooterBuilder() { Text = "Version " + version, IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Info_icon-72a7cf.svg/2000px-Info_icon-72a7cf.svg.png" });
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("about")]
        public async Task About()
        {
            int memberCount = 0;
            int offlineCount = 0;
            foreach (var server in Context.Client.Guilds)
            {
                memberCount += server.MemberCount;
                offlineCount += server.Users.Where(p => p.Status == UserStatus.Offline).Count();
            }

            var embed = new EmbedBuilder();
            embed.WithDescription($"**Statistiken**");
            embed.WithColor(new Color(197, 122, 255));
            embed.AddField("Total Users", memberCount.ToString(), true);
            embed.AddField("Online Users", (memberCount - offlineCount).ToString(), true);
            embed.AddField("Total Servers", Context.Client.Guilds.Count.ToString(), true);
            embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/210496271000141825/529839617113980929/robo2.png";
            embed.AddField("Bot created at", Context.Client.CurrentUser.CreatedAt.ToString(), false);
            embed.WithFooter(new EmbedFooterBuilder() { Text = "Version " + version, IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Info_icon-72a7cf.svg/2000px-Info_icon-72a7cf.svg.png" });
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("settings")]
        public async Task Settings()
        {
            using (discordbotContext db = new discordbotContext())
            {
                var channel = db.Guild.Where(p => p.ServerId == (long)Context.Guild.Id).FirstOrDefault();
                if (channel == null)
                    return;
                var logChannel = Context.Guild.TextChannels.Where(p => (long?)p.Id == channel.LogchannelId).FirstOrDefault();
                var notificationChannel = Context.Guild.TextChannels.Where(p => (long?)p.Id == channel.NotificationchannelId).FirstOrDefault();

                var embed = new EmbedBuilder();
                embed.WithDescription($"**Settings**");
                embed.WithColor(new Color(197, 122, 255));
                if (logChannel != null)
                    embed.AddField("Log Channel", logChannel.Mention, true);
                else
                    embed.AddField("Log Channel", "Nicht gesetzt.", true);

                if (notificationChannel != null)
                    embed.AddField("Notification Channel", notificationChannel.Mention, true);
                else
                    embed.AddField("Notification Channel", "Nicht gesetzt.", true);

                switch (channel.Notify)
                {
                    case 0:
                        embed.AddField("Notification", "Disabled");
                        break;
                    case 1:
                        embed.AddField("Notification", "Enabled");
                        break;
                    default:
                        embed.AddField("Notification", "Unknown");
                        break;
                }

                embed.ThumbnailUrl = "https://cdn.pixabay.com/photo/2018/03/27/23/58/silhouette-3267855_960_720.png";
                embed.WithFooter(new EmbedFooterBuilder() { Text = "Version " + version, IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Info_icon-72a7cf.svg/2000px-Info_icon-72a7cf.svg.png" });
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }

        [Command("ping")]
        public async Task Ping()
        {
            var dbContext = new discordbotContext();
            var users = dbContext.User.ToList();
            await Context.Channel.SendMessageAsync("Pong! `" + Context.Client.Latency + "ms`");
        }
    }
}